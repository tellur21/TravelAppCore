FROM ubuntu:22.04
ENV TARGETARCH="linux-x64"

# Install dependencies for apt over HTTPS and key management
RUN apt-get update && apt-get install -y \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

# Add Docker's official GPG key
RUN mkdir -p /etc/apt/keyrings
RUN curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg

# Add Docker repository with signed-by option
RUN echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null

# Update package index and install Docker CLI only
RUN apt-get update && apt-get install -y docker-ce-cli

# Install gcloud CLI
RUN apt-get update
RUN apt-get install -y apt-transport-https ca-certificates gnupg curl
RUN curl -sSL https://packages.cloud.google.com/apt/doc/apt-key.gpg | gpg --dearmor -o /usr/share/keyrings/cloud.google.gpg
RUN echo "deb [signed-by=/usr/share/keyrings/cloud.google.gpg] https://packages.cloud.google.com/apt cloud-sdk main" | tee -a /etc/apt/sources.list.d/google-cloud-sdk.list
RUN apt-get update && apt-get install -y \
    google-cloud-cli \
    google-cloud-sdk-gke-gcloud-auth-plugin \
    kubectl

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# Install other dependencies for your agent
RUN apt-get install -y git jq libicu70

WORKDIR /azp/

COPY ./start.sh ./
RUN chmod +x ./start.sh

ENV AGENT_ALLOW_RUNASROOT="true"

ENTRYPOINT ["./start.sh"]