using NUnit.Framework;
using BruteGamingMacros.Core.Utils;
using System;
using System.ComponentModel;
using System.Reflection;

namespace BruteGamingMacros.Tests
{
    [TestFixture]
    public class EffectStatusIDsTests
    {
        [Test]
        public void EffectStatusIDs_IsFlagsEnum()
        {
            // Assert
            var flagsAttribute = typeof(EffectStatusIDs).GetCustomAttribute<FlagsAttribute>();
            Assert.IsNotNull(flagsAttribute);
        }

        [Test]
        public void EffectStatusIDs_UnderlyingType_IsUInt32()
        {
            // Assert
            var underlyingType = Enum.GetUnderlyingType(typeof(EffectStatusIDs));
            Assert.AreEqual(typeof(uint), underlyingType);
        }

        // Common buff status ID tests
        [TestCase(EffectStatusIDs.BLESSING, 10u)]
        [TestCase(EffectStatusIDs.INC_AGI, 12u)]
        [TestCase(EffectStatusIDs.KYRIE, 19u)]
        [TestCase(EffectStatusIDs.MAGNIFICAT, 20u)]
        [TestCase(EffectStatusIDs.ASSUMPTIO, 110u)]
        [TestCase(EffectStatusIDs.WINDWALK, 116u)]
        [TestCase(EffectStatusIDs.SOULLINK, 149u)]
        public void CommonBuffs_HaveCorrectValues(EffectStatusIDs status, uint expectedValue)
        {
            Assert.AreEqual(expectedValue, (uint)status);
        }

        // Debuff status ID tests
        [TestCase(EffectStatusIDs.STONEWAIT, 875u)]
        [TestCase(EffectStatusIDs.FROZEN, 876u)]
        [TestCase(EffectStatusIDs.STUN, 877u)]
        [TestCase(EffectStatusIDs.SLEEP, 878u)]
        [TestCase(EffectStatusIDs.STONE, 880u)]
        [TestCase(EffectStatusIDs.POISON, 883u)]
        [TestCase(EffectStatusIDs.CURSE, 884u)]
        [TestCase(EffectStatusIDs.SILENCE, 885u)]
        [TestCase(EffectStatusIDs.CONFUSION, 886u)]
        [TestCase(EffectStatusIDs.BLIND, 887u)]
        public void Debuffs_HaveCorrectValues(EffectStatusIDs status, uint expectedValue)
        {
            Assert.AreEqual(expectedValue, (uint)status);
        }

        // Potion effect tests
        [TestCase(EffectStatusIDs.CONCENTRATION_POTION, 37u)]
        [TestCase(EffectStatusIDs.AWAKENING_POTION, 38u)]
        [TestCase(EffectStatusIDs.BERSERK_POTION, 39u)]
        [TestCase(EffectStatusIDs.SPEED_POT, 41u)]
        public void PotionEffects_HaveCorrectValues(EffectStatusIDs status, uint expectedValue)
        {
            Assert.AreEqual(expectedValue, (uint)status);
        }

        // Weight status tests
        [TestCase(EffectStatusIDs.WEIGHT50, 35u)]
        [TestCase(EffectStatusIDs.WEIGHT90, 36u)]
        public void WeightStatus_HaveCorrectValues(EffectStatusIDs status, uint expectedValue)
        {
            Assert.AreEqual(expectedValue, (uint)status);
        }

        // Description attribute tests
        [TestCase(EffectStatusIDs.BLESSING, "Blessing")]
        [TestCase(EffectStatusIDs.INC_AGI, "Increase AGI")]
        [TestCase(EffectStatusIDs.KYRIE, "Kyrie Eleison")]
        [TestCase(EffectStatusIDs.STUN, "Stun")]
        [TestCase(EffectStatusIDs.FROZEN, "Frozen")]
        [TestCase(EffectStatusIDs.POISON, "Poison")]
        public void StatusEffects_HaveCorrectDescriptions(EffectStatusIDs status, string expectedDescription)
        {
            var description = GetEnumDescription(status);
            Assert.AreEqual(expectedDescription, description);
        }

        [Test]
        public void PROVOKE_HasValueZero()
        {
            Assert.AreEqual(0u, (uint)EffectStatusIDs.PROVOKE);
        }

        [Test]
        public void SIT_HasCorrectValue()
        {
            Assert.AreEqual(622u, (uint)EffectStatusIDs.SIT);
        }

        [Test]
        public void BERSERK_HasCorrectValue()
        {
            Assert.AreEqual(107u, (uint)EffectStatusIDs.BERSERK);
        }

        [Test]
        public void EDP_HasCorrectValue()
        {
            // Enchant Deadly Poison
            Assert.AreEqual(114u, (uint)EffectStatusIDs.EDP);
        }

        [Test]
        public void STRIPWEAPON_HasCorrectValue()
        {
            Assert.AreEqual(50u, (uint)EffectStatusIDs.STRIPWEAPON);
        }

        [Test]
        public void ElementalConverters_HaveCorrectValues()
        {
            Assert.AreEqual(90u, (uint)EffectStatusIDs.PROPERTYFIRE);
            Assert.AreEqual(91u, (uint)EffectStatusIDs.PROPERTYWATER);
            Assert.AreEqual(92u, (uint)EffectStatusIDs.PROPERTYWIND);
            Assert.AreEqual(93u, (uint)EffectStatusIDs.PROPERTYGROUND);
        }

        [Test]
        public void FoodBuffs_HaveCorrectValues()
        {
            Assert.AreEqual(241u, (uint)EffectStatusIDs.FOOD_STR);
            Assert.AreEqual(242u, (uint)EffectStatusIDs.FOOD_AGI);
            Assert.AreEqual(243u, (uint)EffectStatusIDs.FOOD_VIT);
            Assert.AreEqual(244u, (uint)EffectStatusIDs.FOOD_DEX);
            Assert.AreEqual(245u, (uint)EffectStatusIDs.FOOD_INT);
            Assert.AreEqual(246u, (uint)EffectStatusIDs.FOOD_LUK);
        }

        [Test]
        public void ProtectionSkills_HaveSequentialValues()
        {
            Assert.AreEqual(54u, (uint)EffectStatusIDs.PROTECTWEAPON);
            Assert.AreEqual(55u, (uint)EffectStatusIDs.PROTECTSHIELD);
            Assert.AreEqual(56u, (uint)EffectStatusIDs.PROTECTARMOR);
            Assert.AreEqual(57u, (uint)EffectStatusIDs.PROTECTHELM);
        }

        [Test]
        public void StripSkills_HaveSequentialValues()
        {
            Assert.AreEqual(50u, (uint)EffectStatusIDs.STRIPWEAPON);
            Assert.AreEqual(51u, (uint)EffectStatusIDs.STRIPSHIELD);
            Assert.AreEqual(52u, (uint)EffectStatusIDs.STRIPARMOR);
            Assert.AreEqual(53u, (uint)EffectStatusIDs.STRIPHELM);
        }

        [Test]
        public void ResistPotions_HaveCorrectValues()
        {
            Assert.AreEqual(908u, (uint)EffectStatusIDs.RESIST_PROPERTY_WATER);
            Assert.AreEqual(909u, (uint)EffectStatusIDs.RESIST_PROPERTY_GROUND);
            Assert.AreEqual(910u, (uint)EffectStatusIDs.RESIST_PROPERTY_FIRE);
            Assert.AreEqual(911u, (uint)EffectStatusIDs.RESIST_PROPERTY_WIND);
        }

        // Helper method to get enum description
        private static string GetEnumDescription(EffectStatusIDs value)
        {
            var field = typeof(EffectStatusIDs).GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
