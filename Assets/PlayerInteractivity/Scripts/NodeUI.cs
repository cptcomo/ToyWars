using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteractivity {
    public class NodeUI : MonoBehaviour {
        public GameObject nodeUI;
        private GameManager gm;

        private void Start() {
            gm = GameManager.getInstance();
            gm.SelectNodeEvent += show;
            gm.DeselectNodeEvent += hide;
        }

        private void OnDisable() {
            gm.SelectNodeEvent -= show;
            gm.DeselectNodeEvent -= hide;
        }

        void show(Node node) {
            nodeUI.SetActive(true);
        }

        void hide() {
            nodeUI.SetActive(false);
        }
    }
}
