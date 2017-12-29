using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerInteractivity {
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour {
        private Transform destination;
        private NavMeshAgent nva;
        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            MapData mapData = gm.getMapData();
            this.destination = getRandomDestination(mapData);

            nva = GetComponent<NavMeshAgent>();
            nva.SetDestination(this.destination.position);
            nva.stoppingDistance = 1;
        }

        private void Update() {
            if(reachedDestination())
                endPath();
        }

        bool reachedDestination() {
            if(!nva.pathPending) {
                if(nva.remainingDistance <= nva.stoppingDistance) {
                    Debug.Log(this.transform.name);
                    if(!nva.hasPath || nva.velocity.sqrMagnitude == 0f) {
                        return true;
                    }
                }
            }
            return false;
        }

        void endPath() {
            //PlayerStats.lives--;
            gm.enemiesAlive--;
            Destroy(gameObject);
        }

        Transform getRandomDestination(MapData mapData) {
            Transform[] dests = mapData.endNodes;
            int random = Random.Range(0, dests.Length);
            return dests[random];
        }
    }
}
