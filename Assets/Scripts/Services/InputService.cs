using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Services
{
    public class InputService : MonoBehaviour
    {
        
        private static InputService _instance;

        public static InputService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<InputService>();
                return _instance;
            }
        }


        public float RCRate_ROLL;
        public float expo_ROLL;
        public float superRate_ROLL;

        public float RCRate_PITCH;
        public float expo_PITCH;
        public float superRate_PITCH;

        public float RCRate_YAW;
        public float expo_YAW;
        public float superRate_YAW;


        private void Awake()
        {
            InputSystem.onDeviceChange += OnDeviceChange;


            

        }
        private void Start()
        {
            RCRate_ROLL = PlayerPrefs.GetFloat($"RCRate_ROLL");
            expo_ROLL = PlayerPrefs.GetFloat($"expo_ROLL");
            superRate_ROLL = PlayerPrefs.GetFloat($"superRate_ROLL");

            RCRate_PITCH = PlayerPrefs.GetFloat($"RCRate_PITCH");
            expo_PITCH = PlayerPrefs.GetFloat($"expo_PITCH");
            superRate_PITCH = PlayerPrefs.GetFloat($"superRate_PITCH");

            RCRate_YAW = PlayerPrefs.GetFloat($"RCRate_YAW");
            expo_YAW = PlayerPrefs.GetFloat($"superRate_YAW");
            superRate_YAW = PlayerPrefs.GetFloat($"superRate_YAW");
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            //if (device is (Gamepad or Joystick)) Debug.Log($"ch: {device.deviceId} {device.name} {change}");
            if (change == InputDeviceChange.Added)
            {
                var rem = false;
                foreach (var allDevice in InputSystem.devices)
                {
                    if (allDevice == null) continue;
                    if (allDevice is not (Gamepad or Joystick)) continue;
                    if (allDevice == device) continue;
                    rem = true;
                    InputSystem.RemoveDevice(allDevice);
                }

                if (rem) InputSystem.RemoveDevice(device);
            }
        }
    }
}

