using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UnityEngine;

namespace Mission.Tutorial
{
    public class TutorialMission : DroneMission
    {
        [SerializeField]
        private float missionTimeSeconds;

        [SerializeField]
        private float widthX = 300f;

        [SerializeField]
        private float widthZ = 300f;

        [SerializeField]
        private float height = 300f;

        [SerializeField]
        private float grainDelta = 10f;

        [SerializeField]
        private Color lightingColor = new (0.3f, 0.3f, 0.3f);

        [SerializeField]
        private int laps;

        [SerializeField]
        private bool hasFog;

        [SubclassSelector, SerializeReference]
        private List<MissionStep> missionSteps;

        private CancellationTokenSource _cTokenSource;
        private Blob _blob;
        private int _currentLap;
        private bool _missionStarted;

        public int Laps => laps;
        public int CurrentLap => _currentLap;

        protected override void OnAwake()
        {
            base.OnAwake();

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

            RenderSettings.ambientLight = lightingColor;
            RenderSettings.fog = hasFog;
            
            _cTokenSource?.Cancel();
            _cTokenSource = new CancellationTokenSource();
            MissionSteps(_cTokenSource.Token).Forget();
            if (missionTimeSeconds > 0.5f) CheckMissionTime(_cTokenSource.Token).Forget();
            MapBorders(_cTokenSource.Token).Forget();
        }

        private async UniTask CheckMissionTime(CancellationToken cToken)
        {
            while (!_missionStarted && !cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
            }

            var timer = FindObjectOfType<Timer>(true);
            timer.gameObject.SetActive(true);
            var time = 0f;
            while (!cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                time += Time.deltaTime;
                if (time > missionTimeSeconds)
                {
                    MissionManager.Fail();
                    return;
                }

                var lastTime = missionTimeSeconds - time;
                timer.SetText(lastTime.ToString("ОСТАЛОСЬ: 0:00"));
            }
        }

        private async UniTask MissionSteps(CancellationToken cToken)
        {
            _blob = FindObjectOfType<Blob>(true);
            _blob.gameObject.SetActive(false);
            if (missionSteps.Count == 0) return;

            foreach (var missionStep in missionSteps)
            {
                missionStep.Initialize();
            }

            _currentLap = 1;
            var currentIndex = 0;
            while (!cToken.IsCancellationRequested)
            {
                var activeMissionStep = missionSteps[currentIndex];
                SetBlobText(activeMissionStep.GetBlobText());
                await activeMissionStep.MakeStep();
                if (cToken.IsCancellationRequested) return;

                currentIndex++;
                if (currentIndex >= missionSteps.Count)
                {
                    if (laps > 0)
                    {
                        foreach (var step in missionSteps)
                        {
                            step.Initialize();
                        }
                        activeMissionStep = missionSteps[0];
                        SetBlobText(activeMissionStep.GetBlobText());
                        await activeMissionStep.MakeStep();
                        if (cToken.IsCancellationRequested) return;
                        
                        if (laps > _currentLap)
                        {
                            _currentLap++;
                            currentIndex = 1;
                        }
                        else
                        {
                       
                            MissionManager.Success();
                            return;
                        }
                    }
                    else
                    {
                       
                        MissionManager.Success();
                        return;
                    }
                }
            }
        }

        private void SetBlobText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _blob.gameObject.SetActive(false);
            }
            else
            {
                _blob.SetText(text);
                _blob.gameObject.SetActive(true);
            }
        }

        private async UniTask MapBorders(CancellationToken cToken)
        {
            var droneBridge = FindObjectOfType<DroneBridge>();
            var hud = FindObjectOfType<HUD>();
            var startPosition = droneBridge.transform.position;
            while (!cToken.IsCancellationRequested)
            {
                var currentPosition = droneBridge.transform.position;
                var xDelta = Mathf.Abs(startPosition.x - currentPosition.x);
                var yDelta = Mathf.Abs(startPosition.y - currentPosition.y);
                var zDelta = Mathf.Abs(startPosition.z - currentPosition.z);
                if (xDelta > widthX ||
                    zDelta > widthZ)
                {
                    var maxGrainDelta = xDelta > zDelta ? xDelta + grainDelta : zDelta + grainDelta;
                    var maxDelta = xDelta > zDelta ? xDelta : zDelta;
                    var maxMax = xDelta > zDelta ? widthX : widthZ;

                    maxGrainDelta = maxGrainDelta > yDelta ? maxGrainDelta : yDelta + grainDelta;
                    maxDelta = maxDelta > yDelta ? maxDelta : yDelta;
                    maxMax = maxMax > yDelta ? maxMax : height;

                    if (maxDelta > maxGrainDelta)
                    {
                        hud.SetGrain(1f);
                    }
                    else
                    {
                        var maxDif = maxGrainDelta - maxMax;
                        var dif = (maxDif - (maxGrainDelta - maxDelta)) / maxDif;
                        hud.SetGrain(dif);
                    }
                }
                else if (yDelta > height)
                {
                    var maxGrainDelta = yDelta + grainDelta;
                    var maxDelta = yDelta;
                    var maxMax = height;

                    if (maxDelta > maxGrainDelta)
                    {
                        hud.SetGrain(1f);
                    }
                    else
                    {
                        var maxDif = maxGrainDelta - maxMax;
                        var dif = (maxDif - (maxGrainDelta - maxDelta)) / maxDif;
                        hud.SetGrain(dif);
                    }
                }

                await UniTask.Yield();
            }
        }

        public void ClearSteps()
        {
            missionSteps.Clear();
        }

        public void AddStep(MissionStep missionStep)
        {
            missionSteps.Add(missionStep);
        }

        public void MissionStart()
        {
            _missionStarted = true;
        }
    }
}