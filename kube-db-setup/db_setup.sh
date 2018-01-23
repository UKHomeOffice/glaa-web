#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -S $DB_SERVER,$DB_PORT -U $DB_USER -P $DB_PASS -i db_setup.sql -r1
echo "Done DB Setup"
