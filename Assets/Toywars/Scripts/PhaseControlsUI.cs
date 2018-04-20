using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class PhaseControlsUI : MonoBehaviour {
        public GameObject panel;
        public GameObject minionButton;
        public GameObject aiButton;
        public GameObject nextWaveButton;
       
        GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
        }

        private void Update() {
            if(gm.isBuilding()) {
                panel.SetActive(true);
                minionButton.SetActive(true);
                aiButton.SetActive(true);
                nextWaveButton.SetActive(false);
            }
            else if(gm.isAITurn()) {
                minionButton.SetActive(false);
                aiButton.SetActive(false);
                nextWaveButton.SetActive(true);
            }
            else {
                panel.SetActive(false);
                minionButton.SetActive(false);
                aiButton.SetActive(false);
                nextWaveButton.SetActive(false);
            }
        }
    }
}
