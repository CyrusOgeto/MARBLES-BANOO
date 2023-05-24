using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TheARManager : MonoBehaviour
{
    private PlacerScript theplacer;
    private ObjectSpawner thespawner;
    private PlaneToggler thetoggler;    
    public Text[] theirscorevalue;
    public GameObject[] scoreuis;
    public GameObject ingamescreen;
    public GameObject pauseScreen;
    public GameObject exitscreen;
    
    public Camera usercamera;
    public Vector3 theface;
    public Text cameraval;
    public Text aballval;
    //Loader Variables 
    public GameObject LoadingScreen;
    public Slider slider;
    //End of Loader variables

    //Timer text
    public Text theTimer;
    //

    //Alert text
    public Text AlertText;

    public bool pc;
    public Vector3 q;
    private ARSessionOrigin io;

    public bool amepause;
    public int[] savedscores;

    //The Line Renderer values
    public LineRenderer laini;

    void Start()
    {
        AlertText.text = "";
        //ingamescreen.SetActive(true);
        amepause = false;
        pauseScreen.SetActive(false);
        exitscreen.SetActive(false); 
        io = FindObjectOfType<ARSessionOrigin>();
       
        theplacer = FindObjectOfType<PlacerScript>();
        thespawner  = FindObjectOfType<ObjectSpawner>();
       // thetoggler = io.GetComponent<PlaneToggler>();          
    }
       // Update is called once per frame
    void Update()
    {
       
       theface = usercamera.transform.forward;
       
       // q = io.transform.forward;
         
       // cameraval.text = q.ToString();
       /// aballval.tex.t = theface.ToString();
        //the code you added for AR purposes


        //end of AR code
           /*
       if (thespawner.itsthere == true)
       {
           thetoggler.SetAllPlanesActive(false);            
       }  
       */
    }
    public void Gamepause()
    {
        //  thespawner.DestroyPitch();
        amepause = true;
        io.enabled = false;
        ingamescreen.SetActive(false);
        pauseScreen.SetActive(true);
        exitscreen.SetActive(false);
    }
    public void ResumeGame()
    {
        amepause = false;
        io.enabled = true;
        thespawner.itsthere = false;
        ingamescreen.SetActive(true);
        pauseScreen.SetActive(false);
        exitscreen.SetActive(false);
    }
    public void pcGamepause()
    {
        //thespawner.DestroyPitch();

        pauseScreen.SetActive(true);
        exitscreen.SetActive(false);
    }
    public void pcResumeGame()
    {
        ingamescreen.SetActive(true);
        pauseScreen.SetActive(false);
        exitscreen.SetActive(false);
    }
    public void LeaveGame()
    {
        ingamescreen.SetActive(false);
        pauseScreen.SetActive(false);
        exitscreen.SetActive(true);
    }
    public void ConfirmLeave()
    {
        //SceneManager.LoadScene("MainMenuScene");
        ingamescreen.SetActive(false);
        pauseScreen.SetActive(false);
        exitscreen.SetActive(false);
        Loadlevel(0);
    }
    void startUI()
    {
        exitscreen.SetActive(false);
        ingamescreen.SetActive(true);
        pauseScreen.SetActive(false);
    }
    void OnlineUI()
    {
        exitscreen.SetActive(false);
       //OnlineGameScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }
    void Loadlevel(int sceneIndex)
    {
       StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    public void remscore(int thekey,int thescore)
    {
        savedscores[thekey] = thescore; 
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        LoadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            slider.value = progress;
            // progressText.text = progress * 100f + "%";
            yield return null;
            //This waits for the next frame before continuing 
        }
    }
}
