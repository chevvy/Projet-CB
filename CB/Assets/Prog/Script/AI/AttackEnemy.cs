using UnityEngine;

namespace Prog.Script.AI
{
    public class AttackEnemy : IState
    {
        public void Tick()
        {
            // on calcule avec coroutine que ça fait plus que timer d'attack
            // Lorsque ça fait le bon temps, on attack, et on reset le timer
            // Si le player s'est sauvé, on va sortir de ce state
        }

        public void OnEnter()
        {
            Debug.Log("Entering AttackEnemyState");
        }

        public void OnExit()
        {
            Debug.Log("Exiting AttackEnemyState");
        }
    }
}