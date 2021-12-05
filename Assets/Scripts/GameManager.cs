using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
//using Random = System.Random;

public class GameManager : MonoBehaviour
{
    
    // [SerializeField] GameObject platform;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject environment;
    [SerializeField] private GameObject tapToPlayAnimation;
    [SerializeField] private GameObject failWindow;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject[] units;
    [SerializeField] private int valuePlatforms;
    [SerializeField] private float distanceBetweenPlatforms;
    [SerializeField] private Vector3 posInstantiatePlatform;
    [SerializeField] private Vector3 posMake;
    [SerializeField] int stepSpawn;
    [SerializeField] private float speedRotateEnvironment ;
    [SerializeField] private TextMeshProUGUI volueDestroyPlatformsText;
    [SerializeField] private Text curentScoreText; 
    [SerializeField] private Slider levelSlider;
    [SerializeField] private int volueDestroyPlatforms;
    [SerializeField] private Animator platformAnimator;
    [SerializeField] private Vector3 playerStartPosition;
    private GameObject platforma;
    private Material unitMaterial;
    int indexRandomAlgorithm ;
    private bool isFail;
    private bool isWin;
    
    private bool isTouch;
    private bool isTouchOne;

    private int[,] algorithmUnitSet = new int[8, 8]
    {
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 1, 1, 1, 1},
        {0, 1, 1, 1, 1, 1, 1, 1},
        {0, 0, 1, 1, 0, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0},
        {0, 1, 0, 1, 0, 1, 0, 1},
        {1, 1, 0, 0, 0, 0, 0, 0},
        {0, 0, 1, 1, 1, 1, 1, 1}
    };
                                                     
                                                     
    public bool IsTouch
    {
        get { return isTouch; }
        set { isTouch = value; }
    }                                                 

    public bool IsFail
    {
        get { return isFail; }
        set { isFail = value; }
    }

    public bool IsWin
    {
        get { return isWin; }
        set { isWin = value;}
    }

    public int VolueDestroyPlatforms
    {
        get { return volueDestroyPlatforms; }
        set { volueDestroyPlatforms = value; }
    }
    void Awake()
    {
        isFail = false;
        isWin = false;
        MakePlatforms(valuePlatforms,stepSpawn);
    }
    private void Start()
    { 
        //Time.timeScale = 0.1f;
        playerStartPosition = player.transform.position;
        VolueDestroyPlatforms = 0;
    }

    void Update()
    {
        //Logic
        FailGame(IsFail);
        WinGame(IsWin);
        RemoveBoxColliderOfAllPlatform(isTouch);
        
        //Movament
        transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y + Time.deltaTime * 60, 0));// Rotate all platform
        environment.transform.Rotate(0, Time.deltaTime * speedRotateEnvironment, 0); //Rotate Environment
        
        //UI
        UIlevelSlider(volueDestroyPlatforms);
        curentScoreText.text = volueDestroyPlatforms.ToString();            // Score text
        volueDestroyPlatformsText.text = volueDestroyPlatforms.ToString();
        
        //if tap, "tap to play" imag set active off
        if (Input.GetMouseButtonDown(0))
        {
            tapToPlayAnimation.SetActive(false); // this it start text
        }
    }

    private void UIlevelSlider(int volueDestroyPlatforms)
    {
        levelSlider.value = volueDestroyPlatforms;
    }
    private void MakePlatforms(int value, int stepSpawn)
    {
        Application.targetFrameRate = 60;
        int angleOfRotation = Mathf.RoundToInt(360 / stepSpawn); // from make the units
        int angleOfRotationPlatform = 2; // rotat every new platform
        int currentNumberOfRepetitionsAlgorithm = 0;
        int numberOfRepetitionsAlgorithm = UnityEngine.Random.Range(0, 26);
        float color = 1f;
        indexRandomAlgorithm = UnityEngine.Random.Range(0, 8);
        
        //make the Platform
        for (int i = 0; i < value; i++)
        {
            platforma = new GameObject("Platforma_" + i);
            platforma.AddComponent<PlatformControl>();
            platforma.AddComponent<BoxCollider>().size = new Vector3(3.8f, 0.3f, 3.8f);
            platforma.GetComponent<BoxCollider>().isTrigger = true;
            platforma.transform.tag = "Platforma";
            platforma.AddComponent<Animator>();
            platforma.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animation/Platform/Platform")as RuntimeAnimatorController; // add Animator Controller Platform
            platforma.transform.position = posInstantiatePlatform;
            platforma.transform.rotation = Quaternion.Euler(new Vector3(0, -angleOfRotationPlatform * i, 0));
            platforma.transform.parent = transform;
            posMake.z = posMake.z + Mathf.Cos(i * 0.15f) * 0.02f;

            if (currentNumberOfRepetitionsAlgorithm > numberOfRepetitionsAlgorithm)
            {
                indexRandomAlgorithm = UnityEngine.Random.Range(0, 8);
                numberOfRepetitionsAlgorithm = UnityEngine.Random.Range(0, 21);
                currentNumberOfRepetitionsAlgorithm = 0;
            }
            
            //make the Units inside the Platform
             for (int j = 0; j < stepSpawn; j++)
             {
                    GameObject newUnit = Instantiate(units[algorithmUnitSet[indexRandomAlgorithm,j]]); // create new Unit
                    
                    newUnit.transform.localScale = new Vector3( newUnit.transform.localScale.x + Mathf.Sin( i * 0.15f ) * 0.05f ,newUnit.transform.localScale.y,  newUnit.transform.localScale.z + Mathf.Sin( i * 0.15f ) * 0.05f);// change scale
                    newUnit.transform.parent = platforma.transform; // make how child object
                    newUnit.transform.localPosition = new Vector3(posMake.x ,posMake.y ,posMake.z /* + nScail */);
                    newUnit.transform.localRotation = Quaternion.Euler(new Vector3(0, angleOfRotation * j, 0));
                    posMake = Quaternion.Euler(0, 45, 0) * posMake; // rotation posMake Vector;
                     
                     //cheange Color
                     if (newUnit.transform.tag == "TrueUnit")
                     {
                         newUnit.GetComponent<Renderer>().material.color = new Color(color,0.8f,0.1f,1f); //cheange color Unit
                     }
             }
             currentNumberOfRepetitionsAlgorithm++;
             color = color - 0.027f; //change color
             posInstantiatePlatform.y -= distanceBetweenPlatforms; // step down
        }
        posInstantiatePlatform =  new Vector3(0, 42f, 0);
    }

    private void AllBadUnitSwitchTriggerFalse()
    {
        // all BadUnit isTrigger = false
        for (int i = 0; i < transform.childCount; i++)// Disable BoxCollider at a platform and a units
        {
            for (int j = 0; j < gameObject.transform.GetChild(i).childCount; j++ )
            {
                if (gameObject.transform.GetChild(i).transform.GetChild(j).CompareTag("BadUnit"))
                {
                    gameObject.transform.GetChild(i).transform.GetChild(j).GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }
    }
    private void DeleteAllPlatforms()
    {
        
        for (int i = 0; i < transform.childCount; i++)
       
            Destroy(gameObject.transform.GetChild(i).gameObject);
          //  gameObject.transform.GetChild(i).gameObject.transform.parent = null;
            
       
        //Debug.Log(transform.childCount);
       
       
         foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            //child.gameObject.transform.parent = null;
        }
         Debug.Log(transform.childCount);
       
    }
   
    public void FailGame(bool isFail)
    {
        if (isFail)
        {
            failWindow.SetActive(true);

            AllBadUnitSwitchTriggerFalse();


            if (Input.GetMouseButtonDown(0))
            {
                RestartLevel();

                /*
                  DeleteAllPlatforms();
                 
               // Debug.Log(transform.childCount);
                //MakePlatforms(valuePlatforms,stepSpawn);
                
                 
                //UI
                failWindow.SetActive(false);
                VolueDestroyPlatforms = 0;
                
                player.transform.position = playerStartPosition;
                player.GetComponent<PlayerControl>().SetStartSettings();
                player.SetActive(true);
                
                camera.GetComponent<CameraFollow>().SetCameraPosition();
               
                IsFail = false;
                //camera.transform.position = new Vector3(camera.transform.position.x, posInstantiatePlatform.y + camera.GetComponent<CameraFollow>().Offset, camera.transform.position.z);
                
                
                //if (transform.childCount > 0)
                //{
                    // get position top platform 
                   //var  topPlatform = transform.GetChild(0).GetComponent<Transform>();
                    
                   //camera.transform.position = new Vector3(camera.transform.position.x, topPlatform.transform.position.y + 2f,camera.transform.position.z);
               // }  
                //

                //RestartLevel();
                */
            }
        }
    }
        
    public void WinGame(bool isWin)
    {
        if (isWin)
        {
            winWindow.SetActive(true);
           
            if (Input.GetMouseButtonDown(0))
            {
                RestartLevel();
            } 
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RemoveBoxColliderOfAllPlatform(bool isTouch)
    {
        if (isTouch)
        {
            isTouchOne = true;
            
            for (int i = 0; i < transform.childCount; i++)// Disable BoxCollider at a platform and a units
            {
                //gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false; // Disable BoxCollider at the platform
                Destroy(gameObject.transform.GetChild(i).GetComponent<BoxCollider>());// Disable BoxCollider at the platform
                
                for (int j = 0; j < gameObject.transform.GetChild(i).childCount; j++ )
                {
                    if (gameObject.transform.GetChild(i).transform.GetChild(j).CompareTag("TrueUnit"))
                    {
                        Destroy(gameObject.transform.GetChild(i).transform.GetChild(j).GetComponent<BoxCollider>());
                        //gameObject.transform.GetChild(i).transform.GetChild(j).GetComponent<BoxCollider>().enabled = false; // Disable BoxCollider at each the Unit
                    }
                }
            }
        }
        else
        {
            if (isTouchOne)
            {
                for (int i = 0; i < transform.childCount; i++)// Disable BoxCollider at a platform and a units
                {
                    //gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true; // Disable BoxCollider at the platform
                    gameObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>().size = new Vector3(3.8f, 0.3f, 3.8f);
                    gameObject.transform.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;

                    for (int j = 0; j < gameObject.transform.GetChild(i).childCount; j++)
                    { 
                        if (gameObject.transform.GetChild(i).transform.GetChild(j).CompareTag("TrueUnit"))
                        { 
                            gameObject.transform.GetChild(i).transform.GetChild(j).gameObject.AddComponent<BoxCollider>().size = new Vector3(0.93f, 0.3f, 0.75f); // Disable BoxCollider at each the Unit
                            gameObject.transform.GetChild(i).transform.GetChild(j).GetComponent<BoxCollider>().isTrigger = true; // Disable BoxCollider at each the Unit
                        }
                    }
                    if ( i == transform.childCount - 1)
                    {
                        isTouchOne = false;
                    }
                    
                } // isTouchOne = false;
           }
        }
    }
}
 