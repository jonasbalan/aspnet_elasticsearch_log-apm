docker-compose build kong
docker-compose up -d kong-db
timeout 30
docker-compose run --rm kong kong migrations bootstrap
docker-compose run --rm kong kong migrations up

docker-compose run --rm kong kong config db_import /opt/kong/kong.yaml 
docker-compose up -d kong

docker-compose up -d konga
