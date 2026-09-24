# Blackjack Specialist

A Blackjack game developed in C# using WPF. The project was created as a software engineering project with a focus on object-oriented programming, SOLID principles, layered architecture, unit testing, and persistent data storage.

The application allows a player to play Blackjack against a computer-controlled dealer. The game can be played without creating an account, but users can optionally register and log in to have their game statistics saved between sessions.

## Features

- Play Blackjack against a computer-controlled dealer
- Hit and stand actions
- Automatic dealer turn
- Blackjack detection
- Bust detection
- Dealer's hidden card
- Automatic determination of win, loss, or draw
- Optional user registration and login
- Password hashing using BCrypt
- Persistent user data using SQLite
- Persistent statistics for:
  - Wins
  - Losses
  - Draws
- Statistics are displayed in the application
- Unit tests for the domain, application, and infrastructure layers
- Dependency injection through constructors
- Separation between game logic, application services, database access, and user interface

---

# Building, Testing and Running the Project

## Requirements

The project is intended to be developed and run using **Visual Studio**.

You will need:

- Visual Studio with the **.NET desktop development** workload
- A compatible .NET SDK
- Windows, since the application uses WPF
- Git, if cloning the repository

## Getting the Project

Clone the repository from GitHub:

```text
https://github.com/Thafirst/Blackjack-Specialist
```

Alternatively, in Visual Studio:

1. Open **Visual Studio**.
2. Select **Clone a repository**.
3. Enter the repository URL.
4. Choose a local folder.
5. Click **Clone**.

After cloning, open the solution file:

```text
Blackjack.slnx
```

in Visual Studio.

## Building the Project

After opening the solution:

1. In **Solution Explorer**, right-click the solution.
2. Select **Build Solution**.

Alternatively, use:

```text
Build → Build Solution
```

or press:

```text
Ctrl + Shift + B
```

Visual Studio will build all projects in the solution and report any compilation errors in the **Error List**.

The solution contains the following projects:

```text
Blackjack.Application
Blackjack.Application.Tests

Blackjack.Domain
Blackjack.Domain.Tests

Blackjack.Infrastructure
Blackjack.Infrastructure.Tests

Blackjack.Testing.Utils

Blackjack.Wpf
```

## Running the Tests

The project uses **xUnit** for unit testing.

To run all tests in Visual Studio:

1. Open the solution.
2. Go to:

```text
Test → Test Explorer
```

3. Wait for Visual Studio to discover the tests.
4. Click **Run All** in the Test Explorer.

The tests are separated according to the architectural layer they test:

```text
Blackjack.Domain.Tests
    Tests for the core Blackjack domain logic

Blackjack.Application.Tests
    Tests for application services

Blackjack.Infrastructure.Tests
    Tests for repositories and database-related functionality
```

`Blackjack.Testing.Utils` contains test-specific implementations used by the test projects.

A successful test run should show all discovered tests as passed in the Test Explorer.

### Running Individual Tests

Tests can also be run individually by:

1. Opening **Test Explorer**.
2. Expanding the relevant test project.
3. Selecting a test.
4. Clicking **Run**.

This can be useful when developing or debugging a specific class.

## Running the Application

To run the Blackjack application:

1. In **Solution Explorer**, right-click `Blackjack.Wpf`.
2. Select:

```text
Set as Startup Project
```

3. Press:

```text
F5
```

or select:

```text
Debug → Start Debugging
```

The WPF application will start.

The game can be played immediately without logging in.

### Playing Without an Account

A user does not need to register or log in to play the game.

The basic flow is:

1. Start a game.
2. Choose **Hit** or **Stand** during the player's turn.
3. The dealer automatically plays after the player stands.
4. The game determines the result.
5. A new game can then be started.

If the player is not logged in, the game result is not associated with a user and no statistics are saved.

### Using an Account

Users can optionally register an account through the application.

After registering, the user is automatically logged in.

A returning user can enter their username and password and log in.

When a user is logged in, their:

- Wins
- Losses
- Draws

are stored in the SQLite database and loaded again when they log in during a later application session.

## Database

The application uses **SQLite** for persistent user and statistics data.

The database is stored in:

```text
blackjack.db
```

The Entity Framework Core `BlackjackDbContextFactory` configures the SQLite database connection.

Database migrations are applied when the WPF application starts.

The database contains data for:

- Users
- User credentials
- User statistics

Passwords are not stored as plain text. Passwords are hashed using **BCrypt** before being stored in the database.

---

# Software Architecture

The project uses a **layered architecture** with separate projects for the domain, application logic, infrastructure, and user interface.

The main structure can be found here:

https://imgur.com/a/knhYeeb

## Domain Layer

The `Blackjack.Domain` project contains the core business logic of the application.

This layer does not depend on the WPF user interface or the database.

Examples of domain classes include:

- `BlackjackGame`
- `Player`
- `Dealer`
- `Hand`
- `Deck`
- `Card`
- `User`
- `UserCredential`
- `UserStatistics`

The domain also contains game-related enumerations, interfaces, and result objects.

For example, the Blackjack rules for hitting, standing, dealer behavior, calculating hand values, busting, Blackjack, and determining the winner are implemented in the domain layer.

This means the core game logic can be tested independently from the graphical interface.

## Application Layer

The `Blackjack.Application` project contains application-level services and interfaces.

Examples include:

- `GameService`
- `UserService`
- `UserStatisticsService`
- `IUserRepository`
- `IUserStatisticsRepository`
- `IPasswordHasher`

The application layer coordinates operations between the user interface and the domain/infrastructure layers.

For example, `GameService` manages the currently active `BlackjackGame` and exposes operations such as:

- Start game
- Hit
- Stand
- Play dealer turn
- Determine result

`UserService` handles registration and login, while `UserStatisticsService` handles creating and updating persistent statistics.

The application layer uses interfaces for external dependencies. This allows implementations such as repositories and password hashing services to be replaced or tested independently.

## Infrastructure Layer

The `Blackjack.Infrastructure` project contains implementations that interact with external systems.

This includes:

- Entity Framework Core
- SQLite
- Database context
- Database migrations
- Repositories
- BCrypt password hashing
- Random number generation

The `BlackjackDbContext` defines the database structure and relationships.

Repositories such as `UserRepository` and `UserStatisticsRepository` provide the application layer with access to persistent data without requiring the application services to directly interact with Entity Framework Core.

The `BCryptPasswordHasher` implements the application's `IPasswordHasher` interface and provides secure password hashing and verification.

The `Randomizer` implements the domain's `IRandomizer` interface and provides random numbers for shuffling the deck.

## WPF Presentation Layer

The `Blackjack.Wpf` project contains the graphical user interface.

The application uses WPF and follows a ViewModel-based approach.

`MainWindow` is responsible for the WPF window and connecting the application services to the user interface.

`GameViewModel` manages the presentation state of the Blackjack game, including:

- Player cards
- Dealer cards
- Hand values
- Game status
- Button states
- Hidden dealer card state
- User statistics

The ViewModel receives result objects from the application layer and converts them into state that can be displayed by WPF.

The domain objects therefore do not need to know anything about WPF controls or XAML.

## Dependency Direction

The dependency direction is designed to keep the core business logic independent:

```text
WPF
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Domain
```

The WPF project uses both Application and Infrastructure because it is responsible for composing the application and providing the concrete implementations required at runtime.

The Domain project does not depend on WPF, Entity Framework Core, SQLite, or other infrastructure concerns.

This separation makes the game logic easier to test and reduces coupling between the different parts of the application.

## Dependency Injection

Dependencies are passed into classes through constructors.

For example, `GameService` receives an `IRandomizer`:

```csharp
public GameService(IRandomizer randomizer)
{
    _randomizer = randomizer;
}
```

This means the service does not need to create its own randomizer and can instead receive whichever implementation is appropriate.

The same principle is used for repositories and password hashing:

```text
UserService
    ↓
IUserRepository
    ↓
UserRepository
```

and:

```text
UserService
    ↓
IPasswordHasher
    ↓
BCryptPasswordHasher
```

This follows the Dependency Inversion Principle and makes the application easier to test.

## Testing Architecture

The project contains separate test projects for the different layers.

```text
Blackjack.Domain.Tests
Blackjack.Application.Tests
Blackjack.Infrastructure.Tests
```

The tests use xUnit.

The domain tests verify the core Blackjack rules without requiring a database or user interface.

The application tests verify services such as:

- `GameService`
- `UserService`
- `UserStatisticsService`

The infrastructure tests verify functionality such as:

- User repositories
- User statistics repositories
- SQLite database interaction
- Database context configuration

`Blackjack.Testing.Utils` contains reusable test implementations, such as deterministic randomizers, which allow game behavior to be tested without relying on unpredictable random values.

This separation allows the majority of the application's important logic to be tested independently from the WPF interface.

---

# Technologies

The project uses:

- **C#**
- **.NET**
- **WPF**
- **Entity Framework Core**
- **SQLite**
- **BCrypt**
- **xUnit**
- **Git / GitHub**

The project is intended primarily as a software engineering project demonstrating object-oriented programming, SOLID principles, layered architecture, dependency injection, database persistence, and automated testing.
