using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class CoinCounter : MonoBehaviour {
        public TMPro.TMP_Text text;

        void Update() {
            text.text = Player.Instance.money.ToString();
        }
    }
}