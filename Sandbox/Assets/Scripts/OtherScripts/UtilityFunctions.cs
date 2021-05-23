using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{

    public static Transform GetClosestByTransform(Vector3 from, Transform[] list)
    {
        Transform closest = null;
        float maxDist = float.MaxValue;


        foreach (var item in list)
        {
            float distance = Vector3.Distance(from, item.position);
            if(distance < maxDist)
            {
                closest = item;
                maxDist = distance;
            }
        }

        return closest;
    }

    public static Transform[] SortByNearestDistance(Transform[] list)
    {
        Transform closest = null;
        Transform[] sorted = null;

        return sorted;
    }
}
