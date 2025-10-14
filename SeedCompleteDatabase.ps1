# ============================================================================
# MechaSoft - Complete Database Seed Script
# Alimenta a base de dados Azure SQL com dados completos de teste
# ============================================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  MechaSoft - Complete Seed Database" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

$apiUrl = "http://localhost:5039"
$ErrorActionPreference = "Continue"

# ============================================================================
# FUNCOES AUXILIARES
# ============================================================================

function Test-ApiHealth {
    try {
        $health = Invoke-RestMethod -Uri "$apiUrl/health" -Method Get -ErrorAction Stop -TimeoutSec 5
        return $true
    } catch {
        return $false
    }
}

function Invoke-ApiPost {
    param(
        [string]$Endpoint,
        [hashtable]$Body,
        [string]$EntityName
    )
    
    $jsonBody = $Body | ConvertTo-Json -Depth 10
    
    try {
        $response = Invoke-RestMethod -Uri "$apiUrl$Endpoint" -Method Post -Body $jsonBody -ContentType "application/json" -ErrorAction Stop
        Write-Host "   [OK] $EntityName criado com sucesso!" -ForegroundColor Green
        return $response
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        if ($statusCode -eq 409) {
            Write-Host "   [SKIP] $EntityName ja existe" -ForegroundColor Yellow
        } else {
            Write-Host "   [ERRO] $EntityName - Status: $statusCode" -ForegroundColor Red
        }
        return $null
    }
}

# ============================================================================
# VERIFICAR API
# ============================================================================

Write-Host "[1/7] Verificando API..." -ForegroundColor Cyan
if (Test-ApiHealth) {
    Write-Host "      API esta online em $apiUrl" -ForegroundColor Green
} else {
    Write-Host "      ERRO: API nao esta rodando!" -ForegroundColor Red
    Write-Host "      Execute: cd MechaSoft.WebAPI; dotnet run" -ForegroundColor Yellow
    exit 1
}

# ============================================================================
# [2/7] CRIAR USUARIOS
# ============================================================================

Write-Host ""
Write-Host "[2/7] Criando Usuarios..." -ForegroundColor Cyan

# Admin
Invoke-ApiPost -Endpoint "/api/accounts/register" -Body @{
    username = "admin"
    email = "admin@mechasoft.pt"
    password = "Admin123!"
    role = "Owner"
} -EntityName "Admin (Owner)"

# Cliente Maria Santos
Invoke-ApiPost -Endpoint "/api/accounts/register" -Body @{
    username = "maria_santos"
    email = "maria.santos@email.pt"
    password = "Customer123!"
    role = "Customer"
} -EntityName "Maria Santos (Customer)"

Write-Host "      Total: 2 usuarios" -ForegroundColor White

# ============================================================================
# [3/7] CRIAR CLIENTES
# ============================================================================

Write-Host ""
Write-Host "[3/7] Criando Clientes..." -ForegroundColor Cyan

$customer1 = Invoke-ApiPost -Endpoint "/api/customers" -Body @{
    name = "Joao Silva"
    email = "joao.silva@email.pt"
    phone = "+351912345678"
    nif = "123456789"
    street = "Rua das Flores"
    number = "123"
    parish = "Santa Maria Maior"
    city = "Lisboa"
    postalCode = "1100-200"
    country = "Portugal"
} -EntityName "Cliente: Joao Silva"

$customer2 = Invoke-ApiPost -Endpoint "/api/customers" -Body @{
    name = "Ana Costa"
    email = "ana.costa@email.pt"
    phone = "+351923456789"
    nif = "987654321"
    street = "Avenida da Republica"
    number = "456"
    parish = "Arroios"
    city = "Lisboa"
    postalCode = "1050-100"
    country = "Portugal"
} -EntityName "Cliente: Ana Costa"

$customer3 = Invoke-ApiPost -Endpoint "/api/customers" -Body @{
    name = "Pedro Oliveira"
    email = "pedro.oliveira@email.pt"
    phone = "+351934567890"
    nif = "456789123"
    street = "Praca do Comercio"
    number = "78"
    parish = "Baixa"
    city = "Porto"
    postalCode = "4000-100"
    country = "Portugal"
} -EntityName "Cliente: Pedro Oliveira"

Write-Host "      Total: 3 clientes" -ForegroundColor White

# ============================================================================
# [4/7] CRIAR FUNCIONARIOS
# ============================================================================

Write-Host ""
Write-Host "[4/7] Criando Funcionarios..." -ForegroundColor Cyan

$emp1 = Invoke-ApiPost -Endpoint "/api/employees" -Body @{
    firstName = "Carlos"
    lastName = "Ferreira"
    email = "carlos.ferreira@mechasoft.pt"
    phone = "+351911111111"
    role = "Mechanic"
    hourlyRate = 25.00
    specialties = @("Engine", "Transmission", "Brakes")
    canPerformInspections = $true
    inspectionLicenseNumber = "INS-2024-001"
} -EntityName "Funcionario: Carlos Ferreira (Mecanico)"

$emp2 = Invoke-ApiPost -Endpoint "/api/employees" -Body @{
    firstName = "Rita"
    lastName = "Santos"
    email = "rita.santos@mechasoft.pt"
    phone = "+351922222222"
    role = "Receptionist"
    hourlyRate = 15.00
    specialties = @()
    canPerformInspections = $false
} -EntityName "Funcionario: Rita Santos (Rececionista)"

$emp3 = Invoke-ApiPost -Endpoint "/api/employees" -Body @{
    firstName = "Miguel"
    lastName = "Alves"
    email = "miguel.alves@mechasoft.pt"
    phone = "+351933333333"
    role = "PartsClerk"
    hourlyRate = 18.00
    specialties = @()
    canPerformInspections = $false
} -EntityName "Funcionario: Miguel Alves (Responsavel Pecas)"

Write-Host "      Total: 3 funcionarios" -ForegroundColor White

# ============================================================================
# [5/7] CRIAR VEICULOS
# ============================================================================

Write-Host ""
Write-Host "[5/7] Criando Veiculos..." -ForegroundColor Cyan

# Precisamos dos IDs dos clientes - vamos buscar
try {
    $customersResponse = Invoke-RestMethod -Uri "$apiUrl/api/customers?pageNumber=1&pageSize=10" -Method Get
    $customers = $customersResponse.customers
    
    if ($customers.Count -gt 0) {
        # Veiculo 1 - Joao Silva
        $cust1Id = $customers | Where-Object { $_.email -eq "joao.silva@email.pt" } | Select-Object -ExpandProperty id
        if ($cust1Id) {
            Invoke-ApiPost -Endpoint "/api/vehicles" -Body @{
                customerId = $cust1Id
                brand = "BMW"
                model = "320d"
                licensePlate = "AA-12-BB"
                color = "Preto"
                year = 2020
                vin = "WBA1234567890"
                engineType = "2.0 TDI"
                fuelType = "Diesel"
            } -EntityName "Veiculo: BMW 320d"
        }
        
        # Veiculo 2 - Ana Costa
        $cust2Id = $customers | Where-Object { $_.email -eq "ana.costa@email.pt" } | Select-Object -ExpandProperty id
        if ($cust2Id) {
            Invoke-ApiPost -Endpoint "/api/vehicles" -Body @{
                customerId = $cust2Id
                brand = "Mercedes"
                model = "C200"
                licensePlate = "CC-34-DD"
                color = "Branco"
                year = 2021
                vin = "WDB1234567890"
                engineType = "2.0"
                fuelType = "Gasoline"
            } -EntityName "Veiculo: Mercedes C200"
        }
        
        # Veiculo 3 - Pedro Oliveira
        $cust3Id = $customers | Where-Object { $_.email -eq "pedro.oliveira@email.pt" } | Select-Object -ExpandProperty id
        if ($cust3Id) {
            Invoke-ApiPost -Endpoint "/api/vehicles" -Body @{
                customerId = $cust3Id
                brand = "Renault"
                model = "Clio"
                licensePlate = "EE-56-FF"
                color = "Vermelho"
                year = 2019
                vin = "VF11234567890"
                engineType = "1.5 DCI"
                fuelType = "Diesel"
            } -EntityName "Veiculo: Renault Clio"
        }
        
        Write-Host "      Total: 3 veiculos" -ForegroundColor White
    }
} catch {
    Write-Host "      [ERRO] Nao foi possivel criar veiculos" -ForegroundColor Red
}

# ============================================================================
# [6/7] CRIAR SERVICOS
# ============================================================================

Write-Host ""
Write-Host "[6/7] Criando Servicos..." -ForegroundColor Cyan

Invoke-ApiPost -Endpoint "/api/services" -Body @{
    name = "Mudanca de Oleo"
    description = "Mudanca de oleo do motor com filtro"
    category = "Maintenance"
    estimatedHours = 0.5
    pricePerHour = 30.00
    fixedPrice = 45.00
    requiresInspection = $false
} -EntityName "Servico: Mudanca de Oleo"

Invoke-ApiPost -Endpoint "/api/services" -Body @{
    name = "Substituicao de Pastilhas de Travao"
    description = "Substituicao completa de pastilhas de travao dianteiras"
    category = "Brakes"
    estimatedHours = 1.5
    pricePerHour = 35.00
    fixedPrice = $null
    requiresInspection = $true
} -EntityName "Servico: Pastilhas de Travao"

Invoke-ApiPost -Endpoint "/api/services" -Body @{
    name = "Diagnostico Eletronico"
    description = "Diagnostico completo com scanner OBD"
    category = "Diagnostic"
    estimatedHours = 1.0
    pricePerHour = 40.00
    fixedPrice = 40.00
    requiresInspection = $false
} -EntityName "Servico: Diagnostico Eletronico"

Invoke-ApiPost -Endpoint "/api/services" -Body @{
    name = "Alinhamento e Equilibragem"
    description = "Alinhamento de direcao e equilibragem de rodas"
    category = "Tires"
    estimatedHours = 1.0
    pricePerHour = 30.00
    fixedPrice = 35.00
    requiresInspection = $false
} -EntityName "Servico: Alinhamento"

Invoke-ApiPost -Endpoint "/api/services" -Body @{
    name = "Revisao Periodica"
    description = "Revisao geral do veiculo conforme manual"
    category = "Maintenance"
    estimatedHours = 2.0
    pricePerHour = 30.00
    fixedPrice = $null
    requiresInspection = $true
} -EntityName "Servico: Revisao Periodica"

Write-Host "      Total: 5 servicos" -ForegroundColor White

# ============================================================================
# [7/7] CRIAR PECAS
# ============================================================================

Write-Host ""
Write-Host "[7/7] Criando Pecas..." -ForegroundColor Cyan

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "OIL-001"
    name = "Oleo Motor 5W30"
    description = "Oleo sintetico 5W30 para motores diesel e gasolina"
    category = "Lubrificantes"
    brand = "Castrol"
    unitCost = 8.50
    salePrice = 15.00
    stockQuantity = 50
    minStockLevel = 10
    supplierName = "Auto Parts Ltd"
    supplierContact = "fornecedor@autoparts.pt"
    location = "Prateleira A1"
} -EntityName "Peca: Oleo Motor 5W30"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "FILTER-001"
    name = "Filtro de Oleo"
    description = "Filtro de oleo universal"
    category = "Filtros"
    brand = "Bosch"
    unitCost = 5.00
    salePrice = 10.00
    stockQuantity = 100
    minStockLevel = 20
    supplierName = "Auto Parts Ltd"
    supplierContact = "fornecedor@autoparts.pt"
    location = "Prateleira A2"
} -EntityName "Peca: Filtro de Oleo"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "BRAKE-001"
    name = "Pastilhas de Travao Dianteiras"
    description = "Conjunto de pastilhas para travao dianteiro"
    category = "Travoes"
    brand = "Brembo"
    unitCost = 35.00
    salePrice = 65.00
    stockQuantity = 30
    minStockLevel = 5
    supplierName = "Brake Systems SA"
    supplierContact = "vendas@brakesystems.pt"
    location = "Prateleira B1"
} -EntityName "Peca: Pastilhas Travao"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "TIRE-001"
    name = "Pneu 205/55R16"
    description = "Pneu de verao 205/55R16"
    category = "Pneus"
    brand = "Michelin"
    unitCost = 60.00
    salePrice = 110.00
    stockQuantity = 24
    minStockLevel = 8
    supplierName = "Tire Center"
    supplierContact = "comercial@tirecenter.pt"
    location = "Armazem Pneus"
} -EntityName "Peca: Pneu 205/55R16"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "BATTERY-001"
    name = "Bateria 12V 70Ah"
    description = "Bateria de chumbo-acido 12V 70Ah"
    category = "Eletrico"
    brand = "Varta"
    unitCost = 70.00
    salePrice = 120.00
    stockQuantity = 15
    minStockLevel = 3
    supplierName = "Electric Auto Parts"
    supplierContact = "info@electricauto.pt"
    location = "Prateleira C1"
} -EntityName "Peca: Bateria 12V"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "FILTER-002"
    name = "Filtro de Ar"
    description = "Filtro de ar do motor"
    category = "Filtros"
    brand = "Mann Filter"
    unitCost = 8.00
    salePrice = 15.00
    stockQuantity = 80
    minStockLevel = 15
    supplierName = "Auto Parts Ltd"
    supplierContact = "fornecedor@autoparts.pt"
    location = "Prateleira A3"
} -EntityName "Peca: Filtro de Ar"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "FILTER-003"
    name = "Filtro de Combustivel"
    description = "Filtro de combustivel diesel/gasolina"
    category = "Filtros"
    brand = "Bosch"
    unitCost = 12.00
    salePrice = 22.00
    stockQuantity = 60
    minStockLevel = 12
    supplierName = "Auto Parts Ltd"
    supplierContact = "fornecedor@autoparts.pt"
    location = "Prateleira A4"
} -EntityName "Peca: Filtro de Combustivel"

Invoke-ApiPost -Endpoint "/api/parts" -Body @{
    code = "SPARK-001"
    name = "Velas de Ignicao"
    description = "Conjunto de 4 velas de ignicao"
    category = "Motor"
    brand = "NGK"
    unitCost = 15.00
    salePrice = 28.00
    stockQuantity = 40
    minStockLevel = 10
    supplierName = "Engine Parts Pro"
    supplierContact = "vendas@engineparts.pt"
    location = "Prateleira D1"
} -EntityName "Peca: Velas de Ignicao"

Write-Host "      Total: 8 pecas" -ForegroundColor White

# ============================================================================
# RESUMO FINAL
# ============================================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  SEED COMPLETO COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Dados criados:" -ForegroundColor Cyan
Write-Host "  - 2 Usuarios (admin, maria_santos)" -ForegroundColor White
Write-Host "  - 3 Clientes" -ForegroundColor White
Write-Host "  - 3 Funcionarios" -ForegroundColor White
Write-Host "  - 3 Veiculos" -ForegroundColor White
Write-Host "  - 5 Servicos" -ForegroundColor White
Write-Host "  - 8 Pecas em stock" -ForegroundColor White
Write-Host ""
Write-Host "Credenciais de acesso:" -ForegroundColor Cyan
Write-Host "  Admin:    admin@mechasoft.pt     / Admin123!" -ForegroundColor Yellow
Write-Host "  Cliente:  maria.santos@email.pt  / Customer123!" -ForegroundColor Yellow
Write-Host ""
Write-Host "URLs:" -ForegroundColor Cyan
Write-Host "  Frontend: http://localhost:4200" -ForegroundColor White
Write-Host "  API:      http://localhost:5039" -ForegroundColor White
Write-Host "  Swagger:  http://localhost:5039/swagger" -ForegroundColor White
Write-Host ""
Write-Host "Database:" -ForegroundColor Cyan
Write-Host "  Server:   mechasoft-server-2025.database.windows.net" -ForegroundColor White
Write-Host "  Database: DV_RO_MechaSoft" -ForegroundColor White
Write-Host "  User:     mechasoft_admin" -ForegroundColor White
Write-Host ""

