using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Brackeys {
    public class Node : MonoBehaviour {
        public Color hoverColor;
        public Vector3 posOffset;

        private GameObject turret;

        private Renderer rend;
        private Color startColor;

        private BuildManager buildManager;

        private void Start() {
            rend = GetComponent<Renderer>();
            startColor = rend.material.color;
            buildManager = BuildManager.instance;
        }

        private void OnMouseEnter() {
            if(EventSystem.current.IsPointerOverGameObject())
                return;

            if(buildManager.getTurretToBuild() == null)
                return;

            rend.material.color = hoverColor;
        }

        private void OnMouseExit() {
            rend.material.color = startColor;
        }

        private void OnMouseDown() {
            if(buildManager.getTurretToBuild() == null)
                return;

            if(turret != null) {
                Debug.Log("Can't build there! - TODO: Display on screen");
                return;
            }

            GameObject turretToBuild = buildManager.getTurretToBuild();
            turret = (GameObject)Instantiate(turretToBuild, transform.position + posOffset, transform.rotation);
        }
    }
}
