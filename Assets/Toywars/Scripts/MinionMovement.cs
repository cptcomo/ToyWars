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
        public Vector3[] waypoints;
        protected int waypointIndex;

        GameObject target;
        protected Vector3 destination;

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

            Debug.Log(nva.destination);
        }

        void setTarget() {
            Collider[] nearby = Physics.OverlapSphere(this.transform.position, detectionRadius.get());
            GameObject closest = null;
            double dist = Mathf.Infinity;
            foreach(Collider col in nearby) {
                foreach(string tag in tagsToDetect) {
                    if(col.tag.Equals(tag)) {
                        double d = Vector3.Distance(this.transform.position, col.transform.position);
                        if(d < dist) {
                            closest = col.gameObject;
                            dist = d;
                        }
                    }
                }
            }
            target = closest;
            if(target != null) {
                state = State.chase;
                this.destination = target.transform.position;
            }
            else {
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

        public GameObject getTarget() {
            return target;
        }
    }
}
