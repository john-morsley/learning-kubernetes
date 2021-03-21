# Learning Kubernetes

In order to learn multiple Kubernetes concepts, we are going to create multiple applications and have them communicate with each other.

## Steps:

1 - Create a very simple example API
2 - Containerise application using Docker
3 - Host above container on Kubernetes (Docker)

## Prerequisites:

1 - We are in a folder created especially for this work. I have a dedicated repository on GitHub, learning-kubernetes, this is checked out on my local dev machine.
2 - A development tool of choice. I'm using JetBrains Rider.
3 - Docker is installed. My version is: 20.10.5 (docker --version)
4 - Kubernetes is enable within Docker.
5 - .NET is installed. My version is: 5.0.201 (dotnet --version)

## Steps



### 1 - Create a very simple example API

Create an empty solution

```
mkdir Example
cd Example
dotnet new sln -o Example
dotnet new webapi -o ExampleApi --no-https
cd ExampleApi

```

Browse to: https://localhost:5443/swagger/index.html

### 2 - Containerise application using Docker

Add the following Dockerfile

```
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

WORKDIR /publish 

COPY *.csproj ./ 

RUN dotnet restore 

COPY . ./ 

RUN dotnet publish --configuration Release --output out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime

WORKDIR /dotnetapp

COPY --from=build /publish/out .

ENV ASPNETCORE_URLS http://+:5080

ENTRYPOINT ["dotnet", "ExampleApi.dll"]
```

To create the image:

````
docker build --tag example-api:development .
````

To run a Docker container with the above image:

```
docker run --publish 5080:5080 --publish 5443:5443 example-api:development
```

## Todo

Add versioning to API
Istio