using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class ELevel2 : AbilityUpgrade {
        public float pctExplosionInc;
        public float pctDmgInc;

        public override void activate(Player player) {
            E e = (E)player.E;
            e.explosionRadius += e.explosionRadius * pctExplosionInc / 100f;
            e.damage += e.damage * pctDmgInc / 100f;
            e.level++;
        }
    }
}
