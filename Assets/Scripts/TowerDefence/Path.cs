using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Path : MonoBehaviour {

        class Unit {
            public PathUnit pathUnit;
            public Vector3 currentPosition = Vector3.zero;
            public Vector3 offset = Vector3.zero; // Visual offset on the path
            public float positionOnPath = 0f;
        }

        class Segment {
            public Vector3 start, end;

            public Vector3 StartToEnd => end - start;
            public float Length => Vector3.Distance(start, end);

            public float GetClosestT(Vector3 point) {
                float length = StartToEnd.magnitude;
                Vector3 startToPoint = point - start;
                float t = Vector3.Dot(StartToEnd / length, startToPoint);
                return t / length;
            }

            public Vector3 GetPoint(float t) {
                return Vector3.Lerp(start, end, t);
            }

            public Vector3 GetClosestPoint(Vector3 point) {
                return GetPoint(GetClosestT(point));
            }

            public void DrawGizmos() {
                Gizmos.DrawLine(start, end);
            }
        }

        [SerializeField]
        private GameObject unitPrefab;

        [SerializeField]
        private List<GameObject> m_checkpoints = new List<GameObject>();

        private List<Segment> m_segments = new List<Segment>();
        private List<Unit> m_units = new List<Unit>();


        private void Awake() {
            for (int i = 1; i < m_checkpoints.Count; i++) {
                m_segments.Add(new Segment {
                    start = m_checkpoints[i - 1].transform.position,
                    end = m_checkpoints[i].transform.position,
                });
            }
        }

        private void Start() {

        }

        Vector3 GetPositionAlongPath(float distance) {
            float totalDistance = 0f;
            foreach (Segment segment in m_segments) {
                float distanceIntoSegment = distance - totalDistance;
                totalDistance += segment.Length;
                if (distanceIntoSegment <= segment.Length) {
                    return segment.GetPoint(distanceIntoSegment / segment.Length);
                }
            }

            return m_segments[m_segments.Count - 1].end;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, 0f);
                if (groundPlane.Raycast(mouseRay, out float distance)) {
                    Vector3 clickPos = mouseRay.origin + mouseRay.direction * distance;
                    GameObject unitInstance = Instantiate(unitPrefab, clickPos, Quaternion.identity);
                    PathUnit pathUnit = unitInstance.AddComponent<PathUnit>();

                    Vector2 randomOffset = Random.insideUnitCircle;

                    Unit unit = new Unit {
                        pathUnit = pathUnit,
                        currentPosition = clickPos,
                        offset = new Vector3(randomOffset.x, 0f, randomOffset.y),
                    };
                    m_units.Add(unit);
                }
            }


            foreach (var unit in m_units) {
                unit.positionOnPath += unit.pathUnit.speed * Time.deltaTime;
                Vector3 currentPos = GetPositionAlongPath(unit.positionOnPath);

                unit.currentPosition = Vector3.Lerp(unit.currentPosition, currentPos, 0.01f);
                unit.pathUnit.transform.position = unit.currentPosition + unit.offset;
            }
        }

        private void OnDrawGizmos() {
            foreach (var segment in m_segments) {
                segment.DrawGizmos();
            }
        }
    }
}
