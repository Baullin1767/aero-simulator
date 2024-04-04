using System;
using Cysharp.Threading.Tasks;
using Drone2;
using Mission.Tutorial;
using TMPro;
using UI.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using YueUltimateDronePhysics;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI speed;

        [SerializeField]
        private TextMeshProUGUI time;

        [SerializeField]
        private TextMeshProUGUI height;

        [SerializeField]
        private Transform skyline;

        [SerializeField]
        private TextMeshProUGUI flyMode;

        [SerializeField]
        private TextMeshProUGUI laps;

        [SerializeField]
        private GameObject lapsObject;

        [SerializeField]
        private GameObject fps;

        [SerializeField]
        private GameObject topElements;

        [SerializeField]
        private GameObject topTextObject;

        [SerializeField]
        private GameObject gasTextObject;

        [SerializeField]
        private GameObject preStartTextObject;

        [SerializeField]
        private GameObject numTextObject;

        [SerializeField]
        private TextMeshProUGUI topText;

        [SerializeField]
        private InputActionReference actionThrottle;

        private DroneBridge _droneBridge;
        private TutorialMission _mission;

        private bool _preStart;
        public bool PreStart => _preStart;

        private float _startTime;
        public float CurTime => Time.timeSinceLevelLoad - _startTime;

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTask Initialize()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();

            _droneBridge = FindObjectOfType<DroneBridge>();
            _mission = FindObjectOfType<TutorialMission>();
            if (laps != null)
            {
                lapsObject.gameObject.SetActive(_mission != null && _mission.Laps > 0);
            }

            _preStart = false;
            fps.SetActive(GraphicsSettings.IsShowFps);
            if (actionThrottle != null)
            {
                WaitForStart().Forget();
            }
            else
            {
                var inputController = FindObjectOfType<XBOXControllerInput>();
                inputController.ExtraLandingMode(false);
            }
        }

        private void LateUpdate()
        {
            if (_droneBridge != null)
            {
                speed.text = _droneBridge.GetSpeedValue().ToString("F0");
                time.text = GetTime();
                height.text = _droneBridge.GetHeightValue().ToString("F0");
                skyline.eulerAngles = new Vector3(0f, 0f, -_droneBridge.GetDroneZRotation());
                flyMode.text = _droneBridge.GetFlyMode();
            }

            if (_mission != null && laps != null)
            {
                laps.text = $"{_mission.CurrentLap}/{_mission.Laps}";
            }
        }

        private string GetTime()
        {
            var timeSinceLevelLoad = CurTime;
            var minutes = Mathf.Floor(timeSinceLevelLoad / 60);
            var seconds = Mathf.Floor(timeSinceLevelLoad % 60);
            var miliSeconds = 100 * (timeSinceLevelLoad - Math.Truncate(timeSinceLevelLoad));
            return $"{minutes:00}:{seconds:00}″{miliSeconds:00}";
        }

        private async UniTask WaitForStart()
        {
            var inputController = FindObjectOfType<XBOXControllerInput>();
            topElements.SetActive(false);
            preStartTextObject.SetActive(false);
            numTextObject.SetActive(false);

            topTextObject.SetActive(true);
            gasTextObject.SetActive(true);
            _startTime = 999999f;

            while (actionThrottle.action.ReadValue<float>() > -1)
            {
                await UniTask.Yield();
            }

            //inputController.ExtraLandingMode(false);

            if (numTextObject != null) numTextObject.SetActive(true);
            gasTextObject.SetActive(false);
            var timeToStart = 3f;
            while (timeToStart > 0f)
            {
                await UniTask.Yield();
                if (actionThrottle.action.ReadValue<float>() > -1)
                {
                    numTextObject.SetActive(false);
                    preStartTextObject.SetActive(true);
                    _preStart = true;
                    return;
                }

                timeToStart -= Time.deltaTime;
                topText.text = Mathf.CeilToInt(timeToStart).ToString();
            }

            topElements.SetActive(true);
            topTextObject.SetActive(false);
            numTextObject.SetActive(false);
            _startTime = Time.timeSinceLevelLoad;
            inputController.ExtraLandingMode(false);
            if (_mission != null) _mission.MissionStart();
        }
    }
}