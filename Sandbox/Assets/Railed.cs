using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railed : MonoBehaviour
{

    private Transform _tr;
    private Rigidbody _rb;

    public Rail _rail;

    public bool orientToPath = true;

    public int force = 0;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tr = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_rail == null)
            return;

        float minMove = 0.1f;

       // _tr.position = 

    }

}
