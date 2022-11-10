# docker network create --driver nat rpgNetwork || true

$network_name = "rpg"

docker network rm $network_name
docker network create --driver nat $network_name

docker container stop enemy
docker pull jinoptimist/enemyapi
docker run -p 8081:80 -itd --rm --network $network_name --name enemy jinoptimist/enemyapi

echo "!!!!!!!!! enemy is deployed"

docker container stop tavern
docker pull jinoptimist/tavernapi
docker run -p 8082:80 -itd --rm --network $network_name --name tavern jinoptimist/tavernapi

echo "!!!!!!!!! tavern is deployed"