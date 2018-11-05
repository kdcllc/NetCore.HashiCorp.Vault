FROM microsoft/dotnet:2.1.5-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1.403-sdk AS build
WORKDIR /src
COPY src/ConsoleApp/ConsoleApp.csproj src/ConsoleApp/
COPY src/NetCore.HashiCorp.Vault/NetCore.HashiCorp.Vault.csproj src/NetCore.HashiCorp.Vault/
RUN dotnet restore src/ConsoleApp/ConsoleApp.csproj
COPY . .
WORKDIR src/src/ConsoleApp
RUN dotnet build ConsoleApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ConsoleApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]
