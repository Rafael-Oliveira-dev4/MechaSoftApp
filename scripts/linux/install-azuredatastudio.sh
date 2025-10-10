#!/bin/bash

# MechaSoft - Install Azure Data Studio
# Script para instalar o Azure Data Studio no Linux

echo "📥 Baixando Azure Data Studio..."
wget https://go.microsoft.com/fwlink/?linkid=2282284 -O /tmp/azuredatastudio-linux.deb

echo "📦 Instalando Azure Data Studio..."
sudo dpkg -i /tmp/azuredatastudio-linux.deb

echo "🔧 Instalando dependências faltantes (se houver)..."
sudo apt-get install -f -y

echo "✅ Azure Data Studio instalado!"
echo ""
echo "🗄️  Informações de Conexão SQL Server:"
echo "   Host: localhost"
echo "   Porta: 1433"
echo "   Utilizador: sa"
echo "   Senha: CHANGE_ME_PASSWORD"
echo "   Base de Dados: DV_RO_MechaSoft"
echo ""
echo "Para abrir: azuredatastudio"

