using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class AbilityUpgradePath {
        private int qIndex = 0, wIndex = 0, eIndex = 0, rIndex = 0;
        public AbilityUpgrade[] qPath, wPath, ePath, rPath;

        GameManager gm;
        PlayerManager pm;

        public void init() {
            gm = GameManager.getInstance();
            pm = PlayerManager.getInstance();
        }

        public AbilityUpgrade[] getAvailableUpgrades() {
            List<AbilityUpgrade> upgrades = new List<AbilityUpgrade>();
            if(qIndex < qPath.Length) upgrades.Add(qPath[qIndex]);
            else upgrades.Add(null);

            if(wIndex < wPath.Length) upgrades.Add(wPath[wIndex]);
            else upgrades.Add(null);

            if(eIndex < ePath.Length) upgrades.Add(ePath[eIndex]);
            else upgrades.Add(null);

            if(rIndex < rPath.Length) upgrades.Add(rPath[rIndex]);
            else upgrades.Add(null);

            return upgrades.ToArray();
        }

        public void upgrade(int upgradeIndex, Player player) {
            if(upgradeIndex == 0) {
                AbilityUpgrade upgrade = qPath[qIndex];
                if(pm.getExp() >= upgrade.cost) {
                    pm.changeExp(-upgrade.cost);
                    qIndex++;
                    upgrade.activate(player);
                }
            } else if(upgradeIndex == 1) {
                AbilityUpgrade upgrade = wPath[wIndex];
                if(pm.getExp() >= upgrade.cost) {
                    pm.changeExp(-upgrade.cost);
                    wIndex++;
                    upgrade.activate(player);
                }
            } else if(upgradeIndex == 2) {
                AbilityUpgrade upgrade = ePath[eIndex];
                if(pm.getExp() >= upgrade.cost) {
                    pm.changeExp(-upgrade.cost);
                    eIndex++;
                    upgrade.activate(player);
                }
            } else {
                AbilityUpgrade upgrade = rPath[rIndex];
                if(pm.getExp() >= upgrade.cost) {
                    pm.changeExp(-upgrade.cost);
                    rIndex++;
                    upgrade.activate(player);
                }
            }
            gm.callEventShowAbilityUpgrade();
        }
    }
}

