using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD51 {
    public class TowerButton3D : MonoBehaviour {
        public GameObject icon;
        public GameObject iconDisabled;
        public GameObject tooltip;

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

        private void OnDisable() {
            if (tooltip != null) {
                tooltip.SetActive(false);
            }
        }

        private void OnMouseOver() {
            icon.transform.localScale = Vector3.one * 1.25f;
            if (tooltip != null) {
                tooltip.SetActive(true);
            }
        }

        private void OnMouseExit() {
            if (tooltip != null) {
                tooltip.SetActive(false);
            }
            icon.transform.localScale = Vector3.one;
        }

        private void OnMouseDown() {
            if (tooltip != null) {
                tooltip.SetActive(false);
            }
            ClickEvent?.Invoke(type);
        }
    }
}