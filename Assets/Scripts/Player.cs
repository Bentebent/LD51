using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {

    public enum PlayerState {
        Moving,
        Dancing,
        Dead
    }

    [Serializable]
    public class TowerContainer {
        public KeyCode keyCode;
        public GameObject prefab;
    }

    public class Player : MonoBehaviour {

        private CharacterController _characterController = null;

        [SerializeField]
        private float _maxVelocity = 0.0f;

        [SerializeField]
        private float _acceleration = 0.0f;

        [SerializeField]
        private float _deceleration = 0.0f;

        [SerializeField]
        private GameObject _beatContainer = null;

        [SerializeField]
        private GameObject _towerButtonsContainer = null;

        [SerializeField]
        private Vector3 _beatContainerOffset = Vector3.zero;

        public PlayerState state = PlayerState.Moving;

        public Transform visual;

        private Vector3 _inputVector = Vector3.zero;
        private Vector3 _lastInputVector = Vector3.zero;
        private Vector3 _movementVector = Vector3.zero;
        private Vector3 _targetRotationVector = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;

        public HashSet<BuildTile> currentBuildTiles = new HashSet<BuildTile>();

        public GameObject selectedTower = null;
        private BaseTower towerInProgress = null;

        public float currentMultiplier = 1;
        public float score = 0;
        public int hitsInARow = 0;

        public int money = 100;
        public int lives = 100;

        public List<TowerContainer> towerContainers = new List<TowerContainer>();

        public GameObject placementIndicator;

        public int NotesRequiredToBuildCurrentTower { get; set; } = 0;

        private static Player _instance = null;
        public static Player Instance => _instance;

        BuildTile GetClosestBuildTile() {
            float closestDist = float.MaxValue;
            BuildTile closest = null;
            foreach(var tile in currentBuildTiles) {
                if (tile.Occupied) {
                    continue;
                }
                float dist = Vector3.Distance(transform.position, tile.transform.position + Vector3.up * 0.6f);
                if (dist < closestDist) {
                    closest = tile;
                    closestDist = dist;
                }
            }
            return closest;
        }

        BuildTile closestBuildTile = null;

        private void Awake() {
            _instance = this;

            _characterController = GetComponent<CharacterController>();

            _beatContainer.SetActive(false);
            _towerButtonsContainer.SetActive(true);
            placementIndicator.transform.SetParent(null);
        }

        // Start is called before the first frame update
        private void Start() {
            var tds = GameObject.FindGameObjectsWithTag("TD");
            foreach (var td in tds) {
                td.GetComponent<TowerDefence>().RegisterOnUnitEscaped(onEscaped);
            }
        }

        private void onEscaped(Path.Unit unit) {
            lives -= 1;

            if (lives <= 0) {
                state = PlayerState.Dead;
            }
        }

        public void StartBuildTower(TowerType type) {
            if (closestBuildTile == null) {
                Debug.LogWarning("Cannot build here!");
                return;
            }
            foreach (TowerContainer tc in towerContainers) {
                if (tc.prefab.GetComponent<BaseTower>().type == type) {
                    selectedTower = tc.prefab;
                    ToggleState();
                }
            }
        }

        // Update is called once per frame
        private void Update() {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.M)) {
                money += 10000;
                lives = 100000;
            }
#endif

            if (state == PlayerState.Dead) {
                _beatContainer.SetActive(false);
                return;
            }

            if (state == PlayerState.Moving) {
                closestBuildTile = GetClosestBuildTile();

                _towerButtonsContainer.SetActive(closestBuildTile != null);
                if (closestBuildTile != null) {
                    placementIndicator.SetActive(true);
                    placementIndicator.transform.position = closestBuildTile.transform.position + Vector3.up * 0.6f;
                } else {
                    placementIndicator.SetActive(false);
                }

                foreach (TowerContainer tc in towerContainers) {
                    if (Input.GetKeyDown(tc.keyCode)) {
                        selectedTower = tc.prefab;
                        ToggleState();
                    }
                }

                GatherInput();
                _movementVector = Vector3.zero;
                _targetRotationVector = Vector3.zero;

                Vector3 cameraInputVector = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f) * _inputVector.normalized;
                _movementVector = cameraInputVector * _maxVelocity;
                _targetRotationVector = cameraInputVector;

                UpdateMovement();
                UpdateRotation();

            } else if (state == PlayerState.Dancing) {

                _towerButtonsContainer.SetActive(false);
                //_beatContainer.transform.position = transform.position + _beatContainerOffset;
                //_beatContainer.transform.LookAt(Camera.main.transform);

                if (Input.GetKey(KeyCode.Escape)) {
                    ToggleState();
                }
            }
        }

        private void LateUpdate() {
        }

        public void ToggleState() {
            

            if (state == PlayerState.Moving) {
                if (PlaceBuilding(selectedTower)) {
                    //_towerButtonsContainer.SetActive(false);
                    state = PlayerState.Dancing;
                    _beatContainer.SetActive(!_beatContainer.activeInHierarchy);
                    SongConductor.Instance.GetBeatBoxes();
                    //_beatContainer.transform.LookAt(Camera.main.transform);
                }
                else {
                    return;
                }
            } else {
                _beatContainer.SetActive(!_beatContainer.activeInHierarchy);
                state = PlayerState.Moving;

                if (towerInProgress != null && towerInProgress.state == TowerState.Building) {
                    money += towerInProgress.cost;
                    closestBuildTile.Occupied = false;
                    Destroy(towerInProgress.gameObject);
                } else {
                    closestBuildTile = null;
                }

                towerInProgress = null;
            }

        }

        private void GatherInput() {
            _lastInputVector = _inputVector;

            _inputVector.x = Input.GetAxisRaw("Horizontal");
            _inputVector.y = 0f;
            _inputVector.z = Input.GetAxisRaw("Vertical");

            float deadzone = 0.1f;
            if (Mathf.Abs(_inputVector.x) < deadzone) {
                _inputVector.x = 0f;
            }
            if (Mathf.Abs(_inputVector.y) < deadzone) {
                _inputVector.y = 0f;
            }
        }

        private float AccelDecel(float currentSpeed, float targetSpeed, float acceleration, float deceleration) {
            float delta = targetSpeed - currentSpeed;

            if (delta > 0f) {
                return Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            } else {
                return Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
            }
        }

        private void UpdateMovement() {

            _currentVelocity.x = AccelDecel(_currentVelocity.x, _movementVector.x, _acceleration, _deceleration);
            _currentVelocity.z = AccelDecel(_currentVelocity.z, _movementVector.z, _acceleration, _deceleration);

            if (_currentVelocity.magnitude > _maxVelocity) {
                _currentVelocity = _currentVelocity.normalized * _maxVelocity;
            }

            //_animator.SetFloat("speed", Mathf.Clamp01(_currentVelocity.magnitude / _maxVelocity));
            _characterController.Move(_currentVelocity * Time.deltaTime + Physics.gravity * Time.deltaTime);

            if (_currentVelocity.magnitude > 0.1f) {
                float sine = Mathf.Sin(Time.time * 20f);
                visual.transform.localPosition = Vector3.up * Mathf.Abs(sine) * 0.2f;
                visual.transform.localEulerAngles = new Vector3(0f, 0f, sine * 15f);
            } else {
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localEulerAngles = Vector3.zero;
            }
        }

        private void UpdateRotation() {
            Debug.DrawLine(transform.position, transform.position + _targetRotationVector);
            if (_targetRotationVector != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(_targetRotationVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Clamp(Mathf.Abs(Quaternion.Angle(targetRotation, transform.rotation) * 0.1f), 2f, 45f));
            }
        }

        public void AddHit(float inverseDistance, BeatBox beatBox) {
            NoteHitStatus.NoteHitType noteHitType;
            if (inverseDistance > 0.9f) {
                noteHitType = NoteHitStatus.NoteHitType.Perfect;
            } else if (inverseDistance > 0.75f) {
                noteHitType = NoteHitStatus.NoteHitType.Good;
            } else if (inverseDistance > 0.5f) {
                noteHitType = NoteHitStatus.NoteHitType.Ok;
            } else {
                noteHitType = NoteHitStatus.NoteHitType.Bad;
            }

            beatBox.Explode(Mathf.Lerp(1.2f, 3f, inverseDistance), Mathf.Lerp(0.1f, 0.25f, inverseDistance));

            NoteHitStatus.Instance.AddNoteHit(noteHitType);

            float tempScore = inverseDistance * currentMultiplier;
            score += tempScore;
            hitsInARow++;

            if(towerInProgress.AddBuildProgress(Mathf.Clamp01(tempScore))) {
                ToggleState();
            }
        }

        public void AddMiss(bool snuckPast) {
            hitsInARow = 0;
            currentMultiplier = 1;

            NoteHitStatus.Instance.AddNoteHit(NoteHitStatus.NoteHitType.Miss);

            if (snuckPast && towerInProgress.AddBuildProgress(0.1f)) {
                ToggleState();
            }
        }

        private bool PlaceBuilding(GameObject prefab) {
            //Check if we are overlapping with any existing building
            //Else place down selected building and start dancing

            if (closestBuildTile == null) {
                return false;
            }

            if (RemoveMoney(prefab.GetComponent<BaseTower>().cost)) {
                towerInProgress = GameObject.Instantiate(prefab, closestBuildTile.transform.position + Vector3.up * 0.6f, Quaternion.identity).GetComponent<BaseTower>();
                closestBuildTile.Occupied = true;
                NotesRequiredToBuildCurrentTower = towerInProgress.NotesRequiredToBuild;
                return true;
            }
            return false;
        }

        public void AddMoney(int money) {
            this.money += money;
        }

        public bool RemoveMoney(int cost) {
            if (money - cost >= 0) {
                money -= cost;
                return true;
            }

            return false;
        }

        public int GetCost(TowerType towerType) {
            foreach (TowerContainer tc in towerContainers) {
                BaseTower baseTower = tc.prefab.GetComponent<BaseTower>();
                if (baseTower.type == towerType) {
                    return baseTower.cost;
                }
            }

            return int.MaxValue;
        }

        public bool CanAfford(TowerType towerType) {
            return GetCost(towerType) <= money;
        }
    }
}
