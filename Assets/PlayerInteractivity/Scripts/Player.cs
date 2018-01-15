using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerInteractivity {
    public class Player : MonoBehaviour {
        public float startHealth = 100;
        private float health;
        public float startSpeed = 5;
        private float speed;

        public Vector3 roundStartPosition = new Vector3(2.5f, 0.5f, 7);

        private NavMeshAgent nva;
        private Vector3 dest;
        private float cameraHeightOffset;

        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            nva = GetComponent<NavMeshAgent>();
            nva.speed = startSpeed;
            nva.angularSpeed = 360;
            nva.acceleration = 15;
            gm.StartNextWaveEvent += resetPosition;
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= resetPosition;
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Play) {
                if(Input.GetMouseButtonDown(1)) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if(Physics.Raycast(ray, out hit)) {
                        Vector3 pos = Input.mousePosition;
                        pos.z = cameraHeightOffset;
                        pos = Camera.main.ScreenToWorldPoint(pos);
                        this.dest = pos;
                        nva.SetDestination(this.dest);
                    }
                }
            }
        }

        public void resetPosition() {
            this.transform.position = roundStartPosition;
            this.transform.rotation = Quaternion.identity;
            this.nva.SetDestination(roundStartPosition);
        }

        public void setCameraHeightOffset(float newHeight) {
            cameraHeightOffset = newHeight;
        }
    }
}
