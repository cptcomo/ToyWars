using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brackeys {
    public class MoneyUI : MonoBehaviour {
        public Text moneyText;

        private void Update() {
            moneyText.text = "$" + PlayerStats.money.ToString();
        }
    }
}

