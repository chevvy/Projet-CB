using Prog.Script;
using UnityEngine;

public class WeaponImpactDetector : MonoBehaviour
{
    public PlayerCombat playerCombat;

    // TODO Mettre des vérifications pour être certains d'avoir un rigidbody et un collider sur le mesh

    public void CheckForEnemies() // TODO refactor pour une méthode qui return la liste d'ennemy et autre méthode qui envoi l'attaque pour être plus testable
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Enemy")) continue;
            playerCombat.Attack(hitCollider.GetComponent<Enemy>());
        }
    }
}