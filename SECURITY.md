# Security Policy

## Supported versions

Security updates are applied to the latest `main` branch.

## Reporting a vulnerability

If you discover a security issue, please open a private security advisory on
GitHub (preferred) or contact the maintainer directly before public disclosure.

Please include:

- A clear description of the issue.
- Reproduction steps or proof of concept.
- Potential impact and affected areas.

## Response targets

- Initial acknowledgment: within 72 hours.
- First triage update: within 7 days.
- Fix timeline: depends on severity and complexity.

## Secret management requirements

- Never commit real credentials, tokens, keys, or `.env` files.
- Use environment variables or local secret stores for sensitive values.
- Use example config files for documentation.

## Credential rotation notice

Historical secrets were removed from git history. Any credentials previously
used in this repository must be rotated and considered compromised.
