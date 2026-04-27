# Contributing to MechaSoft

Thanks for your interest in contributing.

## Getting started

1. Fork the repository and create a branch from `main`.
2. Configure local settings from example files in `MechaSoft.WebAPI`.
3. Run backend and frontend locally.
4. Ensure build and tests pass before opening a pull request.

## Development commands

Backend:

```bash
dotnet restore
dotnet build
dotnet test
```

Frontend:

```bash
cd Presentation/MechaSoft.Angular
npm install
npm run build
npm run test -- --watch=false --browsers=ChromeHeadless
```

## Pull request guidelines

- Keep PRs focused and small.
- Include a clear problem statement and change summary.
- Reference related issues when applicable.
- Update docs when behavior or setup changes.

## Commit messages

This repository follows Conventional Commits, for example:

- `feat: add vehicle service history filter`
- `fix: handle token refresh race condition`
- `docs: update setup instructions`
