FROM microsoft/mssql-tools

COPY ./db_setup.sql ./db_setup.sh ./
RUN chmod +x ./db_setup.sh

ENTRYPOINT ["./db_setup.sh"]