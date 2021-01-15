using Prog.Script;
using UnityEngine;

public class WeaponImpactDetector : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerBehavior PlayerBehavior;

    // TODO Mettre des vérifications pour être certains d'avoir un rigidbody et un collider sur le mesh

    public void CheckForEnemies(string enemyTag ="Enemy") // TODO refactor pour une méthode qui return la liste d'ennemy et autre méthode qui envoi l'attaque pour être plus testable
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag(enemyTag)) continue;
            playerCombat.Attack(hitCollider.GetComponent<Enemy>());
        }
    }

    public void CheckForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach(var hitCollider in hitColliders)
        {
            if(!hitCollider.CompareTag("Player")) continue;
            // on passe la position du weapon en argument pour avoir une référence de la direction de l'attaque
            PlayerBehavior.OnGetsAttacked(transform.position.x);
            return;
        }
    }
}