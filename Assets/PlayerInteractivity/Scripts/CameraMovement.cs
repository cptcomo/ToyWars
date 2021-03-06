﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class CameraMovement : MonoBehaviour {
        private GameManager gm;

        public Transform player;
        private Player playerScript;
        public Vector3 playerHoverOffset;

        public float panSpeed = 30f;
        public float panBorderThickness = 10f;
        public float scrollSpeed = 5f;
        public float minY = 10f;
        public float maxY = 80f;

        private void Start() {
            gm = GameManager.getInstance();
            playerScript = player.gameObject.GetComponent<Player>();
            playerScript.setCameraHeightOffset(playerHoverOffset.y);
        }

        private void Update() {
            if(gm.gameState == GameManager.GameState.Build) {
                if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) {
                    transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
                }
                if(Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) {
                    transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
                }
                if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) {
                    transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
                }
                if(Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) {
                    transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                Vector3 pos = transform.position;
                pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                transform.position = pos;
            }
            else if(gm.gameState == GameManager.GameState.Play){
                focusOnPlayer();
            }
        }

        public void qAbility() {

        }

        public void wAbility() {

        }

        public void eAbility() {

        }

        public void rAbility() {

        }

        void focusOnPlayer() {
            this.transform.position = player.position + playerHoverOffset;  
        }
    }
}
