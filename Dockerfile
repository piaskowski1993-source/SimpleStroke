FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SS.Api/SS.Api.csproj SS.Api/
COPY SS.Core/SS.Core.csproj SS.Core/
RUN dotnet restore SS.Api/SS.Api.csproj

COPY SS.Api/ SS.Api/
COPY SS.Core/ SS.Core/
RUN dotnet publish SS.Api/SS.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SS.Api.dll"]