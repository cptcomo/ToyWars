using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerInteractivity {
    public class Player : MonoBehaviour {
        public GameObject projectile;

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
            this.speed = startSpeed;
            nva.angularSpeed = 360;
            nva.acceleration = 15;
            gm.StartNextWaveEvent += resetPosition;
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
                    qAbility();
                }
                else if(Input.GetKeyDown(KeyCode.W)) {
                    wAbility();
                }
            }
        }

        public void qAbility() {
            Vector3 pos = Input.mousePosition;
            pos.z = cameraHeightOffset;
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = pos - this.transform.position;
            GameObject proj = (GameObject)Instantiate(projectile, this.transform.position, Quaternion.identity);
            Projectile projScript = proj.GetComponent<Projectile>();
            projScript.seek(dir);
            projScript.setDamage(50);
            projScript.setRange(70);
        }

        public void wAbility() {
            this.speed *= 1.5f;
            Invoke("resetSpeed", 3f);
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
    }
}
