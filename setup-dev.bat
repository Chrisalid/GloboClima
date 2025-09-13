@echo off
echo 🌍 Configurando ambiente de desenvolvimento do GloboClima...

REM Verificar se .NET 8 está instalado
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ .NET 8 SDK não encontrado. Por favor, instale o .NET 8 SDK primeiro.
    echo 📥 Download: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo ✅ .NET SDK encontrado
dotnet --version

REM Verificar se AWS CLI está instalado
aws --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ⚠️  AWS CLI não encontrado. Por favor, instale o AWS CLI primeiro.
    echo 📥 Download: https://aws.amazon.com/cli/
    pause
    exit /b 1
)

echo ✅ AWS CLI encontrado
aws --version

REM Restaurar dependências
echo 📦 Restaurando dependências do projeto...
dotnet restore

REM Construir o projeto
echo 🔨 Construindo o projeto...
dotnet build --configuration Release

REM Executar testes
echo 🧪 Executando testes...
dotnet test --configuration Release --verbosity normal

REM Criar arquivo de configuração local
echo ⚙️  Criando arquivo de configuração local...
(
echo {
echo   "Logging": {
echo     "LogLevel": {
echo       "Default": "Information",
echo       "Microsoft.AspNetCore": "Warning"
echo     }
echo   },
echo   "AllowedHosts": "*",
echo   "Jwt": {
echo     "Key": "Esta_e_uma_chave_super_secreta_que_deve_ter_pelo_menos_256_bits_para_ser_segura_HMACSHA256",
echo     "Issuer": "GloboClima",
echo     "Audience": "GloboClima"
echo   },
echo   "OpenWeatherMap": {
echo     "ApiKey": "YOUR_OPENWEATHERMAP_API_KEY_HERE"
echo   },
echo   "AWS": {
echo     "Region": "us-east-1"
echo   }
echo }
) > src\GloboClima.API\appsettings.Development.json

echo.
echo 🔑 Por favor, configure sua chave da API OpenWeatherMap em:
echo    src\GloboClima.API\appsettings.Development.json
echo.
echo 📋 Próximos passos:
echo    1. Obtenha uma chave gratuita em: https://openweathermap.org/api
echo    2. Configure suas credenciais AWS com: aws configure
echo    3. Execute a API: cd src\GloboClima.API ^&^& dotnet run
echo    4. Execute o Web: cd src\GloboClima.Web ^&^& dotnet run
echo.
echo 🎉 Configuração concluída! Happy coding! 🚀
pause
