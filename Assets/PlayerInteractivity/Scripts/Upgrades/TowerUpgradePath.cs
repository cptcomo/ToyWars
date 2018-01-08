using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    [System.Serializable]
    public class TowerUpgradePath {
        private int leftIndex = 0, rightIndex = 0;
        public TowerUpgrade[] leftPath;
        public TowerUpgrade[] rightPath;

        public TowerUpgrade[] getAvailableUpgrades() {
            if(leftIndex >= 2) {
                if(leftIndex != leftPath.Length)
                    return new TowerUpgrade[] { leftPath[leftIndex] };
            } else if(rightIndex >= 2) {
                if(rightIndex != rightPath.Length)
                    return new TowerUpgrade[] { rightPath[rightIndex] };
            } 
            return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
        }
    }
}
