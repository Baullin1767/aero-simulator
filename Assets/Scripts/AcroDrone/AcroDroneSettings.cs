using TMPro;
using UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using YueUltimateDronePhysics;

namespace AcroDrone
{
    public class AcroDroneSettings : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button saveButton;
        
        [SerializeField]
        private Button resetButton;
        
        [SerializeField]
        private AcceptMenu acceptMenu;
        
        [Header("Physics")]
        [SerializeField]
        private TMP_InputField massInputField;

        [SerializeField]
        private TMP_InputField dragInputField;

        [SerializeField]
        private TMP_InputField angularDragInputField;

        [Header("PIDs")]
        [SerializeField]
        private TMP_InputField pidRotationPInputField;

        //[SerializeField]
        //private InputField pidRotationIInputField;

        [SerializeField]
        private TMP_InputField pidRotationDInputField;


        #region Roll
        [SerializeField]
        private TMP_InputField pidAltitudePInputFieldR;

        [SerializeField]
        private TMP_InputField pidAltitudeIInputFieldR;

        [SerializeField]
        private TMP_InputField pidAltitudeDInputFieldR;

        #endregion

        #region Pithch
        [SerializeField]
        private TMP_InputField pidAltitudePInputFieldP;

        [SerializeField]
        private TMP_InputField pidAltitudeIInputFieldP;

        [SerializeField]
        private TMP_InputField pidAltitudeDInputFieldP;
        #endregion

        #region Yaw

        [SerializeField]
        private TMP_InputField pidAltitudePInputFieldY;

        [SerializeField]
        private TMP_InputField pidAltitudeIInputFieldY;

        [SerializeField]
        private TMP_InputField pidAltitudeDInputFieldY; 
        #endregion


        [Header("Settings")]
        [SerializeField]
        private TMP_InputField thrustInputField;

        [SerializeField]
        private TMP_InputField cameraRotationXInputField;

        [SerializeField]
        private TMP_InputField cameraRotationYInputField;

        [SerializeField]
        private TMP_InputField cameraRotationZInputField;

        [SerializeField]
        private TMP_InputField sizeScaleInputField;

        [Header("Input Module")]
        [SerializeField]
        private TMP_InputField proportionalGainInputField;

        [SerializeField]
        private TMP_InputField exponentialGainInputField;

        [SerializeField]
        private TMP_InputField slMaxAngleInputField;

        [Header("Drone")]
        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private YueInputModule droneInput;

        [SerializeField]
        private Vector3 defaultCameraRotation = new(-25f, 0f, 0f);

        [SerializeField]
        private Vector3 defaultScale = new(1f, 1f, 1f);

        private const string FirstLoadStr = "first_acro_drone_load";
        public static bool IsHasSettings => PlayerPrefs.HasKey(FirstLoadStr);

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(FirstLoadStr))
            {
                SetDefaultValues();
                PlayerPrefs.SetInt(FirstLoadStr, 1);
                PlayerPrefs.Save();
            }

            LoadValues();
        }

        private void OnEnable()
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSaveButtonClick);
            
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(SetDefaultValuesButtonClick);
        }

        private void SaveValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            var floatValue = float.Parse(thrustInputField.text);
            SetValue(thrust, floatValue);

            var mass = $"{prefix}mass";
            floatValue = float.Parse(massInputField.text);
            SetValue(mass, floatValue);

            var drag = $"{prefix}drag";
            floatValue = float.Parse(dragInputField.text);
            SetValue(drag, floatValue);

            var angularDrag = $"{prefix}angularDrag";
            floatValue = float.Parse(angularDragInputField.text);
            SetValue(angularDrag, floatValue);

            var pidRotationP = $"{prefix}pidRotationP";
            floatValue = float.Parse(pidRotationPInputField.text);
            SetValue(pidRotationP, floatValue);

            // var pidRotationI = $"{prefix}pidRotationI";
            // floatValue = float.Parse(pidRotationIInputField.text);
            // SetValue(pidRotationI, floatValue);

            var pidRotationD = $"{prefix}pidRotationD";
            floatValue = float.Parse(pidRotationDInputField.text);
            SetValue(pidRotationD, floatValue);

            var pidAltitudePR = $"{prefix}pidAltitudePR";
            floatValue = float.Parse(pidAltitudePInputFieldR.text);
            SetValue(pidAltitudePR, floatValue);

            var pidAltitudeIR = $"{prefix}pidAltitudeIR";
            floatValue = float.Parse(pidAltitudeIInputFieldR.text);
            SetValue(pidAltitudeIR, floatValue);

            var pidAltitudeDR = $"{prefix}pidAltitudeDR";
            floatValue = float.Parse(pidAltitudeDInputFieldR.text);
            SetValue(pidAltitudeDR, floatValue);

            var pidAltitudePP = $"{prefix}pidAltitudePP";
            floatValue = float.Parse(pidAltitudePInputFieldP.text);
            SetValue(pidAltitudePP, floatValue);

            var pidAltitudeIP = $"{prefix}pidAltitudeIP";
            floatValue = float.Parse(pidAltitudeIInputFieldP.text);
            SetValue(pidAltitudeIP, floatValue);

            var pidAltitudeDP = $"{prefix}pidAltitudeDP";
            floatValue = float.Parse(pidAltitudeDInputFieldP.text);
            SetValue(pidAltitudeDP, floatValue);

            var pidAltitudePY = $"{prefix}pidAltitudePY";
            floatValue = float.Parse(pidAltitudePInputFieldY.text);
            SetValue(pidAltitudePY, floatValue);

            var pidAltitudeIY = $"{prefix}pidAltitudeIY";
            floatValue = float.Parse(pidAltitudeIInputFieldY.text);
            SetValue(pidAltitudeIY, floatValue);

            var pidAltitudeDY = $"{prefix}pidAltitudeDY";
            floatValue = float.Parse(pidAltitudeDInputFieldY.text);
            SetValue(pidAltitudeDY, floatValue);

            var cameraRotationX = $"{prefix}cameraRotationX";
            floatValue = float.Parse(cameraRotationXInputField.text);
            SetValue(cameraRotationX, floatValue);

            var cameraRotationY = $"{prefix}cameraRotationY";
            floatValue = float.Parse(cameraRotationYInputField.text);
            SetValue(cameraRotationY, floatValue);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            floatValue = float.Parse(cameraRotationZInputField.text);
            SetValue(cameraRotationZ, floatValue);

            var sizeScale = $"{prefix}sizeScale";
            floatValue = float.Parse(sizeScaleInputField.text);
            SetValue(sizeScale, floatValue);

            // Input
            if (proportionalGainInputField != null)
            {
                var proportionalGain = $"{prefix}proportionalGain";
                floatValue = float.Parse(proportionalGainInputField.text);
                SetValue(proportionalGain, floatValue);
            }

            if (exponentialGainInputField != null)
            {
                var exponentialGain = $"{prefix}exponentialGain";
                floatValue = float.Parse(exponentialGainInputField.text);
                SetValue(exponentialGain, floatValue);
            }

            var slMaxAngle = $"{prefix}slMaxAngle";
            floatValue = float.Parse(slMaxAngleInputField.text);
            SetValue(slMaxAngle, floatValue);

            PlayerPrefs.Save();
        }

        private void LoadValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            thrustInputField.text = GetValue(thrust).ToString();

            var mass = $"{prefix}mass";
            massInputField.text = GetValue(mass).ToString();

            var drag = $"{prefix}drag";
            dragInputField.text = GetValue(drag).ToString();

            var angularDrag = $"{prefix}angularDrag";
            angularDragInputField.text = GetValue(angularDrag).ToString();

            var pidRotationP = $"{prefix}pidRotationP";
            pidRotationPInputField.text = GetValue(pidRotationP).ToString();

            // var pidRotationI = $"{prefix}pidRotationI";
            // pidRotationIInputField.text = GetValue(pidRotationI).ToString();

            var pidRotationD = $"{prefix}pidRotationD";
            pidRotationDInputField.text = GetValue(pidRotationD).ToString();

            var pidAltitudePR = $"{prefix}pidAltitudePR";
            pidAltitudePInputFieldR.text = GetValue(pidAltitudePR).ToString();

            //var pidAltitudeIR = $"{prefix}pidAltitudeIR";
            //pidAltitudeIInputFieldR.text = GetValue(pidAltitudeIR).ToString();

            var pidAltitudeDR = $"{prefix}pidAltitudeDR";
            pidAltitudeDInputFieldR.text = GetValue(pidAltitudeDR).ToString();


            var pidAltitudePP = $"{prefix}pidAltitudePP";
            pidAltitudePInputFieldP.text = GetValue(pidAltitudePP).ToString();

            var pidAltitudeIP = $"{prefix}pidAltitudeIP";
            pidAltitudeIInputFieldP.text = GetValue(pidAltitudeIP).ToString();

            var pidAltitudeDP = $"{prefix}pidAltitudeDP";
            pidAltitudeDInputFieldP.text = GetValue(pidAltitudeDP).ToString();


            var pidAltitudePY = $"{prefix}pidAltitudePY";
            pidAltitudePInputFieldY.text = GetValue(pidAltitudePY).ToString();

            var pidAltitudeIY = $"{prefix}pidAltitudeIY";
            pidAltitudeIInputFieldY.text = GetValue(pidAltitudeIY).ToString();

            var pidAltitudeDY = $"{prefix}pidAltitudeDY";
            pidAltitudeDInputFieldY.text = GetValue(pidAltitudeDY).ToString();

            var cameraRotationX = $"{prefix}cameraRotationX";
            cameraRotationXInputField.text = GetValue(cameraRotationX).ToString();

            var cameraRotationY = $"{prefix}cameraRotationY";
            cameraRotationYInputField.text = GetValue(cameraRotationY).ToString();

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            cameraRotationZInputField.text = GetValue(cameraRotationZ).ToString();

            var sizeScale = $"{prefix}sizeScale";
            sizeScaleInputField.text = GetValue(sizeScale).ToString();

            // Input
            if (proportionalGainInputField != null)
            {
                var proportionalGain = $"{prefix}proportionalGain";
                proportionalGainInputField.text = GetValue(proportionalGain).ToString();
            }

            if (exponentialGainInputField != null)
            {
                var exponentialGain = $"{prefix}exponentialGain";
                exponentialGainInputField.text = GetValue(exponentialGain).ToString();
            }

            var slMaxAngle = $"{prefix}slMaxAngle";
            slMaxAngleInputField.text = GetValue(slMaxAngle).ToString();
        }

        private void SetDefaultValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            SetValue(thrust, dronePhysics.physicsConfig.thrust);

            var mass = $"{prefix}mass";
            SetValue(mass, dronePhysics.physicsConfig.mass);

            var drag = $"{prefix}drag";
            SetValue(drag, dronePhysics.physicsConfig.drag);

            var angularDrag = $"{prefix}angularDrag";
            SetValue(angularDrag, dronePhysics.physicsConfig.angularDrag);

            var pidRotationP = $"{prefix}pidRotationP";
            SetValue(pidRotationP, dronePhysics.physicsConfig.p);

            var pidRotationI = $"{prefix}pidRotationI";
            SetValue(pidRotationI, dronePhysics.physicsConfig.i);

            var pidRotationD = $"{prefix}pidRotationD";
            SetValue(pidRotationD, dronePhysics.physicsConfig.d);

            var pidAltitudePR = $"{prefix}pidAltitudePR";
            SetValue(pidAltitudePR, dronePhysics.physicsConfig.pAltitudeR);

            var pidAltitudeIR = $"{prefix}pidAltitudeIR";
            SetValue(pidAltitudeIR, dronePhysics.physicsConfig.iAltitudeR);

            var pidAltitudeDR = $"{prefix}pidAltitudeDR";
            SetValue(pidAltitudeDR, dronePhysics.physicsConfig.dAltitudeR);

            var pidAltitudePP = $"{prefix}pidAltitudePP";
            SetValue(pidAltitudePP, dronePhysics.physicsConfig.pAltitudeP);

            var pidAltitudeIP = $"{prefix}pidAltitudeIP";
            SetValue(pidAltitudeIP, dronePhysics.physicsConfig.iAltitudeP);

            var pidAltitudeDP = $"{prefix}pidAltitudeDP";
            SetValue(pidAltitudeDP, dronePhysics.physicsConfig.dAltitudeP);

            var pidAltitudePY = $"{prefix}pidAltitudePY";
            SetValue(pidAltitudePY, dronePhysics.physicsConfig.pAltitudeY);

            var pidAltitudeIY = $"{prefix}pidAltitudeIY";
            SetValue(pidAltitudeIY, dronePhysics.physicsConfig.iAltitudeY);

            var pidAltitudeDY = $"{prefix}pidAltitudeDY";
            SetValue(pidAltitudeDY, dronePhysics.physicsConfig.dAltitudeY);


            var cameraRotationX = $"{prefix}cameraRotationX";
            SetValue(cameraRotationX, defaultCameraRotation.x);

            var cameraRotationY = $"{prefix}cameraRotationY";
            SetValue(cameraRotationY, defaultCameraRotation.y);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            SetValue(cameraRotationZ, defaultCameraRotation.z);

            var sizeScale = $"{prefix}sizeScale";
            SetValue(sizeScale, defaultScale.x);

            // Input
            var proportionalGain = $"{prefix}proportionalGain";
            SetValue(proportionalGain, droneInput.ratesConfig.proportionalGain);

            var exponentialGain = $"{prefix}exponentialGain";
            SetValue(exponentialGain, droneInput.ratesConfig.exponentialGain);

            var slMaxAngle = $"{prefix}slMaxAngle";
            SetValue(slMaxAngle, droneInput.ratesConfig.maxAngle);

            PlayerPrefs.Save();
        }

        public static void GetSettings(
            out YueRatesConfiguration inputRates,
            out YueDronePhysicsConfiguration config,
            out Vector3 cameraRotation,
            out Vector3 droneScale)
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            var thrustValue = GetValue(thrust);

            var mass = $"{prefix}mass";
            var massValue = GetValue(mass);

            var drag = $"{prefix}drag";
            var dragValue = GetValue(drag);

            var angularDrag = $"{prefix}angularDrag";
            var angularDragValue = GetValue(angularDrag);

            var pidRotationP = $"{prefix}pidRotationP";
            var pidRotationPValue = GetValue(pidRotationP);

            var pidRotationI = $"{prefix}pidRotationI";
            var pidRotationIValue = GetValue(pidRotationI);

            var pidRotationD = $"{prefix}pidRotationD";
            var pidRotationDValue = GetValue(pidRotationD);

            var pidAltitudePR = $"{prefix}pidAltitudePR";
            var pidAltitudePValueR = GetValue(pidAltitudePR);

            var pidAltitudeIR = $"{prefix}pidAltitudeIR";
            var pidAltitudeIValueR = GetValue(pidAltitudeIR);

            var pidAltitudeDR = $"{prefix}pidAltitudeDR";
            var pidAltitudeDValueR = GetValue(pidAltitudeDR);

            var pidAltitudePP = $"{prefix}pidAltitudePP";
            var pidAltitudePValueP = GetValue(pidAltitudePP);

            var pidAltitudeIP = $"{prefix}pidAltitudeIP";
            var pidAltitudeIValueP = GetValue(pidAltitudeIP);

            var pidAltitudeDP = $"{prefix}pidAltitudeDP";
            var pidAltitudeDValueP = GetValue(pidAltitudeDP);

            var pidAltitudePY = $"{prefix}pidAltitudePY";
            var pidAltitudePValueY = GetValue(pidAltitudePY);

            var pidAltitudeIY = $"{prefix}pidAltitudeIY";
            var pidAltitudeIValueY = GetValue(pidAltitudeIY);

            var pidAltitudeDY = $"{prefix}pidAltitudeDY";
            var pidAltitudeDValueY = GetValue(pidAltitudeDY);

            var cameraRotationX = $"{prefix}cameraRotationX";
            var cameraRotationXValue = GetValue(cameraRotationX);

            var cameraRotationY = $"{prefix}cameraRotationY";
            var cameraRotationYValue = GetValue(cameraRotationY);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            var cameraRotationZValue = GetValue(cameraRotationZ);

            var sizeScale = $"{prefix}sizeScale";
            var sizeScaleValue = GetValue(sizeScale);

            config = new YueDronePhysicsConfiguration
            {
                thrust = thrustValue,
                mass = massValue,
                drag = dragValue,
                angularDrag = angularDragValue,
                p = pidRotationPValue,
                i = pidRotationIValue,
                d = pidRotationDValue,
                pAltitudeR = pidAltitudePValueR,
                iAltitudeR = pidAltitudeIValueR,
                dAltitudeR = pidAltitudeDValueR,
                pAltitudeP = pidAltitudePValueP,
                iAltitudeP = pidAltitudeIValueP,
                dAltitudeP = pidAltitudeDValueP,
                pAltitudeY = pidAltitudePValueY,
                iAltitudeY = pidAltitudeIValueY,
                dAltitudeY = pidAltitudeDValueY
            };

            cameraRotation = new Vector3(cameraRotationXValue, cameraRotationYValue, cameraRotationZValue);
            droneScale = new Vector3(sizeScaleValue, sizeScaleValue, sizeScaleValue);

            // Input
            var proportionalGain = $"{prefix}proportionalGain";
            var proportionalGainValue = GetValue(proportionalGain);

            var exponentialGain = $"{prefix}exponentialGain";
            var exponentialGainValue = GetValue(exponentialGain);

            var slMaxAngle = $"{prefix}slMaxAngle";
            var slMaxAngleValue = GetValue(slMaxAngle);


            inputRates = new YueRatesConfiguration
            {
                proportionalGain = proportionalGainValue,
                exponentialGain = exponentialGainValue,
                maxAngle = slMaxAngleValue,
                mode = YueTransmitterMode.Mode2
            };
        }

        private static float GetValue(string valueName)
        {
            return PlayerPrefs.GetFloat(valueName);
        }

        private static void SetValue(string valueName, float value)
        {
            PlayerPrefs.SetFloat(valueName, value);
        }

        private void OnSaveButtonClick()
        {
            acceptMenu.Initialize(null, null, OnAccept);
        }

        private void OnAccept()
        {
            SaveValuesButtonClick();
        }

        public void SaveValuesButtonClick()
        {
            SaveValues();

            var acroBridge = FindObjectOfType<AcroSettingsBridge>();
            if (acroBridge != null) acroBridge.LoadSettings();
        }

        public void SetDefaultValuesButtonClick()
        {
            SetDefaultValues();
            LoadValues();
        }

        //public Sprite inputImage;

        private void OnValidate()
        {
            // var inputs = GetComponentsInChildren<InputField>();
            // foreach (var input in inputs)
            // {
            //     var image = input.GetComponent<Image>();
            //     image.sprite = inputImage;
            //     image.pixelsPerUnitMultiplier = 0.5f;
            //     var texts =input.GetComponentsInChildren<Text>();
            //     foreach (var text in texts)
            //     {
            //         text.color = Color.white;
            //     }
            // }
        }
    }
}