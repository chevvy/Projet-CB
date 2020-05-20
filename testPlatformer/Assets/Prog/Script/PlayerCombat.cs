using UnityEngine;

namespace Prog.Script
{
    public class PlayerCombat : MonoBehaviour
    {
        public Transform attackPoint;
        public int dps = 5;
        public float attackRange = 0.5f;

        public AudioClip attackImpactSound;
        
        public float attackRate = 2f;
        private float _nextAttackTime = 0f;
    
        public LayerMask enemyLayers;
        private Collider _weaponCollider;

        // Start is called before the first frame update
        void Start()
        {
            _weaponCollider = GetComponent<Collider>();
        }

        public void Attack()
        {
            if (!CheckIfWeCanAttack()) return;
            SetNextAttackTime();
        
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakesDamage(dps);
                AudioManager.instance.PlaySingleRandomized(attackImpactSound);
            }
        }

        bool CheckIfWeCanAttack()
        {
            return (Time.time >= _nextAttackTime);
        }
    
        private void SetNextAttackTime()
        {
            _nextAttackTime = Time.time + 1f / attackRate;
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
