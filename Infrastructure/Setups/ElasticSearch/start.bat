docker-compose -f docker-compose.yml -f docker-compose.override.yml -f ./apm/docker-compose.apm.yml -f ./filebeat/docker-compose.filebeat.yml -f ./logstash/docker-compose.logstash.yml -f ./zipkin/docker-compose.zipkin.yml up -d