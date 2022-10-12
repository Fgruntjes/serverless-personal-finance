variable "environment" {
  type = string
}

variable "cloudrun_url" {
  type = string
}

terraform {
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = "~> 4.40.0"
    }
  }
}

provider "google" {}

resource "google_pubsub_topic" "bankstransaction-import" {
  name = "bankstransaction-import-${var.environment}"
}

resource "google_pubsub_subscription" "bankstransaction-import" {
  name  = "function-bankstransaction-import-${var.environment}"
  topic = google_pubsub_topic.bankstransaction-import.name

  push_config {
    push_endpoint = var.cloudrun_url
  }
}