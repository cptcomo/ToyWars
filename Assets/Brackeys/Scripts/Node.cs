using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class Node : MonoBehaviour {
        public Color hoverColor;
        public Vector3 posOffset;

        private GameObject turret;

        private Renderer rend;
        private Color startColor;

        private void Start() {
            rend = GetComponent<Renderer>();
            startColor = rend.material.color;
        }

        private void OnMouseEnter() {
            rend.material.color = hoverColor;
        }

        private void OnMouseExit() {
            rend.material.color = startColor;
        }

        private void OnMouseDown() {
            if(turret != null) {
                Debug.Log("Can't build there! - TODO: Display on screen");
                return;
            }

            GameObject turretToBuild = BuildManager.instance.getTurretToBuild();
            turret = (GameObject)Instantiate(turretToBuild, transform.position + posOffset, transform.rotation);
        }
    }
}
