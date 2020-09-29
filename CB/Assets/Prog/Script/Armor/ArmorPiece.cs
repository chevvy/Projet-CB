using Prog.Script.RigidbodyInteraction;
using UnityEngine;

namespace Prog.Script.Armor
{
    public class ArmorPiece : MonoBehaviour
    {
        private Rigidbody Rigidbody { get; set; }
        private IApplyForce ApplyForce { get; set; }
        public GameObject sparkSfx;
        private AudioClip _armorPieceBreakingSound;
        private bool _isRemovedArmorPiece;
        public GameObject armorPiece;

        private void Awake()
        {
            ApplyForce = new ApplyForce(); // Fonction partagé qui permet d'appliquer de la force sur un rigidbody
            GetArmorPieceRigidBody();
        }

        private void GetArmorPieceRigidBody()
        {
            if (TryGetComponent(out Rigidbody rigidBodyOnObject))
            {
                Rigidbody = rigidBodyOnObject;
            }

            if (!Rigidbody.isKinematic && !_isRemovedArmorPiece)
            {
                Debug.LogError("The armorPiece '" + name + "' rigidBody must be kinematic");
            }
        }

        public void RemoveArmorPiece(float xAttackOriginPosition)
        {
            SpawnDestroyedArmorPiece(xAttackOriginPosition);
            SpawnDestructionFx();
            PlayArmorPieceBreakingSound();
            DeleteCurrentArmorPiece();
        }

        private void SpawnDestroyedArmorPiece(float xAttackOriginPosition)
        {
            armorPiece.GetComponent<ArmorPiece>()._isRemovedArmorPiece = true; // Va venir empêcher le check-up on awake pour "is kinematic"
            // Vector3 spawnPosition = transform.TransformVector(Vector3.zero);
            Vector3 spawnPosition = transform.position;
            GameObject armorPieceInstance = Instantiate(armorPiece, spawnPosition, Quaternion.identity); 
            

            Rigidbody armorPieceToSpawnRigidBody = armorPieceInstance.GetComponent<Rigidbody>();
            armorPieceToSpawnRigidBody.isKinematic = false;
            ApplyForce.OnAttack(armorPieceToSpawnRigidBody, null, xAttackOriginPosition, 2, 2);
        }

        private void SpawnDestructionFx()
        {
            Instantiate(sparkSfx, Rigidbody.transform.position, Quaternion.identity);
        }

        private void PlayArmorPieceBreakingSound()
        {
            // AudioManager.Instance.PlaySound("armor_piece_breaking", true);
            AudioManager.Instance.PlayHitSfx();
        }

        /// <summary>
        /// Fonction qui va venir cleaner et retirer de la mémoire la pièce d'armure
        /// Pour l'instant, on ne fait que supprimer le mesh renderer
        /// </summary>
        private void DeleteCurrentArmorPiece()
        {
            Destroy(GetComponent<MeshRenderer>());
        }
    }
}