using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class W : InstantAbility {
        public float pct;
        public float duration;
        public override void activate(Player player) {
            if(level == 3) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)) {
                    Vector3 pos = Input.mousePosition;
                    pos.z = player.getCameraHeightOffset();
                    pos = Camera.main.ScreenToWorldPoint(pos);
                    player.getNVA().enabled = false;
                    player.transform.position = pos;
                    player.getNVA().enabled = true;
                }
            }

            MovementSpeedBuff msb = new MovementSpeedBuff(duration, pct);
            msb.apply(player);
            nextFire = Time.time + cooldown;
        }
    }

}
