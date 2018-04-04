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
            this.current = this.current + (this.getStart() * (pct / 100));
        }

        public void modifyPct(float pct, float min, float max) {
            this.current = Mathf.Clamp(this.current + (this.getStart() * (pct / 100)), min, max);
        }

        public void modifyFlat(float flat) {
            this.current += flat;
        }

        public void modifyFlat(float flat, float min, float max) {
            this.current = Mathf.Clamp(this.current + flat, min, max);
        }

        public float getPctStart(float pct) {
            return pct / 100 * getStart();
        }

        public float getMissing() {
            return getStart() - get();
        }

        public float getPctMissing() {
            return getMissing() / getStart();
        }
    }
}

