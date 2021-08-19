using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private Health hp;
    [SerializeField] private Damage dmg;

    private void Start()
    {
        if(hp != null)
        {
            hp.Initialize();
        }
        if(dmg != null)
        {
            dmg.Initialize();
        }
    }

    public void AddHealth()
    {
        hp = gameObject.AddComponent<Health>();
    }

    public void RemoveHealth()
    {
        if (hp != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(GetComponent<Health>());
#else
            Destroy(GetComponent<Health>());
#endif
            hp = null;
        }
    }

    public void AddDamage()
    {
        dmg = gameObject.AddComponent<Damage>();
    }

    public void RemoveDamage()
    {
        if (dmg != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(GetComponent<Damage>());
#else
            Destroy(GetComponent<Damage>());
#endif
            dmg = null;
        }
    }

    public Health Health
    {
        get { return hp; }
        set { hp = value; }
    }
    public Damage Damage
    {
        get { return dmg; }
        set { dmg = value; }
    }
}