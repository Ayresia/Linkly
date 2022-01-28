FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source

COPY Linkly.Api/*.csproj .
RUN dotnet restore

COPY Linkly.Api/. .
RUN dotnet publish -c release -o /app --no-restore --no-cache /restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Linkly.Api.dll"]
