/opt/mssql-tools/bin/sqlcmd -S $DB_SERVER,$DB_PORT -U $DB_USER -P $DB_PASS -i db_setup.sql
echo "Done DB Setup"