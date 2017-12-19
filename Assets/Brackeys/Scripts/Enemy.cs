using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Enemy : MonoBehaviour {
        public float speed = 10f;
        private Transform target;
        private int wpIndex = 0;
        private const double wpTolerance = 0.4f; //How close to waypoint is close enough

        private void Start() {
            target = Waypoints.wps[0];
        }

        private void Update() {
            Vector3 dir = (target.position - this.transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(transform.position, target.position) <= wpTolerance) {
                nextWaypoint();
            }
        }

        void nextWaypoint() {
            if(wpIndex >= Waypoints.wps.Length - 1) {
                Destroy(gameObject);
                return;
            }
            wpIndex++;
            target = Waypoints.wps[wpIndex];
        }
    }
}
