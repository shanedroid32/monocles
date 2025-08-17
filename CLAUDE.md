# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Monocle is a C# game engine framework built on top of MonoGame 3.8.2, targeting .NET 9. It provides a complete game development framework with entity-component-system architecture, collision detection, input handling, graphics, particles, and utilities.

## Build Commands

```bash
# Build the solution
dotnet build

# Build specific configuration
dotnet build -c Debug
dotnet build -c Release

# Run the example
dotnet run --project Example

# Run unit tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Clean build artifacts
dotnet clean
```

## Architecture

### Core Classes
- **Engine**: Main game class that inherits from MonoGame's Game class. Manages graphics, input, timing, and scene management
- **Scene**: Container that manages entities, renderers, collision detection, and game loop timing. Each scene has its own entity list, tag lists, and tracker
- **Entity**: Game objects with position, visibility, activity state, and component lists. Uses component-based architecture
- **Component**: Modular behavior classes that can be attached to entities (graphics, logic, collision, etc.)

### Key Systems
- **Collision**: Grid-based and shape-based collision detection with Collider base classes (Circle, Hitbox, Grid, etc.)
- **Graphics**: Texture atlases, sprites, tilesets, particle systems, and text rendering
- **Input**: Virtual input system with buttons, axes, joysticks supporting multiple input devices
- **Utilities**: Math helpers (Calc), drawing utilities (Draw), easing functions (Ease), cameras, and more

### Component Organization
- **Graphics Components**: Image, Sprite, Text, ParticleEmitter, TileGrid
- **Logic Components**: Alarm, Coroutine, StateMachine, Tween, Shaker
- **Collision Components**: CollidableComponent for collision handling

## Project Structure
- `Monocle/`: Core engine framework (Library project)
- `Example/`: Sample game demonstrating engine usage (Executable project)
- `Monocle.Tests/`: Unit tests for the engine (NUnit test project)
- Content is managed through MonoGame Content Pipeline

## Documentation Standards
Follow XML documentation standards from `.cursor/rules/documenting-files.mdc`:
- Use `///` XML comments for all public members
- Include `<summary>`, `<param>`, `<returns>`, and `<exception>` tags
- Document purpose, not implementation details
- Use present tense and active voice

## Platform Configuration
- Targets .NET 9 with MonoGame.Framework.DesktopGL
- Uses latest C# language version
- Nullable disabled, ImplicitUsings disabled
- Defines MONOGAME compiler constant
- SYSLIB0011 warnings suppressed (BinaryFormatter obsolescence)

## Modernization Guidelines

### Code Standards & Best Practices

#### Modern C# Features (Priority: High)
- **Use expression-bodied members** for simple properties and methods:
  ```csharp
  public Scene? Scene => Entity?.Scene;
  public bool IsActive => Active && Visible;
  ```
- **Adopt pattern matching** and switch expressions:
  ```csharp
  public InputType GetInputType(object input) => input switch
  {
      Keys => InputType.Keyboard,
      Buttons => InputType.GamePad,
      _ => InputType.Unknown
  };
  ```
- **Use target-typed new expressions**:
  ```csharp
  Vector2 position = new(0, 0);
  Dictionary<Type, List<Component>> components = new();
  ```
- **Prefer records for immutable data structures**:
  ```csharp
  public readonly record struct GameState(float TimeActive, bool Paused);
  ```

#### Performance Optimization (Priority: High)
- **Implement object pooling** for frequently allocated objects:
  ```csharp
  private static readonly ObjectPool<List<Entity>> EntityListPool = new();
  ```
- **Use Span<T> and Memory<T>** for buffer operations and string processing
- **Avoid LINQ in hot paths** - use for loops and cached collections
- **Minimize boxing** in utility methods and prefer generic overloads
- **Use ArrayPool<T>** for temporary arrays in performance-critical code

#### Memory Management
- **Pool collections** that are frequently created/destroyed
- **Cache expensive calculations** in components when possible
- **Use struct-based alternatives** for small, short-lived objects
- **Implement IDisposable** properly for resources that need cleanup

### Architecture Patterns (Priority: Medium)

#### Dependency Injection Support
- **Create service interfaces** for major engine systems:
  ```csharp
  public interface IInputService { }
  public interface IRenderingService { }
  public interface ISceneManager { }
  ```
- **Support optional DI container** integration for advanced scenarios
- **Avoid static dependencies** where possible, prefer injection

#### Async/Await Integration
- **Use async methods** for asset loading and I/O operations:
  ```csharp
  public async ValueTask<T> LoadAssetAsync<T>(string path, CancellationToken cancellationToken = default);
  ```
- **Support progress reporting** for long-running operations
- **Use CancellationToken** for cancellable operations
- **Prefer ValueTask** over Task for frequently called async methods

#### Event System Enhancement
- **Use strongly-typed events** with record structs:
  ```csharp
  public readonly record struct EntityEvent(Entity Entity, string EventType);
  ```
- **Implement modern event bus** patterns for component communication
- **Support async event handlers** where appropriate

### Testing Requirements (Priority: High)

#### Unit Testing
- **All new public APIs** must have unit tests
- **Use test doubles** for dependencies and external systems
- **Test both success and failure scenarios**
- **Include performance benchmarks** for critical paths

#### Test Organization
- **Create test base classes** for common engine testing scenarios
- **Mock engine services** for isolated component testing
- **Use descriptive test names** following Given_When_Then pattern
- **Group related tests** in logical test classes

### Component Development

#### Component Best Practices
- **Implement proper lifecycle methods** (Added, Removed, EntityAdded, EntityRemoved)
- **Use services through dependency injection** when available
- **Avoid direct static references** to engine systems
- **Document component dependencies** clearly

#### Entity-Component Patterns
- **Prefer composition over inheritance** for entity behaviors
- **Use component queries** for efficient entity filtering
- **Implement component communication** through events rather than direct references
- **Keep components focused** on single responsibilities

### Rendering & Graphics

#### Modern Rendering Practices
- **Use SpriteBatch efficiently** - minimize state changes
- **Implement render layers** for proper draw order
- **Support custom shaders** through Effect parameters
- **Cache texture atlas lookups** for better performance

#### Graphics Components
- **Implement proper bounds calculation** for culling
- **Support animation blending** and state machines
- **Use texture atlases** to reduce draw calls
- **Implement LOD systems** for complex graphics

### Input Handling

#### Input System Design
- **Support multiple input devices** simultaneously
- **Implement input buffering** for precise timing
- **Use virtual input abstractions** for remapping
- **Support async input processing** for UI systems

### Asset Management

#### Asset Loading
- **Implement async asset loading** with progress reporting
- **Support hot reloading** during development
- **Use dependency tracking** for asset relationships
- **Implement asset streaming** for large worlds

### Error Handling

#### Exception Management
- **Use specific exception types** rather than generic exceptions
- **Provide meaningful error messages** with context
- **Log errors appropriately** for debugging
- **Implement graceful degradation** where possible

### Documentation Requirements

#### Code Documentation
- **Document all public APIs** with XML comments
- **Include usage examples** for complex systems
- **Document performance characteristics** for critical methods
- **Explain architectural decisions** in design docs

#### Architecture Documentation
- **Maintain system diagrams** for complex interactions
- **Document component relationships** and dependencies
- **Explain data flow** through major systems
- **Keep migration guides** for breaking changes

### Migration Strategy

#### Gradual Modernization
1. **Phase 1**: Enable nullable reference types with warning suppression
2. **Phase 2**: Implement object pooling and performance optimizations
3. **Phase 3**: Add dependency injection and async support
4. **Phase 4**: Enhance ECS system with modern patterns

#### Backward Compatibility
- **Maintain existing APIs** during transition periods
- **Provide migration helpers** for breaking changes
- **Use obsolete attributes** to guide deprecation
- **Document upgrade paths** clearly