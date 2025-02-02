# Contributing to the Government Vehicle Management System

We appreciate your interest in contributing! This document outlines the process for developing new features or fixing bugs, including branching strategy, coding standards, test coverage, and CI requirements.

---

## Table of Contents

- [Contributing to the Government Vehicle Management System](#contributing-to-the-government-vehicle-management-system)
  - [Table of Contents](#table-of-contents)
  - [Issue Tracking \& Branching](#issue-tracking--branching)
  - [Coding Standards](#coding-standards)
  - [Testing \& Coverage](#testing--coverage)
  - [Continuous Integration](#continuous-integration)
  - [Pull Request Guidelines](#pull-request-guidelines)
  - [Code Reviews](#code-reviews)

---

## Issue Tracking & Branching

1. **Issue Creation**  
   - If you find a bug or need a new feature, create an Issue in the repository.  
   - Provide as much detail as possible (steps to reproduce, screenshots if applicable).

2. **Branching**  
   - **Integration Branch**: Base branch for all development work.  
   - **Master Branch**: Merged with the `integration` branch once a week, representing stable releases.  
   - **Feature/Bug Branch**:  
     - Name your branch as `feature/issue-123-description` or `bug/issue-456-fix-description`.  
     - Branch off from `integration`; **do not** branch off from `master`.  
   - **Merging**:  
     - Create Pull Requests against `integration`.  
     - Once tested, we merge `integration` into `master` weekly.

---

## Coding Standards

1. **Naming Conventions**  
   - **Classes**: PascalCase (e.g., `CarHandoverService`).  
   - **Methods**: PascalCase (e.g., `CalculateFuelUsage`).  
   - **Variables & Fields**: camelCase (e.g., `remainingFuel`).
   - **Interfaces**: `I` prefix (e.g., `IEmailService`).
2. **File Organization**  
   - Keep domain entities in the `Domain` layer, services in the `Application` layer, etc.  
   - Follow the clean architecture structure for clarity.
3. **Style**  
   - Use expression-bodied members for short methods when appropriate.  
   - Aim for small, single-purpose methods.

*(Feel free to add your own style rules or reference an `.editorconfig` if you have one.)*

---

## Testing & Coverage

1. **Test Framework**:  
   - We use **xUnit** or **NUnit** (pick whichever you decided). Ensure you have them installed.
2. **Running Tests**:  
   ```bash
   dotnet test
3. **Coverage**:
- Minimum coverage of 80%.
- We use Coverlet for generating coverage reports during CI.
- PRs failing to meet coverage will not be merged.

## Continuous Integration
1. GitHub Actions
- Every PR triggers a build pipeline.
- The pipeline runs dotnet build, dotnet test with coverage, and can also run linting if configured.
2. Passing Checks
- Your PR must pass all checks (build, tests, coverage threshold).
- If any check fails, please fix the issues before requesting review.

## Pull Request Guidelines
1. Descriptive Title & Body
- Summarize the purpose of the PR.
- Reference the Issue (e.g., “Closes #123”).
2. Small & Focused
- Keep changes scoped to a single feature or fix.
- Avoid unrelated refactoring in the same PR if possible.
3. Link to Additional Docs (if necessary)
- If your feature introduces new architecture elements or domain concepts, update the relevant docs in docs/ and mention it.

## Code Reviews
1. Peer Review
- At least one reviewer must approve before merging.
- Address all reviewer comments or questions.
2. Suggestions
- Reviews might offer improvements or suggest changes to keep the code consistent.