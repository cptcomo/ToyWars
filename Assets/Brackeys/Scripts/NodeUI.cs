using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brackeys {
    public class NodeUI : MonoBehaviour {
        private Node target;
        public GameObject ui;
        
        public void setTarget(Node target) {
            this.target = target;
            transform.position = target.getBuildPosition();
            ui.SetActive(true);
        }

        public void hide() {
            ui.SetActive(false);
        }
    }
}

