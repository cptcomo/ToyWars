using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Brackeys {
    public class Node : MonoBehaviour {
        public Color hoverColor;
        public Color notEnoughMoneyColor;
        public Vector3 posOffset;

        [Header("Optional")]
        public GameObject turret;

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

            if(!buildManager.canBuild)
                return;

            if(buildManager.hasMoney)
                rend.material.color = hoverColor;
            else
                rend.material.color = notEnoughMoneyColor;
        }

        private void OnMouseExit() {
            rend.material.color = startColor;
        }

        private void OnMouseDown() {
            if(!buildManager.canBuild || EventSystem.current.IsPointerOverGameObject())
                return;

            if(turret != null) {
                Debug.Log("Can't build there! - TODO: Display on screen");
                return;
            }

            buildManager.buildTurretOn(this);
        }

        public Vector3 getBuildPosition() {
            return transform.position + posOffset;
        }
    }
}
