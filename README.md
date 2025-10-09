# MechaSoftApp

Solução full‑stack para gestão de oficina, com backend em .NET 8 (WebAPI + EF Core) e frontend em Angular.

## Pré‑requisitos

- .NET SDK 8.x
- Node.js 18+ e npm 9+
- Git
- SQL Server (localdb ou instância configurada)

## Estrutura

- `MechaSoft.WebAPI` — API ASP.NET Core (Minimal Endpoints)
- `MechaSoft.Application`, `MechaSoft.Data`, `MechaSoft.Domain`, `MechaSoft.Security`, `MechaSoft.IoC` — camadas de aplicação, domínio, dados e DI
- `Presentation/MechaSoft.Angular` — frontend Angular

## Configuração

### 1) Base de dados
- Verifique a connection string `MechaSoftCS` em:
  - `MechaSoft.WebAPI/appsettings.Development.json`
  - `MechaSoft.Data/Context/ApplicationDbContext.cs` (via DI)
- Opcional: executar as migrações (já existentes)
```bash
dotnet ef database update --project .\MechaSoft.Data\ --startup-project .\MechaSoft.WebAPI\
```

### 2) Certificado HTTPS (opcional para desenvolvimento)
Para evitar avisos de “ligação não é privada” quando usar HTTPS:
```bash
dotnet dev-certs https --trust
```

## Executar em Desenvolvimento

### Backend (.NET)
- Porta HTTP: 5039
- Porta HTTPS (opcional): 7277

Arrancar só em HTTP (recomendado em dev para evitar avisos do browser):
```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\MechaSoft.WebAPI\MechaSoft.WebAPI.csproj --no-build --urls 'http://localhost:5039'
```

Endpoints úteis:
- Health check: `GET http://localhost:5039/health` (200 OK esperado)
- Swagger UI: `http://localhost:5039/swagger` (ou `https://localhost:7277/swagger` se usar HTTPS)

Notas:
- A API usa Minimal Endpoints (sem controllers) e MediatR para CQRS.
- Auditoria ativa via `AuditableEntityInterceptor` (usa `IHttpContextAccessor` opcional).

### Frontend (Angular)
Config da API:
- `Presentation/MechaSoft.Angular/src/environments/environment.development.ts`
```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5039/api'
};
```
- Produção (`environment.ts`) usa por padrão `https://localhost:7277/api`.

Instalar e arrancar:
```powershell
cd Presentation/MechaSoft.Angular
npm ci
npx ng serve --port 4300 --open=false
```
App em `http://localhost:4300/`.

Build produção:
```powershell
npm run build
# output em: Presentation/MechaSoft.Angular/dist/MechaSoft.Angular
```

## Troubleshooting

- Porta ocupada (5039):
  - Identifique e termine o processo (Kestrel/IIS Express/Visual Studio):
```powershell
taskkill /F /IM "MechaSoft.WebAPI.exe" 2>$null
taskkill /F /IM "iisexpress.exe" 2>$null
```

- Aviso HTTPS “ligação não é privada”:
  - Confiar certificado dev: `dotnet dev-certs https --trust`, ou usar HTTP (`http://localhost:5039`).

- Erro MSB3026/MSB3027 (ficheiro bloqueado) ao compilar:
  - Feche instâncias da API/IIS Express/Visual Studio e repita o build.

- Swagger 500 (conflitos de schema):
  - A solução usa `CustomSchemaIds` e nomes distintos de DTOs.
  - Se criar DTOs com o mesmo nome em namespaces diferentes, renomeie (ex.: `VehicleDetailsResponse` vs `VehicleListItemResponse`) ou mantenha a regra `FullName`.

## Convenções

- Mensagens de commit em inglês (ex.: `feat(data): ...`, `fix(swagger): ...`).
- Preferir Endpoints minimalistas em vez de Controllers.
- Configurações mantidas em ficheiros de config (não automáticas).

## Scripts úteis (Windows PowerShell)

- Arrancar API (HTTP):
```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\MechaSoft.WebAPI\MechaSoft.WebAPI.csproj --no-build --urls 'http://localhost:5039'
```

- Parar processos comuns:
```powershell
taskkill /F /IM "MechaSoft.WebAPI.exe" 2>$null; taskkill /F /IM "iisexpress.exe" 2>$null
```

- Arrancar frontend:
```powershell
cd Presentation/MechaSoft.Angular
npx ng serve --port 4300 --open=false
```

## Contacto/Manutenção

- Qualquer alteração estrutural deve manter as camadas desacopladas e o uso de MediatR.
- Em caso de dúvidas, verificar `MechaSoft.WebAPI/Program.cs` e `MechaSoft.Data/DependencyInjection.cs`.