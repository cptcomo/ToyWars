using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public class SideUIController : MonoBehaviour {
        public GameObject playerUI;
        public GameObject tileUI;

        GameManager gm;

        enum State {
            HIDDEN, PLAYER, TILE
        }

        State state;
        State targetState;

        private void Start() {
            gm = GameManager.getInstance();
            setTargetStatePlayer();
            gm.StartNextWaveEvent += setTargetStateHidden;
            gm.EndWaveEvent += setTargetStatePlayer;
            gm.SelectTileEvent += setTargetStateTile;
            gm.DeselectTileEvent += setTargetStatePlayer;
        }

        private void Update() {
            if(state != targetState) {
                state = targetState;
                if(state == State.HIDDEN) {

                }
                else if(state == State.PLAYER) {

                }
                else if(state == State.TILE) {

                }
            }
        }

        void onHiddenState() {

        }

        void onPlayerState() {

        }

        void onTileState() {

        }

        void setTargetStateHidden() {
            targetState = State.HIDDEN;
        }

        void setTargetStatePlayer() {
            targetState = State.PLAYER;
        }

        void setTargetStateTile(Tile tile) {
            targetState = State.TILE;
        }
    }
}
