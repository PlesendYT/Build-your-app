# Variables for the EC2 module

variable "instance_count" {
  description = "Number of EC2 instances to launch"
  type        = number
}

variable "instance_type" {
  description = "EC2 instance type"
  type        = string
}

variable "subnet_ids" {
  description = "List of subnet IDs to launch instances in"
  type        = list(string)
}

variable "security_group_ids" {
  description = "List of security group IDs to attach"
  type        = list(string)
}

variable "environment" {
  description = "Environment name for tagging"
  type        = string
}

variable "key_name" {
  description = "EC2 key pair name for SSH access"
  type        = string
}

variable "vpc_id" {
  description = "VPC ID for the ALB"
  type        = string
  default     = ""
}
