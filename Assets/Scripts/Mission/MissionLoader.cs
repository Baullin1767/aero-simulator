using System;
using Drones;
using Services;
using UnityEngine;

namespace Mission
{
    public class MissionLoader : MonoBehaviour
    {
        private DroneMission _missionObject;

        [SerializeField]
        private bool isNetwork;

        private void Awake()
        {
            if (isNetwork)
            {
                LoadNetwork();
            }
            else
            {
                LoadMission();
            }
        }

        private void LoadNetwork()
        {
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var allConfigs = MissionService.Instance.AllConfigs;
            DroneMission missionObject = null;
            MissionConfig config = null;
            foreach (var missionConfig in allConfigs)
            {
                missionObject = missionConfig.GetMission(missionName);
                if (missionObject == null) continue;
                config = missionConfig;
                break;
            }

            if (missionObject == null) missionObject = allConfigs[0].GetDefaultMission();
            _missionObject = Instantiate(missionObject);
        }

        private void LoadMission()
        {
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var allConfigs = MissionService.Instance.AllConfigs;
            DroneMission missionObject = null;
            MissionConfig config = null;
            foreach (var missionConfig in allConfigs)
            {
                missionObject = missionConfig.GetMission(missionName);
                if (missionObject == null) continue;
                config = missionConfig;
                break;
            }

            if (missionObject == null) missionObject = allConfigs[0].GetDefaultMission();
            _missionObject = Instantiate(missionObject);
            LoadDrone(_missionObject.DronePositionPoint, _missionObject.StartDroneRotation, config);
        }

        private void LoadDrone(Vector3 dronePosition, Quaternion droneRotation, MissionConfig config)
        {
            GameObject droneObject = null;
            if (config.name.Contains("tutorial", StringComparison.OrdinalIgnoreCase))
            {
                droneObject = DroneService.Instance.TutorialDrone?.GameObject;
            }
            
            if (droneObject == null &&
                config.name.Contains("SimpleMission", StringComparison.OrdinalIgnoreCase))
            {
                droneObject = DroneService.Instance.MissionDrone?.GameObject;
            }

            if (droneObject == null) droneObject = DroneService.Instance.GetCurrentDrone()?.GameObject;
            Instantiate(droneObject, dronePosition, droneRotation);
        }
    }
}