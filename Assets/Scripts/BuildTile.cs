using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BuildTile : MonoBehaviour {
        public bool Occupied { get; set; } = false;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.TryGetComponent(out Player player)) {
                player.currentBuildTiles.Add(this);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent(out Player player)) {
                player.currentBuildTiles.Remove(this);
            }
        }
    }
}

