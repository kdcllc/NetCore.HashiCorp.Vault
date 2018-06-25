FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52767
EXPOSE 44339

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/WebApp/WebApp.csproj src/WebApp/
COPY src/NetCore.HashiCorp.Vault/NetCore.HashiCorp.Vault.csproj src/NetCore.HashiCorp.Vault/
RUN dotnet restore src/WebApp/WebApp.csproj
COPY . .
WORKDIR /src/src/WebApp
RUN dotnet build WebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApp.dll"]
