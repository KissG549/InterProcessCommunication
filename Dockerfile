# The Microsoft Container Registry (MCR, mcr.microsoft.com) is a syndicate of Docker Hub - which hosts publicly accessible containers. 

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