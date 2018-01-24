FROM microsoft/mssql-tools
## CREATE APP USER ##

# Create the home directory for the new app user.
RUN mkdir -p /home/app

# Create an app user so our program doesn't run as root.
RUN groupadd -r app &&\
    useradd -r -g app -d /home/app -c "Docker image user" app

# Set the home directory to our app user's home.
ENV HOME=/home/app

WORKDIR $HOME
COPY ./db_setup.sql ./db_setup.sh ./
RUN chmod +x ./db_setup.sh

USER app
ENTRYPOINT ["./db_setup.sh"]
#CMD /bin/bash 
