# Orbulus baseline: kcsnap/Praise

Written by orb-importer 0.5.0 for Orbulus code-only maintenance, describing this repository at revision `19d88e10abdca289bd3ab279ffcb76435c3a041a`. Review before merging.

## Orbulus branches run your CI, with your repository's secrets

Every branch Orbulus pushes starts with `orb/`: this baseline pull request, and each change you ask for later. Your CI runs on those branches like on any other, with your repository's secrets, before anyone has reviewed the code. Opening the pull request as a draft doesn't stop that. Orbulus can't change your workflows, so guard them before you merge this.

This CI configuration was not checked by Orbulus. Guard it so it doesn't run orb/ branches with your secrets:

- ` azure-pipelines.yml `: not checked by Orbulus

For `push`, leave orb/ branches out:

```yaml
on:
  push:
    branches-ignore:
      - 'orb/**'
```

If the workflow already lists `branches:`, add `- '!orb/**'` as the last entry instead (GitHub doesn't allow both).

For `pull_request`, `pull_request_target`, `pull_request_review` and `pull_request_review_comment`, branch filters match the branch the pull request merges into, so they can't leave orb/ branches out. Guard each job instead:

```yaml
jobs:
  build:
    if: ${{ !startsWith(github.event.pull_request.head.ref, 'orb/') }}
```

Keep the `${{ }}`: in YAML a value that starts with `!` is read as a tag. Make the guard the whole condition, or join it with `&&`, never `||`.

For `workflow_run`, which runs with your secrets when a watched run finishes, an orb/ one too, leave orb/ runs out:

```yaml
on:
  workflow_run:
    workflows: [CI]
    types: [completed]
    branches-ignore:
      - 'orb/**'
```

For `create`, guard each job:

```yaml
jobs:
  build:
    if: ${{ !startsWith(github.ref, 'refs/heads/orb/') }}
```

`check_run` and `check_suite` workflows run for every branch's checks and can't be guarded by branch: make sure they use no secrets.

Keep the token read-only, so an orb/ run can't write to your repository: set `permissions:` at the top of each workflow, or make read-only the default in Settings, Actions, Workflow permissions:

```yaml
permissions:
  contents: read
```

Or keep your CI running and hold back the secrets: put the jobs that use them in an environment with required reviewers (Settings, Environments), and move the secrets those jobs use into that environment. Its secrets are released only after your approval; repository secrets are not.

## Purpose
Praise is a workplace recognition application. A signed-in user sends a colleague (identified by email) one of six fixed praise types ("Thank you!", "My inspiration!", "You're a saviour!", "Cheers!", "You're on fire!", "Simply magic!", defined in `Praise.Models/BaseEnumTypeMetadata.cs`), and can view praises sent and received, a dashboard of counts and a crypto balance, and earned badges. Sending praise and signing in are reported as events to a separate gamification service, Gamey (`Gamey.Api`), which awards badges, achievements, streaks and token prizes.

## Units
| Root | Kind | Framework | How it is started |
|---|---|---|---|
| `Praise.AdminApp` | http-api | ASP.NET Core Razor Components (interactive server); only the default shell is visible | entry `Praise.AdminApp/Program.cs` |
| `Praise.App` | inventory: cli; actually a .NET MAUI app (inferred from `MauiProgram.cs`, `Platforms/*`) | .NET MAUI, CommunityToolkit.Maui/Mvvm, Syncfusion.Maui.Toolkit, Microsoft.Data.Sqlite | `MauiProgram.CreateMauiApp()` via platform entry points |
| `Praise.Models` | library | shared praise models/view models | — |


## Architecture
Three tiers for the web path:


Other: `Praise.WebApp/GraphQL/EmployeeService.cs` is a sample GraphQL schema (employees/departments) registered in DI, but its endpoint mapping is commented out. `Praise.App` is a standalone MAUI app with projects, tasks, tags and categories in local SQLite and no visible link to the praise APIs.

## Data
- No migrations of any kind were found.

## External services
- Azure Cosmos DB (`Microsoft.Azure.Cosmos`, `Azure.Identity`).
- An Ethereum-compatible RPC node and an ERC-20 token contract via Nethereum (`Nethereum.Web3`, `Nethereum.HdWallet`) in `Gamey.Api/Logic/TokenWalletLogic.cs`.
- MAUI UI libraries: CommunityToolkit.Maui, CommunityToolkit.Mvvm, Syncfusion.Maui.Toolkit.

## Conventions

## Where changes usually go
- **New data field:** add the property to the model in `Praise.Models` or `Gamey.Models` (Cosmos is schemaless, so there is no migration). Update any view model mapping and the queries in the relevant DataLayer class.
- **Tests:** MSTest classes in `Tests.Praise.Api/Logic` (Praise.Api logic) or the `Praise.Tests.*` projects.

## Gaps
- Unclear roles:
  - `Praise.AdminApp` — content beyond the default shell is not visible.
  - `Praise.App` — the MAUI app's models (projects/tasks) do not relate to praise.
- `README-IaC.txt` describes an ARM template deployment to an Azure resource group, but the inventory reports no infrastructure files; `IaC/README-IaC.txt` is empty.
- No CI/CD workflows were found; nothing deploys on merge.
- `Praise.sln` lists 12 projects while the inventory has 15 units; which units are outside the solution is not shown.
- `Tests.Praise.Api` is classified by the inventory as a library, but it contains MSTest tests.
- 48 line(s) removed from this baseline by the importer's instruction screen.
- 1 line(s) removed from this baseline by the importer's instruction screen.

## Behaviour
- A user views praises sent and received (`GET /praise`)
- A user is registered as a gamer in the gamification service and looked up by email (`POST /gamers`, `GET /gamers/{gamerEmail}`)
- Giving praise or signing in is recorded as a game event that drives rewards (`POST /gameevent`, `GET /gameevent`)
- A user sees and progresses their achievements (`GET /gamers/{gamerId}/achievements`, `PATCH /gamers/{gamerId}/achievements/{achievementId}`, `PATCH /achievements/{achievementId}`)
- A user views the badges available and earned (`GET /badges`)
- A user views token prizes and activity streaks (`GET /gamers/{gamerId}/prizes`, `GET /gamers/{gamerId}/streaks`)
- Game configuration is read by the gamification service (`GET /game`)

## Users and roles
- **Signed-in user (colleague)**: Sends praise to colleagues by email, views praises sent and received, and earns badges, achievements, streaks and token prizes
- **Gamer (in Gamey.Api)**: The gamification record of a user, holding achievements, prizes, streaks and a token wallet
- **Administrator**: Uses Praise.AdminApp; its responsibilities beyond the default shell are not visible in the repository

## Security notes
- Sign-in: The Blazor web app (Praise.WebApp) has its own Login and Logout account pages; the survey draft describes these as Auth0 sign-in, with JWT protection on Praise.Api and API-key headers on Gamey.Api. A separate ClerkAuth MSTest project also exists.
- Secrets: No committed secrets were found by the importer. Encryption settings are bound through Gamey.Api/Configuration/EncryptionOptions.cs, so keys, Cosmos DB connection details and API keys are expected to come from application configuration (appsettings or environment) supplied at deployment rather than…
- No committed secret was found by the importer's scan.

## Risk areas
- high: `.github/workflows/**`: always high for an imported repository (DESIGN §6.1)
- high: `ClerkAuth/**`: Auth-category test project (ClerkAuth.csproj, MSTestSettings.cs, Test1.cs flagged as auth).
- high: `Praise.WebApp/Components/Pages/Account/**`: Sign-in and sign-out pages (Login.razor, Logout.razor) flagged as auth.
- high: `Gamey.Api/Configuration/EncryptionOptions.cs`: Encryption configuration, flagged as crypto; likely carries key settings.
- high: `Gamey.Api/Interfaces/IDeterministicEncryption.cs`: Encryption contract, flagged as crypto.
- high: `Gamey.Api/Interfaces/ISecureEncryption.cs`: Encryption contract, flagged as crypto.
- high: `Gamey.Api/Logic/DeterministicEncryption.cs`: Encryption implementation, flagged as crypto.
- high: `Gamey.Api/Logic/SecureEncryption.cs`: Encryption implementation, flagged as crypto.
- high: `Praise.WebApp/Components/Pages/EarnCryptoForGivingPraise.razor`: Crypto-token reward page, flagged as crypto (touches value-bearing wallet flow).
- high: `Tests/Crypto_Tests.CryptoManager/**`: Crypto test project guarding the encryption behaviour, flagged as crypto.
- high: `Gamey.Api/Handlers/**`: from the Architect survey
- high: `Gamey.Api/Configuration/**`: from the Architect survey
- high: `Gamey.Api/Logic/TokenWalletLogic.cs`: from the Architect survey
- high: `Gamey.Api/Program.cs`: from the Architect survey
- high: `Praise.Api/Program.cs`: from the Architect survey
- high: `Praise.Api/Configuration/**`: from the Architect survey
- high: `Praise.Api/Logic/PraiseUserLogic.cs`: from the Architect survey
- high: `Praise.WebApp/Program.cs`: from the Architect survey
- high: `Praise.WebApp/Configuration/**`: from the Architect survey
- high: `Praise.WebApp/Logic/PraiseApiWrapper.cs`: from the Architect survey
- high: `Praise.WebApp/Logic/PraiseAuthentication.cs`: from the Architect survey
- medium: `Gamey.Api/Controllers/**`: Gamification HTTP API surface (badges, game, gameevent, gamers, achievements, prizes, streaks).
- medium: `Praise.Api/Controllers/**`: Praise HTTP API surface (GET /praise, PATCH /achievements/{achievementId}).
- medium: `Gamey.Api/DataLayer/**`: Cosmos container definitions and queries (GameyDbConnection.cs) acting as the schema.
- medium: `Praise.Api/DataLayer/**`: Cosmos PraiseContainer data access (PraiseDetailsDataLayer.cs) acting as the schema.
- medium: `Gamey.Api/Logic/**`: from the Architect survey
- medium: `Praise.Api/Logic/**`: from the Architect survey
- medium: `Gamey.Models/**`: from the Architect survey
- medium: `Praise.Models/**`: from the Architect survey
- low: `Praise.WebApp/wwwroot/**`: Static web assets such as badge images; flagged only by name match on login-streak.
- low: `praise-vercel-nextjs/**`: Next.js starter front end with no API endpoints or sensitive files listed.
- low: `Praise.App/Pages/**`: from the Architect survey
- low: `Praise.App/PageModels/**`: from the Architect survey
- low: `Praise.AdminApp/**`: from the Architect survey
- low: `praise-vercel-nextjs/app/**`: from the Architect survey
- low: `Praise.Tests.Engine/**`: from the Architect survey
- low: `Praise.Tests.Logic/**`: from the Architect survey
- low: `Tests.Praise.Api/**`: from the Architect survey

## Checks
Orbulus does not run these commands; each is listed as `not-run` (not run by Orbulus).

- build: `dotnet build Praise.sln` in `.` (from `Praise.sln`): not-run
- build: `npm run build` in `praise-vercel-nextjs` (from `praise-vercel-nextjs/package.json`): not-run
- lint: `npm run lint` in `praise-vercel-nextjs` (from `praise-vercel-nextjs/package.json`): not-run
- test: `dotnet test Praise.sln` in `.` (from `Praise.sln`): not-run

## Tests
Orbulus does not run the tests. Each change is still test-first on its branch: a RED commit of tests, then the change.

### Frameworks
- `ClerkAuth`: dotnet-test (2 test files)
- `Gamey.Api`: no test framework found (0 test files)
- `Gamey.Models`: no test framework found (0 test files)
- `Praise.AdminApp`: no test framework found (0 test files)
- `Praise.Api`: no test framework found (0 test files)
- `Praise.App`: no test framework found (0 test files)
- `Praise.Engine`: no test framework found (0 test files)
- `Praise.Engines`: no test framework found (0 test files)
- `Praise.Models`: no test framework found (0 test files)
- `Praise.Tests.Engine`: dotnet-test (3 test files)
- `Praise.Tests.Logic`: dotnet-test (3 test files)
- `Praise.WebApp`: no test framework found (0 test files)
- `Tests.Praise.Api`: no test framework found (0 test files)
- `Tests/Crypto_Tests.CryptoManager`: dotnet-test (2 test files)
- `praise-vercel-nextjs`: no test framework found (0 test files)

### What the tests touch
- `ClerkAuth/MSTestSettings.cs` → `Tests.Praise.Api/MSTestSettings.cs` (by name)
- `ClerkAuth/Test1.cs` → no inventory source file found (by name)
- `Praise.Tests.Engine/MSTestSettings.cs` → `Tests.Praise.Api/MSTestSettings.cs` (by name)
- `Praise.Tests.Engine/Tests_Badges.cs` → no inventory source file found (by name)
- `Praise.Tests.Engine/Tests_DailyStreakReward.cs` → no inventory source file found (by name)
- `Praise.Tests.Logic/DataLayer/TEST_PraiseDetailsDataLayer.cs` → no inventory source file found (by name)
- `Praise.Tests.Logic/MSTestSettings.cs` → `Tests.Praise.Api/MSTestSettings.cs` (by name)
- `Praise.Tests.Logic/Tests_PraiseTypeLogic.cs` → no inventory source file found (by name)
- `Tests/Crypto_Tests.CryptoManager/MSTestSettings.cs` → `Tests.Praise.Api/MSTestSettings.cs` (by name)
- `Tests/Crypto_Tests.CryptoManager/Test1.cs` → no inventory source file found (by name)

### How to run them
- `dotnet test Praise.sln` in `.` (from `Praise.sln`): not run by Orbulus

### Gaps
- endpoint `GET /praise`: The core praise read path of Praise.Api has no test touching it.
- endpoint `POST /gameevent`: Records gamification events that drive badges, achievements, streaks and token prizes; untested writes can corrupt rewards.
- endpoint `POST /gamers`: Creates gamer records that every other Gamey endpoint depends on.
- endpoint `PATCH /gamers/{gamerId}/achievements/{achievementId}`: Mutates a gamer's achievement state with no test coverage.
- endpoint `PATCH /achievements/{achievementId}`: Praise.Api write path for achievements is untested.
- endpoint `GET /gamers/{gamerId}/prizes`: Exposes token prizes tied to the crypto wallet logic, a sensitive area.
- endpoint `GET /gamers/{gamerEmail}`: Looks gamers up by email address (personal data) with no test.
- table `PraiseContainer`: Holds every praise record; the main data of the application.
- table `Wallet`: Stores token wallet data linked to the crypto/ERC-20 logic.
- table `Gamers`: Holds gamer identities, including email, used by all gamification flows.
- table `GameEvents`: Event data from which rewards are calculated.
- table `Achievements`: Achievement state that users see and that the PATCH endpoints change.

### Baseline tests
- Offered: a change request, "Add baseline tests for current behaviour": characterisation tests of what the code does today, with no production code changes.
- In `Gamey.Api`: dotnet-test (the repository's own)
- In `Praise.Api`: dotnet-test (the repository's own)
- Areas: `GET /praise`, `PraiseContainer`, `POST /gameevent`, `GameEvents`, `POST /gamers`, `GET /gamers/{gamerEmail}`, `Gamers`, `GET /gamers/{gamerId}/achievements`, `PATCH /gamers/{gamerId}/achievements/{achievementId}`, `PATCH /achievements/{achievementId}`

## Limitations
- Orbulus has not run any of the repository's commands or tests; every check is listed as not-run.
- Quality review: "every name is grounded in the inventory" did not pass (owner: survey).
- Quality review: "every unit, page, endpoint and table is described" did not pass (owner: survey).
