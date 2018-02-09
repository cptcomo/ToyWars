using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Toywars {
    public class MinionMovement : MonoBehaviour {
        protected GameManager gm;
        protected PlayerManager pm;
        protected EnemiesManager em;
        public Attribute speed;
        public Attribute detectionRadius;
        public string[] tagsToDetect;

        protected NavMeshAgent nva;
        protected Vector3 destination;
        public Vector3[] waypoints;
        protected int waypointIndex;

        protected enum State {
            exit = 0,
            chase = 1
        }

        protected State state;

        protected virtual void Start() {
            state = State.exit;
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
            speed.init();
            detectionRadius.init();
            nva = GetComponent<NavMeshAgent>();
            nva.stoppingDistance = 1;
            nva.speed = speed.getStart();
        }

        protected virtual void Update() {
            setTarget();

            nva.SetDestination(destination);

            if(reachedDestination()) {
                if(waypointIndex == waypoints.Length - 1) {
                    endPath();
                }
                else {
                    if(state == State.exit && Vector3.Distance(this.transform.position, getNextDestination()) < 3) {
                        waypointIndex++;
                        this.destination = getNextDestination();
                    }
                }
            }
        }

        void setTarget() {
            Collider[] nearby = Physics.OverlapSphere(this.transform.position, detectionRadius.get());
            bool hasTarget = false;
            foreach(Collider col in nearby) {
                foreach(string tag in tagsToDetect) {
                    if(col.tag.Equals(tag)) {
                        this.destination = col.transform.position;
                        hasTarget = true;
                        state = State.chase;
                    }
                }
            }
            if(!hasTarget) {
                state = State.exit;
                this.destination = getNextDestination();
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

        protected virtual Vector3 getNextDestination() {
            return waypoints[waypointIndex];
        }

        protected virtual void endPath() {}
    }
}
