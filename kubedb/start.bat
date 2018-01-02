docker build -t glaadb .
docker run -d  -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=%SA_PASS%' -p 1433:1433 --name glaa-db glaadb