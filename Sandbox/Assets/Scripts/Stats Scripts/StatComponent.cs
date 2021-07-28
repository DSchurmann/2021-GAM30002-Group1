using System;
using UnityEngine;

[Serializable]
public class StatComponent : MonoBehaviour
{
    [SerializeField] private StatType type;
    private float currentValue;
    [SerializeField] private float maxValue;

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
}

public enum StatType 
{ 
    health,
    damage,
}