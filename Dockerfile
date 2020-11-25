# The Microsoft Container Registry (MCR, mcr.microsoft.com) is a syndicate of Docker Hub - which hosts publicly accessible containers. 

# COPY - Copy the the specific folder to a folder in the container
# WORKDIR - Change the current directory inside of the container to App
# ENTRYPOINT -  tells Docker to configure the container to run as an executable. 
# When the container starts, the ENTRYPOINT command runs. 
# When this command ends, the container will automatically stop.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InterProcessCommunication.csproj", "./"]
RUN dotnet restore "InterProcessCommunication.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "InterProcessCommunication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterProcessCommunication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterProcessCommunication.dll", "-s", "172.31.255.254", "3001"]