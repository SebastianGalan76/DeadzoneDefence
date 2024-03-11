using UnityEngine;
using UnityEngine.AI;

namespace Zombie {
    public class Movement : MonoBehaviour {
        [SerializeField] private float viewRadius;
        [SerializeField] private LayerMask enemyMask;

        private NavMeshAgent agent;
        private Animator animator;
        private Vector3 currentDestination;

        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Start() {
            InvokeRepeating("FindNewTarget", Random.Range(0,1f), 1f);
        }

        public void StartMoving(Vector3 destination) {
            agent.isStopped = false;

            currentDestination = destination;
            agent.SetDestination(destination);

            animator.Play("Zombie_Walk");
        } 

        private void StopMoving() {
            agent.isStopped = true;

            currentDestination = Vector3.zero;
            animator.Play("Zombie_Idle");
        }

        public float GetViewRadius() {
            return viewRadius;
        }

        private void FindNewTarget() {
            Vector3 closestTarget = GetClosestTargetPosition();

            if(closestTarget != Vector3.zero) {
                StartMoving(closestTarget);
            } else {
                StopMoving();
            }

            Vector3 GetClosestTargetPosition() {
                Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

                if(targets.Length == 0) {
                    return Vector3.zero;
                }

                Transform closestTarget = GetClosestEnemy(targets).transform;
                return closestTarget.position;

                Collider GetClosestEnemy(Collider[] enemies) {
                    Collider bestTarget = null;
                    float closestDistanceSqr = Mathf.Infinity;
                    Vector3 currentPosition = transform.position;

                    foreach(Collider potentialTarget in enemies) {
                        Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                        float dSqrToTarget = directionToTarget.sqrMagnitude;

                        if(dSqrToTarget < closestDistanceSqr) {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }

                    return bestTarget;
                }
            }
        }
    }
}
