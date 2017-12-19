using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Waypoints : MonoBehaviour {
        public static Transform[] wps;

        private void Awake() {
            wps = new Transform[transform.childCount];
            for(int i = 0; i < wps.Length; i++) {
                wps[i] = transform.GetChild(i);
            }
        }
    }
}

