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
  *	Redis 
  *	ElasticSeach
  *	GraphQL
  *	Entity
 
# Projeto

* Feito no padrão DDD, CQRS, EDD. 
* Quando executado algum command é disparado evento notificando ação no elastic.  
* Controlers separados para API e GraphQL  
* Exemplificado insert, udpdate e delete com graphQL.  
* Exemplicicado chamada com GraphQL para Query separadas com banco de dados e Elastic
 
# Rodar no Dokcer:

* ElasticSeach:  
docker run -d --name elasticsearch -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "ES_JAVA_OPTS=-Xms512m -Xmx512m" -e "xpack.security.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:8.11.1  

docker pull docker.elastic.co/elasticsearch/elasticsearch:8.11.1  

* Redis:  
docker run -d -p 6379:6379 redis

* Postgres:  
docker run -d --name postgres-apitest -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=root -e POSTGRES_DB=apitest -p 5432:5432 postgres:15  

* DotNet migration:  
dotnet ef migrations add InitialCreate  
dotnet ef database update  

# Usando com GraphQL  

Url: https://localhost:7105/api/v1/graphql/  

* Exemplos:

Insert usando graphql:     
mutation {
  createProduct(name: "Teste", price: 11) { 
    id
    name
    price
  }
}

Update usando graphql:    
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

Delete usando graphql:    
mutation {
  deleteProduct(
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
  ) 
}

Busca no banco product by id:  
query {
  getProductById (
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
  ) {
    id
    name
    price
  }
}  

Busca no Ellastic product by id:  
query {
  getElasticProductById (
    id: "01956213-1264-73a1-93ab-35f93a929cf8"
  ) {
    id
    name
    price
  }
}  

Busca no Ellastic product by name:   
query {
  getElasticProductsByName (
    name: "aa"
  ) {
    id
    name
    price
  }
}  

Busca no banco product by name:   
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
