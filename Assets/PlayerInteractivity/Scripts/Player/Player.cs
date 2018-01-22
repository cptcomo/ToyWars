using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class Player : MonoBehaviour {
        public GameObject playerUI;

        public float startHealth = 100;
        public Image healthBar;
        public Text healthText;
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
        public AbilityUpgradePath abilityUpgradePath;
        public Image QImage, WImage, EImage, RImage;

        private List<Buff> buffs;

        private void Start() {
            gm = GameManager.getInstance();
            buffs = new List<Buff>();
            nva = GetComponent<NavMeshAgent>();
            this.speed = startSpeed;
            this.health = startHealth;
            nva.angularSpeed = 360;
            nva.acceleration = 15;
            gm.StartNextWaveEvent += resetPosition;
            gm.StartNextWaveEvent += gm.callEventTogglePlayerUI;
            gm.EndWaveEvent += gm.callEventTogglePlayerUI;
            gm.UpgradePlayerEvent += upgradeAbility;
            gm.TogglePlayerUIEvent += toggleUI;
            abilities = new Ability[] { Q, W, E, R };
            foreach(Ability ability in abilities)
                ability.start();
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= resetPosition;
            gm.UpgradePlayerEvent -= upgradeAbility;
            gm.TogglePlayerUIEvent -= toggleUI;
            gm.StartNextWaveEvent -= gm.callEventTogglePlayerUI;
            gm.EndWaveEvent -= gm.callEventTogglePlayerUI;
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Play) {
                resetAttributes();
                updateBuffs();
                updateAbilityUI();
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
                if(health <= 0)
                    gm.callEventGameOver();

                healthBar.fillAmount = health / startHealth;
                healthText.text = "" + Mathf.Round(health);
            }
            else {
                dest = this.transform.position;
                nva.SetDestination(dest);
            }
        }
       
        public void addBuff(Buff buff) {
            buffs.Add(buff);
        }

        public void resetAttributes() {
            resetSpeed();    
        }

        void updateBuffs() {
            for(int i = 0; i < buffs.Count; i++) {
                if(buffs[i].finished)
                    buffs.Remove(buffs[i]);
                else buffs[i].tick();
            }
        }

        void resetSpeed() {
            this.speed = startSpeed;
        }

        public void updateSpeed(float pct) {
            this.speed = speed * (1 + pct / 100);
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

        public void takeDamage(float dmg) {
            this.health -= dmg;
        }
        
        void upgradeAbility(int upgradeIndex) {
            abilityUpgradePath.upgrade(upgradeIndex, this);
        }

        void toggleUI() {
            playerUI.SetActive(!playerUI.activeSelf);
        }

        void updateAbilityUI() {
            QImage.fillAmount = Q.uiFillAmount();
            WImage.fillAmount = W.uiFillAmount();
            EImage.fillAmount = E.uiFillAmount();
            RImage.fillAmount = R.uiFillAmount();
        }
    }
}
