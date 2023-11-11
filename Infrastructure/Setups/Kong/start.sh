docker-compose build kong
docker-compose up -d kong-db
sleep 2m
docker-compose run --rm kong kong migrations bootstrap
docker-compose run --rm kong kong migrations up
docker-compose up -d kong-import
docker-compose up -d kong
docker-compose ps

docker-compose up -d konga
sleep 2m

