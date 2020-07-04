## Prerequisites 

- Docker (or podman)
- Docker compose (or podman-compose)

Tested with podman but should work with Docker too.

## Run the app

`docker-compose build && docker-compose up`

## Stop containers and clean up
`docker-compose down`

- REST API: http://localhost:8080/ (no auth)
- Adminer (SQL): http://localhost:8081/
    - username: root
    - password: developer

# Note

Does not persist data by default on container restart. Uncomment volumes sections in `docker-compose.yml` to do that.

It is also recommended to change the default passwords in `docker-compose.yml`.

# EF Core migrations not working

Add connection string to environment variables with name `RankedEloConnectionString`