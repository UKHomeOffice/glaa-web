FROM microsoft/dotnet:sdk as builder
WORKDIR /app
COPY . .
RUN chmod +x ./kube/build_secrets.sh
RUN ./kube/build_secrets.sh
WORKDIR /app/GLAA.Web
RUN dotnet restore && dotnet build && dotnet publish -c Release -o ./out

FROM microsoft/dotnet:latest
## CREATE APP USER ##

# Create the home directory for the new app user.
RUN mkdir -p /home/app

# Create an app user so our program doesn't run as root.
RUN groupadd -r app &&\
    useradd -r -g app -d /home/app -s /sbin/nologin -c "Docker image user" app

# Set the home directory to our app user's home.
ENV HOME=/home/app
ENV APP_HOME=/home/app/my-project

## SETTING UP THE APP ##
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY --from=builder /app/GLAA.Web/out .
COPY --from=builder /app/appsettings.secrets.json ./secrets/.

USER app
ENTRYPOINT ["dotnet", "GLAA.Web.dll"]

