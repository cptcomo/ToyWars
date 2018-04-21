using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class CameraManager : MonoBehaviour {
        private GameManager gm;

        public Transform player;
        private Player playerScript;
        public Vector3 playerHoverOffset;

        public float panSpeed = 40f;
        public float panBorderThickness = 5f;
        public float scrollSpeed = 8f;
        public float minY = 10f;
        public float maxY = 200f;

        bool playerIsDead;

        private void Start() {
            gm = GameManager.getInstance();
            gm.PlayerDeathEvent += onDeath;
            playerScript = player.gameObject.GetComponent<Player>();
            playerScript.setCameraHeightOffset(playerHoverOffset.y);
        }

        private void OnDisable() {
            gm.PlayerDeathEvent -= onDeath;
        }

        private void Update() {
            if(gm.isBuilding() || gm.isAITurn() || (gm.isPlaying() && playerIsDead)) {
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
            } else if(gm.isPlaying()) {
                focusOnPlayer();
            }
        }

        void onDeath(float timer) {
            playerIsDead = true;
            Invoke("onRevival", timer);
        }

        void onRevival() {
            playerIsDead = false;
        }

        void focusOnPlayer() {
            this.transform.position = player.position + playerHoverOffset;
        }
    }
}

