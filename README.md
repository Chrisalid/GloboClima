# GloboClima - Sistema de Clima e Países Favoritos

## Descrição

O GloboClima é uma aplicação fullstack desenvolvida com .NET 8 que permite aos usuários consultar informações climáticas e dados de países, além de salvar seus favoritos para futuras consultas. A aplicação integra APIs públicas (OpenWeatherMap e REST Countries) e oferece funcionalidades completas de autenticação com JWT.

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

- **GloboClima.Core**: Entidades, DTOs e interfaces
- **GloboClima.Infrastructure**: Repositórios, serviços e integrações externas
- **GloboClima.API**: API REST com documentação Swagger
- **GloboClima.Web**: Interface web responsiva desenvolvida em Blazor Server
- **GloboClima.Tests**: Testes unitários e de integração

## 🚀 Funcionalidades

### Backend (API REST)
- ✅ Consumo de APIs públicas (OpenWeatherMap e REST Countries)
- ✅ Autenticação JWT com registro e login
- ✅ CRUD de cidades e países favoritos
- ✅ Documentação completa com Swagger
- ✅ Arquitetura preparada para AWS Lambda/ECS

### Frontend (Blazor Server)
- ✅ Interface responsiva e moderna
- ✅ Consulta de clima por cidade
- ✅ Exploração de dados de países
- ✅ Gerenciamento de favoritos
- ✅ Sistema de autenticação integrado

### Segurança
- ✅ Autenticação JWT
- ✅ Proteção de rotas sensíveis
- ✅ Configuração HTTPS
- ✅ Hash seguro de senhas com BCrypt

## 🛠️ Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Blazor Server**: Frontend interativo
- **AWS DynamoDB**: Banco de dados NoSQL
- **JWT**: Autenticação
- **Swagger**: Documentação da API
- **Bootstrap 5**: Framework CSS
- **xUnit**: Framework de testes
- **Moq**: Biblioteca para mocking

## 📋 Pré-requisitos

- .NET 8 SDK
- AWS CLI configurado (para DynamoDB)
- Chave da API OpenWeatherMap
- Visual Studio 2022 ou VS Code

## ⚙️ Configuração

### 1. Clonagem do Repositório
```bash
git clone <repository-url>
cd GloboClima
```

### 2. Configuração do AWS DynamoDB
Configure as credenciais AWS e crie as tabelas necessárias:

```bash
aws dynamodb create-table --table-name GloboClima-Users --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --billing-mode PAY_PER_REQUEST

aws dynamodb create-table --table-name GloboClima-FavoriteCities --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --billing-mode PAY_PER_REQUEST

aws dynamodb create-table --table-name GloboClima-FavoriteCountries --attribute-definitions AttributeName=Id,AttributeType=S --key-schema AttributeName=Id,KeyType=HASH --billing-mode PAY_PER_REQUEST
```

## 🏃‍♂️ Executando a Aplicação

### API (Backend)
```bash
cd src/GloboClima.API
dotnet run
```
A API estará disponível em: `https://localhost:7000`
Documentação Swagger: `https://localhost:7000/swagger`

### Web (Frontend)
```bash
cd src/GloboClima.Web
dotnet run
```
A aplicação web estará disponível em: `https://localhost:7001`

## 🧪 Executando Testes

```bash
cd tests/GloboClima.Tests
dotnet test
```

Para executar com cobertura de código:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📖 Documentação da API

A API possui documentação completa no Swagger, incluindo:

### Endpoints de Autenticação
- `POST /api/auth/login` - Autenticação de usuário
- `POST /api/auth/register` - Registro de novo usuário

### Endpoints de Clima
- `GET /api/weather/{cityName}` - Consulta clima por cidade
- `GET /api/weather/coordinates` - Consulta clima por coordenadas

### Endpoints de Países
- `GET /api/countries` - Lista todos os países
- `GET /api/countries/name/{countryName}` - Busca país por nome
- `GET /api/countries/code/{countryCode}` - Busca país por código

### Endpoints de Favoritos (Autenticado)
- `GET /api/favorites/cities` - Lista cidades favoritas
- `POST /api/favorites/cities` - Adiciona cidade aos favoritos
- `DELETE /api/favorites/cities/{id}` - Remove cidade dos favoritos
- `GET /api/favorites/countries` - Lista países favoritos
- `POST /api/favorites/countries` - Adiciona país aos favoritos
- `DELETE /api/favorites/countries/{id}` - Remove país dos favoritos

## 🏗️ Deploy na AWS

### Usando AWS Lambda
1. Instale o AWS Lambda Tools:
```bash
dotnet tool install -g Amazon.Lambda.Tools
```

2. Deploy da API:
```bash
cd src/GloboClima.API
dotnet lambda deploy-serverless
```

### Usando ECS/Fargate
1. Construa a imagem Docker:
```bash
docker build -t globoclima-api .
```

2. Configure o ECS e faça o deploy usando o AWS CLI ou Console.

## 🔧 Configurações de Produção

### Variáveis de Ambiente
```bash
export JWT_KEY="sua-chave-secreta-super-segura-com-256-bits"
export OPENWEATHERMAP_APIKEY="sua-chave-openweathermap"
export AWS_REGION="us-east-1"
```

### HTTPS
Configure certificados SSL/TLS apropriados para produção.

## 🧪 Testes

O projeto inclui:
- **Testes Unitários**: Controllers e Services
- **Testes de Integração**: Endpoints da API
- **Cobertura de Código**: Mínimo de 50% conforme especificado

### Executar todos os testes:
```bash
dotnet test --configuration Release --logger trx --collect:"XPlat Code Coverage"
```

## 🚀 CI/CD

### GitHub Actions
O projeto inclui workflows para:
- Build e teste automatizado
- Deploy para AWS
- Análise de código
- Cobertura de testes

### Pipeline AWS CodePipeline
Configure o pipeline para:
1. Source: GitHub Repository
2. Build: .NET 8 build
3. Test: Executar testes unitários
4. Deploy: AWS Lambda ou ECS

## 📊 Monitoramento

### AWS CloudWatch
- Logs da aplicação
- Métricas de performance
- Alertas configuráveis

### Application Insights
- Telemetria detalhada
- Rastreamento de dependências
- Análise de performance

## 🔒 Segurança

- **JWT Tokens**: Expiração em 24 horas
- **HTTPS Only**: Configuração obrigatória em produção
- **BCrypt**: Hash seguro de senhas
- **CORS**: Configuração adequada para produção
- **Rate Limiting**: Implementar para APIs públicas

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 👥 Equipe

- **Desenvolvedor**: Christopher Feitosa do Monte
- **Email**: christopherfeitosa@gmail.com

## 📞 Suporte

Para suporte técnico, entre em contato através:
- Email: christopherfeitosa@gmail.com
- Issues: GitHub Issues

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no repositório!
