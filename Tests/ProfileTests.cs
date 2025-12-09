using NUnit.Framework;
using BruteGamingMacros.Core.Model;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void Constructor_WithName_SetsName()
        {
            // Arrange & Act
            var profile = new Profile("TestProfile");

            // Assert
            Assert.AreEqual("TestProfile", profile.Name);
        }

        [Test]
        public void Constructor_InitializesUserPreferences()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.UserPreferences);
        }

        [Test]
        public void Constructor_InitializesSkillSpammer()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.SkillSpammer);
        }

        [Test]
        public void Constructor_InitializesAutopot()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.Autopot);
            Assert.AreEqual(Autopot.ACTION_NAME_AUTOPOT, profile.Autopot.ActionName);
        }

        [Test]
        public void Constructor_InitializesAutopotYgg()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.AutopotYgg);
            Assert.AreEqual(Autopot.ACTION_NAME_AUTOPOT_YGG, profile.AutopotYgg.ActionName);
        }

        [Test]
        public void Constructor_InitializesSkillTimer()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.SkillTimer);
        }

        [Test]
        public void Constructor_InitializesAutobuffSkill()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.AutobuffSkill);
        }

        [Test]
        public void Constructor_InitializesAutobuffItem()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.AutobuffItem);
        }

        [Test]
        public void Constructor_InitializesStatusRecovery()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.StatusRecovery);
        }

        [Test]
        public void Constructor_InitializesDebuffsRecovery()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.DebuffsRecovery);
        }

        [Test]
        public void Constructor_InitializesWeightDebuffsRecovery()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.WeightDebuffsRecovery);
        }

        [Test]
        public void Constructor_InitializesSongMacro()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.SongMacro);
        }

        [Test]
        public void Constructor_InitializesMacroSwitch()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.MacroSwitch);
        }

        [Test]
        public void Constructor_InitializesTransferHelper()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.TransferHelper);
        }

        [Test]
        public void Constructor_InitializesAtkDefMode()
        {
            // Arrange & Act
            var profile = new Profile("Test");

            // Assert
            Assert.IsNotNull(profile.AtkDefMode);
        }

        [Test]
        public void Name_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var profile = new Profile("Initial");

            // Act
            profile.Name = "Updated";

            // Assert
            Assert.AreEqual("Updated", profile.Name);
        }

        [Test]
        public void Constructor_DefaultProfile_HasCorrectAutopotActionNames()
        {
            // Arrange & Act
            var profile = new Profile("Default");

            // Assert
            Assert.AreEqual("Autopot", profile.Autopot.GetActionName());
            Assert.AreEqual("AutopotYgg", profile.AutopotYgg.GetActionName());
        }
    }
}
