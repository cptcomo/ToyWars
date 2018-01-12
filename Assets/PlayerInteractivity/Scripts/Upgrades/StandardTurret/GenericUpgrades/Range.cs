﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity
{ 
    public class Range : TowerUpgrade
    {
        public float pct;

        public override void activate(Turret turret)
        {
            turret.updateRange(pct);
        }
    }
}
