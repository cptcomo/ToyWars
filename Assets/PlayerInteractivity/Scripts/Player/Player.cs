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

        public Ability Q, W, E, R;
        private Ability[] abilities;

        private void Start() {
            gm = GameManager.getInstance();
            nva = GetComponent<NavMeshAgent>();
            this.speed = startSpeed;
            nva.angularSpeed = 360;
            nva.acceleration = 15;
            gm.StartNextWaveEvent += resetPosition;
            abilities = new Ability[] { Q, W, E, R };
            foreach(Ability ability in abilities)
                ability.start();
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= resetPosition;
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Play) {
                nva.speed = speed;
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

                if(Input.GetKeyDown(KeyCode.Q)) {
                    if(Q.isAvailable())
                        Q.activate(this);
                }
                else if(Input.GetKeyDown(KeyCode.W)) {
                    if(W.isAvailable())
                        W.activate(this);
                }
                else if(Input.GetKeyDown(KeyCode.E)) {
                    if(E.isAvailable())
                        E.activate(this);
                }
                else if(Input.GetKeyDown(KeyCode.R)) {
                    if(R.isAvailable())
                        R.activate(this);
                }
            }
        }
       
        void resetSpeed() {
            this.speed = startSpeed;
        }

        public void resetPosition() {
            this.transform.position = roundStartPosition;
            this.transform.rotation = Quaternion.identity;
            this.nva.SetDestination(roundStartPosition);
        }

        public void setCameraHeightOffset(float newHeight) {
            cameraHeightOffset = newHeight;
        }

        public float getCameraHeightOffset() {
            return this.cameraHeightOffset;
        }
    }
}
