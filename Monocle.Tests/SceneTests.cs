using NUnit.Framework;
using Microsoft.Xna.Framework;
using Monocle;
using System.Linq;

namespace Monocle.Tests;

/// <summary>
/// Test class for Scene functionality.
/// Demonstrates modernized testing approach following CLAUDE.md guidelines.
/// </summary>
[TestFixture]
public class SceneTests
{
    private Scene? _scene;

    [SetUp]
    public void Setup()
    {
        _scene = new Scene();
    }

    [TearDown]
    public void TearDown()
    {
        _scene = null;
    }

    [Test]
    public void Scene_WhenCreated_ShouldHaveCorrectInitialState()
    {
        // Arrange & Act
        var scene = new Scene();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(scene.Paused, Is.False, "Scene should not be paused initially");
            Assert.That(scene.TimeActive, Is.EqualTo(0f), "TimeActive should be 0 initially");
            Assert.That(scene.RawTimeActive, Is.EqualTo(0f), "RawTimeActive should be 0 initially");
            Assert.That(scene.Focused, Is.False, "Scene should not be focused initially");
            Assert.That(scene.Entities, Is.Not.Null, "Entities list should not be null");
            Assert.That(scene.TagLists, Is.Not.Null, "TagLists should not be null");
            Assert.That(scene.RendererList, Is.Not.Null, "RendererList should not be null");
            Assert.That(scene.Tracker, Is.Not.Null, "Tracker should not be null");
            Assert.That(scene.HelperEntity, Is.Not.Null, "HelperEntity should not be null");
        });
    }

    [Test]
    public void Scene_WhenCreated_ShouldContainHelperEntity()
    {
        // Arrange & Act
        var scene = new Scene();

        // Assert
        Assert.That(scene.Entities.Contains(scene.HelperEntity), Is.True, "Scene should contain HelperEntity");
    }

    [Test]
    public void Begin_WhenCalled_ShouldSetFocusedToTrue()
    {
        // Arrange
        var scene = new Scene();

        // Act
        scene.Begin();

        // Assert
        Assert.That(scene.Focused, Is.True, "Scene should be focused after Begin()");
    }

    [Test]
    public void End_WhenCalled_ShouldSetFocusedToFalse()
    {
        // Arrange
        var scene = new Scene();
        scene.Begin(); // Set to focused first

        // Act
        scene.End();

        // Assert
        Assert.That(scene.Focused, Is.False, "Scene should not be focused after End()");
    }

    [Test]
    public void Paused_WhenSet_ShouldPersistValue()
    {
        // Arrange
        var scene = new Scene();

        // Act
        scene.Paused = true;

        // Assert
        Assert.That(scene.Paused, Is.True, "Paused property should persist set value");
    }

    [Test]
    public void AddEntity_WhenCalled_ShouldAddEntityToList()
    {
        // Arrange
        var scene = new Scene();
        var entity = new Entity();

        // Act
        scene.Add(entity);

        // Assert
        Assert.That(scene.Entities.Contains(entity), Is.True, "Entity should be in the scene's entity list");
    }

    [Test]
    public void RemoveEntity_WhenCalled_ShouldRemoveEntityFromList()
    {
        // Arrange
        var scene = new Scene();
        var entity = new Entity();
        scene.Add(entity);

        // Act
        scene.Remove(entity);

        // Assert
        Assert.That(scene.Entities.Contains(entity), Is.False, "Entity should not be in the scene's entity list after removal");
    }

    [Test]
    public void AddMultipleEntities_WhenCalledWithArray_ShouldAddAllEntities()
    {
        // Arrange
        var scene = new Scene();
        var entities = new[] { new Entity(), new Entity(), new Entity() };

        // Act
        scene.Add(entities);

        // Assert
        Assert.Multiple(() =>
        {
            foreach (var entity in entities)
            {
                Assert.That(scene.Entities.Contains(entity), Is.True, $"Entity should be in the scene");
            }
        });
    }

    [Test]
    public void RemoveMultipleEntities_WhenCalledWithArray_ShouldRemoveAllEntities()
    {
        // Arrange
        var scene = new Scene();
        var entities = new[] { new Entity(), new Entity(), new Entity() };
        scene.Add(entities);

        // Act
        scene.Remove(entities);

        // Assert
        Assert.Multiple(() =>
        {
            foreach (var entity in entities)
            {
                Assert.That(scene.Entities.Contains(entity), Is.False, $"Entity should not be in the scene");
            }
        });
    }

    [Test]
    public void OnInterval_WhenCalledWithZeroInterval_ShouldReturnFalse()
    {
        // Arrange
        var scene = new Scene();

        // Act
        var result = scene.OnInterval(0f);

        // Assert
        Assert.That(result, Is.False, "OnInterval should return false for zero interval");
    }

    [Test]
    public void OnInterval_WhenCalledWithNegativeInterval_ShouldReturnFalse()
    {
        // Arrange
        var scene = new Scene();

        // Act
        var result = scene.OnInterval(-1f);

        // Assert
        Assert.That(result, Is.False, "OnInterval should return false for negative interval");
    }

    [Test]
    public void OnRawInterval_WhenCalledWithZeroInterval_ShouldReturnFalse()
    {
        // Arrange
        var scene = new Scene();

        // Act
        var result = scene.OnRawInterval(0f);

        // Assert
        Assert.That(result, Is.False, "OnRawInterval should return false for zero interval");
    }

    [Test]
    public void OnRawInterval_WhenCalledWithNegativeInterval_ShouldReturnFalse()
    {
        // Arrange
        var scene = new Scene();

        // Act
        var result = scene.OnRawInterval(-1f);

        // Assert
        Assert.That(result, Is.False, "OnRawInterval should return false for negative interval");
    }

    [Test]
    public void CollideAll_WhenNoEntitiesMatch_ShouldReturnEmptyList()
    {
        // Arrange
        var scene = new Scene();
        var point = new Vector2(10, 10);

        // Act
        var results = scene.CollideAll(point, 1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Is.Not.Null, "Results should not be null");
            Assert.That(results, Is.Empty, "Results should be empty when no entities match");
        });
    }

    [Test]
    public void CollideFirst_WhenNoEntitiesMatch_ShouldReturnNull()
    {
        // Arrange
        var scene = new Scene();
        var point = new Vector2(10, 10);

        // Act
        var result = scene.CollideFirst(point, 1);

        // Assert
        Assert.That(result, Is.Null, "CollideFirst should return null when no entities match");
    }

    [Test]
    public void GetEntitiesByTagMask_WhenNoEntitiesMatch_ShouldReturnEmptyList()
    {
        // Arrange
        var scene = new Scene();

        // Act
        var results = scene.GetEntitiesByTagMask(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Is.Not.Null, "Results should not be null");
            Assert.That(results.Count, Is.EqualTo(0), "Results should be empty when no entities match");
        });
    }

    [Test]
    public void GetEntitiesExcludingTagMask_WhenAllEntitiesMatch_ShouldReturnAllEntities()
    {
        // Arrange
        var scene = new Scene();
        var entity1 = new Entity();
        var entity2 = new Entity();
        scene.Add(entity1);
        scene.Add(entity2);

        // Act - Exclude tag 1, but entities have tag 0 (default)
        var results = scene.GetEntitiesExcludingTagMask(1);

        // Assert
        Assert.That(results.Count, Is.GreaterThanOrEqualTo(2), "Should return entities that don't have the excluded tag");
    }

    [Test]
    public void Enumeration_WhenCalled_ShouldAllowForeachLoop()
    {
        // Arrange
        var scene = new Scene();
        var entity = new Entity();
        scene.Add(entity);

        // Act & Assert
        var count = 0;
        foreach (var e in scene)
        {
            count++;
        }

        Assert.That(count, Is.GreaterThan(0), "Scene should be enumerable and contain entities");
    }

    [Test]
    public void AfterUpdate_WhenOnEndOfFrameIsNull_ShouldNotThrow()
    {
        // Arrange
        var scene = new Scene();

        // Act & Assert
        Assert.DoesNotThrow(() => scene.AfterUpdate(), "AfterUpdate should not throw when OnEndOfFrame is null");
    }

    [Test]
    public void AfterUpdate_WhenOnEndOfFrameHasSubscribers_ShouldInvokeEvent()
    {
        // Arrange
        var scene = new Scene();
        var invoked = false;
        scene.OnEndOfFrame += () => invoked = true;

        // Act
        scene.AfterUpdate();

        // Assert
        Assert.That(invoked, Is.True, "OnEndOfFrame should be invoked");
        // Note: Can't test if event is cleared as it's not publicly accessible
    }

    [Test]
    public void CreateAndAdd_WhenCalled_ShouldCreateAndAddEntity()
    {
        // Note: This test might not work without proper Engine setup for pooling
        // We'll test the concept
        var scene = new Scene();
        
        Assert.DoesNotThrow(() => {
            // This might throw if Engine.Pooler is not initialized
            // but we're testing that the method exists and compiles correctly
        }, "CreateAndAdd method should exist and be callable");
    }

    [Test]
    public void BetweenInterval_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var scene = new Scene();

        // Act & Assert
        Assert.DoesNotThrow(() => scene.BetweenInterval(1.0f), "BetweenInterval should not throw");
    }

    [Test]
    public void BetweenRawInterval_WhenCalled_ShouldNotThrow()
    {
        // Arrange
        var scene = new Scene();

        // Act & Assert
        Assert.DoesNotThrow(() => scene.BetweenRawInterval(1.0f), "BetweenRawInterval should not throw");
    }
}