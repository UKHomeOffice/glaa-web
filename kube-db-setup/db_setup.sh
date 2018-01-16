/opt/mssql-tools/bin/sqlcmd -S localhost -U "sa" -P $SA_PASSWORD -i db_setup.sql
echo "Done DB Setup"