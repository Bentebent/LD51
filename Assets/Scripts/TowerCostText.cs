using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class TowerCostText : MonoBehaviour {
        public TowerType towerType;
        public TMPro.TMP_Text text;

        private void Start() {
            text.text = Player.Instance.GetCost(towerType).ToString();
        }
    }
}