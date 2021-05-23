using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemPartsHandler : MonoBehaviour
{
    [Header("Golem Parts")]
    public bool LeftArm;
    public bool RightArm;
    private enum GolemParts { ARMR, ARML}
    private GolemParts[] parts;
    public GameObject[] armL;
    public GameObject[] armR;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParts();
    }



    public void UpdateParts()
    {
        // left arm
        if (LeftArm )
        {
            foreach (var item in armL)
            {
                if(!item.activeSelf)
                    item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in armL)
            {
                if (item.activeSelf)
                    item.SetActive(false);
            }
        }

        // right arm
        if (RightArm)
        {
            foreach (var item in armR)
            {
                if (!item.activeSelf)
                    item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in armR)
            {
                if (item.activeSelf)
                    item.SetActive(false);
            }
        }
    }

    private void HideParts(GolemParts[] parts)
    {
        foreach (var part in parts)
        {
            switch (part)
            {
                case GolemParts.ARML:
                    foreach (var item in armL)
                    {
                        item.SetActive(false);
                    }
                    break;

                case GolemParts.ARMR:
                    foreach (var item in armR)
                    {
                        item.SetActive(false);
                    }
                    break;
            }
        }
        
    }
    private void ShowParts(GolemParts[] parts)
    {
        foreach (var part in parts)
        {
            switch (part)
            {
                case GolemParts.ARML:
                    foreach (var item in armL)
                    {
                        item.SetActive(true);
                    }
                    break;

                case GolemParts.ARMR:
                    foreach (var item in armR)
                    {
                        item.SetActive(true);
                    }
                    break;
            }
        }
    }
}
