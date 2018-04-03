using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Toywars {
    public class MinionManagementUI : MonoBehaviour {
        private static MinionManagementUI instance;

        public GameObject selectionCanvas;
        public GameObject wavesCanvas;

        public Image[] selectionPanels;
        bool[] panelsLockStatus;

        public GameObject[] minions;

        private Selection selectedMinion;
        private Selection[] allSelections = new Selection[] { Selection.NONE, Selection.DELETE, Selection.SIMPLE, Selection.NORMAL, Selection.FAST, Selection.TOUGH, Selection.DIVIDE };

        private Color selectedColor = new Color(0, 200, 0, 155);
        private Color initialColor;

        Dictionary<Selection, Minion> minionMap;
        public int[] minionUnlockLevels;

        GameManager gm;

        public GameObject leftLane, rightLane, centerLane;
        GameObject[] leftLaneSlots, rightLaneSlots, centerLaneSlots;
        public int[] slotUnlockLevels;
        bool[] slotUnlockStatus;

        public static MinionManagementUI getInstance() {
            return instance;
        }

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            selectedMinion = Selection.NONE;
            initialColor = selectionPanels[0].color;
            gm = GameManager.getInstance();
            gm.EndWaveEvent += updateLocks;
            gm.MinionManagementOpenEvent += open;
            gm.MinionManagementCloseEvent += close;
            gm.SlotAddEvent += slotOnClick;
            initialize();
        }

        void OnDisable() {
            gm.EndWaveEvent -= updateLocks;
            gm.MinionManagementOpenEvent -= open;
            gm.MinionManagementCloseEvent -= close;
            gm.SlotAddEvent -= slotOnClick;
        }

        void initialize() {
            initializeSelectionPanels();
            leftLaneSlots = getAllChildren(leftLane);
            rightLaneSlots = getAllChildren(rightLane);
            centerLaneSlots = getAllChildren(centerLane);
            addClickListeners(leftLaneSlots);
            addClickListeners(rightLaneSlots);
            addClickListeners(centerLaneSlots);
            initializeMinionMap();
            initializeLockArrays();
            updateLocks();
            colorLocks();
        }

        void initializeSelectionPanels() {
            for(int i = 0; i < minions.Length; i++) {
                changeSprite(selectionPanels[i + 1].gameObject, minions[i].GetComponent<Minion>().sprite);
            }
        }

        void initializeMinionMap() {
            minionMap = new Dictionary<Selection, Minion>();
            for(int i = 0; i < minions.Length; i++) {
                minionMap[allSelections[i + 2]] = minions[i].GetComponent<Minion>();
            }
        }

        void initializeLockArrays() {
            panelsLockStatus = new bool[selectionPanels.Length - 1];
            slotUnlockStatus = new bool[centerLaneSlots.Length];
        }

        void changeSprite(GameObject slot, Sprite sprite) {
            Transform[] ts = slot.GetComponentsInChildren<Transform>();

            foreach(Transform t in ts) {
                if(t.name.ToLower().Contains("image")) {
                    t.GetComponent<Image>().sprite = sprite;
                }
            }
        }

        GameObject[] getAllChildren(GameObject laneObject) {
            Transform[] transforms = laneObject.GetComponentsInChildren<Transform>();
            List<GameObject> gos = new List<GameObject>();
            for(int i = 0; i < transforms.Length; i++) {
                if(transforms[i].parent.name.ToLower().Contains("lane"))
                    gos.Add(transforms[i].gameObject);
            }
            return gos.ToArray();
        }

        void addClickListeners(GameObject[] slots) {
            foreach(GameObject go in slots) {
                go.gameObject.AddComponent<SlotClickListener>();
            }
        }

        void slotOnClick(GameObject slot) {
            if(minionSelected()) {
                Minion m = minionMap[selectedMinion];
                changeSprite(slot, minionMap[selectedMinion].sprite);
            }
            else if(selectedMinion == Selection.DELETE) {
                changeSprite(slot, null);
            }
        }

        void open() {
            gm.gameState = GameManager.GameState.MinionManagement;
            selectedMinion = Selection.NONE;
            selectionCanvas.SetActive(true);
            wavesCanvas.SetActive(true);
        }

        void close() {
            gm.gameState = GameManager.GameState.Build;
            selectionCanvas.SetActive(false);
            wavesCanvas.SetActive(false);
        }

        private void Update() {
            if(gm.isMinionManagement()) {
                colorLocks();
                colorSelection();
            }
        }

        void updateLocks() {
            int level = gm.waveIndex + 1;
            for(int i = 0; i < minionUnlockLevels.Length; i++) {
                if(minionUnlockLevels[i] == level) {
                    panelsLockStatus[i] = true;
                }
            }
            for(int i = 0; i < slotUnlockLevels.Length; i++) {
                if(slotUnlockLevels[i] == level) {
                    slotUnlockStatus[i] = true;
                }
            }
        }

        void colorLocks() {
            for(int i = 1; i < selectionPanels.Length; i++) {
                if(!panelsLockStatus[i - 1]) {
                    selectionPanels[i].color = Color.red;
                }
            }
        }

        void colorSelection() {
            int index = -1;

            for(int i = 0; i < allSelections.Length; i++) {
                if(allSelections[i] == selectedMinion)
                    index = i;
            }

            for(int i = 0; i < selectionPanels.Length; i++) {
                if(i == index - 1)
                    selectionPanels[i].color = selectedColor;
                else
                    selectionPanels[i].color = initialColor;
            }
        }

        public Wave[] getWaveComposition() {
            Wave[] lanes = new Wave[3];
            lanes[0] = convertSpritesToWave(leftLaneSlots);
            lanes[1] = convertSpritesToWave(centerLaneSlots);
            lanes[2] = convertSpritesToWave(rightLaneSlots);
            return lanes;
        }

        Wave convertSpritesToWave(GameObject[] slots) {
            Wave wave = new Wave();
            for(int i = 0; i < slots.Length; i++) {
                Transform[] ts = slots[i].GetComponentsInChildren<Transform>();
                foreach(Transform t in ts) {
                    if(t.name.ToLower().Contains("image")) {
                        foreach(GameObject go in minions) {
                            if(go.GetComponent<Minion>().sprite == t.GetComponent<Image>().sprite) {
                                WaveSection section = new WaveSection();
                                section.minion = go;
                                section.count = 1;
                                section.rate = 1;
                                wave.sections.Add(section);
                            }
                        }
                    }
                }
            }
            return wave;
        }

        enum Selection {
            NONE, DELETE, SIMPLE, NORMAL, FAST, DIVIDE, TOUGH
        }

        public void selectNone() {
            selectedMinion = Selection.NONE;
        }

        public void selectDelete() {
            selectedMinion = Selection.DELETE;
        }

        public void selectSimple() {
            selectedMinion = Selection.SIMPLE;
        }

        public void selectNormal() {
            selectedMinion = Selection.NORMAL;
        }

        public void selectFast() {
            selectedMinion = Selection.FAST;
        }

        public void selectTough() {
            selectedMinion = Selection.TOUGH;
        }

        public void selectDivide() {
            selectedMinion = Selection.DIVIDE;
        }

        bool minionSelected() {
            return selectedMinion != Selection.NONE && selectedMinion != Selection.DELETE;
        }
    }
}
