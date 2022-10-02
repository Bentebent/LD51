using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {

    public enum PlayerState {
        Moving,
        Dancing
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
        private Vector3 _beatContainerOffset = Vector3.zero;

        public PlayerState state = PlayerState.Moving;

        private Vector3 _inputVector = Vector3.zero;
        private Vector3 _lastInputVector = Vector3.zero;
        private Vector3 _movementVector = Vector3.zero;
        private Vector3 _targetRotationVector = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;

        public GameObject selectedTower = null;
        private BaseTower towerInProgress = null;

        public float currentMultiplier = 1;
        public float score = 0;
        public int hitsInARow = 0;

        public int money = 100;

        public List<TowerContainer> towerContainers = new List<TowerContainer>();

        private void Awake() {
            _characterController = GetComponent<CharacterController>();

            _beatContainer.SetActive(false);
        }

        // Start is called before the first frame update
        private void Start() {

        }

        // Update is called once per frame
        private void Update() {

           

            if (state == PlayerState.Moving) {

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
                //_beatContainer.transform.position = transform.position + _beatContainerOffset;
                //_beatContainer.transform.LookAt(Camera.main.transform);

                if (Input.GetKeyDown(KeyCode.Space)) {
                    ToggleState();
                }
            }
        }

        private void LateUpdate() {
        }

        public void ToggleState() {
            

            if (state == PlayerState.Moving) {
                if (PlaceBuilding(selectedTower)) {
                    state = PlayerState.Dancing;
                    SongConductor.Instance.GetBeatBoxes();
                    //_beatContainer.transform.LookAt(Camera.main.transform);
                }
                else {
                    return;
                }
            } else {
                state = PlayerState.Moving;

                if (towerInProgress != null && towerInProgress.state == TowerState.Building) {
                    Destroy(towerInProgress.gameObject);
                }

                towerInProgress = null;
            }

            _beatContainer.SetActive(!_beatContainer.activeInHierarchy);
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
        }

        private void UpdateRotation() {
            Debug.DrawLine(transform.position, transform.position + _targetRotationVector);
            if (_targetRotationVector != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(_targetRotationVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Clamp(Mathf.Abs(Quaternion.Angle(targetRotation, transform.rotation) * 0.1f), 2f, 45f));
            }
        }

        public void AddHit(float ínverseDistance) {
            float tempScore = ínverseDistance * 100 * currentMultiplier;
            score += tempScore;
            hitsInARow++;

            if(towerInProgress.AddBuildValue(tempScore)) {
                ToggleState();
            }
        }

        public void AddMiss() {
            hitsInARow = 0;
            currentMultiplier = 1;
        }
        private bool PlaceBuilding(GameObject prefab) {
            //Check if we are overlapping with any existing building
            //Else place down selected building and start dancing

            if (RemoveMoney(prefab.GetComponent<BaseTower>().cost)) {
                towerInProgress = GameObject.Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<BaseTower>();
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
    }
}
