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

            if (!Rigidbody.isKinematic)
            {
                Debug.LogError("The armorPiece '" + name + "' rigidBody must be kinematic");
            }
        }

        public void RemoveArmorPiece(float xAttackOriginPosition)
        {
            Rigidbody.isKinematic = false;
            PlayArmorPieceBreakingSound();
            Instantiate(sparkSfx, Rigidbody.transform.position, Quaternion.identity);
            ApplyForce.OnAttack(Rigidbody, null, xAttackOriginPosition, 5, 5);
        }

        private void PlayArmorPieceBreakingSound()
        {
            AudioManager.Instance.PlaySound("armor_piece_breaking", true);
        }
    }
}