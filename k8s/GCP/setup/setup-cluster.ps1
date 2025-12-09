# GCP GKE Cluster Setup Script for Windows PowerShell
# This script creates a GKE cluster and sets up necessary permissions

# Configuration variables
$PROJECT_ID = "YOUR_PROJECT_ID"
$CLUSTER_NAME = "YOUR_CLUSTER_NAME"
$ZONE = "YOUR_ZONE"
$REGION = "YOUR_REGION"
$NODE_COUNT = 2
$MACHINE_TYPE = "e2-standard-4"
$DISK_SIZE = "50"
$SERVICE_ACCOUNT_NAME = "YOUR_SERVICE_ACCOUNT_NAME"
$SERVICE_ACCOUNT_EMAIL = "${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com"

Write-Host "=== GCP GKE Cluster Setup ===" -ForegroundColor Green
Write-Host "Project: $PROJECT_ID" -ForegroundColor Yellow
Write-Host "Cluster: $CLUSTER_NAME" -ForegroundColor Yellow
Write-Host "Zone: $ZONE" -ForegroundColor Yellow

# Check if gcloud is installed
if (-not (Get-Command gcloud -ErrorAction SilentlyContinue)) {
    Write-Host "Error: gcloud CLI is not installed. Please install it first." -ForegroundColor Red
    exit 1
}

# Set the project
Write-Host "Setting project..." -ForegroundColor Cyan
gcloud config set project $PROJECT_ID

# Enable required APIs
Write-Host "Enabling required APIs..." -ForegroundColor Cyan
gcloud services enable container.googleapis.com
gcloud services enable compute.googleapis.com
gcloud services enable iam.googleapis.com
gcloud services enable cloudbuild.googleapis.com
gcloud services enable containerregistry.googleapis.com
gcloud services enable artifactregistry.googleapis.com

# Create service account for cluster operations
Write-Host "Creating service account..." -ForegroundColor Cyan
# Check if service account already exists
$sa_exists = gcloud iam service-accounts list --filter="email=$SERVICE_ACCOUNT_EMAIL" --format="value(email)"
if (-not $sa_exists) {
    gcloud iam service-accounts create $SERVICE_ACCOUNT_NAME --display-name="CI/CD Deployer" --description="Service account for CI/CD and cluster administration"
}
else {
    Write-Host "Service account $SERVICE_ACCOUNT_EMAIL already exists." -ForegroundColor Yellow
}

# Grant necessary roles to service account
# NOTE: The roles granted here are very permissive (e.g., admin roles).
# For a production environment, you should follow the principle of least privilege
# and grant only the necessary permissions.
# For example:
# - roles/container.developer (for CI/CD to deploy)
# - roles/artifactregistry.reader (for GKE nodes to pull images)
# - roles/iam.serviceAccountUser (to allow the service account to be used by GKE nodes)
Write-Host "Granting roles to service account..." -ForegroundColor Cyan
gcloud projects add-iam-policy-binding $PROJECT_ID --member="serviceAccount:$SERVICE_ACCOUNT_EMAIL" --role="roles/container.admin"
gcloud projects add-iam-policy-binding $PROJECT_ID --member="serviceAccount:$SERVICE_ACCOUNT_EMAIL" --role="roles/compute.admin"
gcloud projects add-iam-policy-binding $PROJECT_ID --member="serviceAccount:$SERVICE_ACCOUNT_EMAIL" --role="roles/iam.serviceAccountUser"
gcloud projects add-iam-policy-binding $PROJECT_ID --member="serviceAccount:$SERVICE_ACCOUNT_EMAIL" --role="roles/storage.admin"
gcloud projects add-iam-policy-binding $PROJECT_ID --member="serviceAccount:$SERVICE_ACCOUNT_EMAIL" --role="roles/artifactregistry.admin"

# Create Artifact Registry repository for images
Write-Host "Creating Artifact Registry repository..." -ForegroundColor Cyan
# Check if repository already exists
$repo_exists = gcloud artifacts repositories list --location=$REGION --filter="name:projects/$PROJECT_ID/locations/$REGION/repositories/YOUR_REPO_NAME" --format="value(name)"
if (-not $repo_exists) {
    gcloud artifacts repositories create YOUR_REPO_NAME --repository-format=docker --location=$REGION --description="Travel application images"
}
else {
    Write-Host "Artifact Registry repository 'YOUR_REPO_NAME' already exists." -ForegroundColor Yellow
}

# Verify plugin is available
if (-not (Get-Command gke-gcloud-auth-plugin -ErrorAction SilentlyContinue)) {
    try {
        # Try to install via Cloud SDK components (works when Cloud SDK supports component manager)
        gcloud components install gke-gcloud-auth-plugin --quiet
    }
    catch {
        Write-Host "gcloud components install failed or not supported. Attempting OS package / manual checks..." -ForegroundColor Yellow
    }
}

# Create the GKE cluster
Write-Host "Creating GKE cluster..." -ForegroundColor Cyan
gcloud container clusters create $CLUSTER_NAME `
    --zone=$ZONE `
    --num-nodes=$NODE_COUNT `
    --machine-type=$MACHINE_TYPE `
    --disk-size=$DISK_SIZE `
    --enable-autorepair `
    --enable-autoupgrade `
    --enable-autoscaling --min-nodes=1 --max-nodes=5 `
    --service-account=$SERVICE_ACCOUNT_EMAIL `
    --enable-network-policy `
    --enable-ip-alias `
    --addons HttpLoadBalancing `
    --scopes=https://www.googleapis.com/auth/cloud-platform

 
# Create global static IP address for the Ingress
Write-Host "Creating global static IP address 'YOUR_IP_NAME'..." -ForegroundColor Cyan
try {
    gcloud compute addresses create YOUR_IP_NAME --global --project=$PROJECT_ID
}
catch {
    Write-Host "Global IP address 'YOUR_IP_NAME' already exists." -ForegroundColor Yellow
}
 
# Create service account key for CI/CD
# NOTE: The following section for creating and using a service account key is commented out.
# This is a security best practice. Exporting service account keys creates a security liability.
# Instead of keys, consider using Workload Identity Federation, which allows external workloads
# (like GitHub Actions or other CI/CD systems) to impersonate a service account without a key.
# <#Write-Host "Creating service account key..." -ForegroundColor Cyan
# gcloud iam service-accounts keys create "./cicd-deployer-key.json" --iam-account=$SERVICE_ACCOUNT_EMAIL
# $KeyPathJson = Join-Path $PSScriptRoot 'cicd-deployer-key.json'
# [Convert]::ToBase64String([IO.File]::ReadAllBytes("$KeyPathJson")) > cicd-deployer-key.b64
# $KeyPath = Join-Path $PSScriptRoot 'cicd-deployer-key.b64'
# $GCP_SA_KEY = Get-Content $KeyPath#>

Write-Host "=== Setup Complete ===" -ForegroundColor Green
Write-Host "Cluster Name: $CLUSTER_NAME" -ForegroundColor Yellow
Write-Host "Cluster Zone: $ZONE" -ForegroundColor Yellow
Write-Host "Service Account: $SERVICE_ACCOUNT_EMAIL" -ForegroundColor Yellow
Write-Host "Service Account Key: ./cicd-deployer-key.json" -ForegroundColor Yellow
Write-Host "Service Account Key Base64: $GCP_SA_KEY" -ForegroundColor Yellow
Write-Host "Artifact Registry: $REGION-docker.pkg.dev/$PROJECT_ID/YOUR_REPO_NAME" -ForegroundColor Yellow
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Green
Write-Host "1. Run: .\setup-cluster.ps1" -ForegroundColor White
Write-Host "2. Apply the Kubernetes manifests" -ForegroundColor White
Write-Host "3. Build and push your application images" -ForegroundColor White