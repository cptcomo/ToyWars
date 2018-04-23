using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class WaveNumberUI : MonoBehaviour {
        public Text waveText;

        private void Update() {
            waveText.text = "Wave " + (GameManager.getInstance().waveIndex + 1);
        }
    }
}
