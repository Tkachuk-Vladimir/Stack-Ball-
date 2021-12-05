using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class UnitControl : MonoBehaviour
{
    //[SerializeField] private GameObject mainCamer;
    [SerializeField] private GameObject blot;
    private GameObject parentPlatform;
    private GameManager gameManager;
    private bool isCrеate;
    private bool isCollision;
    private Transform thisTransform;
   
   public bool IsCreate
    {
        get { return isCrеate;}
        set { isCrеate = value; }
    }
    public bool IsCollision
    {
        get { return isCollision; }
        set { isCollision = value; }
    }
    void Start()
    {
        parentPlatform = transform.parent.gameObject;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        isCollision = false;
        thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollision)
        {
            DestuctionUnti();
        }
    }
    private void DestuctionUnti()
   {
       gameObject.transform.parent = null;
       Destroy(gameObject.GetComponent<BoxCollider>());
       
       Vector3 correntPosition = transform.position;
       
       if (transform.position.x < 0)
        { 
            thisTransform.position = Vector3.MoveTowards(correntPosition,  new Vector3(-25f,  50f, 5f), 0.2f);
            thisTransform.rotation = Quaternion.Euler(new Vector3(thisTransform.rotation.eulerAngles.x + Time.deltaTime * 50f , 0 , thisTransform.rotation.eulerAngles.z +Time.deltaTime * 150f));
        }
        else
        {
            thisTransform.position = Vector3.MoveTowards(correntPosition, new Vector3(25f,  50f, 5f), 0.2f);
            thisTransform.Rotate(Time.deltaTime * 150f , 0 , Time.deltaTime * 400f);
        }
       Destroy(gameObject, 1f);
   }
   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject.CompareTag("Player"))
       {
           //Instantiate Blote
           var newBlot = Instantiate(blot, new Vector3(other.gameObject.transform.position.x,transform.position.y + 0.16f, other.gameObject.transform.position.z) , Quaternion.identity);
           newBlot.transform.parent = transform;
       }
   }
}
