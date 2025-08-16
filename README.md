---

# Ficticia S.A. - Sistema de Gestión de Clientes

## Descripción

Este proyecto es un **demo funcional de un sistema para la gestión de clientes** de Ficticia S.A., una empresa que comercializa seguros de vida.
El objetivo del sistema es **permitir registrar, modificar y eliminar clientes**, incluyendo información personal y atributos adicionales relevantes para la gestión de seguros, tales como:

* Nombre completo
* Identificación
* Edad
* Género
* Estado (Activo/Inactivo)
* ¿Maneja?
* ¿Usa lentes?
* ¿Diabético?
* Padece alguna otra enfermedad (especificar)

El sistema también está preparado para **extenderse con nuevos atributos**, siguiendo los principios de diseño escalable.

---

## Tecnologías utilizadas

**Frontend**

* Angular 16+
* Componentes reutilizables y responsivos
* Validaciones en formularios para garantizar datos correctos

**Backend**

* .NET 8+ con arquitectura distribuida
* C#
* Entity Framework Core para todas las operaciones de base de datos
* Patrones de diseño: **Repository** y **CQRS**
* Autenticación y autorización JWT
* Logging integrado para seguimiento de operaciones
* Validaciones de negocio aplicadas en el backend

**Base de datos**

* SQL Server (puede adaptarse a otras bases relacionales)

**Testing**

* XUnit para pruebas unitarias del backend
* Cobertura de escenarios principales: login, CRUD de clientes, validaciones de negocio

---

## Funcionalidades

1. **Autenticación**

   * Demo usuario: `admin`
   * Contraseña: `1234`
   * Generación de token JWT con duración de 60 minutos

2. **Gestión de clientes**

   * Alta, baja y modificación de clientes
   * Validaciones obligatorias y rango de valores
   * Gestión de atributos opcionales (maneja, usa lentes, diabético, enfermedad adicional)

3. **Seguridad**

   * Roles definidos (por defecto demo sin roles adicionales)
   * Protege endpoints de operaciones críticas
   * Token JWT incluye claims de usuario

4. **Persistencia**

   * Entity Framework Core con patrones Repository y CQRS
   * Optimización de consultas y posibilidad de agregar caching

5. **Testing**

   * XUnit para asegurar correcto funcionamiento de endpoints y lógica de negocio
   * Pruebas incluidas para login, creación, edición y eliminación de clientes

---

## Estructura del proyecto

```
FicticiaSA/
├─ FicticiaBackend/       # Backend .NET 8
│  ├─ Controllers/
│  │  └─ AuthController.cs
│  ├─ Repositories/
│  │  └─ PersonaRepository.cs
│  ├─ Models/
│  │  ├─ Persona.cs
│  │  └─ Usuario.cs
│  ├─ DTOs/
│  └─ Program.cs
├─ FicticiaFrontend/      # Frontend Angular 16+
├─ FicticiaSA.Backend.Tests/
│  └─ AuthControllerTests.cs
│  └─ PersonaRepositoryTests.cs
```

---

## Cómo ejecutar el proyecto

### Backend

1. Abrir terminal en `FicticiaBackend`
2. Restaurar paquetes:

```bash
dotnet restore
```

3. Ejecutar migraciones y crear base de datos (si aplica)
4. Ejecutar el backend:

```bash
dotnet run
```

5. Endpoints disponibles:

   * `POST /api/auth/login` → login con usuario demo `admin` / `1234`
   * CRUD de clientes: `/api/personas`

### Frontend

1. Abrir terminal en `FicticiaFrontend`
2. Instalar dependencias:

```bash
npm install
```

3. Ejecutar aplicación:

```bash
ng serve
```

4. Abrir navegador en `http://localhost:4200`

### Tests

```bash
dotnet test FicticiaSA.Backend.Tests
```

---

## Usuario demo

* **Username:** `admin`
* **Password:** `1234`

> Todos los tests están configurados para usar este usuario demo.

---

## Principios aplicados

Este proyecto no solo cumple con los requerimientos funcionales, sino que también refleja **principios de persuasión, claridad y profesionalismo**:

* Comunicación clara y directa del valor de la solución
* Uso de estructuras de código estandarizadas que generan confianza
* Pruebas unitarias que demuestran fiabilidad del sistema
* Diseño escalable y preparado para extensiones futuras

---