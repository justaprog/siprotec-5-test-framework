FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy project file first for better cache reuse
COPY src/TestManagement.Api/TestManagement.Api.csproj src/TestManagement.Api/
RUN dotnet restore src/TestManagement.Api/TestManagement.Api.csproj

# copy the rest of the source
COPY . ./
RUN dotnet publish src/TestManagement.Api/TestManagement.Api.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# listen on port 8080 inside container
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 8080 

ENTRYPOINT ["dotnet", "TestManagement.Api.dll"]