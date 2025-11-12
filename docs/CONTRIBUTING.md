# Contributing to Brute Gaming Macros

Thank you for considering contributing to Brute Gaming Macros! This document provides guidelines and instructions for contributing.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Pull Request Process](#pull-request-process)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)

---

## Code of Conduct

### Our Pledge
We are committed to providing a welcoming and inclusive environment for all contributors.

### Expected Behavior
- Be respectful and considerate
- Accept constructive criticism gracefully
- Focus on what's best for the project
- Show empathy towards other community members

### Unacceptable Behavior
- Harassment, trolling, or insulting comments
- Publishing others' private information
- Political or off-topic discussions
- Any conduct that would be inappropriate in a professional setting

### Enforcement
Violations may result in temporary or permanent ban from the project.

---

## How Can I Contribute?

### Reporting Bugs
Before submitting a bug report:
1. Check the [FAQ](FAQ.md) for common issues
2. Search [existing issues](https://github.com/epicseo/BruteGamingMacros/issues) to avoid duplicates
3. Verify you're using the latest version

When reporting bugs, use the [bug report template](.github/ISSUE_TEMPLATE/bug_report.md) and include:
- Clear reproduction steps
- Expected vs actual behavior
- System information
- Logs from `%LOCALAPPDATA%\BruteGamingMacros\Logs`

### Suggesting Features
Feature requests are welcome! Use the [feature request template](.github/ISSUE_TEMPLATE/feature_request.md).

Consider:
- Is this a common use case?
- Does it fit the project's scope?
- Would it benefit most users?

### Code Contributions
We welcome code contributions for:
- Bug fixes
- New features
- Performance improvements
- Documentation improvements
- Test coverage

---

## Development Setup

### Prerequisites
- **Windows 10/11 (64-bit)**
- **Visual Studio 2019 or later** (Community Edition is fine)
  - Workload: ".NET desktop development"
  - Component: ".NET Framework 4.8.1 SDK"
- **Git**
- **Optional:** Visual Studio Code + C# extension

### Getting Started

1. **Fork the repository**
   ```bash
   # On GitHub, click "Fork" button
   ```

2. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR_USERNAME/BruteGamingMacros.git
   cd BruteGamingMacros
   ```

3. **Add upstream remote**
   ```bash
   git remote add upstream https://github.com/epicseo/BruteGamingMacros.git
   ```

4. **Open solution in Visual Studio**
   ```
   File → Open → Project/Solution → BruteGamingMacros.sln
   ```

5. **Restore NuGet packages**
   ```
   Right-click solution → Restore NuGet Packages
   ```

6. **Build the solution**
   ```
   Build → Build Solution (Ctrl+Shift+B)
   ```

7. **Run the application**
   ```
   Debug → Start Debugging (F5)
   ```

### Project Structure
```
BruteGamingMacros/
├── Core/
│   └── Engine/          # Performance-critical code
├── Forms/               # UI components
├── Model/               # Data models
├── Utils/               # Utility classes
├── Resources/           # Images, sounds
├── docs/                # Documentation
├── build/               # Build scripts
├── installer/           # NSIS installer
└── tests/               # Unit tests (future)
```

---

## Pull Request Process

### Before Submitting
1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   # or
   git checkout -b fix/bug-description
   ```

2. **Keep your fork up to date**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

3. **Make your changes**
   - Follow coding standards (see below)
   - Add comments for complex logic
   - Update documentation if needed

4. **Test your changes**
   - Build in both Debug and Release
   - Test on clean Windows VM if possible
   - Verify no new warnings

5. **Commit your changes**
   ```bash
   git add .
   git commit -m "feat: add new feature"
   ```

   **Commit message format:**
   - `feat:` - New feature
   - `fix:` - Bug fix
   - `docs:` - Documentation changes
   - `refactor:` - Code refactoring
   - `test:` - Adding tests
   - `chore:` - Build/tooling changes

6. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

### Submitting Pull Request
1. Go to GitHub and create a Pull Request
2. Fill out the PR template
3. Link related issues (e.g., "Closes #123")
4. Wait for code review

### PR Review Process
- Maintainers will review your code
- You may be asked to make changes
- Once approved, your PR will be merged
- Your contribution will be credited in release notes

### PR Checklist
- [ ] Code follows project style guidelines
- [ ] No new compiler warnings
- [ ] Builds successfully (Debug and Release)
- [ ] Tested manually
- [ ] Documentation updated (if applicable)
- [ ] CHANGELOG.md updated (for significant changes)
- [ ] No merge conflicts
- [ ] Commits are clean and well-described

---

## Coding Standards

### C# Style Guide

**Naming Conventions:**
```csharp
// Classes: PascalCase
public class SuperiorMemoryEngine { }

// Methods: PascalCase
public void ReadMemory() { }

// Properties: PascalCase
public int CurrentHP { get; set; }

// Private fields: camelCase with underscore
private int _cacheSize;

// Constants: PascalCase
private const int MAX_RETRY_COUNT = 3;

// Local variables: camelCase
int retryCount = 0;
```

**Formatting:**
```csharp
// Braces on new line (Allman style)
if (condition)
{
    DoSomething();
}

// Spaces around operators
int result = x + y;

// No spaces inside parentheses
Method(param1, param2);

// One line per statement
var x = 1;
var y = 2;
```

**Best Practices:**
```csharp
// Use var for obvious types
var client = new Client();
var count = list.Count;

// Explicit types for primitives
int userId = 123;
string userName = "Player";

// Always use braces
if (condition)
{
    statement;
}

// Prefer string interpolation
string message = $"Hello, {name}!";

// Use nameof() for parameter names
throw new ArgumentNullException(nameof(parameter));

// Dispose resources properly
using (var reader = new StreamReader(path))
{
    // ...
}

// Async methods end with "Async"
public async Task<int> LoadDataAsync()
{
    // ...
}
```

### Error Handling
```csharp
// Catch specific exceptions
try
{
    ReadMemory();
}
catch (UnauthorizedAccessException ex)
{
    DebugLogger.Error(ex, "Admin rights required");
    throw;
}
catch (Exception ex)
{
    DebugLogger.Error(ex, "Unexpected error");
    // Handle or rethrow
}

// Don't swallow exceptions silently
// BAD:
catch { }

// GOOD:
catch (Exception ex)
{
    DebugLogger.Error(ex, "Error in critical operation");
    return defaultValue;
}
```

### Logging
```csharp
// Use DebugLogger
DebugLogger.Info("Operation started");
DebugLogger.Warning("Unusual condition detected");
DebugLogger.Error(exception, "Operation failed");

// Include context
DebugLogger.Error(ex, $"Failed to read memory at address 0x{address:X8}");
```

### Comments
```csharp
// Use XML documentation for public APIs
/// <summary>
/// Reads a 32-bit unsigned integer from the specified memory address.
/// </summary>
/// <param name="address">The memory address to read from.</param>
/// <returns>The value at the specified address, or 0 if the read fails.</returns>
public uint ReadUInt32(uint address)
{
    // Implementation comments for complex logic
    // This uses batch reading to improve performance
    return BatchRead(address, 4);
}
```

### Threading
```csharp
// Use locks for thread safety
private readonly object _lock = new object();

public void ThreadSafeMethod()
{
    lock (_lock)
    {
        // Critical section
    }
}

// Prefer async/await over blocking
public async Task<string> FetchDataAsync()
{
    return await httpClient.GetStringAsync(url);
}
```

---

## Testing Guidelines

### Manual Testing
Before submitting a PR, test:
1. **Basic functionality** - Does it work as intended?
2. **Edge cases** - What if input is null/empty/invalid?
3. **Performance** - Is it fast enough?
4. **Memory leaks** - Run for extended period, check Task Manager
5. **Error handling** - What happens when things go wrong?

### Test Checklist
- [ ] Tested on clean Windows 10/11 VM
- [ ] Tested with different server modes (MR/HR/LR)
- [ ] Tested with multiple profiles
- [ ] No memory leaks (checked with Task Manager)
- [ ] No excessive CPU usage
- [ ] No new compiler warnings
- [ ] All features still work (no regressions)

### Future: Unit Tests
We plan to add unit tests. When that happens:
- Write tests for new features
- Maintain existing test coverage
- Use mocking for external dependencies
- Aim for >80% code coverage on critical paths

---

## Documentation

### When to Update Documentation
Update documentation when you:
- Add a new feature
- Change existing behavior
- Fix a significant bug
- Add configuration options
- Change system requirements

### Documentation Files
- **README.md** - Overview, quick start
- **INSTALL.md** - Installation instructions
- **FAQ.md** - Common questions
- **CHANGELOG.md** - Version history
- **CONTRIBUTING.md** - This file
- **Code comments** - Inline documentation

### Documentation Style
- Use clear, concise language
- Include code examples
- Add screenshots for UI changes
- Keep formatting consistent (Markdown)
- Test all links

---

## Contributor License Agreement

By submitting a Pull Request, you agree that:
1. Your contribution is your own original work
2. You grant us a license to use your contribution under the MIT License
3. You have the legal right to grant this license
4. Your contribution does not violate any third-party rights

---

## Recognition

### Contributors
All contributors are credited in:
- GitHub contributors page
- Release notes (for significant contributions)
- CHANGELOG.md (for major features)

### Hall of Fame
Outstanding contributors may be featured in the README.md.

---

## Getting Help

### Questions?
- **Documentation:** Read [README.md](../README.md), [FAQ.md](FAQ.md)
- **Issues:** Search [existing issues](https://github.com/epicseo/BruteGamingMacros/issues)
- **Discussions:** GitHub Discussions (if enabled)
- **Contact:** Open an issue with `[QUESTION]` prefix

### Stuck on Something?
Don't hesitate to ask for help:
1. Open an issue describing what you're trying to do
2. Tag it with `question` or `help wanted`
3. We'll guide you in the right direction

---

## Project Roadmap

See [CHANGELOG.md - Future Roadmap](CHANGELOG.md#future-roadmap) for planned features.

Want to work on a roadmap item? Comment on the related issue or create one!

---

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

See [LICENSE](../LICENSE) for details.

---

**Thank you for contributing to Brute Gaming Macros!**

Your contributions make this project better for everyone. 🎮🚀

---

**Questions about contributing?**
Open an issue or reach out to the maintainers.

**Happy coding!**
