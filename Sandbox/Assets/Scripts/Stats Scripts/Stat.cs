using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private StatType type;
    [SerializeField] private float currentValue;
    [SerializeField] private float maxValue;

    public Stat()
    {

    }

    public Stat(StatType t, float m)
    {
        type = t;
        maxValue = m;
        currentValue = m;
    }

    public void Start()
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

    public StatType Type
    {
        get { return type; }
        set { type = value; }
    }

    public float Max
    {
        get { return maxValue; }
        set { maxValue = value; }
    }
}