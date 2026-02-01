# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Fast Clock App 3 - A Blazor WebAssembly application for model railway module meetings, providing a synchronized fast clock system with digital and analogue displays. Supports 15 European languages.

**Live instance**: https://fastclock.azurewebsites.net/

## Build Commands

```bash
# Build the entire solution
dotnet build "Module Meeting App.slnx"

# Build specific project
dotnet build src/Server/App.Server.csproj

# Run the server (hosts both API and Blazor client)
dotnet run --project src/Server/App.Server.csproj

# Run tests
dotnet test src/Clock.Tests/Clock.Tests.csproj

# Run a specific test
dotnet test src/Clock.Tests/Clock.Tests.csproj --filter "FullyQualifiedName~TestMethodName"

# Publish for specific platform (profiles: win-x64, win-arm64, linux-arm, linux-arm64, osx-x64)
dotnet publish src/Server/App.Server.csproj -p:PublishProfile=win-x64
```

## Architecture

### Solution Structure

```
src/
├── Server/      # ASP.NET Core Web API + Blazor WebAssembly host
├── Client/      # Blazor WebAssembly frontend (SPA)
├── Clock/       # Core business logic library (clock state, timing)
├── Contract/    # Shared API data contracts (published to NuGet)
└── Clock.Tests/ # MSTest unit tests
```

### Key Architectural Patterns

**Server hosts Client**: The Server project references Client and serves it as a hosted Blazor WebAssembly app. Run only the Server project for development.

**Contract package**: `Tellurian.Trains.MeetingApp.Contracts` is published to NuGet for external API consumers. Contains DTOs (`ClockStatus`, `ClockSettings`, `ClockUser`) and service interfaces (`IClockService`, `IClockAdministratorService`).

**Clock management**: `ClockServers` (singleton) manages multiple named `ClockServer` instances. Each clock operates independently with its own settings, users, and state.

### API Routes

Base: `/api/clocks`

- `GET /{clock}/Time` - Get clock status
- `PUT /{clock}/start` - Start clock (requires user registration)
- `PUT /{clock}/stop/{reason}` - Stop clock with reason
- `PUT /{clock}/user` - Register/update user
- `GET/PUT /{clock}/settings` - Admin settings (requires password)
- `POST /create` - Create new clock instance

### Key Interfaces

- `IClock` (Clock/): Core clock behavior - time, state, users
- `ITimeProvider` (Clock/): Time abstraction for testing
- `IClockService`, `IClockAdministratorService` (Contract/): Client service contracts

## Code Style

Enforced via `.editorconfig`:
- File-scoped namespaces
- Expression-bodied members when single-line
- Tabs = 4 spaces, CRLF line endings
- Interfaces prefixed with `I`
- PascalCase for types, properties, methods

Global settings in `Directory.Build.props`:
- Nullable reference types enabled
- Latest C# language version
- .NET analyzers enabled

## Localization

15 languages supported via `.resx` files in:
- `src/Client/Resources/` - UI strings
- `src/Contract/Resources/` - Shared strings

Add translations by creating/editing `App.{culture}.resx` files. Supported cultures: cs, da, de, fi, fr, hu, it, nb, nl, nn, pl, sk, sv.

## Testing

MSTest framework with parallel execution. Test helpers in `Clock.Tests/Helpers/`:
- `TestTimeProvider` - Mock time for deterministic tests

Tests cover clock server functionality, JSON serialization, and settings management.
