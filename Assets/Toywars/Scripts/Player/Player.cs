using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Toywars {
    public class Player : MonoBehaviour, Damageable {
        public GameObject playerUI;

        public Attribute health;
        public Attribute speed;

        public Image healthBar;
        public Text healthText;

        public Vector3 waveStartPosition = new Vector3(0, 0, 96);

        private NavMeshAgent nva;
        private Vector3 dest;
        private float cameraHeightOffset;

        private GameManager gm;

        public string targetTag = "Enemy";

        public Ability Q, W, E, R;
        private Ability[] abilities;
        public AbilityUpgradePath abilityUpgradePath;
        public Image qImage, wImage, eImage, rImage;

        private List<Buff> buffs;
        private List<Attribute> attrs;

        private void Start() {
            gm = GameManager.getInstance();
            buffs = new List<Buff>();
            attrs = new List<Attribute>();
            attrs.Add(health);
            attrs.Add(speed);
            attrs.ForEach(attr => attr.init());
            nva = GetComponent<NavMeshAgent>();
            nva.speed = speed.getStart();
            abilityUpgradePath.init();
            gm.StartNextWaveEvent += resetPosition;
            gm.StartNextWaveEvent += gm.callEventTogglePlayerUI;
            gm.EndWaveEvent += gm.callEventTogglePlayerUI;
            gm.TogglePlayerUIEvent += toggleUI;
            gm.UpgradePlayerEvent += upgradeAbility;
            gm.GameOverEvent += gm.callEventTogglePlayerUI;
            abilities = new Ability[] { Q, W, E, R };
            foreach(Ability ability in abilities)
                ability.start();
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= resetPosition;
            gm.StartNextWaveEvent -= gm.callEventTogglePlayerUI;
            gm.EndWaveEvent -= gm.callEventTogglePlayerUI;
            gm.TogglePlayerUIEvent -= toggleUI;
            gm.UpgradePlayerEvent -= upgradeAbility;
            gm.GameOverEvent -= gm.callEventTogglePlayerUI;
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

                if(Input.GetKeyDown(KeyCode.Q)) {
                    if(Q.isAvailable())
                        Q.activate(this);
                } else if(Input.GetKeyDown(KeyCode.W)) {
                    if(W.isAvailable())
                        W.activate(this);
                } else if(Input.GetKeyDown(KeyCode.E)) {
                    if(E.isAvailable())
                        E.activate(this);
                } else if(Input.GetKeyDown(KeyCode.R)) {
                    if(R.isAvailable())
                        R.activate(this);
                }

                if(health.get() < 0f) {
                    gm.callEventGameOver(false);
                }

                healthBar.fillAmount = health.get() / health.getStart();
                healthText.text = "" + Mathf.Round(health.get());
            }
            else {
                dest = this.transform.position;
                nva.SetDestination(dest);
            }
        }

        public void addBuff(Buff buff) {
            buffs.Add(buff);
        }

        void resetAttributes() {
            attrs.ForEach(attr => attr.reset());
        }

        void updateBuffs() {
            buffs.ForEach(buff => {
                if(buff.finished)
                    buffs.Remove(buff);
                else
                    buff.tick();
            });
        }

        void updateAbilityUI() {
            qImage.fillAmount = Q.uiFillAmount();
            wImage.fillAmount = W.uiFillAmount();
            eImage.fillAmount = E.uiFillAmount();
            rImage.fillAmount = R.uiFillAmount();
        }

        void Damageable.takeDamage(float dmg, bool isPlayerShot) {
            this.health.change(-dmg);
        }

        void toggleUI() {
            playerUI.SetActive(!playerUI.activeSelf);
        }

        public void resetPosition() {
            this.transform.position = waveStartPosition;
            this.transform.rotation = Quaternion.identity;
            this.nva.SetDestination(waveStartPosition);
        }

        void upgradeAbility(int upgradeIndex) {
            abilityUpgradePath.upgrade(upgradeIndex, this);
        }

        public void setCameraHeightOffset(float newHeight) {
            cameraHeightOffset = newHeight;
        }

        public float getCameraHeightOffset() {
            return cameraHeightOffset;
        }
    }

}
