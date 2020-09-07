using System.Collections;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Prog.Script
{
    public class Enemy : MonoBehaviour
    {
        public int maxHealthPoint = 100;
        public float impactForceWhenAttackedOnXAxis = 1;
        public float impactForceWhenAttackedOnYAxis = 1;
        public GameObject enemyObjectToBeDisabledOnDeath;
        
        private MeshRenderer _enemyMesh;
        public Material damageMaterial;
        private Material _enemyMaterial;
        
        private Rigidbody _rigidbody;
        private Armor.Armor _armor;
        private NavMeshAgent _navMeshAgent;

        private EnemyLogic _enemyLogic;
        private readonly ApplyForce _applyForce = new ApplyForce();

        void Awake()
        {
            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                _enemyMesh = meshRenderer;
                _enemyMaterial = _enemyMesh.material;
            }
            _rigidbody = GetComponent<Rigidbody>();
            if (TryGetComponent(out Armor.Armor armorComponent)) { _armor = armorComponent; }

            if (TryGetComponent(out GameObject enemyObject)) { enemyObjectToBeDisabledOnDeath = enemyObject; }
            
            _enemyLogic = new EnemyLogic
            {
                CurrentHealth = maxHealthPoint,
                HasTakenDamage = false,
                IsDead = false
            };
        }

        public void TakesDamage(int damage, float xPlayerPosition)
        {
            if (_armor != null)
            {
                ApplyDamageToArmor(damage, xPlayerPosition);
                if (!_armor.IsArmorBroken()) return;
            }
            
            _enemyLogic.ApplyDamage(damage);
            AudioManager.Instance.PlaySound("melee_weapon_impact", true);
            if(_enemyMesh != null) StartCoroutine(ApplyDamageMaterial());

            MoveEnemyAfterAttack(xPlayerPosition);
            
            CheckIfDead();
        }

        IEnumerator ApplyDamageMaterial()
        {
            _enemyMesh.material = damageMaterial;
            yield return new WaitForSeconds(.1f);
            _enemyMesh.material = _enemyMaterial;
        }
        
        private void ApplyDamageToArmor(int damage, float xPlayerPosition)
        {
            if (_armor.IsArmorBroken()) return;
            _armor.TakeDamage(damage, xPlayerPosition);
        }

        /// <summary>
        /// Fonction qui va recevoir la position de l'emeteur de l'attack et va calculer la force du déplacement
        /// et la direction en fonction de la position initiale de l'attack. Sera appliqué sur le Rigidbody.
        /// </summary>
        /// <param name="xPlayerPosition">Position en x de l'emeteur de l'attack (le player)</param>
        private void MoveEnemyAfterAttack(float xPlayerPosition)
        {
            _applyForce.OnAttack(_rigidbody, null, xPlayerPosition, impactForceWhenAttackedOnXAxis, impactForceWhenAttackedOnYAxis);
        }

        private void CheckIfDead()
        {
            if (!_enemyLogic.IsDead) return;
            Die();
        }

        private void Die() 
        {
            GetComponent<Collider>().enabled = false;
            Destroy(enemyObjectToBeDisabledOnDeath);
        }
    }
}
