using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UnityEngine;
using YueUltimateDronePhysics;

namespace Mission
{
    public class PatrMissionDay : DroneMission
    {
        [SerializeField]
        private Material skyboxDayMaterial;

        [SerializeField]
        private float missionTimeInSeconds = 420f;

        [SerializeField]
        private float maxHeight = 100f;
        
        [SerializeField]
        private float fullGrainHeight = 110f;

        [SerializeField]
        private List<Gate> gates;

        private CancellationTokenSource _cTokenSource;
        private DroneBridge _droneBridge;
        private HUD _hud;
        private XBOXControllerInput _inputController;

        protected override void OnAwake()
        {
            base.OnAwake();

            RenderSettings.fog = false;
            RenderSettings.skybox = skyboxDayMaterial;

            MissionInitialize().Forget();
        }

        private void OnDestroy()
        {
            _cTokenSource?.Cancel();
        }

        private async UniTask MissionInitialize()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();

            _cTokenSource?.Cancel();

            _droneBridge = FindObjectOfType<DroneBridge>();
            _inputController = FindObjectOfType<XBOXControllerInput>();
            _hud = FindObjectOfType<HUD>();

            _cTokenSource = new CancellationTokenSource();
            if (missionTimeInSeconds > 1f) CheckTime(_cTokenSource.Token).Forget();
            if (maxHeight > 1f) CheckHeight(_cTokenSource.Token).Forget();
            GatesMission(_cTokenSource.Token).Forget();
        }

        private async UniTask CheckTime(CancellationToken cToken)
        {
            var time = 0f;
            while (!cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                time += Time.deltaTime;
                if (time > missionTimeInSeconds)
                {
                    MissionManager.Fail();
                    return;
                }
            }
        }

        private async UniTask CheckHeight(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                var height = _droneBridge.GetHeightValue();
                if (height > maxHeight)
                {
                    if (height > fullGrainHeight)
                    {
                        _hud.SetGrain(1f);
                    }
                    else
                    {
                        var maxDif = fullGrainHeight - maxHeight;
                        var dif = (maxDif - (fullGrainHeight - height)) / maxDif;
                        _hud.SetGrain(dif);
                    }
                }
                else
                {
                    _hud.SetGrain(0f);
                }
            }
        }

        private async UniTask GatesMission(CancellationToken cToken)
        {
            if (gates.Count == 0) return;

            foreach (var gate in gates)
            {
                gate.SetColor(Color.red);
            }

            var currentIndex = 0;
            var activeGate = gates[currentIndex];
            activeGate.SetColor(Color.green);

            while (!cToken.IsCancellationRequested)
            {
                await activeGate.AwaitCollision();
                if (cToken.IsCancellationRequested) return;

                currentIndex++;
                if (currentIndex >= gates.Count)
                {
                    MissionManager.Success();
                    return;
                }

                activeGate.SetColor(Color.red);
                activeGate = gates[currentIndex];
                activeGate.SetColor(Color.green);
            }
        }
    }
}