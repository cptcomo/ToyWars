using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class Q : InstantAbility {
        public GameObject bullet;
        public GameObject ablazeEffect;
        public float damage, range;

        public override void activate(Player player) {
            Vector3 pos = Input.mousePosition;
            pos.z = player.getCameraHeightOffset();
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = (pos - player.transform.position).normalized;
            GameObject proj = (GameObject)Instantiate(bullet, player.transform.position, Quaternion.identity);
            SkillshotBullet projScript = proj.GetComponent<SkillshotBullet>();
            projScript.seek(dir);
            projScript.setDamage(damage);
            projScript.setRange(range);
            projScript.setTargetTag(player.targetTag);
            projScript.setIgnoreArmor(level == 3);
            if(level == 3) {
                projScript.setFireball(true);
                proj.GetComponent<Renderer>().enabled = false;
                AblazeBuff ab = new AblazeBuff(2f, damage / 8f, .25f, 0, ablazeEffect);
                projScript.setBuffToApply(ab);
            }
            projScript.setPlayerShot(true);
            nextFire = Time.time + cooldown;
        }
    }

}
