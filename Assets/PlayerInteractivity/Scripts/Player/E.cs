﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class E : RepeatAbility {
        public GameObject projectile;
        public float damagePerBullet;
        public float range;
        Vector3 dir;
        public override void activate(Player player) {
            Vector3 pos = Input.mousePosition;
            pos.z = player.getCameraHeightOffset();
            pos = Camera.main.ScreenToWorldPoint(pos);
            dir = (pos - player.transform.position).normalized;
            StartCoroutine(tripleShot(player));
            nextFire = Time.time + cooldown;
        }

        IEnumerator tripleShot(Player player) {
            for(int i = 0; i < numOfCasts; i++) {
                shoot(player);
                yield return new WaitForSeconds(intervalBtwnCast);
            }
            if(!startCDOnCast)
                nextFire = Time.time + cooldown;
        }

        void shoot(Player player) {
            GameObject proj = (GameObject)Instantiate(projectile, player.transform.position, Quaternion.identity);
            Projectile projScript = proj.GetComponent<Projectile>();
            projScript.seek(dir);
            projScript.setDamage(damagePerBullet);
            projScript.setRange(range);
            projScript.setPlayerShot(true);
        }
    }
}
