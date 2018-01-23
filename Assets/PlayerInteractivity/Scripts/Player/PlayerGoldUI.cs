using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class PlayerGoldUI : MonoBehaviour {
        public GameObject goldUI;
        public Text goldText;
        private GameManager gm;
        private void Start() {
            gm = GameManager.getInstance();
            show();
            gm.StartNextWaveEvent += hide;
            gm.EndWaveEvent += show;
        }

        private void OnDisable() {
            gm.StartNextWaveEvent -= hide;
            gm.EndWaveEvent -= show;
        }

        private void Update() {
            goldText.text = "$" + gm.playerStats.money;
        }

        void show() {
            goldUI.SetActive(true);
        }

        void hide() {
            goldUI.SetActive(false);
        }
    }
}

