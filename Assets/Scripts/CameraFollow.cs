using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform finisch;
    [SerializeField] private float smoothing;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 camPos;
    private Transform topPlatform;

    public float Offset
    {
        get { return offset; }
        
    }
    private void Start()
    {
        //transform.position.z = -7.5;
    }

    void Update()
    {
        //SetCameraPosition();
        LerpCameraPosition();

        // stop before finish
        /* if (transform.position.y - finisch.transform.position.y > 4.5f)
         {
              Vector3 targetCamPos = new Vector3(transform.position.x, minY + offset, transform.position.z);
             transform.position = Vector3.Lerp(transform.position, transform.position, smoothing * Time.deltaTime);
         }
         */
    }

    public void LerpCameraPosition()
    {
        if (gameManager.transform.childCount > 0)
        {
            //Debug.Log("faind platdorm");
            // get position top platform 
            topPlatform = gameManager.transform.GetChild(0).GetComponent<Transform>(); 
          
            if (transform.position.y > playerTarget.position.y && transform.position.y > topPlatform.position.y + offset)
            {
                camPos = new Vector3(transform.position.x, topPlatform.transform.position.y + offset, transform.position.z);
            }
            
            transform.position = Vector3.Lerp(transform.position, camPos , smoothing * Time.deltaTime);
        }
       
    }

    public void SetCameraPosition()
    {
        topPlatform = null;
        topPlatform = gameManager.transform.GetChild(0).GetComponent<Transform>();
        /*
        if (gameManager.transform.childCount > 0)
        {
            // get position top platform 
            topPlatform = gameManager.transform.GetChild(0).GetComponent<Transform>();
            camPos = new Vector3(transform.position.x, topPlatform.transform.position.y + offset, transform.position.z);
            //Debug.Log(camPos);
            
            //transform.position = Vector3.Lerp(transform.position, camPos , 1000 * Time.deltaTime);
            transform.position = camPos;
            
        }  
        */
        transform.position = new Vector3(transform.position.x, 44f, transform.position.z);
    }
    
}
