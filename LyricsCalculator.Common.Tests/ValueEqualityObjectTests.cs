using FluentAssertions;
using NUnit.Framework;
using ParkSquare.Testing.Generators;

namespace LyricsCalculator.Common.Tests
{
    [TestFixture]
    public class ValueEqualityObjectTests
    {
        [Test]
        public void GetHashCode_WhenCalled_ReturnsSameForSameValue()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            var expected = new TestObjectA(testInput1, testInput2).GetHashCode();
            var actual = new TestObjectA(testInput1, testInput2).GetHashCode();

            actual.Should().Be(expected);
        }

        [Test]
        public void GetHashCode_WhenCalled_ReturnsDifferentForDifferentValue()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();
            var testInput3 = IntegerGenerator.AnyIntegerExcept(testInput2);

            var test1 = new TestObjectA(testInput1, testInput2).GetHashCode();
            var actual = new TestObjectA(testInput1, testInput3).GetHashCode();

            actual.Should().NotBe(test1);
        }

        [Test]
        public void Equals_WhenEqual_ReturnsTrue()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            var item1 = new TestObjectB(testInput1, testInput2);
            var item2 = new TestObjectB(testInput1, testInput2);

            item1.Equals(item2).Should().BeTrue();
        }

        [Test]
        public void Equals_WhenDiffTypes_ReturnsFalse()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            object item1 = new TestObjectA(testInput1, testInput2);
            object item2 = new TestObjectB(testInput1, testInput2);

            item1.Equals(item2).Should().BeFalse();
        }

        [Test]
        public void Equals_WhenDerivedClass_ReturnsFalse()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            var item1 = new TestObjectB(testInput1, testInput2);
            var item2 = new TestObjectC(testInput1, testInput2);

            item1.Equals(item2).Should().BeFalse();
        }

        [Test]
        public void Equals_WhenNull_ReturnsFalse()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            var item1 = new TestObjectA(testInput1, testInput2);

            item1.Equals(default(object)).Should().BeFalse();
        }

        [Test]
        public void Equals_WhenNonEntity_ReturnsFalse()
        {
            var testInput1 = IntegerGenerator.AnyPositiveInteger();
            var testInput2 = IntegerGenerator.AnyPositiveInteger();

            object item1 = new TestObjectA(testInput1, testInput2);
            object item2 = testInput1;

            item1.Equals(item2).Should().BeFalse();
        }

        public class TestObjectA : ValueEqualityObject<TestObjectA>
        {
            private readonly int _value1;
            private readonly int _value2;

            public TestObjectA(int value1, int value2)
            {
                _value1 = value1;
                _value2 = value2;
            }

            protected override bool IsEqual(TestObjectA other)
            {
                return other != null
                       && other._value1 == _value1
                       && other._value2 == _value2;
            }

            protected override int TupleBasedHashCode()
            {
                return (_value1, _value2).GetHashCode();
            }
        }

        public class TestObjectB : ValueEqualityObject<TestObjectB>
        {
            private readonly int _value1;
            private readonly int _value2;

            public TestObjectB(int value1, int value2)
            {
                _value1 = value1;
                _value2 = value2;
            }

            protected override bool IsEqual(TestObjectB other)
            {
                return other != null
                       && other._value1 == _value1
                       && other._value2 == _value2;
            }

            protected override int TupleBasedHashCode()
            {
                return (_value1, _value2).GetHashCode();
            }
        }

        private class TestObjectC : TestObjectB
        {
            public TestObjectC(int value1, int value2) : base(value1, value2)
            { }
        }
    }
}