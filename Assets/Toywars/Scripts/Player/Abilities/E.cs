using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class E : InstantAbility {
        public GameObject projectile;
        public float damage;
        public float range;
        public float explosionRadius;
        public float pctDamage;
        public float pctDistanceDamage;
        public override void activate(Player player) {
            Vector3 pos = Input.mousePosition;
            pos.z = player.getCameraHeightOffset();
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = (pos - player.transform.position).normalized;
            shoot(player, dir);
            nextFire = Time.time + cooldown;
        }

        void shoot(Player player, Vector3 dir) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(dir);
            projScript.setDamage(damage);
            projScript.setRange(range);
            projScript.setTargetTag(player.targetTag);
            projScript.setIgnoreArmor(false);
            projScript.setExplosionRadius(explosionRadius);
            projScript.setPlayerShot(true);
            if(level == 3) {
                projScript.setPctDistanceDamage(pctDamage);
                projScript.setPctDistanceDamage(pctDistanceDamage);
            }
            proj.transform.localScale *= level;
        }
    }
}
