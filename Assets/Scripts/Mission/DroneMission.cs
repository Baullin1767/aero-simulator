using Services;
using UnityEngine;

namespace Mission
{
    public abstract class DroneMission : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField]
        private Transform dronePositionPoint;

        public virtual Vector3 DronePositionPoint => dronePositionPoint.position;
        public virtual Quaternion StartDroneRotation => dronePositionPoint.rotation;
        
        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            if (dronePositionPoint.childCount > 0)
            {
                var child = dronePositionPoint.GetChild(0);
                Destroy(child.gameObject);
            }
            Time.timeScale = 1;
        }

        protected void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MissionService.Instance.StartMission();
            }
        }
    }
}