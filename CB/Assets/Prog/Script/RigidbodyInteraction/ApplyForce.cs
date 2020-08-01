using UnityEngine;

namespace Prog.Script.RigidbodyInteraction
{
    public interface IApplyForce
    {
        /// <summary>
        /// Fonction statique permetant d'appliquer de la force en fonction de la position d'origine
        /// de l'attack
        /// (ex: si notre victim est dans le centre, et que l'attack vient de la droite (de la victim),
        /// la victim devrait se déplacer à gauche de sa position initiale)
        /// </summary>
        /// <param name="victim">Rigidbody de la victim qui reçoit l'attack</param>
        /// <param name="attacker">Rigidbody de l'attacker (mettre null si on utilise la xAttackerPosition)</param>
        /// <param name="xAttackerPosition">Position en x d'ou provient l'attaque (mettre à 0 si on utilise le RigidBody</param>
        /// <param name="impactForceOnX">L'amplitude de la force d'attack qui sera appliqué en x</param>
        /// <param name="impactForceOnY">L'amplitude de la force d'attack qui sera appliqué en y</param>
        void OnAttack
        ( 
            Rigidbody victim, 
            Rigidbody attacker = null, 
            float xAttackerPosition = 0,
            float impactForceOnX = 0,
            float impactForceOnY = 0);
    }

    public class ApplyForce : IApplyForce
    {
        /// <summary>
        /// Fonction statique permetant d'appliquer de la force en fonction de la position d'origine
        /// de l'attack
        /// (ex: si notre victim est dans le centre, et que l'attack vient de la droite (de la victim),
        /// la victim devrait se déplacer à gauche de sa position initiale)
        /// </summary>
        /// <param name="victim">Rigidbody de la victim qui reçoit l'attack</param>
        /// <param name="attacker">Rigidbody de l'attacker (mettre null si on utilise la xAttackerPosition)</param>
        /// <param name="xAttackerPosition">Position en x d'ou provient l'attaque (mettre à 0 si on utilise le RigidBody</param>
        /// <param name="impactForceOnX">L'amplitude de la force d'attack qui sera appliqué en x</param>
        /// <param name="impactForceOnY">L'amplitude de la force d'attack qui sera appliqué en y</param>
        public void OnAttack
        ( 
            Rigidbody victim, 
            Rigidbody attacker = null, 
            float xAttackerPosition = 0,
            float impactForceOnX = 0,
            float impactForceOnY = 0)
        {
            var xVictimPosition = victim.transform.position.x;
            if (attacker != null)
            {
                xAttackerPosition = attacker.transform.position.x;
            }

            var movementDirection = xVictimPosition - xAttackerPosition;
            if (movementDirection < 0)
            {
                victim.AddForce(
                    -impactForceOnX,
                    impactForceOnY, 
                    0, 
                    ForceMode.Impulse
                );
            }

            if (movementDirection > 0)
            {
                victim.AddForce(
                    impactForceOnX,
                    impactForceOnY, 
                    0,
                    ForceMode.Impulse
                );
            }
        }
    }
}