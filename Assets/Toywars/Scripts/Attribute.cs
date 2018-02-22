using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class Attribute {
        [SerializeField]
        private float start;
        private float current;

        bool calledInit = false;

        public void init() {
            this.current = this.start;
            calledInit = true;
        }

        public float getStart() {
            return this.start;
        }

        public void set(float val) {
            if(!calledInit)
                Debug.LogWarning("Attribute init() was not called before callng set");

            this.current = val;
        }

        public void change(float delta) {
            this.current += delta;
        }

        public void reset() {
            set(getStart());
        }

        public float get() {
            if(!calledInit)
                Debug.LogWarning("Attribute init() was not called before callng get");

            return this.current;
        }

        public void modifyPct(float pct) {
            this.current *= (1 + pct / 100);
        }

        public void modifyFlat(float flat) {
            this.current += start;
        }
    }
}

