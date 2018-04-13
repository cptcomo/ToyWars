using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public abstract class InstantAbility : Ability {
        public abstract override void activate(Player player);
    }
}
