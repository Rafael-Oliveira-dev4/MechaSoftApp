# 🚀 **TESTES API MECHASOFT - SEQUÊNCIA COMPLETA**

## 📋 **Backend Rodando:**

- **URL:** `http://localhost:5039`
- **Swagger:** `http://localhost:5039/swagger`
- **Status:** ✅ ATIVO

---

## 🔥 **SEQUÊNCIA DE TESTES (COPIE E COLE NO SWAGGER):**

### **🔐 PASSO 1: AUTENTICAÇÃO**

#### **1.1 - Registrar Usuário Cliente**

```json
POST /api/accounts/register

{
  "username": "joao_silva",
  "email": "joao.silva@email.com",
  "password": "MinhaSenh@123",
  "role": 1
}
```

#### **1.2 - Registrar Usuário Admin**

```json
POST /api/accounts/register

{
  "username": "admin_user",
  "email": "admin@mechasoft.pt",
  "password": "Admin@123",
  "role": 2
}
```

#### **1.3 - Fazer Login**

```json
POST /api/accounts/login

{
  "username": "joao_silva",
  "password": "MinhaSenh@123"
}
```

**💡 IMPORTANTE:** Guarde o `token` retornado para usar nos próximos testes!

---

### **👤 PASSO 2: CRIAR CLIENTES**

#### **2.1 - Criar Cliente 1**

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

#### **2.2 - Criar Cliente 2**

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

#### **2.3 - Listar Clientes**

```json
GET /api/customers?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` dos clientes para os próximos testes!

---

### **👷 PASSO 3: CRIAR FUNCIONÁRIOS**

#### **3.1 - Criar Mecânico**

```json
POST /api/employees

{
  "firstName": "Carlos",
  "lastName": "Mecânico",
  "email": "carlos@mechasoft.pt",
  "phone": "+351 934 567 890",
  "role": 1,
  "hourlyRate": 25.00,
  "specialties": [1, 2],
  "canPerformInspections": true,
  "inspectionLicenseNumber": "INS123456"
}
```

#### **3.2 - Criar Administrador**

```json
POST /api/employees

{
  "firstName": "Ana",
  "lastName": "Administradora",
  "email": "ana@mechasoft.pt",
  "phone": "+351 945 678 901",
  "role": 2,
  "hourlyRate": 35.00,
  "specialties": [1, 2, 3, 4],
  "canPerformInspections": true,
  "inspectionLicenseNumber": "INS789012"
}
```

#### **3.3 - Listar Funcionários**

```json
GET /api/employees?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` dos funcionários!

---

### **🚗 PASSO 4: CRIAR VEÍCULOS**

#### **4.1 - Criar Veículo 1**

```json
POST /api/vehicles

{
  "customerId": "COLE_AQUI_ID_DO_CLIENTE_1",
  "brand": "Toyota",
  "model": "Corolla",
  "licensePlate": "12-AB-34",
  "color": "Branco",
  "year": 2020,
  "vin": "1HGBH41JXMN109186",
  "engineType": "1.6L",
  "fuelType": "Gasoline"
}
```

#### **4.2 - Criar Veículo 2**

```json
POST /api/vehicles

{
  "customerId": "COLE_AQUI_ID_DO_CLIENTE_1",
  "brand": "BMW",
  "model": "X3",
  "licensePlate": "56-CD-78",
  "color": "Preto",
  "year": 2019,
  "vin": "WBAFR9C56DD123456",
  "engineType": "2.0L",
  "fuelType": "Diesel"
}
```

#### **4.3 - Listar Veículos**

```json
GET /api/vehicles?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` dos veículos!

---

### **🔧 PASSO 5: CRIAR PEÇAS**

#### **5.1 - Criar Peça 1**

```json
POST /api/parts

{
  "code": "FO-001",
  "name": "Filtro de Óleo",
  "description": "Filtro de óleo para motor 1.6L",
  "category": "Engine",
  "brand": "Bosch",
  "unitCost": 20.00,
  "salePrice": 25.50,
  "stockQuantity": 50,
  "minStockLevel": 10,
  "supplierName": "Auto Parts Ltd",
  "supplierContact": "fornecedor@autoparts.com",
  "location": "A1-B2-C3"
}
```

#### **5.2 - Criar Peça 2**

```json
POST /api/parts

{
  "code": "PT-002",
  "name": "Pastilhas de Travão",
  "description": "Pastilhas de travão dianteiras",
  "category": "Brakes",
  "brand": "Brembo",
  "unitCost": 70.00,
  "salePrice": 89.99,
  "stockQuantity": 25,
  "minStockLevel": 5,
  "supplierName": "Brake Systems Inc",
  "supplierContact": "vendas@brakesystems.com",
  "location": "B2-C3-D4"
}
```

#### **5.3 - Listar Peças**

```json
GET /api/parts?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` das peças!

---

### **🛠️ PASSO 6: CRIAR SERVIÇOS**

#### **6.1 - Criar Serviço 1**

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

#### **6.2 - Criar Serviço 2**

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

#### **6.3 - Listar Serviços**

```json
GET /api/services?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` dos serviços!

---

### **📋 PASSO 7: CRIAR ORDENS DE SERVIÇO**

#### **7.1 - Criar Ordem de Serviço 1**

```json
POST /api/service-orders

{
  "customerId": "COLE_AQUI_ID_DO_CLIENTE_1",
  "vehicleId": "COLE_AQUI_ID_DO_VEICULO_1",
  "description": "Manutenção completa do veículo - mudança de óleo e inspeção",
  "priority": 1,
  "estimatedCost": 150.00,
  "estimatedDelivery": "2024-12-15T16:00:00Z",
  "mechanicId": "COLE_AQUI_ID_DO_MECANICO",
  "requiresInspection": true,
  "internalNotes": "Cliente preferencial - prioridade alta"
}
```

#### **7.2 - Criar Ordem de Serviço 2**

```json
POST /api/service-orders

{
  "customerId": "COLE_AQUI_ID_DO_CLIENTE_2",
  "vehicleId": "COLE_AQUI_ID_DO_VEICULO_2",
  "description": "Reparação de travões - ruído anormal",
  "priority": 2,
  "estimatedCost": 200.00,
  "estimatedDelivery": "2024-12-16T14:00:00Z",
  "mechanicId": null,
  "requiresInspection": false,
  "internalNotes": "Verificar sistema de travagem completo"
}
```

#### **7.3 - Listar Ordens de Serviço**

```json
GET /api/service-orders?pageNumber=1&pageSize=10
```

**💡 IMPORTANTE:** Guarde os `IDs` das ordens!

---

### **⚙️ PASSO 8: OPERAÇÕES NAS ORDENS**

#### **8.1 - Atualizar Status da Ordem**

```json
PUT /api/service-orders/COLE_AQUI_ID_DA_ORDEM/status

{
  "status": 2,
  "notes": "Iniciada reparação - peças encomendadas"
}
```

#### **8.2 - Atribuir Mecânico**

```json
PUT /api/service-orders/COLE_AQUI_ID_DA_ORDEM/mechanic

{
  "mechanicId": "COLE_AQUI_ID_DO_MECANICO"
}
```

#### **8.3 - Adicionar Serviço à Ordem**

```json
POST /api/service-orders/COLE_AQUI_ID_DA_ORDEM/services

{
  "serviceId": "COLE_AQUI_ID_DO_SERVICO",
  "quantity": 1,
  "estimatedHours": 1.5,
  "discountPercentage": 10.0,
  "mechanicId": "COLE_AQUI_ID_DO_MECANICO"
}
```

#### **8.4 - Adicionar Peça à Ordem**

```json
POST /api/service-orders/COLE_AQUI_ID_DA_ORDEM/parts

{
  "partId": "COLE_AQUI_ID_DA_PECA",
  "quantity": 2,
  "discountPercentage": 5.0
}
```

---

### **🔍 PASSO 9: INSPEÇÕES**

#### **9.1 - Agendar Inspeção**

```json
POST /api/inspections

{
  "vehicleId": "COLE_AQUI_ID_DO_VEICULO",
  "customerId": "COLE_AQUI_ID_DO_CLIENTE",
  "scheduledDate": "2024-12-20T10:00:00Z",
  "type": 1,
  "notes": "Inspeção anual obrigatória"
}
```

#### **9.2 - Listar Inspeções**

```json
GET /api/inspections?pageNumber=1&pageSize=10
```

---

### **📊 PASSO 10: DASHBOARD**

#### **10.1 - Estatísticas do Dashboard**

```json
GET /api/dashboard/stats
```

#### **10.2 - Ordens Recentes**

```json
GET /api/dashboard/recent-orders
```

#### **10.3 - Peças com Stock Baixo**

```json
GET /api/dashboard/low-stock
```

---

### **✅ PASSO 11: VERIFICAÇÕES FINAIS**

#### **11.1 - Health Check**

```json
GET /health
```

#### **11.2 - Listar Usuários**

```json
GET /api/accounts/users?pageNumber=1&pageSize=10
```

#### **11.3 - Obter Cliente por ID**

```json
GET /api/customers/COLE_AQUI_ID_DO_CLIENTE
```

#### **11.4 - Obter Ordem por ID**

```json
GET /api/service-orders/COLE_AQUI_ID_DA_ORDEM
```

---

## 📚 **REFERÊNCIA DE ENUMS:**

### **UserRole:**

- `1` = Customer (Cliente)
- `2` = Employee (Funcionário)
- `3` = Admin (Administrador)

### **EmployeeRole:**

- `1` = Mechanic (Mecânico)
- `2` = Admin (Administrador)

### **ServiceOrderStatus:**

- `1` = Pending (Pendente)
- `2` = InProgress (Em Progresso)
- `3` = Completed (Concluída)
- `4` = Cancelled (Cancelada)

### **Priority:**

- `1` = Low (Baixa)
- `2` = Medium (Média)
- `3` = High (Alta)
- `4` = Urgent (Urgente)

### **ServiceCategory:**

- `1` = Engine (Motor)
- `2` = Brakes (Travões)
- `3` = Transmission (Transmissão)
- `4` = Suspension (Suspensão)

### **InspectionType:**

- `1` = Annual (Anual)
- `2` = PrePurchase (Pré-compra)
- `3` = Insurance (Seguro)

---

## 💡 **DICAS IMPORTANTES:**

1. **Execute na ordem** - Cada passo depende do anterior
2. **Guarde os IDs** - Substitua `COLE_AQUI_ID_...` pelos IDs reais
3. **Use o Swagger** - `http://localhost:5039/swagger`
4. **Autenticação** - Use o token JWT quando necessário
5. **Verifique respostas** - Confirme se cada teste retorna sucesso

---

## 🎯 **RESULTADO ESPERADO:**

Após executar todos os testes, você terá:

- ✅ Usuários registrados e autenticados
- ✅ Clientes cadastrados
- ✅ Funcionários cadastrados
- ✅ Veículos registrados
- ✅ Peças em stock
- ✅ Serviços disponíveis
- ✅ Ordens de serviço criadas e gerenciadas
- ✅ Inspeções agendadas
- ✅ Dashboard com dados

**🚀 API MECHASOFT TOTALMENTE TESTADA E FUNCIONAL!**




