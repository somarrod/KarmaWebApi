
!/bin/bash

# Esperar a que SQL Server estiga llest
sleep 20s

# Executar script per a crear la BD si no existeix, sino, no fa res
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Password123! -i /var/opt/mssql/backup/init.sql
