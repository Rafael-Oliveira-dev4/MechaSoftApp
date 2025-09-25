#!/bin/bash

echo "🔨 Iniciando build completo do MechaSoft..."
echo "========================================"

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Função para exibir status
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Verificar se estamos no diretório correto
if [ ! -f "MechaSoft.sln" ]; then
    print_error "Este script deve ser executado no diretório raiz do projeto MechaSoft"
    exit 1
fi

# 1. Build do Backend (.NET)
echo ""
print_status "Iniciando build do backend (.NET)..."
echo "----------------------------------------"

if dotnet build --configuration Release --verbosity minimal; then
    print_success "Build do backend concluído com sucesso!"
else
    print_error "Falha no build do backend!"
    exit 1
fi

# 2. Build do Frontend (Angular)
echo ""
print_status "Iniciando build do frontend (Angular)..."
echo "------------------------------------------"

# Navegar para o diretório do Angular
cd Presentation/MechaSoft.Angular

# Verificar se o package.json existe
if [ ! -f "package.json" ]; then
    print_error "package.json não encontrado no diretório Angular"
    exit 1
fi

# Instalar dependências se necessário
print_status "Verificando dependências do Angular..."
if npm install --silent; then
    print_success "Dependências do Angular verificadas/instaladas"
else
    print_error "Falha ao instalar dependências do Angular"
    exit 1
fi

# Fazer build do Angular
print_status "Executando build de produção do Angular..."
if npm run build; then
    print_success "Build do frontend concluído com sucesso!"
else
    print_error "Falha no build do frontend!"
    exit 1
fi

# Voltar ao diretório raiz
cd ../..

echo ""
echo "========================================"
print_success "🎉 Build completo do MechaSoft finalizado com sucesso!"
echo ""
echo "📁 Artefatos gerados:"
echo "  • Backend: bin/Release/net8.0/"
echo "  • Frontend: Presentation/MechaSoft.Angular/dist/MechaSoft.Angular/"
echo ""
echo "🚀 Para executar a aplicação:"
echo "  • Backend: dotnet run --project MechaSoft.WebAPI"
echo "  • Frontend: cd Presentation/MechaSoft.Angular && npm start"
echo ""
