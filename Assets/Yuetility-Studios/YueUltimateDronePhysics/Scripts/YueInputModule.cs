// Written by Marcel Remmers for Yuetility-Studios 2022
using Services;
using UnityEngine;

namespace YueUltimateDronePhysics
{
    public class YueInputModule : MonoBehaviour
    {
        #region
        public YueRatesConfiguration ratesConfig;

        [Header("Raw Input")]
        [Tooltip("Raw Inputs should be between -1 to 1")]

        [Range(-1f, 1f)]
        public float rawLeftHorizontal = 0f;

        [Range(-1f, 1f)]
        public float rawLeftVertical = 0f;

        [Range(-1f, 1f)]
        public float rawRightHorizontal = 0f;

        [Range(-1f, 1f)]
        public float rawRightVertical = 0f;

        [HideInInspector]
        public float thrust = 0f;
        [HideInInspector]
        public float yaw = 0f;
        [HideInInspector]
        public float pitch = 0f;
        [HideInInspector]
        public float roll = 0f;

        [HideInInspector]
        public float rawThrust = 0f;
        [HideInInspector]
        public float rawYaw = 0f;
        [HideInInspector]
        public float rawPitch = 0f;
        [HideInInspector]
        public float rawRoll = 0f;

        #endregion
        BetaflightCalculator calculator = new BetaflightCalculator();

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
            InputService inputService = FindObjectOfType<InputService>();

            RCRate_ROLL = inputService.RCRate_ROLL;
            expo_ROLL = inputService.expo_ROLL;
            superRate_ROLL = inputService.superRate_ROLL;

            RCRate_PITCH = inputService.RCRate_PITCH;
            expo_PITCH = inputService.expo_ROLL;
            superRate_PITCH = inputService.superRate_PITCH;

            RCRate_YAW = inputService.RCRate_YAW;
            expo_YAW = inputService.expo_YAW;
            superRate_YAW = inputService.superRate_YAW;
        }
        public void Update()
        {
            CalculateInputWithRatesConfig();
            ratesConfig.exponentialGain = calculator.BfCalc(1, RCRate_YAW, expo_YAW, superRate_YAW);
        }

        private void CalculateInputWithRatesConfig()
        {
            // Implement according to FlightMode
            switch (ratesConfig.mode)
            {
                case (YueTransmitterMode.Mode1):
                    rawThrust = rawRightVertical;
                    rawYaw = rawLeftHorizontal;
                    rawPitch = rawLeftVertical;
                    rawRoll = rawRightHorizontal;
                    break;

                case (YueTransmitterMode.Mode2):
                    rawThrust = rawLeftVertical;
                    rawYaw = rawLeftHorizontal;
                    rawPitch = rawRightVertical;
                    rawRoll = rawRightHorizontal;
                    break;

                case (YueTransmitterMode.Mode3):
                    rawThrust = rawRightVertical;
                    rawYaw = rawRightHorizontal;
                    rawPitch = rawLeftVertical;
                    rawRoll = rawLeftHorizontal;
                    break;

                case (YueTransmitterMode.Mode4):
                    rawThrust = rawLeftVertical;
                    rawYaw = rawRightHorizontal;
                    rawPitch = rawRightVertical;
                    rawRoll = rawLeftHorizontal;
                    break;
            }
            
            // Scale with Rates
            var thrustRate = (rawThrust + 1) * 0.5f;
            var thrustExp = TransformInput(thrustRate, ratesConfig.proportionalGain, ratesConfig.exponentialGain);
            thrust = thrustExp / ratesConfig.exponentialGain;
            yaw = calculator.BfCalc(rawYaw, RCRate_YAW, expo_YAW, superRate_YAW);
            pitch = calculator.BfCalc(rawPitch, RCRate_PITCH, expo_PITCH, superRate_PITCH);
            roll = calculator.BfCalc(rawRoll, RCRate_ROLL, expo_ROLL, superRate_ROLL);
            //Debug.Log(yaw.ToString() + "__" + transform.localRotation.y.ToString());
        }

        public float TransformInput(float input, float p, float e)
        {
            return (input * p) + (Mathf.Pow(input, 2) * Mathf.Sign(input) * e);
        }

        public void ConfigChange(YueRatesConfiguration config)
        {
            ratesConfig = config;
        }
    }
}
