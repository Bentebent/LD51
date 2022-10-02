using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD51 {
    public class TowerButton3D : MonoBehaviour {
        public GameObject icon;

        public TowerType type;

        public UnityEvent<TowerType> ClickEvent;

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