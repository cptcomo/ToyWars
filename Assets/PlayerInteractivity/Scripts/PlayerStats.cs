using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    [System.Serializable]
    public class PlayerStats {
        public int startMoney;
        public int startLives;
        public int startExperience;

        [HideInInspector]
        public int money;
        [HideInInspector]
        public int lives;
        [HideInInspector]
        public int exp;

        public void init() {
            this.money = startMoney;
            this.lives = startLives;
            this.exp = startExperience;
        }
    }
}