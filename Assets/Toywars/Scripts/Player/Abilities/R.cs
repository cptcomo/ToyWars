using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class R : RepeatAbility {
        public GameObject projectileBullet;
        public GameObject projectileMissile;
        public float damagePerProjectile;
        public float range;
        public float armorShred;
        public float explosionRadius;
        public override void activate(Player player) {
            player.usingR = true;
            StartCoroutine(shootWaves(player));
            nextFire = Time.time + cooldown;
        }

        IEnumerator shootWaves(Player player) {
            player.setDest(player.transform.position);
            Vector3 pos = Input.mousePosition;
            pos.z = player.getCameraHeightOffset();
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = (pos - player.transform.position).normalized;
            for(int i = 0; i < numOfCasts; i++) {
                shootWave(player, dir);
                yield return new WaitForSeconds(intervalBtwnCast);
            }
            if(!startCDOnCast)
                nextFire = Time.time + cooldown;
            player.usingR = false;
        }

        void shootWave(Player player, Vector3 mouseDir) {
            shoot(player, mouseDir);
            shoot(player, Quaternion.Euler(0, 5, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, -5, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, 10, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, -10, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, 15, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, -15, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, 20, 0) * mouseDir);
            shoot(player, Quaternion.Euler(0, -20, 0) * mouseDir);
        }

        void shoot(Player player, Vector3 dir) {
            GameObject proj = (GameObject)Instantiate(level == 3 ? projectileMissile : projectileBullet, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            proj.transform.localScale /= 1.6f;
            projScript.seek(dir);
            projScript.setDamage(damagePerProjectile);
            projScript.setRange(range);
            projScript.setTargetTag(player.targetTag);
            projScript.setIgnoreArmor(false);
            projScript.setPlayerShot(true);
            if(level >= 2) {
                ArmorShredBuff buff = new ArmorShredBuff(2, armorShred);
                projScript.setBuffToApply(buff);
            }
            projScript.setExplosionRadius(level == 3 ? explosionRadius : 0f);
            projScript.setChanceToImpactEffect(level == 3 ? .2f : 1f);
        }
    }
}
