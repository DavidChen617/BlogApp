FROM mcr.microsoft.com/dotnet/sdk:10.0 AS migration

WORKDIR /app

COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Api/Api.csproj src/Api/

RUN dotnet restore src/Api/Api.csproj

COPY . .

RUN dotnet build src/Api/Api.csproj -c Release --no-restore
RUN dotnet tool install --global dotnet-ef --version 10.0.7

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app/src/Infrastructure
ENTRYPOINT ["dotnet", "ef", "database", "update", "--startup-project", "../Api", "--configuration", "Release", "--no-build"]
