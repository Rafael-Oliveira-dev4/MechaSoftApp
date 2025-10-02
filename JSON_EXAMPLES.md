# 🚀 MechaSoft API - Exemplos JSON para Testes

## 📋 **Backend Rodando**

- **URL Base:** `http://localhost:5039`
- **Swagger UI:** `http://localhost:5039/swagger`
- **Health Check:** `http://localhost:5039/health`

---

## 🔐 **1. AUTENTICAÇÃO E CONTAS**

### 📝 **Registrar Usuário (Cliente)**

```json
POST /api/accounts/register
{
  "username": "joao_silva",
  "email": "joao.silva@email.com",
  "password": "MinhaSenh@123",
  "role": 1
}
```

### 📝 **Registrar Funcionário**

```json
POST /api/accounts/register
{
  "username": "admin_user",
  "email": "admin@mechasoft.pt",
  "password": "Admin@123",
  "role": 2
}
```

### 🔑 **Login**

```json
POST /api/accounts/login
{
  "username": "joao_silva",
  "password": "MinhaSenh@123"
}
```

### 👥 **Listar Usuários**

```json
GET /api/accounts/users?pageNumber=1&pageSize=10
```

---

## 👤 **2. CLIENTES**

### ➕ **Criar Cliente**

```json
POST /api/customers
{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "phone": "+351 912 345 678",
  "nif": "123456789",
  "street": "Rua das Flores",
  "number": "123",
  "parish": "Santo António",
  "city": "Lisboa",
  "postalCode": "1000-001"
}
```

### ➕ **Criar Cliente 2**

```json
POST /api/customers
{
  "name": "Maria Santos",
  "email": "maria.santos@email.com",
  "phone": "+351 923 456 789",
  "nif": "987654321",
  "street": "Avenida da Liberdade",
  "number": "456",
  "parish": "Marquês de Pombal",
  "city": "Lisboa",
  "postalCode": "1250-096"
}
```

### 📋 **Listar Clientes**

```json
GET /api/customers?pageNumber=1&pageSize=10&searchTerm=João
```

### 🔍 **Obter Cliente por ID**

```json
GET /api/customers/{id}
```

### ✏️ **Atualizar Cliente**

```json
PUT /api/customers/{id}
{
  "name": "João Silva Santos",
  "email": "joao.silva.santos@email.com",
  "phone": "+351 912 345 679",
  "nif": "123456789",
  "street": "Rua das Flores",
  "number": "125",
  "parish": "Santo António",
  "city": "Lisboa",
  "postalCode": "1000-001"
}
```

---

## 🚗 **3. VEÍCULOS**

### ➕ **Criar Veículo**

```json
POST /api/vehicles
{
  "licensePlate": "12-AB-34",
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2020,
  "vin": "1HGBH41JXMN109186",
  "color": "Branco",
  "fuelType": 1,
  "engineSize": 1.6,
  "customerId": "00000000-0000-0000-0000-000000000001"
}
```

### ➕ **Criar Veículo 2**

```json
POST /api/vehicles
{
  "licensePlate": "56-CD-78",
  "brand": "BMW",
  "model": "X3",
  "year": 2019,
  "vin": "WBAFR9C56DD123456",
  "color": "Preto",
  "fuelType": 2,
  "engineSize": 2.0,
  "customerId": "00000000-0000-0000-0000-000000000001"
}
```

### 📋 **Listar Veículos**

```json
GET /api/vehicles?pageNumber=1&pageSize=10
```

### 🔍 **Veículos do Cliente**

```json
GET /api/vehicles/customer/{customerId}
```

---

## 👷 **4. FUNCIONÁRIOS**

### ➕ **Criar Funcionário (Mecânico)**

```json
POST /api/employees
{
  "name": "Carlos Mecânico",
  "email": "carlos@mechasoft.pt",
  "phone": "+351 934 567 890",
  "role": 1,
  "specialty": "Motor",
  "hireDate": "2023-01-15T00:00:00Z",
  "salary": 1500.00,
  "isActive": true
}
```

### ➕ **Criar Administrador**

```json
POST /api/employees
{
  "name": "Ana Administradora",
  "email": "ana@mechasoft.pt",
  "phone": "+351 945 678 901",
  "role": 2,
  "specialty": "Gestão",
  "hireDate": "2023-01-01T00:00:00Z",
  "salary": 2000.00,
  "isActive": true
}
```

### 📋 **Listar Funcionários**

```json
GET /api/employees?pageNumber=1&pageSize=10
```

---

## 🔧 **5. PEÇAS**

### ➕ **Criar Peça**

```json
POST /api/parts
{
  "name": "Filtro de Óleo",
  "description": "Filtro de óleo para motor 1.6L",
  "category": 1,
  "brand": "Bosch",
  "partNumber": "BO-123456",
  "price": 25.50,
  "stockQuantity": 50,
  "minimumStock": 10,
  "supplier": "Auto Parts Ltd",
  "location": "A1-B2-C3"
}
```

### ➕ **Criar Peça 2**

```json
POST /api/parts
{
  "name": "Pastilhas de Travão",
  "description": "Pastilhas de travão dianteiras",
  "category": 2,
  "brand": "Brembo",
  "partNumber": "BR-789012",
  "price": 89.99,
  "stockQuantity": 25,
  "minimumStock": 5,
  "supplier": "Brake Systems Inc",
  "location": "B2-C3-D4"
}
```

### 📋 **Listar Peças**

```json
GET /api/parts?pageNumber=1&pageSize=10
```

---

## 🛠️ **6. SERVIÇOS**

### ➕ **Criar Serviço**

```json
POST /api/services
{
  "name": "Mudança de Óleo",
  "description": "Troca de óleo e filtro do motor",
  "category": 1,
  "estimatedHours": 1.5,
  "pricePerHour": 25.00,
  "fixedPrice": 45.00,
  "requiresInspection": false
}
```

### ➕ **Criar Serviço 2**

```json
POST /api/services
{
  "name": "Reparação de Travões",
  "description": "Substituição de pastilhas e discos de travão",
  "category": 2,
  "estimatedHours": 3.0,
  "pricePerHour": 30.00,
  "fixedPrice": null,
  "requiresInspection": true
}
```

### 📋 **Listar Serviços**

```json
GET /api/services?pageNumber=1&pageSize=10
```

---

## 📋 **7. ORDENS DE SERVIÇO**

### ➕ **Criar Ordem de Serviço**

```json
POST /api/service-orders
{
  "customerId": "00000000-0000-0000-0000-000000000001",
  "vehicleId": "00000000-0000-0000-0000-000000000003",
  "description": "Manutenção completa do veículo - mudança de óleo e inspeção",
  "priority": 1,
  "estimatedCost": 150.00,
  "estimatedDelivery": "2024-12-15T16:00:00Z",
  "mechanicId": "00000000-0000-0000-0000-000000000005",
  "requiresInspection": true,
  "internalNotes": "Cliente preferencial - prioridade alta"
}
```

### ➕ **Criar Ordem de Serviço 2**

```json
POST /api/service-orders
{
  "customerId": "00000000-0000-0000-0000-000000000002",
  "vehicleId": "00000000-0000-0000-0000-000000000004",
  "description": "Reparação de travões - ruído anormal",
  "priority": 2,
  "estimatedCost": 200.00,
  "estimatedDelivery": "2024-12-16T14:00:00Z",
  "mechanicId": null,
  "requiresInspection": false,
  "internalNotes": "Verificar sistema de travagem completo"
}
```

### 📋 **Listar Ordens de Serviço**

```json
GET /api/service-orders?pageNumber=1&pageSize=10&status=1
```

### 🔍 **Obter Ordem por ID**

```json
GET /api/service-orders/{id}
```

### ✏️ **Atualizar Status**

```json
PUT /api/service-orders/{id}/status
{
  "status": 2,
  "notes": "Iniciada reparação - peças encomendadas"
}
```

### 👷 **Atribuir Mecânico**

```json
PUT /api/service-orders/{id}/mechanic
{
  "mechanicId": "00000000-0000-0000-0000-000000000005"
}
```

### ➕ **Adicionar Serviço à Ordem**

```json
POST /api/service-orders/{id}/services
{
  "serviceId": "00000000-0000-0000-0000-000000000009",
  "quantity": 1,
  "estimatedHours": 1.5,
  "discountPercentage": 10.0,
  "mechanicId": "00000000-0000-0000-0000-000000000005"
}
```

### ➕ **Adicionar Peça à Ordem**

```json
POST /api/service-orders/{id}/parts
{
  "partId": "00000000-0000-0000-0000-000000000006",
  "quantity": 2,
  "discountPercentage": 5.0
}
```

---

## 🔍 **8. INSPEÇÕES**

### ➕ **Agendar Inspeção**

```json
POST /api/inspections
{
  "vehicleId": "00000000-0000-0000-0000-000000000003",
  "customerId": "00000000-0000-0000-0000-000000000001",
  "scheduledDate": "2024-12-20T10:00:00Z",
  "type": 1,
  "notes": "Inspeção anual obrigatória"
}
```

### 📋 **Listar Inspeções**

```json
GET /api/inspections?pageNumber=1&pageSize=10
```

---

## 📊 **9. DASHBOARD**

### 📈 **Estatísticas do Dashboard**

```json
GET /api/dashboard/stats
```

### 📋 **Ordens Recentes**

```json
GET /api/dashboard/recent-orders
```

### ⚠️ **Peças com Stock Baixo**

```json
GET /api/dashboard/low-stock
```

---

## 🏥 **10. HEALTH CHECK**

### ✅ **Verificar Saúde da API**

```json
GET /health
```

---

## 📚 **REFERÊNCIA DE ENUMS**

### 👥 **UserRole**

- `1` = Customer (Cliente)
- `2` = Employee (Funcionário)
- `3` = Admin (Administrador)

### 👷 **EmployeeRole**

- `1` = Mechanic (Mecânico)
- `2` = Admin (Administrador)

### 📋 **ServiceOrderStatus**

- `1` = Pending (Pendente)
- `2` = InProgress (Em Progresso)
- `3` = Completed (Concluída)
- `4` = Cancelled (Cancelada)

### ⚡ **Priority**

- `1` = Low (Baixa)
- `2` = Medium (Média)
- `3` = High (Alta)
- `4` = Urgent (Urgente)

### ⛽ **FuelType**

- `1` = Gasoline (Gasolina)
- `2` = Diesel
- `3` = Electric (Elétrico)
- `4` = Hybrid (Híbrido)

### 🔧 **PartCategory**

- `1` = Engine (Motor)
- `2` = Brakes (Travões)
- `3` = Transmission (Transmissão)
- `4` = Suspension (Suspensão)

### 🛠️ **ServiceCategory**

- `1` = Maintenance (Manutenção)
- `2` = Repair (Reparação)
- `3` = Inspection (Inspeção)
- `4` = Diagnostic (Diagnóstico)

### 🔍 **InspectionType**

- `1` = Annual (Anual)
- `2` = PrePurchase (Pré-compra)
- `3` = Insurance (Seguro)

---

## 🚀 **ORDEM DE TESTES RECOMENDADA**

1. **Criar Clientes** → Obter IDs
2. **Criar Veículos** → Usar IDs dos clientes
3. **Criar Funcionários** → Obter IDs
4. **Criar Peças e Serviços** → Obter IDs
5. **Criar Ordens de Serviço** → Usar IDs anteriores
6. **Testar Operações CRUD** → Listar, Atualizar, etc.
7. **Testar Dashboard** → Ver estatísticas
8. **Testar Health Check** → Verificar API

---

## 💡 **DICAS IMPORTANTES**

- ✅ **Substitua os IDs** pelos IDs reais retornados pelas APIs
- ✅ **Use o Swagger UI** em `http://localhost:5039/swagger` para testes interativos
- ✅ **Verifique os logs** do backend para debug
- ✅ **Use Postman ou Thunder Client** para testes mais avançados
- ✅ **Teste cenários de erro** com dados inválidos
