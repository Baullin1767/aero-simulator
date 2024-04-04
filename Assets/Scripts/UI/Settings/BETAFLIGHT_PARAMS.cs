using Services;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using YueUltimateDronePhysics;

public class BETAFLIGHT_PARAMS : MonoBehaviour
{
    [SerializeField] GraphPainter paramROLL;
    [SerializeField] GraphPainter paramPITCH;
    [SerializeField] GraphPainter paramYAW;

    [SerializeField]
    private YueDronePhysics dronePhysics;

    [SerializeField] TMP_InputField rollP;
    [SerializeField] TMP_InputField rollI;
    [SerializeField] TMP_InputField rollD;
    [SerializeField] TMP_InputField pitchP;
    [SerializeField] TMP_InputField pitchI;
    [SerializeField] TMP_InputField pitchD;
    [SerializeField] TMP_InputField yawP;
    [SerializeField] TMP_InputField yawI;

    [SerializeField] InputService inputService;
    YueInputModule yueInputModule;
    public void SaveParams()
    {
        SaveParam("ROLL", paramROLL.RCRate.value, paramROLL.superRate.value, paramROLL.expo.value);
        SaveParam("PITCH", paramPITCH.RCRate.value, paramPITCH.superRate.value, paramPITCH.expo.value);
        SaveParam("YAW", paramYAW.RCRate.value, paramYAW.superRate.value, paramYAW.expo.value);
        Debug.Log(paramROLL.RCRate.value);
    }
    void SaveParam(string axis, float RCRate, float superRate, float expo)
    {

        PlayerPrefs.SetFloat($"RCRate_{axis}", RCRate);
        PlayerPrefs.SetFloat($"superRate_{axis}", superRate);
        PlayerPrefs.SetFloat($"expo_{axis}", expo);
        inputService = FindObjectOfType<InputService>();
        yueInputModule = FindObjectOfType<YueInputModule>();

        switch (axis)
        {
            case "ROLL":
                inputService.RCRate_ROLL = RCRate;
                inputService.expo_ROLL = expo;
                inputService.superRate_ROLL = superRate;
                if (yueInputModule != null)
                {
                    yueInputModule.RCRate_ROLL = RCRate;
                    yueInputModule.expo_ROLL = expo;
                    yueInputModule.superRate_ROLL = superRate;
                }
                break;
            case "PITCH":
                inputService.RCRate_PITCH = RCRate;
                inputService.expo_PITCH = expo;
                inputService.superRate_PITCH = superRate;
                if (yueInputModule != null)
                {
                    yueInputModule.RCRate_PITCH = RCRate;
                    yueInputModule.expo_PITCH = expo;
                    yueInputModule.superRate_PITCH = superRate;
                }
                break;
            case "YAW":
                inputService.RCRate_YAW = RCRate;
                inputService.expo_YAW = expo;
                inputService.superRate_YAW = superRate;
                if (yueInputModule != null)
                {
                    yueInputModule.RCRate_YAW = RCRate;
                    yueInputModule.expo_YAW = expo;
                    yueInputModule.superRate_YAW = superRate;
                }
                break;
        }
    }
    public void DafaultSettings()
    {
        SaveParam("ROLL", 1, 0, 0);
        paramROLL.Reload();
        SaveParam("PITCH", 1, 0, 0);
        paramPITCH.Reload();
        SaveParam("YAW", 1, 0, 0);
        paramYAW.Reload();

        var prefix = "acro_drone_";
        var pidAltitudePR = $"{prefix}pidAltitudePR";
        PlayerPrefs.SetFloat(pidAltitudePR, dronePhysics.physicsConfig.pAltitudeR);

        var pidAltitudeIR = $"{prefix}pidAltitudeIR";
        PlayerPrefs.SetFloat(pidAltitudeIR, dronePhysics.physicsConfig.iAltitudeR);

        var pidAltitudeDR = $"{prefix}pidAltitudeDR";
        PlayerPrefs.SetFloat(pidAltitudeDR, dronePhysics.physicsConfig.dAltitudeR);

        var pidAltitudePP = $"{prefix}pidAltitudePP";
        PlayerPrefs.SetFloat(pidAltitudePP, dronePhysics.physicsConfig.pAltitudeP);

        var pidAltitudeIP = $"{prefix}pidAltitudeIP";
        PlayerPrefs.SetFloat(pidAltitudeIP, dronePhysics.physicsConfig.iAltitudeP);

        var pidAltitudeDP = $"{prefix}pidAltitudeDP";
        PlayerPrefs.SetFloat(pidAltitudeDP, dronePhysics.physicsConfig.dAltitudeP);

        var pidAltitudePY = $"{prefix}pidAltitudePY";
        PlayerPrefs.SetFloat(pidAltitudePY, dronePhysics.physicsConfig.pAltitudeY);

        var pidAltitudeIY = $"{prefix}pidAltitudeIY";
        PlayerPrefs.SetFloat(pidAltitudeIY, dronePhysics.physicsConfig.iAltitudeY);

        var pidAltitudeDY = $"{prefix}pidAltitudeDY";
        PlayerPrefs.SetFloat(pidAltitudeDY, dronePhysics.physicsConfig.dAltitudeY);


        rollP.text = "1";
        rollI.text = "1";
        rollD.text = "1";
        pitchP.text = "1";
        pitchI.text = "1";
        pitchD.text = "1";
        yawP.text = "1";
        yawI.text = "1";

    }
}