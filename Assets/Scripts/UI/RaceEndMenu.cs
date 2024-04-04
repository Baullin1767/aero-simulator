using Services;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RaceEndMenu : SuccessMenu
    {
        private const string RecordKey = "Record";
        
        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private GameObject newRecord;

        [SerializeField]
        private TextMeshProUGUI bestRecordText;

        public override void Open()
        {
            base.Open();
            var curTime = CurTime();
            timeText.text = GetTime(curTime);

            var key = $"{RecordKey}{MissionService.Instance.GetCurrentMissionName()}";
            var bestRecord = PlayerPrefs.GetFloat(key, 9999999f);
            var time = Time.timeSinceLevelLoad;

            var isNewRecord = time < bestRecord; 
            newRecord.SetActive(isNewRecord);
            bestRecordText.gameObject.SetActive(!isNewRecord);
            bestRecordText.text = GetTime(bestRecord);

            if (isNewRecord)
            {
                PlayerPrefs.SetFloat(key, time);
                PlayerPrefs.Save();
            }
        }
    }
}