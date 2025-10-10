# 🗄️ Acesso à Base de Dados MechaSoft

## 📊 Informações de Conexão

```
Host:          localhost
Porta:         1433
Utilizador:    sa
Senha:         CHANGE_ME_PASSWORD
Base de Dados: DV_RO_MechaSoft
```

---

## 🛠️ Opções para Aceder

### 1️⃣ **Linha de Comando (Rápido)**

#### Executar Query
```bash
./scripts/linux/db-query.sh "SELECT * FROM [MechaSoftCS].[Users]"
```

#### Console Interativo
```bash
./scripts/linux/db-console.sh
```

#### Queries Pré-definidas
```bash
# Listar utilizadores
./scripts/linux/db-queries.sh users

# Listar clientes
./scripts/linux/db-queries.sh clientes

# Listar tabelas
./scripts/linux/db-queries.sh tabelas

# Remover CustomerId de um utilizador
./scripts/linux/db-queries.sh clear-customer <user-id>
```

---

### 2️⃣ **Azure Data Studio (Interface Gráfica)** ⭐

#### Instalar
```bash
cd /home/rick/Documents/GitHub/MechaSoftApp
./scripts/linux/install-azuredatastudio.sh
```

Ou manualmente:
```bash
wget https://go.microsoft.com/fwlink/?linkid=2282284 -O /tmp/azuredatastudio-linux.deb
sudo dpkg -i /tmp/azuredatastudio-linux.deb
sudo apt-get install -f -y
```

#### Configurar Conexão
1. Abrir Azure Data Studio: `azuredatastudio`
2. Clicar em "New Connection"
3. Preencher:
   - **Server**: `localhost,1433`
   - **Authentication type**: SQL Login
   - **User name**: `sa`
   - **Password**: `CHANGE_ME_PASSWORD`
   - **Database**: `DV_RO_MechaSoft`
   - **Encrypt**: `Optional`
   - **Trust server certificate**: ✅ Sim
4. Clicar em "Connect"

---

### 3️⃣ **DBeaver (Alternativa)**

```bash
sudo snap install dbeaver-ce
```

Configurar da mesma forma que o Azure Data Studio.

---

## 📋 Queries Úteis

### Listar Utilizadores
```sql
SELECT Id, Username, Email, Role, CustomerId, EmployeeId 
FROM [MechaSoftCS].[Users] 
WHERE IsDeleted = 0
```

### Listar Clientes
```sql
SELECT Id, FirstName + ' ' + LastName AS Nome, Email, Phone, Type 
FROM [MechaSoftCS].[Customer] 
WHERE IsDeleted = 0
```

### Listar Tabelas
```sql
SELECT TABLE_SCHEMA, TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'MechaSoftCS' 
ORDER BY TABLE_NAME
```

### Remover CustomerId de um Utilizador (para testes)
```sql
SET QUOTED_IDENTIFIER ON;
UPDATE [MechaSoftCS].[Users] 
SET CustomerId = NULL 
WHERE Id = '<user-id>'
```

---

## 🔧 Troubleshooting

### Erro: "Login failed for user 'sa'"
- Verifique se o container SQL Server está rodando: `docker ps | grep mechasoft-sqlserver`
- Reinicie o container: `docker restart mechasoft-sqlserver`

### Erro: "Connection timeout"
- Verifique se a porta 1433 está mapeada: `docker port mechasoft-sqlserver`
- Verifique firewall: `sudo ufw status`

### Container não está rodando
```bash
docker start mechasoft-sqlserver
```

---

## 📝 Notas

- O SQL Server está rodando em Docker
- Dados persistem em volume Docker
- Para backup: use o Azure Data Studio ou `docker exec` com sqlcmd

