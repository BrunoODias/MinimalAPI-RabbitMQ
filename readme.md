# Product API + RabbitMQ Worker

Projeto de estudo desenvolvido para praticar:

- Minimal APIs
- RabbitMQ
- Event Driven Architecture
- Entity Framework Core
- Background Workers

## Tecnologias Utilizadas

- .NET 7
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- RabbitMQ
- Worker Service
- Docker

## Arquitetura

A aplicação é composta por dois projetos principais.

### Product.Api

Responsável pelas operações CRUD de produtos.

Ao realizar operações de criação, edição ou exclusão de produtos, eventos são publicados no RabbitMQ.

### Products.Worker

Consumidor RabbitMQ responsável por processar os eventos recebidos e registrar logs das operações no banco de dados.

## Fluxo

```text
Cliente
    |
    v
Product.Api
    |
    v
RabbitMQ
    |
    v
Products.Worker
    |
    v
ProductLogs (SQLite)
```

## Pré-requisitos

- .NET SDK 7.0
- Docker Desktop
- Git

## Clonando o projeto

```bash
git clone https://github.com/BrunoODias/MinimalAPI-RabbitMQ
cd MinimalAPI-RabbitMQ
```

## Configurando RabbitMQ

### Windows

```bash
docker run -d ^
--hostname rabbitmq ^
--name rabbitmq ^
-p 5672:5672 ^
-p 15672:15672 ^
rabbitmq:3-management
```

### Linux / Mac

```bash
docker run -d \
--hostname rabbitmq \
--name rabbitmq \
-p 5672:5672 \
-p 15672:15672 \
rabbitmq:3-management
```

Painel:

```text
http://localhost:15672
```

Usuário:

```text
guest
```

Senha:

```text
guest
```

## Restaurando dependências

```bash
dotnet restore
```

## Criando banco de dados

```bash
cd Product.Api
dotnet ef database update
```

## Executando a API

```bash
dotnet run
```

Swagger:

```text
https://localhost:xxxx/swagger
```

## Executando o Worker

```bash
cd Products.Worker
dotnet run
```

## Testando

```http
POST /product
```

```json
{
  "name": "Feijão",
  "price": 10.5
}
```

Após a criação:

1. O produto será salvo no SQLite.
2. Um evento será publicado no RabbitMQ.
3. O Worker consumirá o evento.
4. Um log será salvo na tabela ProductLogs.

## Melhorias Futuras

- Autenticação JWT
- Testes unitários
- Retry de mensagens
- Dead Letter Queue (DLQ)
- Logging com Serilog
- Cache com Redis
- Docker Compose