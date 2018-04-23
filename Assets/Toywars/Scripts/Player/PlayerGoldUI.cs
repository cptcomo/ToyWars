using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class PlayerGoldUI : MonoBehaviour {
        public GameObject goldUI;
        public Text goldText;
        GameManager gm;
        PlayerManager pm;
        private void Start() {
            gm = GameManager.getInstance();
            show();
            gm.AIStartTurnEvent += hide;
            gm.EndWaveEvent += show;
            pm = PlayerManager.getInstance();
        }

        private void OnDisable() {
            gm.AIStartTurnEvent -= hide;
            gm.EndWaveEvent -= show;
        }

        private void Update() {
            goldText.text = "Money: $" + pm.getMoney();
        }

        void show() {
            goldUI.SetActive(true);
        }

        void hide() {
            goldUI.SetActive(false);
        }
    }

}
