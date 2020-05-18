using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Prog.Script;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

namespace Tests
{
    public class CombatTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void remove_5_health_point_over_100_health_point()
        {
            var enemyLogic = new EnemyLogic {CurrentHealth = 100};

            enemyLogic.ApplyDamage(5);

            Assert.True(enemyLogic.CurrentHealth == 95);
            Assert.False(enemyLogic.IsDead);
        }

        [Test]
        public void enemy_takes_105_dmg_and_dies()
        {
            var enemyLogic = new EnemyLogic {CurrentHealth = 100};

            enemyLogic.ApplyDamage(105);

            Assert.True(enemyLogic.IsDead);
        }

        [Test]
        public void enemy_takes_two_attack_of_45_dmg_and_is_alive()
        {
            var enemyLogic = new EnemyLogic {CurrentHealth = 100};
            
            enemyLogic.ApplyDamage(45);
            enemyLogic.ApplyDamage(45);
            
            Assert.True(enemyLogic.CurrentHealth == 10);
            Assert.False(enemyLogic.IsDead);
        }

        [Test]
        public void enemy_gets_negative_dmg_and_throws_error()
        {
            var enemyLogic = new EnemyLogic();

            Assert.Throws<Exception>(()=> enemyLogic.ApplyDamage(-5));
        }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CombatTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
