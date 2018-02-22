using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class R : RepeatAbility {
        public GameObject projectile;
        public float damagePerProjectile;
        public float range;
        public override void activate(Player player) {
            StartCoroutine(shootWaves(player));
            nextFire = Time.time + cooldown;
        }

        IEnumerator shootWaves(Player player) {
            for(int i = 0; i < numOfCasts; i++) {
                shootNorth(player);
                shootEast(player);
                shootWest(player);
                shootSouth(player);
                yield return new WaitForSeconds(intervalBtwnCast);
            }
            if(!startCDOnCast)
                nextFire = Time.time + cooldown;
        }

        void shootNorth(Player player) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(Vector3.forward);
            projScript.setDamage(damagePerProjectile);
            projScript.setRange(range);
            projScript.setPlayerShot(true);
        }

        void shootEast(Player player) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(Vector3.right);
            projScript.setDamage(damagePerProjectile);
            projScript.setRange(range);
            projScript.setPlayerShot(true);
        }

        void shootWest(Player player) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(Vector3.left);
            projScript.setDamage(damagePerProjectile);
            projScript.setRange(range);
            projScript.setPlayerShot(true);
        }

        void shootSouth(Player player) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(Vector3.back);
            projScript.setDamage(damagePerProjectile);
            projScript.setRange(range);
            projScript.setPlayerShot(true);
        }
    }
}
