using System;
using UI;
using UI.Settings;
using UnityEngine;
using YueUltimateDronePhysics;
using Vector3 = UnityEngine.Vector3;

namespace Drone2
{
    public class DroneBridge : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody droneRigidbody;

        [SerializeField]
        private Transform droneTransform;

        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private float speedCoefficient = 3.6f;

        [Header("Camera")]
        [SerializeField]
        private Camera fpvCamera;

        [SerializeField]
        private float cameraMoveScaler;

        private float Speed => droneRigidbody.velocity.magnitude;

        private float _heightCorrection;
        private Transform _targetQuad;
        private YueDronePhysics _physics;

        private void Start()
        {
            _heightCorrection = droneRigidbody.position.y - 1f;
            fpvCamera.farClipPlane = GraphicsSettings.CameraDistance;
            _physics = droneTransform.GetComponent<YueDronePhysics>();
            if (_physics != null) _targetQuad = _physics.TargetQuad;
        }

        private void Update()
        {
            Shader.SetGlobalVector("_Position", transform.position);
            ArrowsCameraMove();
            CheckButton();
            CheckLanding();
        }

        public string GetSpeed()
        {
            // 3.6 is m/s to km/h
            var speed = Mathf.RoundToInt(droneRigidbody.velocity.magnitude * speedCoefficient);
            return $"{speed} km/h";
        }

        public string GetHeight()
        {
            var height = Mathf.RoundToInt(GetHeightValue());
            return $"{height} m";
        }

        public float GetSpeedValue() =>
            Mathf.RoundToInt(droneRigidbody.velocity.magnitude * speedCoefficient);

        public float GetHeightValue() =>
            droneRigidbody == null
                ? 0f
                : droneRigidbody.position.y - _heightCorrection;

        public float GetDroneZRotation() =>
            droneTransform == null
                ? 0f
                : droneTransform.eulerAngles.z;

        public string GetFlyMode()
        {
            if (dronePhysics == null) return "ACRO";
            return dronePhysics.flightConfig switch
            {
                YueDronePhysicsFlightConfiguration.AcroMode => "ACRO",
                YueDronePhysicsFlightConfiguration.SelfLeveling => "SELF",
                _ => "ACRO"
            };
        }

        private void OnRestart()
        {
            var gameMenu = FindObjectOfType<GameMenu>(true);
            gameMenu.Open();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains(
                    "gate", StringComparison.OrdinalIgnoreCase)) return;

            switch (other.gameObject.name)
            {
                case "finish":
                    //MissionManager.Success();
                    return;
                case "Cargo":
                    return;
                // case "buildings":
                //     MissionManager.Fail();
                //     return;
                default:
                    //    if (Speed > 5f)
                    //        MissionManager.Fail();
                    if (droneTransform.localEulerAngles.x is > 175 and < 185 ||
                        droneTransform.localEulerAngles.z is > 175 and < 185)
                        dronePhysics.armed = false;
                    return;
            }
        }

        private void ArrowsCameraMove()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveCamera(-1f);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveCamera(1f);
            }
        }

        private void MoveCamera(float delta)
        {
            //if (InputSystem.devices.FirstOrDefault(d => d is Joystick or Gamepad) != null)
            {
                var angle = fpvCamera.transform.eulerAngles;
                var x = angle.x;
                var nx = x + (Time.deltaTime * cameraMoveScaler * delta);
                if (nx is > 65 and < 280)
                {
                    nx = x;
                }


                // if (nx < 280 && delta > 0)
                // {
                //     nx = 280;
                // }
                fpvCamera.transform.eulerAngles = new Vector3(nx, angle.y, angle.z);
            }
        }

        private void CheckButton()
        {
            if (Input.GetKey(KeyCode.R) && _targetQuad != null)
            {
                Landing();
            }
        }

        private void CheckLanding()
        {
            var magnitude = _physics.Rigidbody.velocity.magnitude;
            if (!(magnitude < 0.01f)) return;
            var euler = droneTransform.localEulerAngles;
            var eulerQuad = _targetQuad.localEulerAngles;
            if ((euler.x <= 10f &&
                euler.z <= 10f) &&
                (Mathf.Abs(euler.x - eulerQuad.x) > 1f ||
                 Mathf.Abs(euler.y - eulerQuad.y) > 1f ||
                 Mathf.Abs(euler.z - eulerQuad.z) > 1f))
            {
                Landing();
            }
        }

        private void Landing()
        {
            var rotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0f, rotation.y, 0f);
            var rotation2 = _targetQuad.eulerAngles;
            _targetQuad.eulerAngles = new Vector3(0f, rotation2.y, 0f);
        }
    }
}