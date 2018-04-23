using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class TowerUpgradePath {
        [HideInInspector]
        public int leftIndex = 0, rightIndex = 0;
        public TowerUpgrade[] leftPath;
        public TowerUpgrade[] rightPath;

        GameManager gm;
        PlayerManager pm;
        EnemiesManager em;

        public void init() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
            em = EnemiesManager.getInstance();
        }

        public TowerUpgrade[] getAvailableUpgrades() {
            if(leftIndex > 2) {
                if(leftIndex != leftPath.Length) {
                    if(rightIndex >= 2)
                        return new TowerUpgrade[] { leftPath[leftIndex], null };
                    else return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
                } else {
                    if(rightIndex >= 2)
                        return new TowerUpgrade[] { null, null };
                    else
                        return new TowerUpgrade[] { null, rightPath[rightIndex] };
                }

            }
            if(rightIndex > 2) {
                if(rightIndex != rightPath.Length) {
                    if(leftIndex >= 2)
                        return new TowerUpgrade[] { null, rightPath[rightIndex] };
                    else
                        return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
                } else {
                    if(leftIndex >= 2)
                        return new TowerUpgrade[] { null, null };
                    else return new TowerUpgrade[] { leftPath[leftIndex], null };
                }
            }
            return new TowerUpgrade[] { leftPath[leftIndex], rightPath[rightIndex] };
        }

        public bool hasRemainingUpgrades() {
            TowerUpgrade[] upgrades = getAvailableUpgrades();
            if(upgrades[0] == null && upgrades[1] == null)
                return false;
            else
                return true;
        }

        public void upgrade(int upgradeIndex, Turret turret, bool playerTurret) {
            if(upgradeIndex == 0) {
                TowerUpgrade upgrade = leftPath[leftIndex];
                if(playerTurret) {
                    if(pm.getMoney() >= upgrade.cost) {
                        pm.changeMoney(-upgrade.cost);
                        leftIndex++;
                        upgrade.activate(turret);
                    }
                }
                else {
                    if(em.getMoney() >= upgrade.cost) {
                        em.changeMoney(-upgrade.cost);
                        leftIndex++;
                        upgrade.activate(turret);
                    }
                }
                
            } else {
                TowerUpgrade upgrade = rightPath[rightIndex];
                if(playerTurret) {
                    if(pm.getMoney() >= upgrade.cost) {
                        pm.changeMoney(-upgrade.cost);
                        rightIndex++;
                        upgrade.activate(turret);
                    }
                }
                else {
                    if(em.getMoney() >= upgrade.cost) {
                        em.changeMoney(-upgrade.cost);
                        rightIndex++;
                        upgrade.activate(turret);
                    }
                }
            }
        }
    }
}
