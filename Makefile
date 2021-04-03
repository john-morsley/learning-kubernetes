.DEFAULT_GOAL=help
CURRENT_FOLDER:=$(shell pwd)
DOCKER_ID=johnmorsley

#######################################################################################################################
# Help
#######################################################################################################################
.SILENT:help
.PHONY:help
help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

#######################################################################################################################
# .NET
#######################################################################################################################
.SILENT:dotnet-run-ui
.PHONY:dotnet-run-ui
dotnet-run-ui: ## Builds the .NET code for the UI project
	@echo "Current Folder: ${CURRENT_FOLDER}"
	@cd ${CURRENT_FOLDER}/src/Example.UI && \
	rm --force --recursive obj/ && \
	rm --force --recursive bin/ && \
	dotnet clean --nologo && \
	dotnet restore --nologo && \
	dotnet build --nologo --no-restore --configuration Debug && \
	dotnet run --nologo --no-restore --no-build --configuration Debug

#######################################################################################################################
# Docker
#######################################################################################################################

.SILENT:docker-build-api
.PHONY:docker-build-api
docker-build-api: ## Builds Docker image for the API project
	@cd ${CURRENT_FOLDER}/src/ && \
	cp ./Example.API/Dockerfile ./ && \
	docker build --tag "${DOCKER_ID}/example-api" . && \
	rm Dockerfile && \
	docker images | grep "${DOCKER_ID}/example-api"

.SILENT:docker-run-api
.PHONY:docker-run-api
docker-run-api: ## Builds Docker image for the API project
	@docker run --detach --publish 5080:80 "${DOCKER_ID}/example-api" && \
	docker ps | grep "${DOCKER_ID}/example-api"

.SILENT:docker-build-ui
.PHONY:docker-build-ui
docker-build-ui: ## Builds Docker image for the UI project
	@cd ${CURRENT_FOLDER}/src/ && \
	cp ./Example.UI/Dockerfile ./ && \
	docker build --tag "${DOCKER_ID}/example-ui" . && \
	rm Dockerfile && \
	docker images | grep "${DOCKER_ID}/example-ui"

.SILENT:docker-run-ui
.PHONY:docker-run-ui
docker-run-ui: ## Builds Docker image for the UI project
	@docker run --detach --env API_ENDPOINT=http://host.docker.internal:5080 --publish 6080:80 "${DOCKER_ID}/example-ui" && \
	docker ps | grep "${DOCKER_ID}/example-ui"

.SILENT:docker-delete-ui
.PHONY:docker-delete-ui
docker-delete-ui: ## Deletes Docker image for the UI project
	docker ps | grep "${DOCKER_ID}/example-ui" | awk "{ print $1 }"
	
.SILENT:docker-login
.PHONY:docker-login
docker-login: ## Login to Docker Hub
	docker login

.SILENT:docker-push-api
.PHONY:docker-push-api
docker-push-api: docker-login ## Push API image to Docker Hub
	docker push ${DOCKER_ID}/example-api:latest

.SILENT:docker-push-ui
.PHONY:docker-push-ui
docker-push-ui: docker-login ## Push UI image to Docker Hub
	docker push ${DOCKER_ID}/example-ui:latest

#######################################################################################################################
# Kubernetes
#######################################################################################################################

.SILENT:namespace
.PHONY:namespace
namespace: ## Applies the namespace YAML for the project
	@cd ./k8s/ && \
	kubectl apply --filename ./namespace.yaml

.SILENT:deploy-ui
.PHONY:deploy-ui
deploy-ui: ## Applies the deployment YAML for the UI
	@cd ./k8s/ && \
	kubectl apply --filename ./deployment-ui.yaml

.SILENT:service-ui
.PHONY:service-ui
service-ui: ## Applies the service YAML for the UI
	@cd ./k8s/ && \
	kubectl apply --filename ./service-ui.yaml

.SILENT:deploy-api
.PHONY:deploy-api
deploy-api: ## Applies the deployment YAML for the API
	@cd ./k8s/ && \
	kubectl apply --filename ./deployment-api.yaml

.SILENT:service-api
.PHONY:service-api
service-api: ## Applies the service YAML for the API
	@cd ./k8s/ && \
	kubectl apply --filename ./service-api.yaml

.SILENT:kubernetes-all
.PHONY:kubernetes-all
kubernetes-all: namespace deploy-api service-api deploy-ui service-ui ## Deletes the namespace YAML for the project

.SILENT:delete-namespace
.PHONY:delete-namespace
	@cd ./k8s/ && \
	kubectl delete --filename ./namespace.yaml

#######################################################################################################################
# Istio
#######################################################################################################################

.SILENT:istio-gateway-ui
.PHONY:istio-gateway-ui
istio-gateway-ui: ## Applies the gateway YAML for the UI
	@cd ./k8s/ && \
	kubectl apply --filename ./gateway-ui.yaml

.SILENT:istio-virtual-service-ui
.PHONY:istio-virtual-service-ui
istio-virtual-service-ui: ## Applies the virtual service YAML for the UI
	@cd ./k8s/ && \
	kubectl apply --filename ./virtual-service-ui.yaml

.SILENT:istio-all
.PHONY:istio-all
istio-all: namespace istio-gateway-ui istio-virtual-service-ui ## Deletes the namespace YAML for the project
