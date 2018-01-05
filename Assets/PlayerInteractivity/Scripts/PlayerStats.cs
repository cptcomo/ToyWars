using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    [System.Serializable]
    public class PlayerStats {
        public int startMoney;
        public int startLives;

        [HideInInspector]
        public int money;
        [HideInInspector]
        public int lives;

        public void init() {
            this.money = startMoney;
            this.lives = startLives;
        }
    }
}