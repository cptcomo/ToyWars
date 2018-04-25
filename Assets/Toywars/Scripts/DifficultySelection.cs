using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class DifficultySelection : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(this);
        }

        public Difficulty difficulty;
    }
}

