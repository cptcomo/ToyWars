using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public abstract class Ability : MonoBehaviour {
        public string abilityName, description;
        public float cooldown;
        protected float nextFire;

        public virtual void start() {
            this.nextFire = Time.time;
        }

        public abstract void activate(Player player);
        public bool isAvailable() {
            return Time.time > nextFire;
        }

        public float uiFillAmount() {
            return Mathf.Clamp(1 - (nextFire - Time.time) / cooldown, 0, 1);
        }
    }
}
