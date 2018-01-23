using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerInteractivity {
    public class PlayerExperienceUI : MonoBehaviour {
        public Text expText;
        GameManager gm;
        private void Start() {
            gm = GameManager.getInstance();
        }
        private void Update() {
            expText.text = "" + gm.playerStats.exp;
        }
    }
}
