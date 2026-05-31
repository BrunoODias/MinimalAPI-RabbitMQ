```md
# Product API + RabbitMQ Worker

Projeto de estudo desenvolvido para praticar:

- Minimal APIs
- RabbitMQ
- Event Driven Architecture
- Entity Framework Core
- Background Workers

Projeto de estudo desenvolvido em .NET 7 utilizando:

- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- RabbitMQ
- Worker Service
- Docker

## Arquitetura

A aplica��o � composta por dois projetos principais:

### Product.Api

Respons�vel pelas opera��es CRUD de produtos.

Ao realizar opera��es de cria��o, edi��o ou exclus�o de produtos, eventos s�o publicados no RabbitMQ.

### Products.Worker

Consumidor RabbitMQ respons�vel por processar os eventos recebidos e registrar logs das opera��es no banco de dados.

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

---

## Pr�-requisitos

Instalar:

- .NET SDK 7.0
- Docker Desktop
- Git

Verificar instala��o:

```bash
dotnet --version
docker --version
```

---

## Clonando o projeto

```bash
git clone https://github.com/BrunoODias/MinimalAPI-RabbitMQ
cd MinimalAPI-C-
```

---

## Configurando RabbitMQ

Subir container RabbitMQ com Management UI:

```bash
docker run -d ^
--hostname rabbitmq ^
--name rabbitmq ^
-p 5672:5672 ^
-p 15672:15672 ^
rabbitmq:3-management
```

Linux/Mac:

```bash
docker run -d \
--hostname rabbitmq \
--name rabbitmq \
-p 5672:5672 \
-p 15672:15672 \
rabbitmq:3-management
```

Acessar painel:

http://localhost:15672

Usu�rio:

```text
guest
```

Senha:

```text
guest
```

---

## Restaurando depend�ncias

Na raiz da solu��o:

```bash
dotnet restore
```

---

## Criando banco de dados

Navegar at� o projeto da API:

```bash
cd Product.Api
```

Aplicar migrations:

```bash
dotnet ef database update
```

Ser� criado o banco SQLite utilizado pela aplica��o.

---

## Executando a API

Na pasta Product.Api:

```bash
dotnet run
```

Swagger:

```text
https://localhost:xxxx/swagger
```

---

## Executando o Worker

Abrir outro terminal.

Na pasta Products.Worker:

```bash
dotnet run
```

O worker ficar� escutando eventos do RabbitMQ.

---

## Testando

Criar um produto via Swagger:

```http
POST /product
```

Exemplo:

```json
{
  "name": "Feij�o",
  "price": 10.5
}
```

Ap�s a cria��o:

1. O produto ser� salvo no SQLite
2. Um evento ser� publicado no RabbitMQ
3. O Worker consumir� o evento
4. Um log ser� salvo na tabela ProductLogs

---

## Tecnologias Utilizadas

- .NET 7
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- RabbitMQ
- Docker

---

## Objetivo do Projeto

Projeto desenvolvido para estudo de:

- Mensageria com RabbitMQ
- Arquitetura orientada a eventos
- Background Workers
- Minimal APIs
- Entity Framework Core
- Integra��o entre servi�os