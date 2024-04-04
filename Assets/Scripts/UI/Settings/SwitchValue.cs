using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using TMPro;

public class SwitchValue : MonoBehaviour
{
    public float value = 1f;
    float result;
    [SerializeField] float step = 0.01f;

    public TMP_InputField valueText;
    GraphPainter tuning;

    [SerializeField] float maxLimit;
    [SerializeField] float minLimit;

    private void Start()
    {
        valueText.text = System.Math.Round(value, 2).ToString();
        tuning = GetComponentInParent<GraphPainter>();
    }
    private void Update()
    {
        if (valueText.isFocused)
        {
            try
            {
                result = System.Convert.ToSingle(valueText.text);
            }
            catch (System.FormatException)
            {
                valueText.text = "0";
                result = 0f;
            }
            Debug.Log(result);
            if (result != value && result <= maxLimit && result >= minLimit)
            {
                value = result;

                tuning.UpdateGraph();
            }
            else
            {
                if (result > maxLimit)
                {
                    valueText.text = (maxLimit).ToString();
                }
                if (result < minLimit)
                {
                    valueText.text = (minLimit - step).ToString();
                }

                tuning.UpdateGraph();
            } 
        }
    }

    public void Switch(bool plus)
    {
        if (plus && value < maxLimit)
        {
            value += step;
        }
        else if (!plus && value >= minLimit)
        {
            value -= step;
        } 
        valueText.text = System.Math.Round(value, 2).ToString();
        tuning.UpdateGraph();
    }
}
