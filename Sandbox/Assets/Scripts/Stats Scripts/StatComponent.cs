using System;
using UnityEngine;

[Serializable]
public class StatComponent : MonoBehaviour
{
    private float currentValue;
    private int maxValue;

    public void Initialize()
    {
        currentValue = maxValue;
    }

    public void MinusValue(float v)
    {
        currentValue -= v;
        ClampValue();
    }

    public void AddValue(float v)
    {
        currentValue += v;
        ClampValue();
    }

    private float ClampValue()
    {
        float result;
        if (currentValue < 0)
        {
            result = 0;
        }
        else if (currentValue >= maxValue)
        {
            result = maxValue;
        }
        else
        {
            result = currentValue;
        }
        return result;
    }

    public int Max
    {
        get { return maxValue; }
        set { maxValue = value; }
    }
}