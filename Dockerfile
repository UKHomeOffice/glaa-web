FROM microsoft/dotnet:sdk as builder
WORKDIR /app
COPY . .
WORKDIR /app/GLAA.Web
RUN dotnet restore && dotnet build && dotnet publish -c Release -o ./out

FROM microsoft/dotnet:latest
WORKDIR /app
COPY --from=builder /app/GLAA.Web/out .
ENTRYPOINT ["dotnet", "GLAA.Web.dll"]

