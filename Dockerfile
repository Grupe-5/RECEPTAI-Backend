# Use the official ASP.NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5169

# Copy the published output from the GitHub Actions workflow
COPY . .

ENTRYPOINT ["dotnet", "receptai.api.dll"]
