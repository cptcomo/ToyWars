using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerInteractivity {
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour {
        public float startSpeed = 3f;
        public float detectionRadius = 30f;
        private Transform destination;
        private NavMeshAgent nva;
        private GameManager gm;

        private enum State {
            exit = 0,
            chase = 1
        }
        private State state;

        private void Start() {
            state = State.exit;
            gm = GameManager.getInstance();
            MapData mapData = gm.getMapData();
            this.destination = getRandomDestination(mapData);

            nva = GetComponent<NavMeshAgent>();
            nva.SetDestination(this.destination.position);
            nva.stoppingDistance = 1;
            nva.speed = startSpeed;
        }

        private void Update() {
            setTarget();

            if(reachedDestination())
                endPath();
        }

        void setTarget() {
            Collider[] nearby = Physics.OverlapSphere(this.transform.position, detectionRadius);
            bool playerInRange = false;
            foreach(Collider col in nearby) {
                if(col.tag.Equals("Player")) {
                    this.nva.SetDestination(col.transform.position);
                    playerInRange = true;
                    state = State.chase;
                }
            }
            if(!playerInRange) {
                state = State.exit;
                this.nva.SetDestination(destination.position);
            }
        }

        bool reachedDestination() {
            if(state == State.exit) {
                if(!nva.pathPending) {
                    if(nva.remainingDistance <= nva.stoppingDistance) {
                        if(!nva.hasPath || nva.velocity.sqrMagnitude == 0f) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void slow(float slowPct) {
            nva.speed = Mathf.Min(nva.speed, startSpeed * (1f - slowPct));
        }

        void endPath() {
            gm.enemiesAlive--;
            Destroy(gameObject);
        }

        Transform getRandomDestination(MapData mapData) {
            Transform[] dests = mapData.endNodes;
            int random = Random.Range(0, dests.Length);
            return dests[random];
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, detectionRadius);
        }
    }
}
