using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentFail : MonoBehaviour
{
    [SerializeField] private Vector3 failVector;
    [SerializeField] private float modulForce;
    private Rigidbody fragmentRb;
    
    private void Awake()
    {
        fragmentRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
       fragmentRb.AddForce(failVector * modulForce, ForceMode.Impulse); 
    }
    
    void Update()
    {
        Destroy(gameObject, 1f);
    }
}
