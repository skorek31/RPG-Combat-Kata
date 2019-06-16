using System;
using Xunit;
using Combat;

namespace Combat.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void New_Warrior_Should_Have_1000_Health()
        {
            var testObject = new Warrior("Melee", 1, 0, 0);
            Assert.Equal(1000, testObject.GetHealth());
        }
        
        [Fact]
        public void New_Warrior_Should_Have_1st_Level()
        {
            var testObject = new Warrior("Melee", 1, 0, 0);
            Assert.Equal(1, testObject.GetLevel()); 
        }

        [Fact]
        public void New_Warrior_ShouldBe_Alive()
        {
            var testObject = new Warrior("Melee", 1, 0, 0);
            Assert.True(testObject.IsAlive());
        }

        [Fact]
        public void After_Attack_Warrior_Should_Have_Less_Than_1000HP()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            int hpBeforeAttack = testTarget.GetHealth();
            testAttacker.Attack(testTarget);
            Assert.True(testTarget.GetHealth() < hpBeforeAttack);
        }

        [Fact]
        public void Warriors_Attack_Dmg_Between_50_and_100()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            // int hpBeforeAttack = testTarget.GetHealth();
            // testAttacker.Attack(testTarget);
            // int hpAfterAttack = testTarget.GetHealth();
            // int result = hpBeforeAttack - hpAfterAttack;
            int damage = testAttacker.Attack(testTarget);
            Assert.True(damage > 49 && damage < 101);
        }

        [Fact]
        public void After_20_Hits_Warrior_Has_0HPs()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            for (int i = 0; i < 20; i++)
            {
                testAttacker.Attack(testTarget);
            }
            Assert.Equal(0, testTarget.GetHealth());
        }

        [Fact]
        public void After_20_Hits_Warrior_Dies()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            for (int i = 0; i < 20; i++)
            {
                testAttacker.Attack(testTarget);
            }
            Assert.False(testTarget.IsAlive());
        }

        [Fact]
        public void Warriors_Belong_To_The_Same_Faction_Are_Allies()
        {
            var testObject1 = new Warrior("Melee", 1, 0, 0);
            testObject1.JoinFaction(0);

            var testObject2 = new Warrior("Melee", 1, 0, 1);
            testObject2.JoinFaction(0);

            Assert.True(testObject1.IsAlly(testObject2));
        }
        
        [Fact]
        public void Warrior_Cannot_Be_Healed_Above_1000HP()
        {
            var testObject1 = new Warrior("Melee", 1, 0, 0);
            testObject1.JoinFaction(0);

            var testObject2 = new Warrior("Melee", 1, 0, 1);
            testObject2.JoinFaction(0);

            Assert.Equal(0, testObject1.Heal(testObject2));
        }

        [Fact]
        public void Dead_Warrior_Cannot_Be_Healed()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            for (int i = 0; i < 20; i++)
            {
                testAttacker.Attack(testTarget);
            }
            Assert.Equal(-1, testAttacker.Heal(testTarget));
        }

        [Fact]
        public void Ally_Can_Be_Healed()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            testAttacker.JoinFaction(0);
            testTarget.JoinFaction(0);
            Assert.Equal(0, testAttacker.Heal(testTarget));
        }

        [Fact]
        public void Enemy_Cannot_Be_Healed()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            testAttacker.JoinFaction(0);
            testTarget.JoinFaction(1);
            Assert.Equal(-1, testAttacker.Heal(testTarget));
        }
        
        [Fact]
        public void Warrior_Cannot_Damage_Itself()
        {
            var testObject = new Warrior("Melee", 1, 0, 0);
            Assert.Equal(-1, testObject.Attack(testObject));
        }
        
        [Fact]
        public void Melee_Attack_OutOf_Range()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 10);
            var testTarget = new Warrior("Melee", 1, 0, 5);
            Assert.Equal(-1, testAttacker.Attack(testTarget)); 
        }

        [Fact]
        public void Melee_Attack_In_Range()
        {
            var testAttacker = new Warrior("Melee", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 1);
            Assert.True(testAttacker.Attack(testTarget) > 0);
        }

        [Fact]
        public void Ranged_Attack_OutOf_Range()
        {
            var testAttacker = new Warrior("Ranged", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 30);
            Assert.Equal(-1, testAttacker.Attack(testTarget));
        }

        [Fact]
        public void Ranged_Attack_In_Range()
        {
            var testAttacker = new Warrior("Ranged", 1, 0, 0);
            var testTarget = new Warrior("Melee", 1, 0, 10);
            Assert.True(testAttacker.Attack(testTarget) > 0);
        }

        [Fact]
        public void Less_Damage_If_Target_Is_5lvl_Higher()
        {
            var testAttackerEqualLevel = new Warrior("Melee", 1, 0, 0);
            var testTargetEqualLevel = new Warrior("Melee", 1, 0, 1);

            var testAttackerFirstLevel = new Warrior("Melee", 1, 0, 0);
            var testTargetSixthLevel = new Warrior("Melee", 6, 0, 1);

            Random damageValue = new Random();

            // We make 100 tests where we are counting lower level damage to equal level damage ratio
            // In each test we sum damage from 100 attacks
            // Then we will count average ratio, round to 1 decimal point and assert to 0.5
            float sumRatio = 0;
            for (int i = 0; i < 100; i++)
            {
                int damageEqualLevel = 0;
                int damageLowerLevel = 0;
                for (int j = 0; j < 100; j++)
                {
                    damageEqualLevel += damageValue.Next(50, 101);
                }

                for (int j = 0; j < 100; j++)
                {
                    damageLowerLevel += (int)(damageValue.Next(50, 101) * 0.5);
                }

                float ratio = (float)damageLowerLevel / damageEqualLevel;
                sumRatio += ratio;
            }

            float avgRatio = (float)Math.Round(sumRatio / 100, 1, MidpointRounding.AwayFromZero);

            Assert.Equal(0.5, avgRatio);
        }

        [Fact]
        public void Greater_Damage_If_Target_Is_5lvl_Lower()
        {
            var testAttackerEqualLevel = new Warrior("Melee", 1, 0, 0);
            var testTargetEqualLevel = new Warrior("Melee", 1, 0, 1);

            var testAttackerSixthLevel = new Warrior("Melee", 6, 0, 0);
            var testTargetFirstLevel = new Warrior("Melee", 1, 0, 1);

            Random damageValue = new Random();
            
            // We are makeing 100 tests where we are counting higher level damage to equal level damage ratio
            // In each test we sum damage from 100 attacks
            // Then we will count average ratio, round to 1 decimal point and assert to 1.5
            float sumRatio = 0;
            for (int i = 0; i < 100; i++)
            {
                int damageEqualLevel = 0;
                int damageHigherLevel = 0;

                for (int j = 0; j < 100; j++)
                {
                    damageEqualLevel += damageValue.Next(50, 101);
                }

                for (int j = 0; j < 100; j++)
                {
                    damageHigherLevel += (int)(damageValue.Next(50, 101) * 1.5);
                }

                float ratio = (float)damageHigherLevel / damageEqualLevel;
                sumRatio += ratio;
            }

            float avgRatio = (float)Math.Round(sumRatio / 100, 1, MidpointRounding.AwayFromZero);

            Assert.Equal(1.5, avgRatio);
        }
    }
}
