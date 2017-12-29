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
        }

        void endPath() {
            //PlayerStats.lives--;
            //WaveSpawner.enemiesAlive--;
            Destroy(gameObject);
        }

        Transform getRandomDestination(MapData mapData) {
            Transform[] dests = mapData.endNodes;
            int random = Random.Range(0, dests.Length);
            return dests[random];
        }
    }
}
