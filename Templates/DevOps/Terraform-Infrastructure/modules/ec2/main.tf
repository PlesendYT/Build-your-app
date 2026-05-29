# EC2 module — launches compute instances with an optional Application Load Balancer

# Security group allowing traffic from the VPC security group
resource "aws_security_group" "ec2" {
  name        = "${var.environment}-ec2-sg"
  description = "Security group for EC2 instances"
  vpc_id      = data.aws_vpc.selected.id

  ingress {
    description     = "HTTP from ALB"
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    security_groups = [aws_security_group.alb.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name        = "${var.environment}-ec2-sg"
    Environment = var.environment
  }
}

# Look up the default VPC to associate with the ALB
data "aws_vpc" "selected" {
  id = var.vpc_id
}

# Look up the latest Amazon Linux 2 AMI
data "aws_ami" "amazon_linux" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-gp2"]
  }
}

# EC2 instances launched in the specified subnets
resource "aws_instance" "app" {
  count                  = var.instance_count
  ami                    = data.aws_ami.amazon_linux.id
  instance_type          = var.instance_type
  subnet_id              = var.subnet_ids[count.index % length(var.subnet_ids)]
  vpc_security_group_ids = [aws_security_group.ec2.id]
  key_name               = var.key_name

  user_data = <<-EOF
    #!/bin/bash
    # Install a simple HTTP server on boot for demo purposes
    yum update -y
    yum install -y httpd
    systemctl start httpd
    systemctl enable httpd
    echo "<h1>${var.environment} instance ${count.index + 1}</h1>" > /var/www/html/index.html
  EOF

  tags = {
    Name        = "${var.environment}-app-${count.index + 1}"
    Environment = var.environment
  }
}

# ALB security group allowing inbound HTTP traffic
resource "aws_security_group" "alb" {
  name        = "${var.environment}-alb-sg"
  description = "Security group for ALB"
  vpc_id      = data.aws_vpc.selected.id

  ingress {
    description = "HTTP from anywhere"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name        = "${var.environment}-alb-sg"
    Environment = var.environment
  }
}

# Application Load Balancer
resource "aws_lb" "main" {
  name               = "${var.environment}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb.id]
  subnets            = var.subnet_ids

  tags = {
    Name        = "${var.environment}-alb"
    Environment = var.environment
  }
}

# ALB target group pointing to the EC2 instances
resource "aws_lb_target_group" "app" {
  name     = "${var.environment}-tg"
  port     = 80
  protocol = "HTTP"
  vpc_id   = data.aws_vpc.selected.id

  health_check {
    path                = "/"
    healthy_threshold   = 2
    unhealthy_threshold = 2
    timeout             = 5
    interval            = 30
  }

  tags = {
    Name        = "${var.environment}-tg"
    Environment = var.environment
  }
}

# Attach EC2 instances to the target group
resource "aws_lb_target_group_attachment" "app" {
  count            = var.instance_count
  target_group_arn = aws_lb_target_group.app.arn
  target_id        = aws_instance.app[count.index].id
  port             = 80
}

# ALB listener forwarding HTTP traffic to the target group
resource "aws_lb_listener" "front_end" {
  load_balancer_arn = aws_lb.main.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.app.arn
  }
}

# Outputs consumed by the root module
output "instance_ids" {
  value = aws_instance.app[*].id
}

output "public_ips" {
  value = aws_instance.app[*].public_ip
}

output "alb_dns_name" {
  value = aws_lb.main.dns_name
}
