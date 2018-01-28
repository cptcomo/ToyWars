using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public abstract class InstantAbility : Ability {
        public abstract override void activate(Player player);
    }
}
