using TMPro;
using UnityEngine;
namespace Mission
{
    public class Blob : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void SetText(string textStr)
        {
            text.text = textStr;
        }
    }
}
