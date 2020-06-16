using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prog.Script
{
    public class PlayerCombat : MonoBehaviour
    {
        public Transform attackPoint;
        public int dps = 5;
        public float attackRange = 0.5f;
        public float attackDuration = 0.5f;

        public AudioClip attackImpactSound;
        
        public float attackRate = 2f;
        private float _nextAttackTime = 0f;
    
        public LayerMask enemyLayers;

        private bool _isAttacking = false;

        public void Attack()
        {
            // on vient checker si le player a appuyé sur la touche d'attack
            // et s'il a attaqué récemment (determiné par next attack time)
            if (!CheckIfWeCanAttack()) return;
            SetNextAttackTime();
            
            // On vient attacké tous les enemis qui sont dans le range de l'épeé
            // determiné par le attackPoint sur l'arme
            AttackEnemyInRange();
        }
        
        bool CheckIfWeCanAttack()
        {
            return (Time.time >= _nextAttackTime && _isAttacking);
        }
        
        private void SetNextAttackTime()
        {
            _nextAttackTime = Time.time + 1f / attackRate;
        }

        private void AttackEnemyInRange()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                AttackEnemy(enemy);
            }
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

        private void AttackEnemy(Collider enemy)
        {
            enemy.GetComponent<Enemy>().TakesDamage(dps, transform.position.x); // on passe le position du player en x 
            AudioManager.instance.PlaySingleRandomized(attackImpactSound);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
