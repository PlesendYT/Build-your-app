# Terraform configuration for deploying AWS infrastructure
# Creates a VPC, subnets, security groups, and EC2 instances

# Specify the required Terraform version and provider versions
terraform {
  required_version = ">= 1.6"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

# Configure the AWS provider with the target region
provider "aws" {
  region = var.aws_region
}

# ---------- VPC and networking ----------

# Deploy the VPC module which creates the VPC, subnets, and gateways
module "vpc" {
  source = "./modules/vpc"

  vpc_cidr             = var.vpc_cidr
  environment          = var.environment
  availability_zones   = var.availability_zones
  public_subnet_cidrs  = var.public_subnet_cidrs
  private_subnet_cidrs = var.private_subnet_cidrs
}

# ---------- EC2 compute instances ----------

# Deploy EC2 instances in the public subnets created by the VPC module
module "ec2" {
  source = "./modules/ec2"

  instance_count     = var.instance_count
  instance_type      = var.instance_type
  subnet_ids         = module.vpc.public_subnet_ids
  security_group_ids = [module.vpc.security_group_id]
  environment        = var.environment
  key_name           = var.key_name
  vpc_id             = module.vpc.vpc_id
}
