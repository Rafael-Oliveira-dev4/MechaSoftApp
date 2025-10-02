# 🔧 **Correções dos Erros da API**

## ❌ **Problemas Identificados:**

### 1. **Erro de Username**

```
"Username can only contain letters, numbers, and underscores"
```

**❌ Problema:** Username com ponto (`joao.silva`)  
**✅ Solução:** Usar underscore (`joao_silva`)

### 2. **Erro de Vinculação**

```
"User cannot be linked to both customer and employee"
```

**❌ Problema:** Tentativa de criar usuário com `customerId` e `employeeId` simultaneamente  
**✅ Solução:** Remover os IDs na criação inicial do usuário

---

## ✅ **JSONs Corrigidos:**

### 🔐 **1. Registrar Usuário (Cliente)**

```json
POST http://localhost:5039/api/accounts/register
Content-Type: application/json

{
  "username": "joao_silva",
  "email": "joao.silva@email.com",
  "password": "MinhaSenh@123",
  "role": 1
}
```

### 🔐 **2. Registrar Funcionário**

```json
POST http://localhost:5039/api/accounts/register
Content-Type: application/json

{
  "username": "admin_user",
  "email": "admin@mechasoft.pt",
  "password": "Admin@123",
  "role": 2
}
```

### 🔐 **3. Login**

```json
POST http://localhost:5039/api/accounts/login
Content-Type: application/json

{
  "username": "joao_silva",
  "password": "MinhaSenh@123"
}
```

---

## 📋 **Regras de Validação:**

### 👤 **Username:**

- ✅ Apenas letras, números e underscores (`a-zA-Z0-9_`)
- ✅ Mínimo 3 caracteres
- ✅ Máximo 50 caracteres
- ❌ **NÃO** usar pontos, hífens ou espaços

### 🔒 **Password:**

- ✅ Mínimo 8 caracteres
- ✅ Pelo menos 1 letra maiúscula
- ✅ Pelo menos 1 letra minúscula
- ✅ Pelo menos 1 número
- ✅ Pelo menos 1 caractere especial

### 📧 **Email:**

- ✅ Formato de email válido
- ✅ Campo obrigatório

### 👥 **Role (UserRole):**

- `1` = Customer (Cliente)
- `2` = Employee (Funcionário)
- `3` = Admin (Administrador)

### 🔗 **Vinculação:**

- ❌ **NÃO** pode ter `customerId` E `employeeId` ao mesmo tempo
- ✅ Pode ter apenas `customerId` OU apenas `employeeId`
- ✅ Pode não ter nenhum dos dois (usuário independente)

---

## 🚀 **Sequência de Testes Recomendada:**

### **Passo 1: Criar Usuários**

```http
# 1. Registrar usuário cliente
POST http://localhost:5039/api/accounts/register
{
  "username": "joao_silva",
  "email": "joao.silva@email.com",
  "password": "MinhaSenh@123",
  "role": 1
}

# 2. Registrar usuário funcionário
POST http://localhost:5039/api/accounts/register
{
  "username": "admin_user",
  "email": "admin@mechasoft.pt",
  "password": "Admin@123",
  "role": 2
}
```

### **Passo 2: Fazer Login**

```http
POST http://localhost:5039/api/accounts/login
{
  "username": "joao_silva",
  "password": "MinhaSenh@123"
}
```

### **Passo 3: Criar Dados Base**

```http
# 3. Criar cliente
POST http://localhost:5039/api/customers
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

# 4. Criar funcionário
POST http://localhost:5039/api/employees
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

---

## 💡 **Dicas Importantes:**

### ✅ **Usernames Válidos:**

- `joao_silva` ✅
- `admin_user` ✅
- `carlos123` ✅
- `user_2024` ✅

### ❌ **Usernames Inválidos:**

- `joao.silva` ❌ (ponto não permitido)
- `admin-user` ❌ (hífen não permitido)
- `user name` ❌ (espaço não permitido)
- `user@123` ❌ (@ não permitido)

### 🔒 **Passwords Válidas:**

- `MinhaSenh@123` ✅
- `Admin@123` ✅
- `Password1!` ✅
- `MyPass2024#` ✅

### ❌ **Passwords Inválidas:**

- `password` ❌ (sem maiúscula, número e especial)
- `PASSWORD` ❌ (sem minúscula, número e especial)
- `Pass123` ❌ (sem caractere especial)
- `Pass@` ❌ (menos de 8 caracteres)

---

## 🔄 **Workflow Completo:**

1. **Registrar usuários** → Obter tokens de autenticação
2. **Criar clientes** → Obter IDs dos clientes
3. **Criar funcionários** → Obter IDs dos funcionários
4. **Vincular usuários** → Associar users a customers/employees (se necessário)
5. **Criar veículos** → Usar IDs dos clientes
6. **Criar ordens de serviço** → Usar IDs de clientes, veículos e funcionários

**Agora os JSONs estão corrigidos e devem funcionar! 🎉**
