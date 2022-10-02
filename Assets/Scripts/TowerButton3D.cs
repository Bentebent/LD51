using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD51 {
    public class TowerButton3D : MonoBehaviour {
        public GameObject icon;
        public GameObject iconDisabled;

        public TowerType type;

        public UnityEvent<TowerType> ClickEvent;

        private void Update() {
            if (Player.Instance.CanAfford(type)) {
                icon.SetActive(true);
                iconDisabled.SetActive(false);
            } else {
                icon.SetActive(false);
                iconDisabled.SetActive(true);
            }
        }

        private void OnMouseOver() {
            icon.transform.localScale = Vector3.one * 1.25f;
        }

        private void OnMouseExit() {
            icon.transform.localScale = Vector3.one;
        }

        private void OnMouseDown() {
            ClickEvent?.Invoke(type);
        }
    }
}