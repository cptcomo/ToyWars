using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public abstract class RepeatAbility : Ability {
        public float numOfCasts;
        public float intervalBtwnCast;
        public bool startCDOnCast;
        public abstract override void activate(Player player);
    }
}
