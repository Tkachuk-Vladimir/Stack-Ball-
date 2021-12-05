using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;


public class PlatformControl : MonoBehaviour
{
    private int speedRotatePlatform = 60;
    private GameObject grandChild;
    private int countChildren;
    private int randomUnitnamber;
    private bool isDestroy;
    private Transform thisTransform;
    private GameObject player;
    
    public bool IsDestroy
    {
        get { return isDestroy;}
        set { isDestroy = value; }
    }
    void Start()
    {
        // countChildren =  gameObject.transform.childCount; //number of children
        thisTransform = transform;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //thisTransform.Rotate(0, Time.deltaTime * speedRotatePlatform, 0); // Rotate platform
        //thisTransform.rotation = Quaternion.Euler(new Vector3(0,thisTransform.rotation.eulerAngles.y + Time.deltaTime * speedRotatePlatform, 0));
       
        if (isDestroy)
        {
            Destuction();
        }
        
        //if the platform is higher Player
        if (transform.position.y - player.transform.position.y > 0.3f)
        {
           Destuction();
        }
    }

    public void Destuction()
    {
        gameObject.GetComponentInParent<GameManager>().VolueDestroyPlatforms++;
      
        Destroy(gameObject.GetComponent<BoxCollider>());// Disable BoxCollider at the platform
        
        for (int i = 0; i < transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<UnitControl>().IsCollision = true;
        }
        //Destroy(gameObject);
         Destroy(gameObject, 0.0001f);
    }
}
  