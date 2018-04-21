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
        protected int livesValue;
        float kiteRange;

        protected enum State {
            Exit, Chase, Kite
        }

        protected State state;
        protected MinionType minionType;

        protected virtual void Start() {
            state = State.Exit;
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
            nva = GetComponent<NavMeshAgent>();
            nva.stoppingDistance = 1;
            nva.speed = speed.getStart();
        }

        public void setMinionType(MinionType type) {
            minionType = type;
        }

        public void setKiteRange(float range) {
            this.kiteRange = range;
        }

        public void setLivesValue(int livesValue) {
            this.livesValue = livesValue;
        }

        protected virtual void Update() {
            setTarget();

            nva.SetDestination(destination);
            nva.speed = speed.get();

            if(Vector3.Distance(this.transform.position, waypoints[waypoints.Length - 1]) < 10f) {
                endPath();
                return;
            }

            if(reachedDestination()) {
                if(waypointIndex == waypoints.Length - 1) {
                    endPath();
                }
                else {
                    if(state == State.Exit && Vector3.Distance(this.transform.position, getNextDestination()) < 5f) {
                        waypointIndex++;
                        this.destination = getNextDestination();
                    }
                }
            }
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
                Vector3 dir = (target.transform.position - this.transform.position).normalized;
                if(target.tag.Equals("Player")) {
                    Vector3 basedir = (waypoints[waypoints.Length - 1] - this.transform.position).normalized;
                    float angle = Mathf.Acos(Vector3.Dot(basedir, dir) / (basedir.magnitude * dir.magnitude)) * 180f / 3.14159265f;
                    if(angle > 115 && minionType != MinionType.Fast) {
                        state = State.Exit;
                        this.destination = getNextDestination();
                        return;
                    }
                }

                if(minionType == MinionType.Range) {
                    state = State.Kite;
                    this.destination = -dir.normalized * (kiteRange / 2);
                }
                else {
                    state = State.Chase;
                    this.destination = target.transform.position;
                }
            }
            else {
                state = State.Exit;
                this.destination = getNextDestination();
            }
        }

        bool reachedDestination() {
            if(state == State.Exit) {
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

        public int getWaypointIndex() {
            return this.waypointIndex;
        }

        public void setWaypointIndex(int index) {
            this.waypointIndex = index;
        }
    }
}
