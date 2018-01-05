FROM microsoft/mssql-server-linux:latest

ENV RUN_USER 500
ENV RUN_GROUP 0

RUN mkdir /var/opt/mssql \
    && chown -R ${RUN_USER}:${RUN_GROUP} /var/opt/mssql \
    && chmod -R 770 /var/opt/mssql

WORKDIR /var/opt/mssql

COPY ./db_setup.sql . 

COPY ./db_setup.sh .

RUN chmod +x db_setup.sh

USER ${RUN_USER}:${RUN_GROUP}

ENTRYPOINT ["./entrypoint"]
