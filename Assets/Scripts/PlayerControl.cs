using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float repulsiveForce;
    [SerializeField] private float failSpeed;
    [SerializeField] private float gravityModifier;
    [SerializeField] private bool touch;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject fragments;
    [SerializeField] private ParticleSystem transformPS;
    [SerializeField] private ParticleSystem firePS;
    [SerializeField] private ParticleSystem flashingPS;
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private ParticleSystem ExplosionPS;    
    [SerializeField] private Slider deathleesSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform finisch;
    private bool isClick;
    private bool firstCollisionBad;
    private bool isDeathless;
    private int countUnitCollision;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private MeshRenderer playerMr;
    private float touchTimer = 0;
    private float valueSlider = 0;
    private Transform topPlatform;

    private float timeSmooth;
    private float startLocalScaleX;
    private float startLocalScaleY;

    public bool IsDeathless
    {
        get { return isDeathless; }
        set { isDeathless = value;}
    }
    
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
       // playerAnim = GetComponent<Animator>();
       // ExplosionPS = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void Start()
    {
        SetStartSettings();
        
        /*
        //ParticleSystem
        flashingPS.Stop();
        firePS.gameObject.SetActive(false);
        // transformPS.gameObject.SetActive(true);
       
        //UI
        exclamationMark.SetActive(false);
        
        //Physics.gravity = new Vector2(0,-9.8f); 
        //Physics.gravity *= gravityModifier;
        
        firstCollisionBad = false;
        isClick = false;
        isDeathless = false;
        touch = false;
        
        timeSmooth = 0;
        startLocalScaleX = transform.localScale.x;
        startLocalScaleY = transform.localScale.y;
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        DeathlessControll(touch);
       
        if (Input.GetMouseButtonDown(0)){touch = true;}

        if (Input.GetMouseButtonUp(0))
        {
            touch = false;
            isClick = true;
        }

        if (Input.GetMouseButton(0))
        {
            touch = true;
        }
        else
        {
            touch = false;
        }
        
        
        if (!gameManager.IsFail  )
        { 
            if (!gameManager.IsWin)
            {
                if (touch)
                {
                    if (transform.position.y > 1f)
                    {
                        playerRb.velocity = Vector3.up * -failSpeed ;
                        //playerAnim.SetBool("Touch", true);
                        
                        //Chenge LocalScale
                        transform.localScale = new Vector3( startLocalScaleX ,0.53f, transform.localScale.z );
                        
                         gameManager.IsTouch = true;
                     
                        timeSmooth = 0;
                    }
                }
                else
                {
                   // playerAnim.SetBool("Touch", false);
                    
                    if (!isClick)
                    {
                        if (gameManager.transform.childCount > 0)
                        { 
                           // get position top platform
                           topPlatform = gameManager.transform.GetChild(0).GetComponent<Transform>();
                           
                           // Chenge LocalScale
                           //transform.localScale = new Vector3( startLocalScaleX + Mathf.Cos( timeSmooth * 15f) * 0.05f,startLocalScaleY + Mathf.Sin( timeSmooth * 11f) * 0.08f, transform.localScale.z );
                           transform.localScale = new Vector3( startLocalScaleX + Mathf.Sin( timeSmooth * 14f) * 0.05f,startLocalScaleY + Mathf.Cos( timeSmooth * 17f) * 0.09f, transform.localScale.z );
                           
                            // Chenge Position
                           transform.position = new Vector3(transform.position.x,topPlatform.transform.position.y + 0.15f + Mathf.Sin( timeSmooth * 6.25f ) * 1.5f  ,transform.position.z);// transform.position = new Vector3(transform.position.x,topPlatform.transform.position.y + 0.95f + Mathf.Sin( Time.time  * 11f ) * 0.7f  ,transform.position.z);
                         
                           timeSmooth += Time.deltaTime;
                           
                           if (timeSmooth > 0.5f)
                           {
                               timeSmooth = 0.0f;
                           }
                        }
                    }
                   

                  //  playerAnim.SetBool("Touch", false);
                    gameManager.IsTouch = false;
                   // gameObject.AddComponent<Rigidbody>();
                   //playerRb
                }
            }
            else
            {
                //if win
                deathleesSlider.gameObject.SetActive(false); // enable dethless
                exclamationMark.SetActive(false);
                firePS.gameObject.SetActive(false);
                transformPS.gameObject.SetActive(true);
                
                //Chenge LocalScale
                transform.localScale = new Vector3( startLocalScaleX + Mathf.Cos( timeSmooth * 13f) * 0.05f,startLocalScaleY + Mathf.Sin( timeSmooth * 11f) * 0.08f, transform.localScale.z );
                //Chenge Positio
                transform.position = new Vector3(transform.position.x,finisch.transform.position.y + 0.15f + Mathf.Sin( timeSmooth * 6f ) * 1.5f  ,transform.position.z);
               
                timeSmooth += Time.deltaTime;
                if (timeSmooth > 0.5f)
                {
                    timeSmooth = 0.0f;
                }
               
                //if win and if touch
                if (Input.GetMouseButtonDown(0))
                {
                    gameManager.RestartLevel();
                } 
            }
        }
        else
        {
            //if Fail
            deathleesSlider.gameObject.SetActive(false); // enable dethless Slider
            exclamationMark.SetActive(false); // enable image Exclamation mark
            gameObject.SetActive(false); // Enable Player
            Instantiate(fragments, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z - 0.2f) , Quaternion.identity);// instantiate fragments
        }
        
        if (transform.position.y < 0.5f)
        {
            gameManager.IsWin = true;
        }
    }
    
    private void OnTriggerEnter( Collider other)
    {
        if (touch)
        { 
            // if collision thr TrueUnit
            if (other.gameObject.CompareTag("TrueUnit") )
            {
                other.transform.parent.gameObject.GetComponent<PlatformControl>().IsDestroy = true; // destory platform  -> units
                firstCollisionBad = true;
                //other.gameObject.GetComponent<BoxCollider>().enabled = false; // enabled BoxCollider after Platform
                //gameManager.VolueDestroyPlatforms++;
            }
            // if collision thr BadUnit
            if (other.gameObject.CompareTag("BadUnit"))
            {
                if (isDeathless)
                {
                    //gameManager.VolueDestroyPlatforms++;
                    other.transform.parent.gameObject.GetComponent<PlatformControl>().IsDestroy = true; // destory platform  -> units
                }
                else
                {
                    other.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("swipSize"); // trig animation chenge size platform
                    gameManager.IsFail = true;
                }
            }
        }
        else
        {
            // check first collision platform
            if (other.gameObject.CompareTag("TrueUnit"))
            {
               // firstCollisionBad = false;
               
                // if you just clicked
                if (isClick)  
                {
                    other.transform.parent.gameObject.GetComponent<PlatformControl>().IsDestroy = true; // destory units -> platform
                    isClick = false;
                    //gameManager.VolueDestroyPlatforms++;
                    //playerRb.velocity = Vector3.up * repulsiveForce; // the player jump to the top
                   // firstCollisionBad = true;
                   
                }
            }

            if (other.gameObject.CompareTag("BadUnit"))
            {
                isClick = false;
                /* if (firstCollisionBad)
                 {
                     other.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("swipSize");
                     firstCollisionBad = false;
                 }
                */
            }

            if (other.gameObject.CompareTag("Platforma"))
            {
                // playerRb.velocity = Vector3.up * repulsiveForce; // the player jump 
               // playerAnim.SetTrigger("isCollision");
              //  ExplosionPS.transform.position = gameObject.transform.position;
              //  ExplosionPS.Play();
            }
        }
        
        //if collision finisch
        if (other.gameObject.CompareTag("Finisch"))
        {
            playerRb.velocity = Vector3.up * repulsiveForce; // the player jump 
            other.gameObject.GetComponent<Animator>().SetBool("swipSizeFinischBool", true); // on animation swipe
            gameManager.IsWin = true;
           // ExplosionPS.Play();
           // playerAnim.SetTrigger("isCollision");
        }
    }

    public void SetStartSettings()
    {
        // get position top platform
        topPlatform = gameManager.transform.GetChild(0).GetComponent<Transform>();
        
        //ParticleSystem
        flashingPS.Stop();
        firePS.gameObject.SetActive(false);
        // transformPS.gameObject.SetActive(true);
       
        //UI
        exclamationMark.SetActive(false);
        
        firstCollisionBad = false;
        isClick = false;
        isDeathless = false;
        touch = false;
        
        timeSmooth = 0;
        startLocalScaleX = transform.localScale.x;
        startLocalScaleY = transform.localScale.y;
    }

    private void DeathlessControll(bool touch)
    {
        if(touch)
        {
            if (deathleesSlider.gameObject.activeSelf)
            {
                if (isDeathless)
                {
                    valueSlider -= Time.deltaTime * 0.7f;
                    deathleesSlider.value = valueSlider;

                    if (valueSlider <= 1)
                    {
                        // on blink !
                        exclamationMark.SetActive(true);
                    }
                    if (valueSlider <= 0)
                    {
                        deathleesSlider.gameObject.SetActive(false); // active deathSlider
                        isDeathless = false;
                        touchTimer = 0;
                        firePS.gameObject.SetActive(false);
                        //transformPS.gameObject.SetActive(true);
                        exclamationMark.SetActive(false);
                    }
                }
                else
                {
                    valueSlider += Time.deltaTime * 1.5f ;
                    deathleesSlider.value = valueSlider;

                    if (valueSlider >= 2)
                    {
                         fillImage.color = Color.red;
                         isDeathless = true;
                        // mast active fire patical sistem
                        firePS.gameObject.SetActive(true);
                        //transformPS.gameObject.SetActive(false);
                        flashingPS.Play();
                        // mast add Sound
                    }
                }
            }
            else
            {
                touchTimer += Time.deltaTime;
                //Debug.Log(touchTimer);
                
                if (touchTimer >= 0.45f)
                {
                    deathleesSlider.gameObject.SetActive(true); // active deathSlider
                    fillImage.color = Color.white;
                    touchTimer = 0;
                }
            }
        }
        else
        {
            if (deathleesSlider.gameObject.activeSelf)
            {
                if (isDeathless)
                {
                    valueSlider -= Time.deltaTime * 0.7f;
                    deathleesSlider.value = valueSlider;
                
                    if (valueSlider <= 1)
                    {
                        // on blink !
                        exclamationMark.SetActive(true);
                    }
                    if (valueSlider <= 0)
                    {
                        deathleesSlider.gameObject.SetActive(false); // active deathSlider
                        isDeathless = false;
                        touchTimer = 0;
                        firePS.gameObject.SetActive(false);
                        //transformPS.gameObject.SetActive(true);
                        exclamationMark.SetActive(false);
                    }
                }
                else
                {
                    valueSlider -= Time.deltaTime * 0.7f;
                    deathleesSlider.value = valueSlider;
                    
                    if (valueSlider <= 0)
                    {
                        deathleesSlider.gameObject.SetActive(false); // active deathSlider
                        isDeathless = false;
                        touchTimer = 0;
                        firePS.gameObject.SetActive(false);
                       // transformPS.gameObject.SetActive(true);
                        exclamationMark.SetActive(false);
                    }
                }
            }
        }
    }
    
}