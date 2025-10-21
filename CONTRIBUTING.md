# Contributing to Brute Gaming Macros

Thank you for your interest in contributing to Brute Gaming Macros! This document provides guidelines and instructions for contributing to the project.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Submitting Changes](#submitting-changes)
- [Testing](#testing)
- [Documentation](#documentation)

---

## Code of Conduct

### Our Standards
- Be respectful and inclusive
- Welcome newcomers and help them learn
- Focus on constructive criticism
- Respect differing viewpoints
- Accept responsibility for mistakes

### Unacceptable Behavior
- Harassment or discriminatory language
- Trolling or insulting comments
- Public or private harassment
- Publishing others' private information

---

## Getting Started

### Prerequisites
- Git installed and configured
- Visual Studio 2017 or later (2022 recommended)
- .NET Framework 4.8.1 SDK
- Basic knowledge of C# and Windows Forms

### Fork and Clone
```bash
# Fork the repository on GitHub first, then:
git clone https://github.com/YOUR_USERNAME/BruteGamingMacros.git
cd BruteGamingMacros

# Add upstream remote
git remote add upstream https://github.com/epicseo/BruteGamingMacros.git
```

### Stay Updated
```bash
# Fetch latest changes from upstream
git fetch upstream

# Merge into your local main branch
git checkout main
git merge upstream/main
```

---

## Development Setup

### 1. Open Solution
```bash
# Open in Visual Studio
start BruteGamingMacros.sln

# OR open in VS Code (for quick edits)
code .
```

### 2. Restore Packages
```bash
# Visual Studio: Build > Restore NuGet Packages
# OR command line:
nuget restore BruteGamingMacros.sln
```

### 3. Build Project
```bash
# Visual Studio: Build > Build Solution (Ctrl+Shift+B)
# OR command line:
msbuild BruteGamingMacros.sln /p:Configuration=Debug
```

### 4. Run with Debugger
- Press F5 in Visual Studio
- Or right-click project > Debug > Start New Instance
- **Important**: Run Visual Studio as Administrator (required for memory access)

---

## Coding Standards

### C# Style Guide

#### Naming Conventions
```csharp
// Classes: PascalCase
public class SuperiorInputEngine { }

// Methods: PascalCase
public void SendKeyPress() { }

// Properties: PascalCase
public int CurrentAPS { get; set; }

// Private fields: camelCase with underscore
private int _totalActions;

// Constants: PascalCase or UPPER_CASE
public const int MaxRetries = 3;
public const int ULTRA_SPAM_DELAY_MS = 1;

// Interfaces: IPascalCase
public interface IAction { }
```

#### Code Organization
```csharp
// Order within class:
1. Constants
2. Static fields
3. Fields
4. Constructors
5. Properties
6. Methods
7. Nested classes

// Example:
public class Example
{
    // 1. Constants
    private const int DEFAULT_DELAY = 50;

    // 2. Static fields
    private static readonly object _lock = new object();

    // 3. Fields
    private int _counter;

    // 4. Constructors
    public Example() { }

    // 5. Properties
    public int Counter { get; set; }

    // 6. Methods
    public void DoSomething() { }

    // 7. Nested classes
    private class NestedClass { }
}
```

#### Formatting
```csharp
// Use braces even for single-line statements
if (condition)
{
    DoSomething();
}

// Proper spacing
public void Method(int param1, int param2)
{
    var result = param1 + param2;
}

// Line length: max 120 characters (soft limit)
```

### Performance Guidelines

1. **Use appropriate data structures**
   ```csharp
   // Good: Dictionary for lookups
   private Dictionary<int, CachedValue> memoryCache;

   // Bad: List for lookups
   private List<CachedValue> memoryCache;
   ```

2. **Dispose resources properly**
   ```csharp
   public class MemoryReader : IDisposable
   {
       public void Dispose()
       {
           // Clean up resources
       }
   }
   ```

3. **Use locks for thread safety**
   ```csharp
   lock (cacheLock)
   {
       // Thread-safe operations
   }
   ```

### Documentation

#### XML Comments
```csharp
/// <summary>
/// Sends a keyboard key press using hardware-level SendInput API
/// </summary>
/// <param name="key">The key to press</param>
/// <returns>True if successful, false otherwise</returns>
public bool SendKeyPress(Keys key)
{
    // Implementation
}
```

#### Inline Comments
```csharp
// Good: Explain WHY, not WHAT
// Cache expired, remove it
memoryCache.Remove(address);

// Bad: Obvious comment
// Remove from cache
memoryCache.Remove(address);
```

---

## Submitting Changes

### Branch Naming

Follow this convention:
```bash
feature/descriptive-name    # New features
bugfix/issue-description     # Bug fixes
hotfix/critical-fix          # Critical production fixes
refactor/component-name      # Code refactoring
docs/what-changed           # Documentation updates
```

Examples:
```bash
git checkout -b feature/add-skill-queue
git checkout -b bugfix/memory-leak-in-cache
git checkout -b docs/update-readme
```

### Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```bash
# Format:
<type>(<scope>): <subject>

# Types:
feat:     New feature
fix:      Bug fix
docs:     Documentation changes
style:    Code style changes (formatting, etc.)
refactor: Code refactoring
test:     Adding or updating tests
chore:    Maintenance tasks

# Examples:
feat(engine): add skill queue system
fix(memory): resolve cache memory leak
docs(readme): update installation instructions
refactor(profile): improve thread safety
test(engine): add input engine unit tests
```

### Pull Request Process

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/my-feature
   ```

2. **Make Changes**
   - Write code following style guide
   - Add tests for new features
   - Update documentation

3. **Test Locally**
   ```bash
   # Build all configurations
   msbuild BruteGamingMacros.sln /p:Configuration=Debug /t:Rebuild

   # Run tests (once implemented)
   nunit3-console.exe BruteGamingMacros.Tests\bin\Debug\BruteGamingMacros.Tests.dll

   # Test the application
   .\bin\Debug\BruteGamingMacros.exe
   ```

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat(engine): add skill queue system"
   ```

5. **Push to Fork**
   ```bash
   git push origin feature/my-feature
   ```

6. **Create Pull Request**
   - Go to GitHub repository
   - Click "New Pull Request"
   - Select your branch
   - Fill in template (see below)

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tested locally
- [ ] All builds pass
- [ ] Tests added/updated

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings generated
- [ ] CHANGELOG.md updated (for releases)

## Related Issues
Closes #123
```

### Code Review Process

Your PR will be reviewed for:
1. **Code Quality**: Follows standards, well-structured
2. **Functionality**: Works as intended, no bugs
3. **Performance**: No performance regressions
4. **Tests**: Adequate test coverage
5. **Documentation**: Properly documented

---

## Testing

### Unit Tests

We use NUnit for testing. Example:

```csharp
using NUnit.Framework;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class SuperiorInputEngineTests
    {
        private SuperiorInputEngine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new SuperiorInputEngine();
        }

        [Test]
        public void SendKeyPress_ValidKey_ReturnsTrue()
        {
            // Arrange
            var key = Keys.F1;

            // Act
            var result = _engine.SendKeyPress(key);

            // Assert
            Assert.IsTrue(result);
        }

        [TearDown]
        public void Cleanup()
        {
            _engine = null;
        }
    }
}
```

### Running Tests

```bash
# Visual Studio: Test > Run All Tests

# Command line with NUnit Console
nunit3-console.exe BruteGamingMacros.Tests\bin\Debug\BruteGamingMacros.Tests.dll

# Generate coverage report (optional)
dotnet test /p:CollectCoverage=true
```

### Integration Testing

For features involving game interaction:
1. Test against actual game client
2. Verify memory addresses are correct
3. Test with different server configurations (MR/HR/LR)
4. Test edge cases (character dead, disconnected, etc.)

---

## Documentation

### What to Document

1. **Public APIs**: All public methods and properties
2. **Complex Logic**: Algorithms, calculations, workarounds
3. **Configuration**: Settings and their effects
4. **Architecture**: Design decisions and patterns

### Where to Document

- **Code**: XML comments for APIs
- **README.md**: User-facing documentation
- **ARCHITECTURE.md**: System design
- **CHANGELOG.md**: Version history
- **This file**: Contribution guidelines

---

## Project Structure

```
BruteGamingMacros/
├── Core/
│   └── Engine/           # Performance-critical engines
│       ├── SuperiorInputEngine.cs
│       ├── SuperiorMemoryEngine.cs
│       └── SuperiorSkillSpammer.cs
├── Forms/                # UI components (Windows Forms)
├── Model/                # Business logic and data models
├── Utils/                # Utility classes and helpers
├── Resources/            # Embedded resources (icons, sounds)
├── Properties/           # Assembly info and settings
├── .github/
│   └── workflows/        # GitHub Actions CI/CD
├── CHANGELOG.md          # Version history
├── CONTRIBUTING.md       # This file
├── DEPLOYMENT.md         # Build and release guide
├── README.md             # User documentation
└── BruteGamingMacros.sln # Visual Studio solution
```

---

## Common Tasks

### Adding a New Feature

1. Create feature branch
2. Implement feature in appropriate namespace
3. Add tests
4. Update documentation
5. Submit PR

### Fixing a Bug

1. Create bugfix branch
2. Write test that reproduces bug
3. Fix the bug
4. Verify test passes
5. Submit PR with "Fixes #issue_number"

### Updating Documentation

1. Create docs branch
2. Update relevant files
3. Submit PR
4. Label as "documentation"

---

## Performance Optimization

### Profiling

Use Visual Studio Profiler:
1. Debug > Performance Profiler
2. Select CPU Usage or Memory Usage
3. Run application
4. Analyze results

### Benchmarking

Use the built-in benchmark tools:
```csharp
var engine = new SuperiorInputEngine();
var result = engine.RunBenchmark(SpeedMode.Ultra, durationSeconds: 5);
Console.WriteLine(result.ToString());
```

### Optimization Checklist
- [ ] Use appropriate data structures
- [ ] Minimize allocations in hot paths
- [ ] Cache frequently accessed data
- [ ] Use batch operations when possible
- [ ] Profile before and after changes

---

## Release Process

See [DEPLOYMENT.md](DEPLOYMENT.md) for detailed release instructions.

Quick version:
1. Update version numbers
2. Update CHANGELOG.md
3. Create git tag: `git tag -a v2.0.1 -m "Release 2.0.1"`
4. Push tag: `git push origin v2.0.1`
5. GitHub Actions automatically builds and releases

---

## Getting Help

- **Issues**: https://github.com/epicseo/BruteGamingMacros/issues
- **Discussions**: Use GitHub Discussions for questions
- **Discord**: [Join server links in README]

---

## License

By contributing to Brute Gaming Macros, you agree that your contributions will be licensed under the MIT License.

---

## Recognition

Contributors will be:
- Listed in README.md (Contributors section)
- Credited in CHANGELOG.md for their changes
- Mentioned in release notes

Thank you for contributing! 🎉
