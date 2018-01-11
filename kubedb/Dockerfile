FROM microsoft/mssql-server-linux:latest

ENV USER_ID 2147483640

COPY ./db_setup.sql . 

COPY ./db_setup.sh .

RUN chmod +x db_setup.sh

USER ${USER_ID}
