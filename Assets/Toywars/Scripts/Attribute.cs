using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class Attribute {
        [SerializeField]
        private float start;
        private float current;

        public void init() {
            this.current = this.start;
        }

        public float getStart() {
            return this.start;
        }

        public void set(float val) {
            this.current = val;
        }

        public void change(float delta) {
            this.current += delta;
        }

        public float get() {
            return this.current;
        }

        public void modifyPct(float pct) {

        }

        public void modifyFlat(float flat) {

        }
    }
}

