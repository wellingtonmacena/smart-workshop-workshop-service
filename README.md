# Smart Workshop - Workshop Service

## Visão Geral

O Workshop Service é responsável por gerenciar ordens de serviço, veículos, clientes e serviços disponíveis na oficina mecânica inteligente. Este microservço segue os princípios de Clean Architecture e CQRS.

## Arquitetura

O projeto está organizado em 4 camadas principais:

### 1. Domain (SmartWorkshop.Workshop.Domain)

- **Entities**: Entidades do domínio (ServiceOrder, Vehicle, Person, etc.)
- **ValueObjects**: Objetos de valor (Email, Phone, Address, etc.)
- **States**: Implementação do padrão State para ServiceOrder
- **DTOs**: Data Transfer Objects para comunicação entre camadas
- **Common**: Classes compartilhadas (Response, Paginate, DomainException, etc.)

### 2. Application (SmartWorkshop.Workshop.Application)

- **UseCases**: Implementação de casos de uso usando CQRS com MediatR
  - Commands: Operações que modificam o estado
  - Queries: Operações de leitura
  - Handlers: Processadores dos Commands e Queries
- **Adapters**: Interfaces de repositórios e serviços
- **Mappers**: Configuração do AutoMapper

### 3. Infrastructure (SmartWorkshop.Workshop.Infrastructure)

- **Data**: DbContext e configurações do Entity Framework
- **Repositories**: Implementação dos repositórios
- **Configurations**: Mapeamento de entidades para o banco

### 4. API (SmartWorkshop.Workshop.Api)

- **Controllers**: Endpoints HTTP
- **Consumers**: Consumidores de eventos do MassTransit
- **Extensions**: Configurações e extensões

## Principais Funcionalidades Implementadas

### UseCases Implementados

#### ServiceOrders

- `CreateServiceOrderCommand` - Criar nova ordem de serviço
- `GetServiceOrderByIdQuery` - Buscar ordem de serviço por ID
- `ListServiceOrdersQuery` - Listar ordens de serviço (paginado)

#### Vehicles

- `CreateVehicleCommand` - Criar novo veículo
- `GetVehicleByIdQuery` - Buscar veículo por ID
- `UpdateVehicleCommand` - Atualizar veículo

#### People

- `CreatePersonCommand` - Criar nova pessoa (cliente)

#### AvailableServices

- `CreateAvailableServiceCommand` - Criar novo serviço disponível

#### Supplies

- `CreateSupplyCommand` - Criar novo suprimento

## Tecnologias Utilizadas

- **.NET 9.0**
- **Entity Framework Core** - ORM para PostgreSQL
- **MediatR** - Implementação de CQRS
- **AutoMapper** - Mapeamento objeto-objeto
- **FluentResults** - Tratamento de resultados
- **Serilog** - Logging estruturado
- **MassTransit** - Message Bus com RabbitMQ
- **Swagger/OpenAPI** - Documentação da API

## Configuração

### Pré-requisitos

- .NET 9.0 SDK
- PostgreSQL
- RabbitMQ (para mensageria)
- MongoDB (para logs de diagnóstico - opcional)

### Banco de Dados

Configure a connection string no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=workshop_db;Username=workshop_user;Password=workshop_pass"
  }
}
```

### Executando o Projeto

```bash
# Restaurar dependências
dotnet restore

# Executar migrações (se necessário)
dotnet ef database update --project SmartWorkshop.Workshop.Infrastructure --startup-project SmartWorkshop.Workshop.Api

# Executar o projeto
dotnet run --project SmartWorkshop.Workshop.Api
```

O serviço estará disponível em:

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`

## Endpoints Principais

### Service Orders

```
POST   /api/v1/serviceorders          - Criar ordem de serviço
GET    /api/v1/serviceorders/{id}     - Buscar ordem de serviço
GET    /api/v1/serviceorders           - Listar ordens de serviço
```

### Vehicles

```
POST   /api/v1/vehicles                - Criar veículo
GET    /api/v1/vehicles/{id}           - Buscar veículo
PUT    /api/v1/vehicles/{id}           - Atualizar veículo
```

## Eventos de Integração

### Publicados

- `ServiceOrderCreatedIntegrationEvent` - Quando uma ordem de serviço é criada
- `WorkCompletedIntegrationEvent` - Quando o trabalho é concluído

### Consumidos

- `QuoteApprovedConsumer` - Quando um orçamento é aprovado
- `PaymentConfirmedConsumer` - Quando um pagamento é confirmado

## Padrões Implementados

1. **Clean Architecture**: Separação clara de responsabilidades
2. **CQRS**: Separação de comandos e consultas
3. **Repository Pattern**: Abstração de acesso a dados
4. **State Pattern**: Gerenciamento de estados da ordem de serviço
5. **Dependency Injection**: Injeção de dependências nativa do .NET
6. **Response Pattern**: Tratamento consistente de respostas

## Health Checks

O serviço expõe um endpoint de health check:

```
GET /health
```

## Estrutura de Response

Todas as respostas seguem o padrão `Response<T>`:

```csharp
{
  "isSuccess": true,
  "data": { ... },
  "reasons": []
}
```

Em caso de erro:

```csharp
{
  "isSuccess": false,
  "data": null,
  "reasons": [
    {
      "message": "Error message"
    }
  ]
}
```

## Próximos Passos

- [ ] Implementar autenticação e autorização
- [ ] Adicionar validações com FluentValidation
- [ ] Implementar testes unitários e de integração
- [ ] Configurar MongoDB para logs de diagnóstico
- [ ] Implementar caching com Redis
- [ ] Adicionar métricas e observabilidade
- [ ] Implementar rate limiting
- [ ] Adicionar mais UseCases (Update, Delete, etc.)

## Contribuindo

1. Siga os princípios SOLID
2. Mantenha a separação de responsabilidades
3. Escreva testes para novos recursos
4. Documente APIs públicas
5. Use conventional commits

## Licença

Este projeto faz parte do Tech Challenge FIAP - Pós-graduação em Arquitetura de Software.
