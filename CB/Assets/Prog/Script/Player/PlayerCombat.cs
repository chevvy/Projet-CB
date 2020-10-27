using System.Collections;
using UnityEngine;

namespace Prog.Script
{
    public class PlayerCombat : MonoBehaviour
    {
        public int dps = 5;
        public float attackDuration = 0.5f;
        public float attackRate = 2f;
        public bool enableAttackRate = true;
        
        private float _nextAttackTime = 0f;
        private bool _isAttacking = false;

        public WeaponImpactDetector WeaponImpactDetector;

        public void Attack(Enemy enemy)
        {
            // on vient checker si le player a appuyé sur la touche d'attack
            // et s'il a attaqué récemment (determiné par next attack time)
            if (!CheckIfWeCanAttack())
            {
                Debug.LogWarning("Player can't attack (spam attack): voir component PlayerCombat");
                return;
            }
            SetNextAttackTime();
            
            // On vient attacké l'ennemi qui est reçu par la méthode attack (component enemi) 
            ApplyAttackToEnemy(enemy);
        }
        
        private bool CheckIfWeCanAttack()
        {
            if (!enableAttackRate) return true; // si on desactive l'attack rate, on peut toujours attaquer
            return (Time.time >= _nextAttackTime && _isAttacking);
        }
        
        private void SetNextAttackTime()
        {
            _nextAttackTime = Time.time + 1f / attackRate;
        }

        public void StartAttackTimer()
        {
            StartCoroutine(AttackTimer());
        }

        IEnumerator AttackTimer()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(attackDuration);
            _isAttacking = false;
        }

        private void ApplyAttackToEnemy(Enemy enemy)
        {
            enemy.TakesDamage(dps, transform.position.x); // on passe le position du player en x
        }
        
        /// <summary>
        /// Vérifie avec le WeaponImpactDetector (assigné) si on a des ennemis
        /// Si on a des ennemis, on les attaque.
        /// </summary>
        public void CheckForEnemies()
        {
            WeaponImpactDetector.CheckForEnemies();
        }
    }
}
