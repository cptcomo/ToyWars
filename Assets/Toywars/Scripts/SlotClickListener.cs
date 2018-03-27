using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Toywars {
    public class SlotClickListener : MonoBehaviour, IPointerClickHandler {
        GameManager gm;

        void Start() {
            gm = GameManager.getInstance();
        }

        public void OnPointerClick(PointerEventData eventData) {
            gm.callEventSlotAdd(this.gameObject);
        }
    }
}
