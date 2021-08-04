using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRailController : MonoBehaviour
{

    [SerializeField] private Rail railStaight;
    [SerializeField] private Rail railAround;
    public bool isStaight = false;

    // Start is called before the first frame update
    void Start()
    {
        railStaight.enable = isStaight;
        railAround.enable = !isStaight;
    }

    public void SetRailStaight(bool staight)
    {
        isStaight = staight;
        railStaight.enable = staight;
        railAround.enable = !staight;
    }
}
