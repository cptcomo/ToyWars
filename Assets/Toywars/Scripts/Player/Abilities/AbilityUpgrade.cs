using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public abstract class AbilityUpgrade : MonoBehaviour {
        public string upgradeName;
        public int cost;

        public abstract void activate(Player player);
    }
}
