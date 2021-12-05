using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlotControl : MonoBehaviour
{
    private SpriteRenderer blotImageObject;
    private float newAlpha;
    private float liveTime = 6f;
    private float timer;
    void Start()
    {
        blotImageObject = GetComponentInChildren<SpriteRenderer>();
        timer = liveTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
            return;
        }
        blotImageObject.color =  new Color(blotImageObject.color.r, blotImageObject.color.g, blotImageObject.color.b, timer/ liveTime);
         
    }
}
