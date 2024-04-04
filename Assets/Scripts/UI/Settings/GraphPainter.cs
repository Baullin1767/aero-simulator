
using ChartAndGraph;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GraphPainter : MonoBehaviour
{
    public SwitchValue RCRate;
    public SwitchValue expo;
    public SwitchValue superRate;
    BetaflightCalculator calculator = new BetaflightCalculator();
    [SerializeField] float step = 0.01f;
    [SerializeField] string axis;

    [SerializeField] GraphChartBase graph;

    [SerializeField] TextMeshProUGUI speedValue;
    void Start()
    {
        Reload();
    }
    public void Reload()
    {
        RCRate.value = PlayerPrefs.GetFloat($"RCRate_{axis}");
        expo.value = PlayerPrefs.GetFloat($"expo_{axis}");
        superRate.value = PlayerPrefs.GetFloat($"superRate_{axis}");
        if(RCRate.value > 0)
            RCRate.valueText.text = Math.Round(RCRate.value, 1).ToString();
        else
        {
            RCRate.valueText.text = "1";
            RCRate.value = 1;
            PlayerPrefs.SetFloat($"RCRate_{axis}", 1);

        }
        expo.valueText.text = Math.Round(expo.value, 1).ToString();
        superRate.valueText.text = Math.Round(superRate.value, 1).ToString();


        UpdateGraph();
    }


    public void UpdateGraph()
    {
        if (graph.DataSource.GetPointCount("Graph") > 0)
        {
            graph.DataSource.ClearCategory("Graph");
        }
        for (float i = -1; i < 1; i+=step)
        {
            graph.DataSource.AddPointToCategory("Graph", i,
                calculator.BfCalc(i, RCRate.value, expo.value, superRate.value));
        }
        speedValue.text = Math.Round(
            calculator.BfCalc(1, RCRate.value, expo.value, superRate.value)).ToString() + " гр/c";
    }
}


public class BetaflightCalculator
{
    private float Clamp(float value, float min, float max)
    {
        return Math.Max(min, Math.Min(max, value));
    }

    public float BfCalc(float rcCommand, float rcRate, float expo, float superRate)
    {
        rcCommand = Clamp(rcCommand, -1, 1);
        float absRcCommand = Math.Abs(rcCommand);

        if (rcRate > 2.0f)
        {
            rcRate = rcRate + (14.54f * (rcRate - 2.0f));
        }

        if (expo != 0)
        {
            rcCommand = rcCommand * Mathf.Pow(Mathf.Abs(rcCommand), 3f) * expo + rcCommand * (1.0f - expo);
        }

        float angleRate = 200.0f * rcRate * rcCommand;
        if (superRate != 0)
        {
            float rcSuperFactor = 1.0f / Clamp(1.0f - absRcCommand * superRate, 0.01f, 1.00f);
            angleRate *= rcSuperFactor;
        }

        return angleRate;
    }
}
