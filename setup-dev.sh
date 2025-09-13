#!/bin/bash

# GloboClima Development Setup Script
echo "🌍 Configurando ambiente de desenvolvimento do GloboClima..."

# Verificar se .NET 8 está instalado
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 8 SDK não encontrado. Por favor, instale o .NET 8 SDK primeiro."
    echo "📥 Download: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "✅ .NET SDK encontrado: $(dotnet --version)"

# Verificar se AWS CLI está instalado
if ! command -v aws &> /dev/null; then
    echo "⚠️  AWS CLI não encontrado. Instalando..."
    curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
    unzip awscliv2.zip
    sudo ./aws/install
    rm -rf aws awscliv2.zip
fi

echo "✅ AWS CLI encontrado: $(aws --version)"

# Restaurar dependências
echo "📦 Restaurando dependências do projeto..."
dotnet restore

# Construir o projeto
echo "🔨 Construindo o projeto..."
dotnet build --configuration Release

# Executar testes
echo "🧪 Executando testes..."
dotnet test --configuration Release --verbosity normal

# Configurar DynamoDB Local (opcional)
echo "🗄️  Você deseja configurar DynamoDB Local para desenvolvimento? (y/n)"
read -r setup_dynamodb

if [ "$setup_dynamodb" = "y" ] || [ "$setup_dynamodb" = "Y" ]; then
    if ! command -v java &> /dev/null; then
        echo "❌ Java não encontrado. DynamoDB Local requer Java 8 ou superior."
    else
        echo "📥 Baixando DynamoDB Local..."
        mkdir -p dynamodb-local
        cd dynamodb-local
        curl -O https://s3.us-west-2.amazonaws.com/dynamodb-local/dynamodb_local_latest.tar.gz
        tar -xzf dynamodb_local_latest.tar.gz
        cd ..
        echo "✅ DynamoDB Local configurado. Execute 'npm run dynamodb' para iniciar."
    fi
fi

# Criar arquivo de configuração local
echo "⚙️  Criando arquivo de configuração local..."
cat > src/GloboClima.API/appsettings.Development.json << EOF
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "Esta_e_uma_chave_super_secreta_que_deve_ter_pelo_menos_256_bits_para_ser_segura_HMACSHA256",
    "Issuer": "GloboClima",
    "Audience": "GloboClima"
  },
  "OpenWeatherMap": {
    "ApiKey": "YOUR_OPENWEATHERMAP_API_KEY_HERE"
  },
  "AWS": {
    "Region": "us-east-1"
  }
}
EOF

echo "🔑 Por favor, configure sua chave da API OpenWeatherMap em:"
echo "   src/GloboClima.API/appsettings.Development.json"
echo ""
echo "📋 Próximos passos:"
echo "   1. Obtenha uma chave gratuita em: https://openweathermap.org/api"
echo "   2. Configure suas credenciais AWS com: aws configure"
echo "   3. Execute a API: cd src/GloboClima.API && dotnet run"
echo "   4. Execute o Web: cd src/GloboClima.Web && dotnet run"
echo ""
echo "🎉 Configuração concluída! Happy coding! 🚀"
