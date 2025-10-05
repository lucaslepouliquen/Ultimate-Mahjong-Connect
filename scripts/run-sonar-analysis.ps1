# Script PowerShell pour analyse SonarQube locale (gratuite)
param(
    [string]$ProjectKey = "ultimate-mahjong-connect",
    [string]$SonarUrl = "http://localhost:9000",
    [Parameter(Mandatory=$true)]
    [string]$SonarToken
)

Write-Host "🔍 Démarrage de l'analyse SonarQube locale..." -ForegroundColor Green

# Vérifier si SonarQube est accessible
try {
    $response = Invoke-WebRequest -Uri "$SonarUrl/api/system/status" -UseBasicParsing
    Write-Host "✅ SonarQube accessible" -ForegroundColor Green
} catch {
    Write-Host "❌ SonarQube non accessible. Démarrez-le avec: docker-compose -f docker-compose.sonarqube.yml up -d" -ForegroundColor Red
    exit 1
}

# Installation du scanner si nécessaire
if (!(Test-Path ".\.sonar\scanner\dotnet-sonarscanner.exe")) {
    Write-Host "📦 Installation du scanner SonarQube..." -ForegroundColor Yellow
    New-Item -Path ".\.sonar\scanner" -ItemType Directory -Force
    dotnet tool install dotnet-sonarscanner --tool-path .\.sonar\scanner
}

# Nettoyage
Write-Host "🧹 Nettoyage des artefacts précédents..." -ForegroundColor Yellow
dotnet clean

# Début de l'analyse
Write-Host "🚀 Début de l'analyse SonarQube..." -ForegroundColor Green
.\.sonar\scanner\dotnet-sonarscanner begin `
    /k:"$ProjectKey" `
    /d:sonar.host.url="$SonarUrl" `
    /d:sonar.token="$SonarToken" `
    /d:sonar.cs.opencover.reportsPaths="coverage.opencover.xml"

# Build du projet
Write-Host "🔨 Build du projet..." -ForegroundColor Yellow
dotnet build --no-incremental

# Tests avec couverture
Write-Host "🧪 Exécution des tests avec couverture..." -ForegroundColor Yellow
dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./TestResults/ --logger trx --settings coverlet.runsettings

# Fin de l'analyse
Write-Host "📊 Finalisation de l'analyse..." -ForegroundColor Green
.\.sonar\scanner\dotnet-sonarscanner end /d:sonar.token="$SonarToken"

Write-Host "✅ Analyse terminée ! Consultez les résultats sur: $SonarUrl" -ForegroundColor Green
Write-Host "🔗 URL directe: $SonarUrl/dashboard?id=$ProjectKey" -ForegroundColor Cyan
