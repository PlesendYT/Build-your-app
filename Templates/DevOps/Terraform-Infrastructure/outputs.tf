# Terraform outputs that display created resource information
# After apply, these are shown in the CLI and can be used by other tools

output "vpc_id" {
  description = "The ID of the created VPC"
  value       = module.vpc.vpc_id
}

output "public_subnet_ids" {
  description = "IDs of the public subnets"
  value       = module.vpc.public_subnet_ids
}

output "private_subnet_ids" {
  description = "IDs of the private subnets"
  value       = module.vpc.private_subnet_ids
}

output "security_group_id" {
  description = "ID of the security group attached to the instances"
  value       = module.vpc.security_group_id
}

output "ec2_instance_ids" {
  description = "IDs of the launched EC2 instances"
  value       = module.ec2.instance_ids
}

output "ec2_public_ips" {
  description = "Public IP addresses of the EC2 instances"
  value       = module.ec2.public_ips
}

output "alb_dns_name" {
  description = "DNS name of the Application Load Balancer (if created)"
  value       = module.ec2.alb_dns_name
}
