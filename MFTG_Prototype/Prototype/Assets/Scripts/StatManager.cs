using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private List<Stat> stats;

    private void Start()
    {
        foreach (Stat s in stats)
        {
            s.Start();
        }

    }

    public void CreateStat(StatType type, float max)
    {
        stats.Add(new Stat(type, max));
    }

    public Stat GetStat(int i)
    {
        Stat result = null;
        if (i < stats.Count && i > 0)
        {
            result = stats[i];
        }
        return result;
    }
}