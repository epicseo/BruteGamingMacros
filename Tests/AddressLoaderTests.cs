using NUnit.Framework;
using BruteGamingMacros.Core.Utils;
using BruteGamingMacros.Core.Model;
using System;
using System.IO;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class AddressLoaderTests
    {
        private string _testConfigPath;

        [SetUp]
        public void Setup()
        {
            // Create a temporary test config directory
            _testConfigPath = Path.Combine(Path.GetTempPath(), "BGM_Tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testConfigPath);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up test directory
            if (Directory.Exists(_testConfigPath))
            {
                try
                {
                    Directory.Delete(_testConfigPath, true);
                }
                catch { }
            }
        }

        [Test]
        public void GetServerConfig_MidRate_ReturnsValidConfig()
        {
            // Arrange & Act
            var config = AddressLoader.GetServerConfig(0);

            // Assert
            Assert.IsNotNull(config);
            Assert.AreEqual("OsRO Midrate", config.Name);
            Assert.IsNotNull(config.Addresses);
            Assert.IsNotEmpty(config.Addresses.Hp);
        }

        [Test]
        public void GetServerConfig_HighRate_ReturnsValidConfig()
        {
            // Arrange & Act
            var config = AddressLoader.GetServerConfig(1);

            // Assert
            Assert.IsNotNull(config);
            Assert.AreEqual("OsRO Highrate", config.Name);
            Assert.IsNotNull(config.Addresses);
            Assert.IsNotEmpty(config.Addresses.Hp);
        }

        [Test]
        public void GetServerConfig_LowRate_ReturnsPlaceholderConfig()
        {
            // Arrange & Act
            var config = AddressLoader.GetServerConfig(2);

            // Assert
            Assert.IsNotNull(config);
            Assert.That(config.Name, Does.Contain("Revo"));
            // LR should have placeholder addresses (00000000)
            Assert.AreEqual("00000000", config.Addresses.Hp);
        }

        [Test]
        public void HasValidAddresses_MidRate_ReturnsTrue()
        {
            // Act
            var hasValid = AddressLoader.HasValidAddresses(0);

            // Assert
            Assert.IsTrue(hasValid);
        }

        [Test]
        public void HasValidAddresses_LowRate_ReturnsFalse()
        {
            // LR has placeholder addresses
            var hasValid = AddressLoader.HasValidAddresses(2);

            // Assert - LR should not have valid addresses (all zeros)
            Assert.IsFalse(hasValid);
        }

        [Test]
        public void GetServerAddresses_ReturnsListWithSingleServer()
        {
            // Act
            var servers = AddressLoader.GetServerAddresses(0);

            // Assert
            Assert.IsNotNull(servers);
            Assert.AreEqual(1, servers.Count);
        }

        [Test]
        public void AddressConfiguration_GetServerKey_ReturnsCorrectKeys()
        {
            // Assert
            Assert.AreEqual("MR", AddressConfiguration.GetServerKey(0));
            Assert.AreEqual("HR", AddressConfiguration.GetServerKey(1));
            Assert.AreEqual("LR", AddressConfiguration.GetServerKey(2));
            Assert.AreEqual("MR", AddressConfiguration.GetServerKey(99)); // Unknown defaults to MR
        }
    }

    [TestFixture]
    public class MemoryAddressesTests
    {
        [Test]
        public void ToPointer_ValidHex_ReturnsCorrectValue()
        {
            // Arrange & Act
            var pointer = MemoryAddresses.ToPointer("00E8F434");

            // Assert
            Assert.AreEqual(0x00E8F434, pointer);
        }

        [Test]
        public void ToPointer_ZeroAddress_ReturnsZero()
        {
            // Arrange & Act
            var pointer = MemoryAddresses.ToPointer("00000000");

            // Assert
            Assert.AreEqual(0, pointer);
        }

        [Test]
        public void ToPointer_NullOrEmpty_ReturnsZero()
        {
            // Assert
            Assert.AreEqual(0, MemoryAddresses.ToPointer(null));
            Assert.AreEqual(0, MemoryAddresses.ToPointer(""));
            Assert.AreEqual(0, MemoryAddresses.ToPointer("00000000"));
        }

        [Test]
        public void ToPointer_InvalidHex_ReturnsZero()
        {
            // Arrange & Act
            var pointer = MemoryAddresses.ToPointer("ZZZZZZZZ");

            // Assert
            Assert.AreEqual(0, pointer);
        }
    }

    [TestFixture]
    public class ServerAddressConfigTests
    {
        [Test]
        public void HasValidAddresses_WithValidAddresses_ReturnsTrue()
        {
            // Arrange
            var config = new ServerAddressConfig
            {
                Addresses = new MemoryAddresses
                {
                    Hp = "00E8F434",
                    Name = "00E91C00"
                }
            };

            // Act & Assert
            Assert.IsTrue(config.HasValidAddresses());
        }

        [Test]
        public void HasValidAddresses_WithZeroAddresses_ReturnsFalse()
        {
            // Arrange
            var config = new ServerAddressConfig
            {
                Addresses = new MemoryAddresses
                {
                    Hp = "00000000",
                    Name = "00000000"
                }
            };

            // Act & Assert
            Assert.IsFalse(config.HasValidAddresses());
        }

        [Test]
        public void HasValidAddresses_WithNullAddresses_ReturnsFalse()
        {
            // Arrange
            var config = new ServerAddressConfig
            {
                Addresses = null
            };

            // Act & Assert
            Assert.IsFalse(config.HasValidAddresses());
        }
    }
}
