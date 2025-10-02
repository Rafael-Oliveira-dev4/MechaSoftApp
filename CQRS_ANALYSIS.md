# 🔍 **Análise CQRS - Problemas e Correções**

## 📋 **Problemas Identificados na Implementação CQRS:**

### ❌ **1. Inconsistências nos DTOs de Veículos**

**Problema:** O DTO `CreateVehicleRequest` usa `string FuelType` mas o comando espera `FuelType` enum.

**DTO Atual:**

```csharp
public record CreateVehicleRequest(
    // ...
    string FuelType  // ❌ String em vez de enum
);
```

**Endpoint Atual:**

```csharp
FuelType = Enum.Parse<MechaSoft.Domain.Model.FuelType>(request.FuelType, true)  // ❌ Parse manual
```

**✅ Correção Necessária:**

```csharp
public record CreateVehicleRequest(
    // ...
    FuelType FuelType  // ✅ Usar enum diretamente
);
```

### ❌ **2. Inconsistências nos DTOs de Funcionários**

**Problema:** O DTO usa `FirstName` e `LastName` separados, mas outros DTOs usam `Name` completo.

**DTO Atual:**

```csharp
public record CreateEmployeeRequest(
    string FirstName,    // ❌ Inconsistente com outros DTOs
    string LastName,     // ❌ Inconsistente com outros DTOs
    // ...
);
```

**✅ Correção Sugerida:**

```csharp
public record CreateEmployeeRequest(
    string Name,         // ✅ Consistente com outros DTOs
    // ...
);
```

### ❌ **3. Inconsistências nos DTOs de Peças**

**Problema:** O DTO usa `Category` como string, mas deveria ser enum.

**DTO Atual:**

```csharp
public record CreatePartRequest(
    // ...
    string Category,     // ❌ String em vez de enum
    // ...
);
```

**✅ Correção Necessária:**

```csharp
public record CreatePartRequest(
    // ...
    PartCategory Category,  // ✅ Usar enum
    // ...
);
```

### ❌ **4. Falta de DTOs nos Endpoints**

**Problema:** Alguns endpoints não têm DTOs definidos no mesmo arquivo.

**Exemplo - VehicleEndpoints.cs:**

```csharp
// ❌ DTOs não estão definidos no arquivo do endpoint
```

**✅ Correção:** Adicionar DTOs no final do arquivo como nos outros endpoints.

---

## 🔧 **Correções Implementadas:**

### ✅ **1. Correção do DTO de Veículos**

<function_calls>
<invoke name="search_replace">
<parameter name="file_path">MechaSoft.Application/CQ/Vehicles/Common/Requests.cs




