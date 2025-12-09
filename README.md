# TravelAppCore

This repository contains the source code for the TravelAppCore project, a comprehensive travel application built with modern .NET technologies and cloud-native practices.

## Project Overview

The application consists of a backend API and a frontend Web UI, designed to be scalable and deployable to Kubernetes clusters. It leverages a microservices-oriented architecture with containerization.

## Technologies Used

The project utilizes a robust stack of modern technologies:

### Backend
- **.NET 9**: The core framework for both the API and Web UI, ensuring high performance and cross-platform compatibility.
- **ASP.NET Core Web API**: Powers the backend services.
- **Entity Framework Core**: (Implied by MSSQL usage) Likely used for data access.
- **MSSQL**: Relational database management system.
- **Serilog**: Structured logging for better observability.
- **NSwag**: Used for OpenAPI/Swagger generation and client code generation.

### Frontend
- **ASP.NET Core MVC / Razor Pages**: Used for the Web UI implementation.
- **Bootstrap / Custom CSS**: For styling the user interface.

### DevOps & Infrastructure
- **Docker**: Containerization of applications for consistent environments.
- **Kubernetes (K8s)**: Orchestration for deploying and managing containers.
  - **GKE (Google Kubernetes Engine)**: Target cloud environment for production.
  - **Local K8s**: Support for local development clusters.
- **Azure DevOps Pipelines**: CI/CD pipelines for automated building and deployment.
- **Google Cloud Platform (GCP)**: Cloud provider for hosting resources.

## Features

- **Containerized Architecture**: Both API and UI are fully containerized using Docker.
- **Kubernetes Deployment**: Complete set of Kubernetes manifests for deploying to GKE, including:
  - Deployments, Services, and Ingress
  - ConfigMaps and Secrets management
  - Network Policies for security
  - SSL Certificate management
- **CI/CD Integration**: Automated pipelines defined in `TravelAppHosting.yml` and `BuildAndDeployDocker.yml` for building, testing, and deploying to cloud environments.
- **Database Integration**: MSSQL Server deployment configuration within Kubernetes with persistent storage.

## Setup & Deployment

### Prerequisites
- Docker Desktop
- Kubernetes Cluster (Local or Cloud)
- .NET 9 SDK

### Local Development
1. **Docker Compose**: You can use `docker-compose` to run the services locally.
   ```bash
   cd src/Api
   docker-compose up
   ```

### Kubernetes Deployment
The `k8s` directory contains all necessary manifests.

1. **Apply Configurations**:
   ```bash
   kubectl apply -f k8s/GCP/mssql-namespace.yaml
   kubectl apply -f k8s/GCP/webapp-namespace.yaml
   # Apply other config and secret files...
   ```

# TravelAppCore

This repository contains the source code for the TravelAppCore project, a comprehensive travel application built with modern .NET technologies and cloud-native practices.

## Project Overview

The application consists of a backend API and a frontend Web UI, designed to be scalable and deployable to Kubernetes clusters. It leverages a microservices-oriented architecture with containerization.

## Technologies Used

The project utilizes a robust stack of modern technologies:

### Backend
- **.NET 9**: The core framework for both the API and Web UI, ensuring high performance and cross-platform compatibility.
- **ASP.NET Core Web API**: Powers the backend services.
- **Entity Framework Core**: (Implied by MSSQL usage) Likely used for data access.
- **MSSQL**: Relational database management system.
- **Serilog**: Structured logging for better observability.
- **NSwag**: Used for OpenAPI/Swagger generation and client code generation.

### Frontend
- **ASP.NET Core MVC / Razor Pages**: Used for the Web UI implementation.
- **Bootstrap / Custom CSS**: For styling the user interface.

### DevOps & Infrastructure
- **Docker**: Containerization of applications for consistent environments.
- **Kubernetes (K8s)**: Orchestration for deploying and managing containers.
  - **GKE (Google Kubernetes Engine)**: Target cloud environment for production.
  - **Local K8s**: Support for local development clusters.
- **Azure DevOps Pipelines**: CI/CD pipelines for automated building and deployment.
- **Google Cloud Platform (GCP)**: Cloud provider for hosting resources.

## Features

- **Containerized Architecture**: Both API and UI are fully containerized using Docker.
- **Kubernetes Deployment**: Complete set of Kubernetes manifests for deploying to GKE, including:
  - Deployments, Services, and Ingress
  - ConfigMaps and Secrets management
  - Network Policies for security
  - SSL Certificate management
- **CI/CD Integration**: Automated pipelines defined in `TravelAppHosting.yml` and `BuildAndDeployDocker.yml` for building, testing, and deploying to cloud environments.
- **Database Integration**: MSSQL Server deployment configuration within Kubernetes with persistent storage.

## Setup & Deployment

### Prerequisites
- Docker Desktop
- Kubernetes Cluster (Local or Cloud)
- .NET 9 SDK

### Local Development
1. **Docker Compose**: You can use `docker-compose` to run the services locally.
   ```bash
   cd src/Api
   docker-compose up
   ```

### Kubernetes Deployment
The `k8s` directory contains all necessary manifests.

1. **Apply Configurations**:
   ```bash
   kubectl apply -f k8s/GCP/mssql-namespace.yaml
   kubectl apply -f k8s/GCP/webapp-namespace.yaml
   # Apply other config and secret files...
   ```

2. **Deploy Services**:
   ```bash
   kubectl apply -f k8s/GCP/mssql-deployment.yaml
   kubectl apply -f k8s/GCP/webapp-deployment.yaml
   ```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.