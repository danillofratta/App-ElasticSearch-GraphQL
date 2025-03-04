# Objetivo

Este Projeto tem o objetivo de exemplificar o uso ElasticSeach e GraphQL

# Tecnologias aplicadas

Este projeto as seguintes tecnologias:
* Backend DOTNET:
  *	Serviços API
  *	DDD  
  *	CQRS
  *	EDD  
  *	MediatR
  *	Regis 
  *	ElasticSeach
  *	GraphQL
  *	Entity
 
# Rodar no Dokcer:

ElasticSeach:
docker run -d --name elasticsearch -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "ES_JAVA_OPTS=-Xms512m -Xmx512m" -e "xpack.security.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:8.11.1

docker pull docker.elastic.co/elasticsearch/elasticsearch:8.11.1

Redis:
docker run -d -p 6379:6379 redis

Postgres:
docker run -d --name postgres-apitest -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=root -e POSTGRES_DB=apitest -p 5432:5432 postgres:15

DotNet migration:
dotnet ef migrations add InitialCreate
dotnet ef database update

# Usando com GraphQL

Url: https://localhost:7105/api/v1/graphql/

Exemplos:
mutation {
  createProduct(name: "Teste", price: 11) {
    id
    name
    price
  }
}

mutation {
  updateProduct(
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
    name: "Updated Product Name"
    price: 99.99
  ) {
    id
    name
    price
  }
}

Busca do banco de dados
query {
  getProductById (
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
  ) {
    id
    name
    price
  }
}

Busca ElasticSeach
query {
  getElasticProductById (
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
  ) {
    id
    name
    price
  }
}

Busca ElasticSeach
query {
  getElasticProductsByName (
    name: "aa"
  ) {
    id
    name
    price
  }
}

Busca Banco
query {
  getProductsByName (
    name: "aa"
  ) {
    id
    name
    price
  }
}

## TODO

- add getall com paginação
- add frontend em angular, blazor e nesxtjs
- add signalr
- revisar arquitetura 