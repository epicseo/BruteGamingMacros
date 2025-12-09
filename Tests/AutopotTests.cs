using NUnit.Framework;
using BruteGamingMacros.Core.Model;
using System.Windows.Input;
using Newtonsoft.Json;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class AutopotTests
    {
        [Test]
        public void Constructor_Default_CreatesInstance()
        {
            // Act
            var autopot = new Autopot();

            // Assert
            Assert.IsNotNull(autopot);
        }

        [Test]
        public void Constructor_WithActionName_SetsActionName()
        {
            // Arrange & Act
            var autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT);

            // Assert
            Assert.AreEqual(Autopot.ACTION_NAME_AUTOPOT, autopot.ActionName);
        }

        [Test]
        public void Constructor_WithParameters_SetsAllProperties()
        {
            // Arrange & Act
            var autopot = new Autopot(Key.F1, 50, 100, Key.F2, 30, Key.None);

            // Assert
            Assert.AreEqual(Key.F1, autopot.HPKey);
            Assert.AreEqual(50, autopot.HPPercent);
            Assert.AreEqual(100, autopot.Delay);
            Assert.AreEqual(Key.F2, autopot.SPKey);
            Assert.AreEqual(30, autopot.SPPercent);
        }

        [Test]
        public void HPPercent_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.HPPercent = 75;

            // Assert
            Assert.AreEqual(75, autopot.HPPercent);
        }

        [Test]
        public void SPPercent_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.SPPercent = 60;

            // Assert
            Assert.AreEqual(60, autopot.SPPercent);
        }

        [Test]
        public void Delay_WhenZeroOrNegative_ReturnsDefault()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.Delay = 0;

            // Assert - should return default, not zero
            Assert.Greater(autopot.Delay, 0);
        }

        [Test]
        public void Delay_WhenPositive_ReturnsSetValue()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.Delay = 250;

            // Assert
            Assert.AreEqual(250, autopot.Delay);
        }

        [Test]
        public void DelayYgg_WhenZeroOrNegative_ReturnsDefault()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.DelayYgg = 0;

            // Assert - should return default, not zero
            Assert.Greater(autopot.DelayYgg, 0);
        }

        [Test]
        public void StopOnCriticalInjury_DefaultValue_IsFalse()
        {
            // Arrange & Act
            var autopot = new Autopot();

            // Assert
            Assert.IsFalse(autopot.StopOnCriticalInjury);
        }

        [Test]
        public void StopOnCriticalInjury_SetTrue_ReturnsTrue()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.StopOnCriticalInjury = true;

            // Assert
            Assert.IsTrue(autopot.StopOnCriticalInjury);
        }

        [Test]
        public void FirstHeal_DefaultValue_IsFirstHP()
        {
            // Arrange & Act
            var autopot = new Autopot();

            // Assert
            Assert.AreEqual(Autopot.FIRSTHP, autopot.FirstHeal);
        }

        [Test]
        public void FirstHeal_SetToFirstSP_ReturnsFirstSP()
        {
            // Arrange
            var autopot = new Autopot();

            // Act
            autopot.FirstHeal = Autopot.FIRSTSP;

            // Assert
            Assert.AreEqual(Autopot.FIRSTSP, autopot.FirstHeal);
        }

        [Test]
        public void GetActionName_WhenActionNameNull_ReturnsDefault()
        {
            // Arrange
            var autopot = new Autopot();
            autopot.ActionName = null;

            // Act
            var name = autopot.GetActionName();

            // Assert
            Assert.AreEqual(Autopot.ACTION_NAME_AUTOPOT, name);
        }

        [Test]
        public void GetActionName_WhenActionNameSet_ReturnsSetValue()
        {
            // Arrange
            var autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG);

            // Act
            var name = autopot.GetActionName();

            // Assert
            Assert.AreEqual(Autopot.ACTION_NAME_AUTOPOT_YGG, name);
        }

        [Test]
        public void GetConfiguration_ReturnsValidJson()
        {
            // Arrange
            var autopot = new Autopot
            {
                HPKey = Key.F1,
                HPPercent = 50,
                SPKey = Key.F2,
                SPPercent = 30,
                Delay = 100,
                StopOnCriticalInjury = true,
                FirstHeal = Autopot.FIRSTHP
            };

            // Act
            var json = autopot.GetConfiguration();

            // Assert
            Assert.IsNotEmpty(json);
            Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<Autopot>(json));
        }

        [Test]
        public void GetConfiguration_DeserializedMatchesOriginal()
        {
            // Arrange
            var autopot = new Autopot
            {
                HPKey = Key.F3,
                HPPercent = 70,
                SPKey = Key.F4,
                SPPercent = 40,
                Delay = 150,
                StopOnCriticalInjury = false,
                FirstHeal = Autopot.FIRSTSP
            };

            // Act
            var json = autopot.GetConfiguration();
            var deserialized = JsonConvert.DeserializeObject<Autopot>(json);

            // Assert
            Assert.AreEqual(autopot.HPKey, deserialized.HPKey);
            Assert.AreEqual(autopot.HPPercent, deserialized.HPPercent);
            Assert.AreEqual(autopot.SPKey, deserialized.SPKey);
            Assert.AreEqual(autopot.SPPercent, deserialized.SPPercent);
            Assert.AreEqual(autopot.StopOnCriticalInjury, deserialized.StopOnCriticalInjury);
            Assert.AreEqual(autopot.FirstHeal, deserialized.FirstHeal);
        }

        [Test]
        public void ACTION_NAME_AUTOPOT_IsCorrectValue()
        {
            Assert.AreEqual("Autopot", Autopot.ACTION_NAME_AUTOPOT);
        }

        [Test]
        public void ACTION_NAME_AUTOPOT_YGG_IsCorrectValue()
        {
            Assert.AreEqual("AutopotYgg", Autopot.ACTION_NAME_AUTOPOT_YGG);
        }

        [Test]
        public void FIRSTHP_IsCorrectValue()
        {
            Assert.AreEqual("firstHP", Autopot.FIRSTHP);
        }

        [Test]
        public void FIRSTSP_IsCorrectValue()
        {
            Assert.AreEqual("firstSP", Autopot.FIRSTSP);
        }

        [Test]
        public void Stop_WhenNotStarted_DoesNotThrow()
        {
            // Arrange
            var autopot = new Autopot();

            // Act & Assert
            Assert.DoesNotThrow(() => autopot.Stop());
        }
    }
}
