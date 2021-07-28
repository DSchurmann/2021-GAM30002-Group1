using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform meleeWeapon;
    private Collider meleeWeaponCollider;


    // Start is called before the first frame update
    void Start()
    {
        meleeWeaponCollider = meleeWeapon.GetComponent<Collider>();
        //meleeWeaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitEnemy()
    {

    }

    public void Animation_EnableMeleeWeaponHit()
    {
        //Debug.Log("HIT WITH WEAPON ENABLED");
        meleeWeaponCollider.enabled = true;
    }

    public void Animation_DisableMeleeWeaponHit()
    {
        //Debug.Log("HIT WITH WEAPON DISABLED");
        meleeWeaponCollider.enabled = false;
    }
}
