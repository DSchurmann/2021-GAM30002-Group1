using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LightTexture : MonoBehaviour
{
    public Transform spotlight;

    private bool materialSet;

    public float range;
    // Start is called before the first frame update
    void Start()
    {
        AddHiddenMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (spotlight != null)
        {
            SetShaderProperties();
        }
    }
    

    // add HiddenMaterial 
    public void AddHiddenMaterial()
    {
        /*Material[] combo = new Material[CurrentMaterial.Length + 1];
        combo[0] = CurrentMaterial[0];
        combo[combo.Length - 1] = HiddenMaterial;
        gameObject.GetComponent<Renderer>().sharedMaterials = combo;*/
    }
    public void RemoveHiddenMaterial()
    {
       /* Material[] combo = new Material[CurrentMaterial.Length];
        combo[0] = CurrentMaterial[0];
        gameObject.GetComponent<Renderer>().sharedMaterials = CurrentMaterial;*/
    }

    // set material properties
    public void SetMaterialProperties(Color _color)
    {
        Material[] m = GetComponent<Renderer>().sharedMaterials; 
        if(m[1]!=null)
            m[1].color = _color;
    }

    // set shader properties
    private void SetShaderProperties()
    {
        if (spotlight)
        {
            GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_SpotAngle", spotlight.GetComponent<Light>().spotAngle);
            GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_Range", spotlight.GetComponent<Light>().range);
            GetComponent<Renderer>().sharedMaterials[1]?.SetVector("_LightPos", spotlight.position);
            GetComponent<Renderer>().sharedMaterials[1]?.SetVector("_LightDir", spotlight.forward);
        }

    }
}
