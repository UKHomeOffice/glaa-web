FROM microsoft/dotnet:sdk as builder
WORKDIR /app
## install tooling
RUN apt-get update
RUN curl -sL https://deb.nodesource.com/setup_9.x | bash -
RUN apt-get install -y nodejs
RUN apt-get install -y build-essential

COPY . .
WORKDIR /app/
RUN dotnet test
WORKDIR /app/GLAA.Web
RUN npm install webpack -g
RUN npm install
RUN webpack
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
ENV APP_HOME=/home/app/glaa-web

## SETTING UP THE APP ##
RUN mkdir $APP_HOME
WORKDIR $APP_HOME
RUN mkdir secrets

COPY --from=builder /app/GLAA.Web/out .

USER app
ENTRYPOINT ["dotnet", "GLAA.Web.dll"]

