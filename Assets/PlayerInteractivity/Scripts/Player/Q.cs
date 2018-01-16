using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class Q : InstantAbility {
        public GameObject projectile;
        public float damage, range;

        public override void activate(Player player) {
            Vector3 pos = Input.mousePosition;
            pos.z = player.getCameraHeightOffset();
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = (pos - player.transform.position).normalized;
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            Projectile projScript = proj.GetComponent<Projectile>();
            projScript.seek(dir);
            projScript.setDamage(damage);
            projScript.setRange(range);
            nextFire = Time.time + cooldown;
        }
    }
}
