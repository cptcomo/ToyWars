using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class LivesUI : MonoBehaviour {
        public Text livesText;
        private void Update() {
            livesText.text = PlayerStats.lives + " LIVES";
        }
    }
}
