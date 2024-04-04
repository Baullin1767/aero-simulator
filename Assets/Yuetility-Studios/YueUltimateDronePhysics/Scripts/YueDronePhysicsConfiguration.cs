// Written by Marcel Remmers for Yuetility-Studios 2022

using UnityEngine;

namespace YueUltimateDronePhysics
{
    [System.Serializable]
    public class YueDronePhysicsConfiguration
    {
        public YueDronePhysicsConfiguration()
        {
        }

        public YueDronePhysicsConfiguration(YueDronePhysicsConfiguration physicsConfig)
        {
            thrust = physicsConfig.thrust;

            mass = physicsConfig.mass;
            drag = physicsConfig.drag;
            angularDrag = physicsConfig.angularDrag;

            p = physicsConfig.p;
            i = physicsConfig.i;
            d = physicsConfig.d;

            pAltitudeR = physicsConfig.pAltitudeR;
            iAltitudeR = physicsConfig.iAltitudeR;
            dAltitudeR = physicsConfig.dAltitudeR;
            pAltitudeP = physicsConfig.pAltitudeP;
            iAltitudeP = physicsConfig.iAltitudeP;
            dAltitudeP = physicsConfig.dAltitudeP;
            pAltitudeY = physicsConfig.pAltitudeY;
            iAltitudeY = physicsConfig.iAltitudeY;
            dAltitudeY = physicsConfig.dAltitudeY;
        }

        [Header("Maximum Thrust [N]")]
        public float thrust = 22650f;

        [Header("Physics")]
        public float mass = 1f;

        public float drag = 0.1f;
        public float angularDrag = 0.1f;

        [Header("PID Rotation [Nm/Deg]")]
        public float p = 0f;

        public float i = 0f;
        public float d = 0f;

        [Header("PID Altitude [N/m]")]

        public float pAltitudeR = 0f;
        public float iAltitudeR = 0f;
        public float dAltitudeR = 0f;

        public float pAltitudeP = 0f;
        public float iAltitudeP = 0f;
        public float dAltitudeP = 0f;

        public float pAltitudeY = 0f;
        public float iAltitudeY = 0f;
        public float dAltitudeY = 0f;
    }
}