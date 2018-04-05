using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class Attribute {
        [SerializeField]
        private float start;
        private float currentUnbuffed;
        private float currentBuffed;

        bool calledInit = false;

        public void init() {
            this.currentUnbuffed = this.currentBuffed = this.start;
            calledInit = true;
        }

        public float getStart() {
            return this.start;
        }

        public void set(float val) {
            if(!calledInit)
                Debug.LogWarning("Attribute init() was not called before callng set");

            this.currentUnbuffed = this.currentBuffed = val;
        }

        public void reset() {
            this.currentBuffed = this.currentUnbuffed;
        }

        public float get() {
            if(!calledInit)
                Debug.LogWarning("Attribute init() was not called before callng get");

            return this.currentBuffed;
        }

        public void modifyPct(float pct) {
            this.currentUnbuffed = this.currentBuffed = this.currentUnbuffed + (this.getStart() * (pct / 100));
        }

        public void modifyPct(float pct, float min, float max) {
            this.currentUnbuffed = this.currentBuffed = Mathf.Clamp(this.currentUnbuffed + (this.getStart() * (pct / 100)), min, max);
        }

        public void modifyFlat(float flat) {
            this.currentUnbuffed = this.currentBuffed = this.currentUnbuffed + flat;
        }

        public void modifyFlat(float flat, float min, float max) {
            this.currentUnbuffed = this.currentBuffed = Mathf.Clamp(this.currentUnbuffed + flat, min, max);
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

        public void buffFlat(float flat) {
            this.currentBuffed = this.currentBuffed + flat;
        }

        public void buffPct(float pct) {
            this.currentBuffed = this.currentUnbuffed + (this.currentBuffed * (pct / 100));
        }

        public void buffFlat(float flat, float min, float max) {
            this.currentBuffed = Mathf.Clamp(this.currentBuffed + flat, min, max);
        }
    }
}

