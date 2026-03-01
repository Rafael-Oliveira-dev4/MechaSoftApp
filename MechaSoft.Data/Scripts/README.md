# Scripts de base de dados

## SeedParts.sql

Insere 20 peças de teste na tabela `MechaSoftCS.Part`.

### Pré-requisitos

- SQL Server acessível (mesma connection string do projeto).
- Connection string em `MechaSoft.WebAPI/appsettings.Development.json`:
  - Chave: `ConnectionStrings:MechaSoftCS`
  - Exemplo: `Server=192.168.1.222,1433;Initial Catalog=MechaSoft;User ID=sa;Password=...;Encrypt=False;TrustServerCertificate=True;...`

### Como executar

**SQL Server Management Studio (SSMS):**

1. Conectar ao servidor (ex.: `192.168.1.222,1433`) com o utilizador da connection string.
2. Abrir `SeedParts.sql`.
3. Executar (F5 ou Execute).

**Linha de comandos (sqlcmd):**

```bash
sqlcmd -S 192.168.1.222,1433 -U sa -P <password> -d MechaSoft -i "MechaSoft.Data/Scripts/SeedParts.sql"
```

Substituir `<password>` pela password definida em `appsettings.Development.json`.

### Conteúdo do seed

- Códigos: `PEC-001` a `PEC-020`.
- Categorias: Filtros, Travagens, Fluidos, Motor, Elétrico, Acessórios.
- Cada execução remove antes as linhas com esses códigos, pelo que o script pode ser re-executado para repor os dados de teste.
