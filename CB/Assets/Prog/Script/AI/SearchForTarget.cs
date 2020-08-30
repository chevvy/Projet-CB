using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class SearchForTarget : IState
    {
        private readonly BasicRobotBehavior _robot;
        private Target[] Points => _robot.patrolPoints;
        private int _destinationPoint = 0;
        private NavMeshAgent NavMeshAgent => _robot.GetComponent<NavMeshAgent>();

        public SearchForTarget(BasicRobotBehavior robot)
        {
            _robot = robot;
        }

        public void Tick()
        { 
            ChooseNextDestination();  
        }

        private void ChooseNextDestination()
        {
            // on check si la target est null
            // Si null, on va à la première target de l'array de position de patrol
            
            // Set the agent to go to the currently selected destination.
            // NavMeshAgent.SetDestination(Points[_destinationPoint].position);
            _robot.Target = Points[_destinationPoint];
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _destinationPoint = (_destinationPoint + 1) % Points.Length;      
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}
