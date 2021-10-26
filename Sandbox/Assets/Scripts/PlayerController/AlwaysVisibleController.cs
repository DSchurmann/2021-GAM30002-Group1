using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysVisibleController : MonoBehaviour
{

    [SerializeField] private bool isAlwaysVisible = true;

    private List<Material> materials;

    private const string isActivePropertyName = "_IsAVActive";

    // Start is called before the first frame update
    void Start()
    {
        materials = new List<Material>();
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty(isActivePropertyName))
                {
                    materials.Add(mat);
                }
            }
        }
        AlwaysVisible = isAlwaysVisible;
    }

    public void Update()
    {
        // Match always visible to players being friends
        if(GameController.GH.IsFriend != isAlwaysVisible)
        {
            Debug.Log("Set always visible to " + GameController.GH.IsFriend);
            AlwaysVisible = GameController.GH.IsFriend;
        }
    }
    public bool AlwaysVisible
    {
        get
        {
            return isAlwaysVisible;
        }
        set
        {
            isAlwaysVisible = value;
            foreach (Material mat in materials)
            {
                mat.SetFloat(isActivePropertyName, isAlwaysVisible ? 1f : 0f);
            }
        }
    }

}
