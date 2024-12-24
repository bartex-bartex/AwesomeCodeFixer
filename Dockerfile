# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/AwesomeCodeFixerApp.sln", "./src/"]
COPY ["src/AwesomeCodeFixerApi/AwesomeCodeFixerApi.csproj", "src/AwesomeCodeFixerApi/"]
COPY ["src/AwesomeCodeFixerLibrary/AwesomeCodeFixerLibrary.csproj", "src/AwesomeCodeFixerLibrary/"]
RUN dotnet restore "src/AwesomeCodeFixerApp.sln"

COPY . .

RUN dotnet publish "src/AwesomeCodeFixerApi/AwesomeCodeFixerApi.csproj" -c Release -o /app/publish --no-restore

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Install necessary tools
RUN apt-get update && apt-get install -y \
    pipx \
    npm \
    wget \
    gnupg \
    gnupg2 \
    && apt-get clean

# Add pipx binary locatin to PATH
ENV PATH="/root/.local/bin:${PATH}"
    
# Install formatters
RUN npm install -g --save-exact prettier \
    && npm install -g prettier-plugin-latex \
    && npm install -g prettier-plugin-sql \
    && pipx install black \
    && pipx install clang-format

# Install linters
RUN npm install -g eslint@8.57.0 \
    && npm install markdownlint-cli -g \
    && npm install eslint @babel/core @babel/eslint-parser -g \
    && pipx install clang-tidy \
    && pipx install sqlfluff \
    && pipx install flake8

# Debian doesn't include chktex in repository, so get it from ubnuntu
RUN apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 3B4FE6ACC0B21F32 871920D1991BC93C
RUN echo "deb http://archive.ubuntu.com/ubuntu/ focal main universe" > /etc/apt/sources.list.d/ubuntu.list
RUN apt-get update && apt-get install -y chktex
RUN rm /etc/apt/sources.list.d/ubuntu.list && apt-get clean

# Copy the published output from the build stage
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AwesomeCodeFixerApi.dll"]

