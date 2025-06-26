# ========================
# STAGE 1: Build the app
# ========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy project files and restore dependencies
COPY *.sln ./
COPY FamilyDinnerVotingAPI/*.csproj ./FamilyDinnerVotingAPI/
RUN dotnet restore ./FamilyDinnerVotingAPI/FamilyDinnerVotingAPI.csproj

# Copy the rest of the source code
COPY . ./

# Build the application in Release mode
RUN dotnet publish ./FamilyDinnerVotingAPI/FamilyDinnerVotingAPI.csproj -c Release -o /app/publish

# ================================
# STAGE 2: Run the published app
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

# Expose the port your app uses
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "FamilyDinnerVotingAPI.dll"]

