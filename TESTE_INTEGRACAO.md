
# 🧪 TESTE DE INTEGRAÇÃO - MechaSoft

## ✅ PRÉ-REQUISITOS:

### 1. SQL Server
- [ ] SQL Server está rodando
- [ ] Database `DV_RO_MechaSoft` existe
- [ ] Connection string está correta

### 2. Backend (.NET)
- [ ] Packages restaurados (`dotnet restore`)
- [ ] Migrations aplicadas
- [ ] Backend compila sem erros

### 3. Frontend (Angular)
- [ ] Node modules instalados (`npm install`)
- [ ] Angular compila sem erros

---

## 🚀 PASSOS PARA TESTAR:

### PASSO 1: Iniciar Backend

```bash
# Terminal 1 - Backend
cd MechaSoft.WebAPI
dotnet run --launch-profile https
```

**Resultado esperado:**
```
Now listening on: https://localhost:7277
Now listening on: http://localhost:5039
Application started. Press Ctrl+C to shut down.
```

**Verificar:**
- Abrir navegador: https://localhost:7277/swagger
- Deve mostrar Swagger UI com todos os endpoints

---

### PASSO 2: Iniciar Frontend

```bash
# Terminal 2 - Frontend
cd Presentation/MechaSoft.Angular
npm start
```

**Resultado esperado:**
```
** Angular Live Development Server is listening on localhost:4200 **
✔ Compiled successfully.
```

**Verificar:**
- Abrir navegador: http://localhost:4200
- Deve mostrar página de login

---

### PASSO 3: Criar Usuário de Teste (via Swagger)

1. Abrir: https://localhost:7277/swagger
2. Endpoint: `POST /api/accounts/register`
3. Body:
```json
{
  "username": "admin",
  "email": "admin@mechasoft.pt",
  "password": "Admin@123",
  "confirmPassword": "Admin@123",
  "role": "Owner"
}
```
4. Executar → Deve retornar **200 OK**

---

### PASSO 4: Testar Login

1. Frontend: http://localhost:4200
2. Deve redirecionar para: http://localhost:4200/login
3. Inserir credenciais:
   - Username: `admin`
   - Password: `Admin@123`
4. Clicar "Entrar"

**Resultado esperado:**
- ✅ Loading spinner aparece
- ✅ Request vai para backend
- ✅ Token é recebido
- ✅ Redirect para /dashboard
- ✅ Nome "admin" aparece no header
- ✅ Role "Owner" aparece no header

---

### PASSO 5: Testar Dashboard

**URL:** http://localhost:4200/dashboard

**Verificar:**
- [ ] Stats cards carregam (pode estar tudo a 0 - normal)
- [ ] Sem erros na console
- [ ] Loading spinner funcionou

---

### PASSO 6: Testar CRUD de Customers

**URL:** http://localhost:4200/customers

1. **Criar Cliente:**
   - Clicar "Novo Cliente"
   - Preencher formulário:
     ```
     Nome: João Silva
     Email: joao@email.pt
     Telefone: 912345678
     NIF: 123456789
     Rua: Rua Principal
     Número: 123
     Freguesia: Centro
     Cidade: Lisboa
     Código Postal: 1000-001
     País: Portugal
     ```
   - Clicar "Criar"

**Resultado esperado:**
- ✅ Modal fecha
- ✅ Cliente aparece na tabela
- ✅ Mensagem de sucesso (se implementada) ou refresh automático

2. **Editar Cliente:**
   - Clicar no ícone de editar
   - Alterar email
   - Guardar

3. **Toggle Active:**
   - Clicar no ícone vermelho (desativar)
   - Confirmar
   - Cliente fica cinza/opaco

---

### PASSO 7: Testar CRUD de Vehicles

**URL:** http://localhost:4200/vehicles

1. **Criar Veículo:**
   - Clicar "Novo Veículo"
   - Selecionar cliente criado acima
   - Preencher:
     ```
     Matrícula: AB-12-CD
     Marca: Toyota
     Modelo: Corolla
     Ano: 2020
     Cor: Branco
     Combustível: Gasolina
     ```
   - Clicar "Criar"

**Resultado esperado:**
- ✅ Veículo aparece na tabela
- ✅ Nome do cliente aparece corretamente

---

### PASSO 8: Testar Service Orders

**URL:** http://localhost:4200/service-orders

1. **Criar Ordem:**
   - Clicar "Nova Ordem"
   - Selecionar cliente → Veículos carregam automaticamente
   - Selecionar veículo
   - Descrição: "Revisão dos 10.000km"
   - Prioridade: Normal
   - Valor Estimado: 150
   - Clicar "Criar Ordem"

**Resultado esperado:**
- ✅ Ordem criada com número automático
- ✅ Aparece na tabela com status "Pendente"

2. **Ver Detalhes:**
   - Clicar no ícone de olho
   - Modal abre com detalhes

3. **Atualizar Status:**
   - No modal de detalhes
   - Clicar "Em Progresso"
   - Badge muda de amarelo para azul

---

## 🐛 PROBLEMAS COMUNS:

### ❌ CORS Error
```
Access to XMLHttpRequest has been blocked by CORS policy
```
**Solução:** Backend CORS já configurado. Reiniciar backend.

### ❌ Connection Refused
```
ERR_CONNECTION_REFUSED
```
**Solução:** Backend não está rodando. Iniciar backend (Passo 1).

### ❌ 401 Unauthorized
```
Unauthorized
```
**Solução:** Token inválido ou expirado. Fazer logout e login novamente.

### ❌ Database Error
```
Cannot open database
```
**Solução:** 
1. Verificar SQL Server está rodando
2. Verificar connection string
3. Aplicar migrations: `dotnet ef database update`

---

## ✅ CHECKLIST DE SUCESSO:

- [ ] Backend inicia sem erros
- [ ] Frontend inicia sem erros
- [ ] Swagger acessível
- [ ] Login funciona
- [ ] Redirect para dashboard funciona
- [ ] Dashboard carrega stats
- [ ] Criar cliente funciona
- [ ] Criar veículo funciona
- [ ] Criar ordem funciona
- [ ] Logout funciona
- [ ] Token refresh automático funciona

---

## 📊 ENDPOINTS PARA TESTAR:

| Endpoint | Método | Autenticação | Teste |
|----------|--------|--------------|-------|
| `/api/accounts/register` | POST | Não | ✅ Criar user |
| `/api/accounts/login` | POST | Não | ✅ Login |
| `/api/dashboard/stats` | GET | Sim | ✅ Stats |
| `/api/customers` | GET | Sim | ✅ Listar |
| `/api/customers` | POST | Sim | ✅ Criar |
| `/api/vehicles` | GET | Sim | ✅ Listar |
| `/api/vehicles` | POST | Sim | ✅ Criar |
| `/api/service-orders` | GET | Sim | ✅ Listar |
| `/api/service-orders` | POST | Sim | ✅ Criar |

---

## 🎯 PRÓXIMOS PASSOS SE TUDO FUNCIONAR:

1. ✅ Testar todas as páginas
2. ✅ Testar validações
3. ✅ Testar error handling
4. ✅ Testar paginação
5. ✅ Fazer ajustes finais
6. ✅ Deploy!

