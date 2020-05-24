using System.Collections;
using UnityEngine;

namespace Prog.Script
{
    public class Enemy : MonoBehaviour
    {
        public int maxHealthPoint = 100;
        public float impactForceWhenAttacked = 1;

        private MeshRenderer _enemyMesh;
        public Material damageMaterial;
        private Material _enemyMaterial;
        private Rigidbody _rigidbody;

        private EnemyLogic _enemyLogic;

        void Start()
        {
            _enemyMesh = GetComponent<MeshRenderer>();
            _enemyMaterial = _enemyMesh.material;
            _rigidbody = GetComponent<Rigidbody>();

            _enemyLogic = new EnemyLogic
            {
                CurrentHealth = maxHealthPoint,
                HasTakenDamage = false,
                IsDead = false
            };
        }

        public void TakesDamage(int damage, float xPlayerPosition)
        {
            _enemyLogic.ApplyDamage(damage);
            StartCoroutine(ApplyDamageMaterial());
            
            ApplyMovement(xPlayerPosition);
            
            CheckIfDead();
        }

        IEnumerator ApplyDamageMaterial()
        {
            _enemyMesh.material = damageMaterial;
            yield return new WaitForSeconds(.1f);
            _enemyMesh.material = _enemyMaterial;
        }

        private void ApplyMovement(float xPlayerPosition)
        {
            var xEnemyPosition = transform.position.x;
            var movementDirection = xEnemyPosition - xPlayerPosition;
            if (movementDirection < 0)
            {
                _rigidbody.AddForce(-impactForceWhenAttacked,1, 0, ForceMode.Impulse);
            }

            if (movementDirection > 0)
            {
                _rigidbody.AddForce(impactForceWhenAttacked,1, 0, ForceMode.Impulse);
            }
            
        }

        private void CheckIfDead() // A tester 
        {
            if (!_enemyLogic.IsDead) return;
            Die();
        }

        private void Die() 
        {
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
}
