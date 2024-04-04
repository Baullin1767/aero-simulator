using UnityEngine;

namespace YueUltimateDronePhysics
{
    public class YuePIDController
    {
        private Vector3 pd;

        private Vector3 p;
        private Vector3 d;
        private Vector3 lastE;

        public YuePIDController()
        {

        }

        public Vector3 CalculatePD(Vector3 e, float pr, float dr, float pp=0, float dp=0, float py = 0, float dy = 0)
        {
            if (pp == 0 || dp == 0 || py == 0 || dy == 0)
            {
                p = e * pr;

                d = ((e - lastE) / Time.fixedDeltaTime)* dr;

                pd = p + d;

                lastE = e;
            }
            else
            {
                p = new Vector3(e.x * pr, e.y * pp, e.z * py);

                d = ((e - lastE) / Time.fixedDeltaTime);
                d = new Vector3(d.x * dr, d.y * dp, d.z * dy);

                pd = p + d;

                lastE = e; 
            }
            return pd;
        }
    }
}
