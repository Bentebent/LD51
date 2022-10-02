using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Path : MonoBehaviour {
        public class Unit {
            public PathingUnit pathUnit;
            public Vector3 currentPosition = Vector3.zero;
            public Vector3 offset = Vector3.zero; // Visual offset on the path
            public float positionOnPath = 0f;
        }

        public delegate void UnitEscapedDelegate(Unit unit);

        public event UnitEscapedDelegate UnitEscaped;


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
                Color oldColor = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(start, end);
                Gizmos.color = oldColor;
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

        Vector3 GetPositionAlongPath(float distance, out Segment outputSegment) {

            float totalDistance = 0f;
            foreach (Segment segment in m_segments) {
                float distanceIntoSegment = distance - totalDistance;
                totalDistance += segment.Length;
                if (distanceIntoSegment <= segment.Length) {
                    outputSegment = segment;
                    return segment.GetPoint(distanceIntoSegment / segment.Length);
                }
            }

            outputSegment = m_segments[m_segments.Count - 1];
            return m_segments[m_segments.Count - 1].end;
        }

        public Unit AddUnit(PathingUnit pathUnit) {
            Vector2 randomOffset = Random.insideUnitCircle;

            Unit unit = new Unit {
                pathUnit = pathUnit,
                currentPosition = m_segments[0].start,
                offset = new Vector3(randomOffset.x, 0f, randomOffset.y),
            };

            pathUnit.gameObject.transform.position = unit.currentPosition + unit.offset;
            m_units.Add(unit);
            return unit;
        }

        public void RemoveUnit(Unit unit) {
            m_units.Remove(unit);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, 0f);
                if (groundPlane.Raycast(mouseRay, out float distance)) {
                    Vector3 clickPos = mouseRay.origin + mouseRay.direction * distance;
                    GameObject unitInstance = Instantiate(unitPrefab, clickPos, Quaternion.identity);
                    AddUnit(unitInstance.GetComponent<PathingUnit>());
                }
            }

            List<int> removeList = new List<int>();
            for (int i = m_units.Count - 1; i >= 0; i--) {
                Unit unit = m_units[i];

                if (unit.pathUnit == null) {
                    removeList.Add(i);
                    continue;
                }


                unit.positionOnPath += unit.pathUnit.speed * unit.pathUnit.speedMultiplier * Time.deltaTime;
                Vector3 currentPos = GetPositionAlongPath(unit.positionOnPath, out Segment outputSegment);

                unit.currentPosition = Vector3.Lerp(unit.currentPosition, currentPos, unit.pathUnit.speed * 0.01f);
                unit.pathUnit.transform.position = unit.currentPosition + unit.offset;
                unit.pathUnit.transform.LookAt(outputSegment.end);

              

                // We can compare here bacuse position is set to end when path is complete
                if (currentPos == m_segments[m_segments.Count - 1].end) {
                    m_units.RemoveAt(i);
                    UnitEscaped?.Invoke(unit);
                }
            }

            removeList.ForEach(x => m_units.RemoveAt(x));
        }

        private void OnDrawGizmos() {
            foreach (var segment in m_segments) {
                segment.DrawGizmos();
            }
        }
    }
}
