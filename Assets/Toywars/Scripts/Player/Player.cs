using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Toywars {
    public class Player : MonoBehaviour {
        public GameObject playerUI;

        public Attribute health;
        public Attribute speed;

        //Health UI here

        public Vector3 waveStartPosition = new Vector3(0, 0, 96);

        private NavMeshAgent nva;
        private Vector3 dest;
        private float cameraHeightOffset;

        private GameManager gm;

        //Abilities here


        //Buffs here


        private void Start() {
            gm = GameManager.getInstance();
            //buffs = new List<Buff>();
            health.init();
            speed.init();
            nva = GetComponent<NavMeshAgent>();
            nva.speed = speed.getStart();
            nva.angularSpeed = 360;
            nva.acceleration = 15;
            gm.StartNextWaveEvent += resetPosition;
            
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= resetPosition;
        }

        private void Update() {
            if(gm.isPlaying()) {
                resetAttributes();
                updateBuffs();
                updateAbilityUI();
                nva.speed = speed.get();
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
                //Abilities

                //Health Bar
            }
        }

        void resetAttributes() {

        }

        void updateBuffs() {

        }

        void updateAbilityUI() {

        }

        public void resetPosition() {
            this.transform.position = waveStartPosition;
            this.transform.rotation = Quaternion.identity;
            this.nva.SetDestination(waveStartPosition);
        }
    }

}
