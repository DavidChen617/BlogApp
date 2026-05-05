FROM mcr.microsoft.com/dotnet/sdk:10.0 AS migration

WORKDIR /app
COPY . .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app/src/Infrastructure
ENTRYPOINT ["dotnet", "ef", "database", "update", "--startup-project", "../Api", "--no-build"]
