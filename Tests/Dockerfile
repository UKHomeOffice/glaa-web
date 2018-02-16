FROM microsoft/dotnet:sdk as builder
WORKDIR /app

COPY .. .
WORKDIR /app/
RUN dotnet test

