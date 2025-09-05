# 🔐 MechaSoft App - Credenciais SQL Server

## 📊 Informações de Acesso ao Banco de Dados

### SQL Server 2022 (Docker)
- **Host**: localhost
- **Porta**: 1433
- **Database**: DV_RO_MechaSoft
- **Usuário**: sa
- **Senha**: CHANGE_ME_PASSWORD

### Connection String Completa
```
Server=localhost,1433;Database=DV_RO_MechaSoft;User Id=sa;Password=CHANGE_ME_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### URLs da Aplicação
- **WebAPI**: http://localhost:5000
- **Angular**: http://localhost:4200
- **Swagger**: http://localhost:5000/swagger

### Comandos Úteis
```bash
# Iniciar SQL Server
docker start mechasoft-sqlserver

# Parar SQL Server
docker stop mechasoft-sqlserver

# Ver logs do SQL Server
docker logs mechasoft-sqlserver

# Conectar via sqlcmd
sqlcmd -S localhost,1433 -U sa -P "CHANGE_ME_PASSWORD" -C

# Iniciar aplicação completa
./start-mechasoft.sh

# Parar aplicação completa
./stop-mechasoft.sh
```

### Container Docker
- **Nome**: mechasoft-sqlserver
- **Imagem**: mcr.microsoft.com/mssql/server:2022-latest
- **Status**: `docker ps` para verificar

---
*Arquivo gerado automaticamente pelo setup-sqlserver.sh*
*Data: sex 05 set 2025 14:35:51 WEST*
