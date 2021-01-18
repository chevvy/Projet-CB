using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class AttackPlayer : IState
    {
        private readonly Animator _robotAnimator;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private NavMeshAgent _robotAgent;

        public AttackPlayer(Animator robotAnimator, NavMeshAgent robotAgent)
        {
            _robotAnimator = robotAnimator;
            _robotAgent = robotAgent;
        }
        
        public void Tick() { }

        public void OnEnter()
        {
            // Debug.Log("Enter attack player");
            _robotAgent.enabled = false;
            _robotAnimator.SetTrigger(Attack);
        }

        public void OnExit()
        {
            _robotAgent.enabled = true;
            // Debug.Log("exit attack player");
        }
    }
}