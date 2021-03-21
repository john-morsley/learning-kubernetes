#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5080

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ExampleApi.csproj", "./"]
RUN dotnet restore "ExampleApi.csproj"
COPY . .
WORKDIR "/src/."
ENV ASPNETCORE_ENVIRONMENT=Development
RUN dotnet build "ExampleApi.csproj" -c Release -o /build

FROM build AS publish
RUN dotnet publish "ExampleApi.csproj" -c Release -o /publish

FROM base AS final
WORKDIR /app
COPY --from=publish /publish .
EXPOSE 5080
ENV ASPNETCORE_URLS http://+:5080
ENV ASPNET_URLS http://+:5080
ENTRYPOINT ["dotnet", "ExampleApi.dll"]