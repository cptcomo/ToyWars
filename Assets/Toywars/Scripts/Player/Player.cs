using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Toywars {
    public class Player : MonoBehaviour, Damageable {
        public GameObject playerUI;

        [HideInInspector]
        public float damageDone;

        public Attribute health;
        public Attribute armor;
        public Attribute speed;

        public Image healthBar;
        public Text healthText;

        public Vector3 waveStartPosition = new Vector3(0, 0, -96);

        private NavMeshAgent nva;
        private Vector3 dest;
        private float cameraHeightOffset;

        private GameManager gm;

        public string targetTag = "Enemy";

        public Ability Q, W, E, R;
        private Ability[] abilities;
        [HideInInspector]
        public bool usingR = false;
        public AbilityUpgradePath abilityUpgradePath;
        public Image qImage, wImage, eImage, rImage;

        private List<Buff> buffs;
        private List<Attribute> attrs;

        bool dead;

        private void Start() {
            gm = GameManager.getInstance();
            buffs = new List<Buff>();
            attrs = new List<Attribute>();
            attrs.Add(health);
            attrs.Add(armor);
            attrs.Add(speed);
            attrs.ForEach(attr => attr.init());
            nva = GetComponent<NavMeshAgent>();
            nva.speed = speed.getStart();
            abilityUpgradePath.init();
            gm.EndWaveEvent += endWave;
            gm.StartNextWaveEvent += gm.callEventTogglePlayerUI;
            gm.EndWaveEvent += gm.callEventTogglePlayerUI;
            gm.TogglePlayerUIEvent += toggleUI;
            gm.UpgradePlayerEvent += upgradeAbility;
            gm.GameOverEvent += gm.callEventTogglePlayerUI;
            gm.PlayerDeathEvent += playerDeath;
            abilities = new Ability[] { Q, W, E, R };
            foreach(Ability ability in abilities)
                ability.start();
        }

        private void OnDisable() {
            gm.EndWaveEvent -= endWave;
            gm.StartNextWaveEvent -= gm.callEventTogglePlayerUI;
            gm.EndWaveEvent -= gm.callEventTogglePlayerUI;
            gm.TogglePlayerUIEvent -= toggleUI;
            gm.UpgradePlayerEvent -= upgradeAbility;
            gm.GameOverEvent -= gm.callEventTogglePlayerUI;
        }

        private void Update() {
            if(gm.isPlaying() && !dead) {
                resetAttributes();
                updateBuffs();
                updateAbilityUI();
                nva.speed = speed.get();
                if(Input.GetMouseButtonDown(1) && !usingR) {
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
                    gm.callEventPlayerDeath(5 + (gm.waveIndex / 2));
                }

                healthBar.fillAmount = health.get() / health.getStart();
                healthText.text = "" + Mathf.Round(health.get());
            }
            else if(!gm.isPlaying()){
                dest = this.transform.position;
                nva.SetDestination(dest);
            }
        }

        public void addBuff(Buff buff) {
            buffs.Add(buff);
        }

        public void removeBuff(Buff buff) {
            buffs.Remove(buff);
        }

        void resetAttributes() {
            attrs.ForEach(attr => attr.reset());
        }

        void updateBuffs() {
            buffs.ForEach(buff => {
                if(buff.finished)
                    buff.finish();
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

        void Damageable.takeDamage(float dmg, GameObject source, bool ignoreArmor) {
            this.health.modifyFlat(-dmg * armorDamageMultiplier(ignoreArmor, armor.get()) * Random.Range(0.9f, 1.1f), -1, health.getStart());
        }

        void playerDeath(float deathTime) {
            dead = true;
            resetPosition();
            updateAbilityUI();
            this.nva.enabled = false;
            Invoke("revive", deathTime);
        }

        void endWave() {
            resetPosition();
            resetHealth();
            PlayerManager.getInstance().changeExp(10 + 3 * gm.waveIndex);
        }

        void resetHealth() {
            this.health.set(this.health.getStart());
        }

        void revive() {
            this.health.set(this.health.getStart());
            dead = false;
            this.nva.enabled = true;
            this.dest = this.transform.position;
            nva.SetDestination(dest);
            MovementSpeedBuff homeguards = new MovementSpeedBuff(3f, 100f);
            homeguards.apply(this);
        }

        float armorDamageMultiplier(bool ignoreArmor, float armor) {
            if(ignoreArmor)
                return 1;

            if(armor > 0) {
                return 100 / (100 + armor);
            }
            else {
                return 2 - 100 / (100 - armor);
            }
        }

        void toggleUI() {
            playerUI.SetActive(!playerUI.activeSelf);
        }

        public void resetPosition() {
            this.nva.enabled = false;
            this.transform.position = waveStartPosition;
            this.transform.rotation = Quaternion.identity;
            this.nva.enabled = true;
            this.dest = waveStartPosition;
            this.nva.SetDestination(dest);
        }

        public NavMeshAgent getNVA() {
            return nva;
        }

        public void setDest(Vector3 dest) {
            this.dest = dest;
            nva.SetDestination(dest);
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
