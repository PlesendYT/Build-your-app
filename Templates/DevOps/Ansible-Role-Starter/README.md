# Ansible Role Starter

> Struktur für eine wiederverwendbare Ansible-Rolle. / Skeleton for a reusable Ansible role.

## Features
- tasks/, handlers/, templates/, files/, vars/, defaults/ / standard layout
- Beispiel-Task und Template / Example task and template

## How to Use
```bash
cd Ansible-Role-Starter
ansible-galaxy init --role-name=myrole myrole
# copy tasks/main.yml, handlers/main.yml, templates/*, vars/* as needed
ansible-playbook -i inventory playbook.yml
```

## Tech Stack
- Ansible

## Extension Ideas
- Molecule für Rollen-Tests / Molecule for role tests
- Ansible-Lint für Quality Gates / Ansible-lint for quality gates
- Collections für geteilte Logik / Collections for shared logic