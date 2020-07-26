using System;
using UnityEngine;

namespace Prog.Script
{
    public class EnemyLogic
    {
        public bool IsDead { get; set; }
        public bool HasTakenDamage { get; set; }
        public int CurrentHealth { get; set; }
        
        public void ApplyDamage(int damage) // A tester 
        {
            CurrentHealth = DamageCalculator(damage, CurrentHealth);
            HasTakenDamage = true;
            IsDead = CheckIfDead();
        }
        
        private int DamageCalculator(int damage, int currentHealth)
        {
            if(damage < 0){ throw new Exception("received damage on enemy cannot be < 0");}
            return (currentHealth - damage);
        }
        
        private bool CheckIfDead()
        {
            return CurrentHealth <= 0;
        }
    }
}