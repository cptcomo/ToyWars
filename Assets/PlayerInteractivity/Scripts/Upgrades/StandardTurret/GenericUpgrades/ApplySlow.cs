using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity
{
    public class ApplySlow : TowerUpgrade
    {
        public float pct;
        public override void activate(Turret turret)
        {
            turret.updateSlowPct(pct);
        }
    }
}
