using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.Collections.Generic;

namespace Monocle.Tests;

/// <summary>
/// Test class for Calc utility functionality.
/// Demonstrates modernized testing approach following CLAUDE.md guidelines.
/// </summary>
[TestFixture]
public class CalcTests
{
    [SetUp]
    public void Setup()
    {
        // Reset global random state for predictable tests
        Calc.Random = new Random(42);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up any random stack state
        while (true)
        {
            try
            {
                Calc.PopRandom();
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }
    }

    #region Enum Tests

    [Test]
    public void EnumLength_WhenCalledWithValidEnum_ShouldReturnCorrectCount()
    {
        // Arrange
        var enumType = typeof(StringSplitOptions);

        // Act
        var result = Calc.EnumLength(enumType);

        // Assert - StringSplitOptions has 3 values in .NET 9: None (0), RemoveEmptyEntries (1), TrimEntries (2)
        Assert.That(result, Is.EqualTo(3), "StringSplitOptions should have 3 values in .NET 9");
    }

    [Test]
    public void EnumLength_WhenCalledWithNonEnum_ShouldThrowArgumentException()
    {
        // Arrange
        var nonEnumType = typeof(string);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Calc.EnumLength(nonEnumType));
    }

    [Test]
    public void EnumLength_WhenCalledWithNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Calc.EnumLength(null!));
    }

    [Test]
    public void StringToEnum_WhenCalledWithValidString_ShouldReturnEnum()
    {
        // Act
        var result = Calc.StringToEnum<StringSplitOptions>("None");

        // Assert
        Assert.That(result, Is.EqualTo(StringSplitOptions.None));
    }

    [Test]
    public void StringToEnum_WhenCalledWithInvalidString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Calc.StringToEnum<StringSplitOptions>("InvalidValue"));
    }

    [Test]
    public void StringToEnum_WhenCalledWithNullOrEmpty_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentNullException>(() => Calc.StringToEnum<StringSplitOptions>(null!));
            Assert.Throws<ArgumentException>(() => Calc.StringToEnum<StringSplitOptions>(""));
        });
    }

    [Test]
    public void StringsToEnums_WhenCalledWithValidStrings_ShouldReturnEnumArray()
    {
        // Arrange
        var strings = new[] { "None", "RemoveEmptyEntries" };

        // Act
        var result = Calc.StringsToEnums<StringSplitOptions>(strings);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(StringSplitOptions.None));
            Assert.That(result[1], Is.EqualTo(StringSplitOptions.RemoveEmptyEntries));
        });
    }

    [Test]
    public void StringsToEnums_WhenCalledWithNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Calc.StringsToEnums<StringSplitOptions>(null!));
    }

    [Test]
    public void EnumHasString_WhenCalledWithValidString_ShouldReturnTrue()
    {
        // Act
        var result = Calc.EnumHasString<StringSplitOptions>("None");

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void EnumHasString_WhenCalledWithInvalidString_ShouldReturnFalse()
    {
        // Act
        var result = Calc.EnumHasString<StringSplitOptions>("InvalidValue");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void EnumHasString_WhenCalledWithNullOrEmpty_ShouldReturnFalse()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(Calc.EnumHasString<StringSplitOptions>(null!), Is.False);
            Assert.That(Calc.EnumHasString<StringSplitOptions>(""), Is.False);
        });
    }

    #endregion

    #region String Tests

    [Test]
    public void IsIgnoreCase_WhenStringMatchesOneOption_ShouldReturnTrue()
    {
        // Arrange
        var testString = "Hello";

        // Act
        var result = testString.IsIgnoreCase("hello", "world", "test");

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsIgnoreCase_WhenStringMatchesNoOptions_ShouldReturnFalse()
    {
        // Arrange
        var testString = "Hello";

        // Act
        var result = testString.IsIgnoreCase("world", "test", "example");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsIgnoreCase_WhenStringIsNullOrEmpty_ShouldReturnFalse()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(((string)null!).IsIgnoreCase("test"), Is.False);
            Assert.That("".IsIgnoreCase("test"), Is.False);
        });
    }

    [Test]
    public void ToString_WithMinDigits_ShouldZeroPadCorrectly()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(5.ToString(3), Is.EqualTo("005"));
            Assert.That(123.ToString(2), Is.EqualTo("123"));
            Assert.That(42.ToString(0), Is.EqualTo("42"));
            Assert.That((-5).ToString(4), Is.EqualTo("-0005")); // D format pads after the negative sign
        });
    }

    #endregion

    #region Count Tests

    [Test]
    public void Count_WhenCountingMatches_ShouldReturnCorrectCount()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(Calc.Count(5, 5, 3), Is.EqualTo(1));
            Assert.That(Calc.Count(5, 5, 5), Is.EqualTo(2));
            Assert.That(Calc.Count(5, 3, 7), Is.EqualTo(0));
            Assert.That(Calc.Count(5, 5, 3, 5), Is.EqualTo(2));
            Assert.That(Calc.Count(5, 5, 5, 5, 5), Is.EqualTo(4));
            Assert.That(Calc.Count(5, 5, 5, 5, 5, 5), Is.EqualTo(5));
            Assert.That(Calc.Count(5, 5, 5, 5, 5, 5, 5), Is.EqualTo(6));
        });
    }

    [Test]
    public void Count_WithSpan_ShouldCountCorrectly()
    {
        // Arrange
        ReadOnlySpan<int> values = stackalloc int[] { 1, 2, 3, 2, 4, 2 };

        // Act
        var result = Calc.Count(2, values);

        // Assert
        Assert.That(result, Is.EqualTo(3));
    }

    #endregion

    #region GiveMe Tests

    [Test]
    public void GiveMe_WhenIndexIsValid_ShouldReturnCorrectValue()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(Calc.GiveMe(0, "a", "b"), Is.EqualTo("a"));
            Assert.That(Calc.GiveMe(1, "a", "b"), Is.EqualTo("b"));
            Assert.That(Calc.GiveMe(2, "a", "b", "c"), Is.EqualTo("c"));
            Assert.That(Calc.GiveMe(3, "a", "b", "c", "d"), Is.EqualTo("d"));
            Assert.That(Calc.GiveMe(4, "a", "b", "c", "d", "e"), Is.EqualTo("e"));
            Assert.That(Calc.GiveMe(5, "a", "b", "c", "d", "e", "f"), Is.EqualTo("f"));
        });
    }

    [Test]
    public void GiveMe_WhenIndexIsOutOfRange_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Calc.GiveMe(2, "a", "b"));
            Assert.Throws<ArgumentOutOfRangeException>(() => Calc.GiveMe(-1, "a", "b"));
            Assert.Throws<ArgumentOutOfRangeException>(() => Calc.GiveMe(3, "a", "b", "c"));
        });
    }

    #endregion

    #region Random Tests

    [Test]
    public void PushRandom_WithSeed_ShouldChangeRandomState()
    {
        // Arrange
        var originalNext = Calc.Random.Next(100);
        Calc.Random = new Random(42); // Reset to known state
        var firstValue = Calc.Random.Next(100);

        // Act
        Calc.PushRandom(123);
        var newValue = Calc.Random.Next(100);

        // Assert
        Assert.That(newValue, Is.Not.EqualTo(firstValue), "Random state should change with new seed");
    }

    [Test]
    public void PushRandom_WithNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Calc.PushRandom(null!));
    }

    [Test]
    public void PopRandom_WhenStackIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => Calc.PopRandom());
    }

    [Test]
    public void PushPopRandom_ShouldRestoreOriginalState()
    {
        // Arrange
        Calc.Random = new Random(42);
        var originalValue = Calc.Random.Next(100);
        Calc.Random = new Random(42); // Reset to get same sequence

        // Act
        Calc.PushRandom(123);
        var pushedValue = Calc.Random.Next(100);
        Calc.PopRandom();
        var restoredValue = Calc.Random.Next(100);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(pushedValue, Is.Not.EqualTo(originalValue), "Pushed random should be different");
            Assert.That(restoredValue, Is.EqualTo(originalValue), "Popped random should restore original");
        });
    }

    #endregion

    #region Choose Tests

    [Test]
    public void Choose_WithTwoOptions_ShouldReturnOneOfThem()
    {
        // Arrange
        var random = new Random(42);

        // Act
        var result = random.Choose("a", "b");

        // Assert
        Assert.That(result, Is.AnyOf("a", "b"));
    }

    [Test]
    public void Choose_WithArray_ShouldReturnOneElement()
    {
        // Arrange
        var random = new Random(42);
        var choices = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = random.Choose(choices);

        // Assert
        Assert.That(choices, Contains.Item(result));
    }

    [Test]
    public void Choose_WithEmptyArray_ShouldThrowArgumentException()
    {
        // Arrange
        var random = new Random(42);
        var emptyArray = new string[0];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => random.Choose(emptyArray));
    }

    [Test]
    public void Choose_WithNullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        var random = new Random(42);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => random.Choose((string[])null!));
    }

    [Test]
    public void Choose_WithList_ShouldReturnOneElement()
    {
        // Arrange
        var random = new Random(42);
        var choices = new List<string> { "apple", "banana", "cherry" };

        // Act
        var result = random.Choose(choices);

        // Assert
        Assert.That(choices, Contains.Item(result));
    }

    [Test]
    public void Choose_WithEmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var random = new Random(42);
        var emptyList = new List<string>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => random.Choose(emptyList));
    }

    [Test]
    public void Choose_WithSpan_ShouldReturnOneElement()
    {
        // Arrange
        var random = new Random(42);
        ReadOnlySpan<int> choices = stackalloc int[] { 10, 20, 30 };

        // Act
        var result = random.Choose(choices);

        // Assert
        Assert.That(result, Is.AnyOf(10, 20, 30));
    }

    [Test]
    public void Choose_WithEmptySpan_ShouldThrowArgumentException()
    {
        // Arrange
        var random = new Random(42);

        // Act & Assert - Using empty span directly in lambda
        Assert.Throws<ArgumentException>(() => random.Choose(ReadOnlySpan<int>.Empty));
    }

    #endregion

    #region Statistical Tests

    [Test]
    public void Choose_WithManyIterations_ShouldDistributeEvenly()
    {
        // Arrange
        var random = new Random(42);
        var counts = new Dictionary<string, int> { ["a"] = 0, ["b"] = 0 };
        const int iterations = 1000;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var choice = random.Choose("a", "b");
            counts[choice]++;
        }

        // Assert - Each option should be chosen roughly 50% of the time (within reasonable bounds)
        Assert.Multiple(() =>
        {
            Assert.That(counts["a"], Is.GreaterThan(300), "Option 'a' should be chosen frequently");
            Assert.That(counts["a"], Is.LessThan(700), "Option 'a' should not dominate completely");
            Assert.That(counts["b"], Is.GreaterThan(300), "Option 'b' should be chosen frequently");
            Assert.That(counts["b"], Is.LessThan(700), "Option 'b' should not dominate completely");
            Assert.That(counts["a"] + counts["b"], Is.EqualTo(iterations), "Total should equal iterations");
        });
    }

    #endregion
}