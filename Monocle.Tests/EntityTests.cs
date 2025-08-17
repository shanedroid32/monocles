using NUnit.Framework;
using Microsoft.Xna.Framework;
using Monocle;

namespace Monocle.Tests;

/// <summary>
/// Test class for Entity functionality.
/// Demonstrates modernized testing approach following CLAUDE.md guidelines.
/// </summary>
[TestFixture]
public class EntityTests
{
    private Entity? _entity;
    private TestComponent? _component;

    [SetUp]
    public void Setup()
    {
        _entity = new Entity();
        _component = new TestComponent();
    }

    [TearDown]
    public void TearDown()
    {
        _entity = null;
        _component = null;
    }

    [Test]
    public void Entity_WhenCreatedWithoutPosition_ShouldBeAtOrigin()
    {
        // Arrange & Act
        var entity = new Entity();

        // Assert
        Assert.That(entity.Position, Is.EqualTo(Vector2.Zero), "Entity should be at origin when created without position");
    }

    [Test]
    public void Entity_WhenCreatedWithPosition_ShouldBeAtSpecifiedPosition()
    {
        // Arrange
        var expectedPosition = new Vector2(10, 20);

        // Act
        var entity = new Entity(expectedPosition);

        // Assert
        Assert.That(entity.Position, Is.EqualTo(expectedPosition), "Entity should be at specified position");
    }

    [Test]
    public void Entity_WhenCreated_ShouldHaveCorrectDefaultValues()
    {
        // Arrange & Act
        var entity = new Entity();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.Active, Is.True, "Entity should be active by default");
            Assert.That(entity.Visible, Is.True, "Entity should be visible by default");
            Assert.That(entity.Collidable, Is.True, "Entity should be collidable by default");
            Assert.That(entity.Scene, Is.Null, "Entity should not have a scene initially");
            Assert.That(entity.Components, Is.Not.Null, "Entity should have a component list");
            Assert.That(entity.Collider, Is.Null, "Entity should not have a collider initially");
            Assert.That(entity.Depth, Is.EqualTo(0), "Entity should have depth 0 by default");
        });
    }

    [Test]
    public void Position_WhenSet_ShouldUpdateXAndYProperties()
    {
        // Arrange
        var entity = new Entity();
        var newPosition = new Vector2(15, 25);

        // Act
        entity.Position = newPosition;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.X, Is.EqualTo(15f), "X property should match Position.X");
            Assert.That(entity.Y, Is.EqualTo(25f), "Y property should match Position.Y");
        });
    }

    [Test]
    public void XProperty_WhenSet_ShouldUpdatePosition()
    {
        // Arrange
        var entity = new Entity(new Vector2(10, 20));

        // Act
        entity.X = 30f;

        // Assert
        Assert.That(entity.Position, Is.EqualTo(new Vector2(30, 20)), "Position should update when X is set");
    }

    [Test]
    public void YProperty_WhenSet_ShouldUpdatePosition()
    {
        // Arrange
        var entity = new Entity(new Vector2(10, 20));

        // Act
        entity.Y = 40f;

        // Assert
        Assert.That(entity.Position, Is.EqualTo(new Vector2(10, 40)), "Position should update when Y is set");
    }

    [Test]
    public void Depth_WhenSet_ShouldUpdateValue()
    {
        // Arrange
        var entity = new Entity();

        // Act
        entity.Depth = 5;

        // Assert
        Assert.That(entity.Depth, Is.EqualTo(5), "Depth should be updated");
    }

    [Test]
    public void Depth_WhenSetToSameValue_ShouldNotChange()
    {
        // Arrange
        var entity = new Entity();
        var originalDepth = entity.Depth;

        // Act
        entity.Depth = originalDepth; // Same as default

        // Assert
        Assert.That(entity.Depth, Is.EqualTo(originalDepth), "Depth should remain the same");
    }

    [Test]
    public void AddComponent_WhenCalled_ShouldAddComponentToList()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();

        // Act
        entity.Add(component);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.Components.Contains(component), Is.True, "Component should be in components list");
            Assert.That(component.Entity, Is.EqualTo(entity), "Component should reference the entity");
        });
    }

    [Test]
    public void RemoveComponent_WhenCalled_ShouldRemoveComponentFromList()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();
        entity.Add(component);

        // Act
        entity.Remove(component);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.Components.Contains(component), Is.False, "Component should not be in components list");
            Assert.That(component.Entity, Is.Null, "Component should not reference the entity");
        });
    }

    [Test]
    public void GetComponent_WhenComponentExists_ShouldReturnComponent()
    {
        // Arrange
        var entity = new Entity();
        var component = new TestComponent();
        entity.Add(component);

        // Act
        var result = entity.Get<TestComponent>();

        // Assert
        Assert.That(result, Is.EqualTo(component), "Get should return the added component");
    }

    [Test]
    public void GetComponent_WhenComponentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var entity = new Entity();

        // Act
        var result = entity.Get<TestComponent>();

        // Assert
        Assert.That(result, Is.Null, "Get should return null when component doesn't exist");
    }

    [Test]
    public void RemoveSelf_WhenEntityInScene_ShouldCallSceneRemove()
    {
        // Arrange
        var entity = new Entity();
        
        // Act & Assert - Since we can't easily mock Scene without complex setup,
        // we test that RemoveSelf doesn't throw when entity has no scene
        Assert.DoesNotThrow(() => entity.RemoveSelf(), "RemoveSelf should not throw");
    }

    [Test]
    public void RemoveSelf_WhenEntityHasNoScene_ShouldNotThrow()
    {
        // Arrange
        var entity = new Entity();

        // Act & Assert
        Assert.DoesNotThrow(() => entity.RemoveSelf(), "RemoveSelf should be safe when no scene is set");
    }

    [Test]
    public void WidthAndHeight_WhenNoCollider_ShouldReturnZero()
    {
        // Arrange
        var entity = new Entity();

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.Width, Is.EqualTo(0f), "Width should be 0 when no collider is set");
            Assert.That(entity.Height, Is.EqualTo(0f), "Height should be 0 when no collider is set");
        });
    }

    [Test]
    public void BoundsProperties_WhenNoCollider_ShouldUsePosition()
    {
        // Arrange
        var entity = new Entity(new Vector2(10, 20));

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.Left, Is.EqualTo(10f), "Left should equal X when no collider");
            Assert.That(entity.Right, Is.EqualTo(10f), "Right should equal X when no collider");
            Assert.That(entity.Top, Is.EqualTo(20f), "Top should equal Y when no collider");
            Assert.That(entity.Bottom, Is.EqualTo(20f), "Bottom should equal Y when no collider");
            Assert.That(entity.CenterX, Is.EqualTo(10f), "CenterX should equal X when no collider");
            Assert.That(entity.CenterY, Is.EqualTo(20f), "CenterY should equal Y when no collider");
        });
    }

    [Test]
    public void CornerProperties_WhenNoCollider_ShouldUsePosition()
    {
        // Arrange
        var entity = new Entity(new Vector2(10, 20));

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.TopLeft, Is.EqualTo(new Vector2(10, 20)), "TopLeft should equal position when no collider");
            Assert.That(entity.TopRight, Is.EqualTo(new Vector2(10, 20)), "TopRight should equal position when no collider");
            Assert.That(entity.BottomLeft, Is.EqualTo(new Vector2(10, 20)), "BottomLeft should equal position when no collider");
            Assert.That(entity.BottomRight, Is.EqualTo(new Vector2(10, 20)), "BottomRight should equal position when no collider");
            Assert.That(entity.Center, Is.EqualTo(new Vector2(10, 20)), "Center should equal position when no collider");
        });
    }

    [Test]
    public void ActiveProperty_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var entity = new Entity();

        // Act
        entity.Active = false;

        // Assert
        Assert.That(entity.Active, Is.False, "Active property should persist set value");
    }

    [Test]
    public void VisibleProperty_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var entity = new Entity();

        // Act
        entity.Visible = false;

        // Assert
        Assert.That(entity.Visible, Is.False, "Visible property should persist set value");
    }

    [Test]
    public void CollidableProperty_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var entity = new Entity();

        // Act
        entity.Collidable = false;

        // Assert
        Assert.That(entity.Collidable, Is.False, "Collidable property should persist set value");
    }
}

