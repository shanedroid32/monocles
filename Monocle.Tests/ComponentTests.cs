using NUnit.Framework;
using Microsoft.Xna.Framework;
using Monocle;

namespace Monocle.Tests;

/// <summary>
/// Test class for Component functionality.
/// Demonstrates modernized testing approach following CLAUDE.md guidelines.
/// </summary>
[TestFixture]
public class ComponentTests
{
    private TestComponent? _component;
    private Entity? _entity;

    [SetUp]
    public void Setup()
    {
        _component = new TestComponent();
        _entity = new Entity();
    }

    [TearDown]
    public void TearDown()
    {
        _component = null;
        _entity = null;
    }

    [Test]
    public void Component_WhenCreated_ShouldHaveCorrectInitialState()
    {
        // Arrange & Act
        var component = new TestComponent();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(component.Active, Is.True, "Component should be active by default");
            Assert.That(component.Visible, Is.True, "Component should be visible by default");
            Assert.That(component.Entity, Is.Null, "Component should not have an entity initially");
            Assert.That(component.Scene, Is.Null, "Component should not have a scene initially");
        });
    }

    [Test]
    public void Component_WhenAddedToEntity_ShouldReceiveEntityReference()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();

        // Act
        entity.Add(component);

        // Assert
        Assert.That(component.Entity, Is.EqualTo(entity), "Component should reference its parent entity");
    }

    [Test]
    public void Component_WhenRemovedFromEntity_ShouldClearEntityReference()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();
        entity.Add(component);

        // Act
        entity.Remove(component);

        // Assert
        Assert.That(component.Entity, Is.Null, "Component should not reference entity after removal");
    }

    [Test]
    public void RemoveSelf_WhenComponentHasEntity_ShouldRemoveFromEntity()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();
        entity.Add(component);

        // Act
        component.RemoveSelf();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(component.Entity, Is.Null, "Component should not reference entity after RemoveSelf");
            Assert.That(entity.Components.Contains(component), Is.False, "Entity should not contain component after RemoveSelf");
        });
    }

    [Test]
    public void RemoveSelf_WhenComponentHasNoEntity_ShouldNotThrow()
    {
        // Arrange
        var component = new TestComponent();

        // Act & Assert
        Assert.DoesNotThrow(() => component.RemoveSelf(), "RemoveSelf should be safe to call without an entity");
    }

    [Test]
    public void EntityAs_WhenEntityIsCorrectType_ShouldReturnTypedEntity()
    {
        // Arrange
        var player = new TestPlayer();
        var component = new TestComponent();
        player.Add(component);

        // Act
        var result = component.EntityAs<TestPlayer>();

        // Assert
        Assert.That(result, Is.EqualTo(player), "EntityAs should return correctly typed entity");
    }

    [Test]
    public void EntityAs_WhenEntityIsWrongType_ShouldReturnNull()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();
        entity.Add(component);

        // Act
        var result = component.EntityAs<TestPlayer>();

        // Assert
        Assert.That(result, Is.Null, "EntityAs should return null for incorrect type");
    }

    [Test]
    public void ActiveProperty_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var component = new TestComponent();

        // Act
        component.Active = false;

        // Assert
        Assert.That(component.Active, Is.False, "Active property should persist set value");
    }

    [Test]
    public void VisibleProperty_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var component = new TestComponent();

        // Act
        component.Visible = false;

        // Assert
        Assert.That(component.Visible, Is.False, "Visible property should persist set value");
    }
}

/// <summary>
/// Test component for unit testing.
/// </summary>
public class TestComponent : Component
{
    public TestComponent() : base(active: true, visible: true)
    {
    }

    public int UpdateCallCount { get; private set; }
    public int RenderCallCount { get; private set; }
    public bool WasAdded { get; private set; }
    public bool WasRemoved { get; private set; }

    public override void Added(Entity entity)
    {
        base.Added(entity);
        WasAdded = true;
    }

    public override void Removed(Entity entity)
    {
        base.Removed(entity);
        WasRemoved = true;
    }

    public override void Update()
    {
        UpdateCallCount++;
    }

    public override void Render()
    {
        RenderCallCount++;
    }
}

/// <summary>
/// Test entity for unit testing.
/// </summary>
public class TestPlayer : Entity
{
    public TestPlayer() : base()
    {
    }
}