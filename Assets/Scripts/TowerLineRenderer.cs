using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class TowerLineRenderer : MonoBehaviour {
        public LineRenderer lineRenderer;

        private PathingUnit unit = null;

        public void SetUnitTarget(PathingUnit unit) {
            this.unit = unit;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, unit.transform.position + Vector3.up * 0.5f - transform.position);
        }

        private void Update() {
            if (unit != null) {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, unit.transform.position + Vector3.up * 0.5f - transform.position);
            }
        }
    }
}