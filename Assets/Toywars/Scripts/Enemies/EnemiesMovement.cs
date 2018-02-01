using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class EnemiesMovement : MinionMovement {
        protected override void Start() {
            base.Start();
        }

        protected override void Update() {
            base.Update();
        }

        protected override void endPath() {
            base.endPath();
            pm.baseHealth--;
            Destroy(gameObject);
        }
    }
}
