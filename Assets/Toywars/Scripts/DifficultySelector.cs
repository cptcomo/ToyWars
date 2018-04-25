using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toywars {
    public class DifficultySelector : MonoBehaviour {
        public GameObject selectionContainer;
        DifficultySelection selection;

        public Image normalPanel, hardPanel;

        Color startColor, selectColor;

        private void Start() {
            startColor = normalPanel.color;
            selectColor = new Color(0, 200, 0, 155);

            selection = selectionContainer.GetComponent<DifficultySelection>();

            normalPanel.color = selectColor;
            selection.difficulty = Difficulty.normal;
        }

        public void selectNormal() {
            normalPanel.color = selectColor;
            hardPanel.color = startColor;
            selection.difficulty = Difficulty.normal;
        }

        public void selectHard() {
            hardPanel.color = selectColor;
            normalPanel.color = startColor;
            selection.difficulty = Difficulty.hard;
        }
    }

    public enum Difficulty {
        normal, hard
    }
}
