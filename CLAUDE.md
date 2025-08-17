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