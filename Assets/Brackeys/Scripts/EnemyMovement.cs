using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour {
        private Transform target;
        private int wpIndex = 0;
        private const double wpTolerance = 0.4f; //How close to waypoint is close enough
        private Enemy enemy;

        private void Start() {
            target = Waypoints.wps[0];
            enemy = GetComponent<Enemy>();
        }

        private void Update() {
            Vector3 dir = (target.position - this.transform.position).normalized;
            transform.Translate(dir * enemy.speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(transform.position, target.position) <= wpTolerance) {
                nextWaypoint();
            }

            enemy.speed = enemy.startSpeed;
        }

        void nextWaypoint() {
            if(wpIndex >= Waypoints.wps.Length - 1) {
                endPath();
                return;
            }
            wpIndex++;
            target = Waypoints.wps[wpIndex];
        }

        void endPath() {
            PlayerStats.lives--;
            Destroy(gameObject);
        }
    }
}
