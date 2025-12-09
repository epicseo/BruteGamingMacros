using NUnit.Framework;
using BruteGamingMacros.Core.Utils;
using System;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class PatternScannerTests
    {
        [Test]
        public void ParsePattern_ValidPattern_ReturnsCorrectResult()
        {
            // Arrange
            string patternString = "89 45 ?? 8B 4D";

            // Act
            bool result = PatternScanner.ParsePattern(patternString, out byte[] pattern, out string mask);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(pattern);
            Assert.IsNotNull(mask);
            Assert.AreEqual(5, pattern.Length);
            Assert.AreEqual(5, mask.Length);
            Assert.AreEqual(0x89, pattern[0]);
            Assert.AreEqual(0x45, pattern[1]);
            Assert.AreEqual(0x00, pattern[2]); // Wildcard
            Assert.AreEqual(0x8B, pattern[3]);
            Assert.AreEqual(0x4D, pattern[4]);
            Assert.AreEqual("xx?xx", mask);
        }

        [Test]
        public void ParsePattern_WithSingleQuestionMark_ReturnsCorrectResult()
        {
            // Arrange
            string patternString = "89 ? 45";

            // Act
            bool result = PatternScanner.ParsePattern(patternString, out byte[] pattern, out string mask);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(3, pattern.Length);
            Assert.AreEqual("x?x", mask);
        }

        [Test]
        public void ParsePattern_AllWildcards_ReturnsCorrectResult()
        {
            // Arrange
            string patternString = "?? ?? ??";

            // Act
            bool result = PatternScanner.ParsePattern(patternString, out byte[] pattern, out string mask);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(3, pattern.Length);
            Assert.AreEqual("???", mask);
        }

        [Test]
        public void ParsePattern_EmptyString_ReturnsFalse()
        {
            // Act
            bool result = PatternScanner.ParsePattern("", out byte[] pattern, out string mask);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ParsePattern_NullString_ReturnsFalse()
        {
            // Act
            bool result = PatternScanner.ParsePattern(null, out byte[] pattern, out string mask);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ParsePattern_WhitespaceOnly_ReturnsFalse()
        {
            // Act
            bool result = PatternScanner.ParsePattern("   ", out byte[] pattern, out string mask);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ParsePattern_LongPattern_ReturnsCorrectResult()
        {
            // Arrange
            string patternString = "55 8B EC 83 EC ?? 53 56 57 8B ?? ?? ?? ?? ?? 33 FF";

            // Act
            bool result = PatternScanner.ParsePattern(patternString, out byte[] pattern, out string mask);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(17, pattern.Length);
            Assert.AreEqual(17, mask.Length);
        }

        [Test]
        public void ParsePattern_InvalidHex_ReturnsFalse()
        {
            // Arrange
            string patternString = "ZZ XX YY";

            // Act
            bool result = PatternScanner.ParsePattern(patternString, out byte[] pattern, out string mask);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Constructor_CreatesInstance()
        {
            // Act
            using (var scanner = new PatternScanner())
            {
                // Assert
                Assert.IsNotNull(scanner);
                Assert.IsFalse(scanner.IsAttached);
                Assert.IsNull(scanner.AttachedProcess);
            }
        }

        [Test]
        public void Dispose_NoProcess_DoesNotThrow()
        {
            // Arrange
            var scanner = new PatternScanner();

            // Act & Assert
            Assert.DoesNotThrow(() => scanner.Dispose());
        }

        [Test]
        public void FindPattern_NotAttached_ReturnsZero()
        {
            // Arrange
            using (var scanner = new PatternScanner())
            {
                byte[] pattern = { 0x89, 0x45 };
                string mask = "xx";

                // Act
                var result = scanner.FindPattern(pattern, mask);

                // Assert
                Assert.AreEqual(IntPtr.Zero, result);
            }
        }

        [Test]
        public void FindPattern_InvalidPattern_ReturnsZero()
        {
            // Arrange
            using (var scanner = new PatternScanner())
            {
                // Act
                var result = scanner.FindPattern(null, null);

                // Assert
                Assert.AreEqual(IntPtr.Zero, result);
            }
        }

        [Test]
        public void FindPattern_MismatchedLengths_ReturnsZero()
        {
            // Arrange
            using (var scanner = new PatternScanner())
            {
                byte[] pattern = { 0x89, 0x45, 0x00 };
                string mask = "xx"; // Length mismatch

                // Act
                var result = scanner.FindPattern(pattern, mask);

                // Assert
                Assert.AreEqual(IntPtr.Zero, result);
            }
        }
    }

    [TestFixture]
    public class ROPatternsTests
    {
        [Test]
        public void GetPatternDiscoveryGuide_ReturnsNonEmptyString()
        {
            // Act
            var guide = ROPatterns.GetPatternDiscoveryGuide();

            // Assert
            Assert.IsNotEmpty(guide);
            Assert.That(guide, Does.Contain("Cheat Engine"));
            Assert.That(guide, Does.Contain("HP"));
            Assert.That(guide, Does.Contain("addresses.json"));
        }
    }
}
