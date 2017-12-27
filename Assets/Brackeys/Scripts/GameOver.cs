using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Brackeys {
    public class GameOver : MonoBehaviour {
        public Text roundsText;
        private void OnEnable() {
            roundsText.text = PlayerStats.rounds.ToString();
        }

        public void retry() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void menu() {
            SceneManager.LoadScene(0);
        }
    }
}

