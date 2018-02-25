using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class PlayerExperienceUI : MonoBehaviour {
        public Text expText;
        PlayerManager pm;
        private void Start() {
            pm = PlayerManager.getInstance();
        }
        private void Update() {
            expText.text = "" + pm.exp;
        }
    }
}

