using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject f;
    //private MovementScript m;
    //T = GetComponent<MovementScript>();


    public kiFirebase Onliner;
    public MovementScript m;
    public Vector2 r;
    bool InMenu;
    public GameObject Exitation;
    
    public GameObject StartScreen;
    public GameObject TheExitbtn;
    public GameObject LogoScreen;
    public GameObject GameLogoScreen;
    public GameObject OptionScreen;
    public GameObject Signupscreeen;
    public GameObject Loginscreen;
    public GameObject Localscreen;
    public GameObject currentscreen;
    public GameObject nextscreen;
    public GameObject pickgamescreen;
    public GameObject creategamescreen;
    bool InGame;
    public static bool started = false;

    private bool onlineReady = true;
    public Dropdown playerno;

    // 
    public GameObject LoadingScreen;
    public Slider slider;
    //

    //Rotatable buttons
    public Image kalogo;
    public Image kalogolocal;
    public Image kalogoonline;

    //login variables
    public Text LoginText;
    public Button Signupbtn;
    public Button Loginbtn;

    //waiter variables
    public GameObject startgamebutton;
    public GameObject backgamebutton;
    public GameObject oppanel;
    public Text newgametext;

    //other player variables
    public GameObject[] playerbars;
    public Text[] theirnames;
    public Text[] theirscores;

    public bool s;
    public  IEnumerator ChangeScreen()
    {
        if (!started)
        {
            LogoScreen.SetActive(true);
            StartScreen.SetActive(false);
            yield return new WaitForSeconds(5);

            LogoScreen.SetActive(false);
            StartScreen.SetActive(true);

            StartScreen.SetActive(true);
            yield return new WaitForSeconds(5);
            StartScreen.SetActive(false);
        }
         
        GameLogoScreen.SetActive(true);
        currentscreen = GameLogoScreen;
        nextscreen = GameLogoScreen;
        // print(Time.time);
    }
   private void switchscreen()
    {
        currentscreen.SetActive(false);
        nextscreen.SetActive(true);
        currentscreen = nextscreen;
    }
    public void ExitGameplay()
    {      
        Exitation.SetActive(true);        
        StartScreen.SetActive(false);
      //  InGame = false;
    }
    public void ConfirmExit()
    {
        if(currentscreen == GameLogoScreen)
        {
            Application.Quit();
        }
        else
        {
            nextscreen = GameLogoScreen;
            switchscreen();
        }
    }
    public void DenyExit()
    {
        nextscreen = currentscreen;
        switchscreen();
    }
    //Start is called before the first frame update
    void Start()
    {
       // LogoScreen.SetActive(true);
        Exitation.SetActive(false);
        StartScreen.SetActive(false);
        started = true;
        StartCoroutine(ChangeScreen());
        started = true;
        InGame = false;
        InMenu = true;
        Exitation.SetActive(false);
        Onliner = FindObjectOfType<kiFirebase>();      
    }
    public void homescreen()
    {
        startgamebutton.gameObject.SetActive(true);
        backgamebutton.gameObject.SetActive(true);
        oppanel.SetActive(false);
        nextscreen = GameLogoScreen;
        switchscreen();
    }
    public void GoLocal()
    {
        nextscreen = Localscreen;
        kalogo = kalogolocal;
        switchscreen();
    }
    public void GoOnline()
    {
        startgamebutton.gameObject.SetActive(false);
        backgamebutton.gameObject.SetActive(false);
        if (Onliner.SignedIN)
        {
            nextscreen = pickgamescreen;
        }
        else
        {
            nextscreen = Loginscreen;
        }
        kalogo = kalogoonline;
        switchscreen();
    }
    public void StartNewGame()
    {
        nextscreen = LoadingScreen;
        switchscreen();
        if (Onliner.GameSet)
        {
            Loadlevel(2);
            Onliner.startsesh();
        }
        else
        {
            Loadlevel(1);
        }
    }
    public void QuittingGame()
    {
        currentscreen.SetActive(false);
        Exitation.SetActive(true);
    }
   public void PickAGame()
    {   /*
        nextscreen = pickgamescreen;
        switchscreen();
        */
        Loginbtn.interactable = false;
        Onliner.PressSignIn();
        StartCoroutine(GetRelevantValues());
       // Debug.Log("PickaGame");
    }
    public void createGame()
    {
        nextscreen = creategamescreen;
        switchscreen();
    }
    public void OnlineBack()
    {
        nextscreen = pickgamescreen;
        switchscreen();
    }
    public void SigningUP()
    {
        nextscreen = Signupscreeen;
        Onliner.SignedIN = false;
        switchscreen();
    }
    public void Loadlevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
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
    void RotateButton(Image ToBeRotated)
    {
        ToBeRotated.transform.Rotate(0, 0, -3.5f);       
    }
    IEnumerator GetRelevantValues()
    {
        Debug.Log("Looking For User");
        LoginText.text = "Signing in...";
        Signupbtn.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        Onliner.calG();
        yield return new WaitForSeconds(3);
        if (Onliner.SignedIN)
        {
            LoginText.text = "Sign in successful...";
            nextscreen = pickgamescreen;
            switchscreen();
        }
        else
        {
            Debug.Log("Bado");
            LoginText.text = "Invalid Login,Try Again or ";
            Loginbtn.interactable = true;
            Signupbtn.gameObject.SetActive(true);
        }
        Debug.Log("8 Seconds Later");
    }
    
    IEnumerator waitforplayers()
    {
        Onliner.loadplayers();
        newgametext.text = "WAITING FOR PLAYERS...";
        yield return new WaitForSeconds(3f);
        oppanel.SetActive(true);
        while (!s)
        {
            Debug.Log("filling list");
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < Onliner.othernames.Length; i++)
            {
                playerbars[i].SetActive(true);
                theirnames[i].text = Onliner.othernames[i];
            }
            if (Onliner.remainingplayers == 0)
            {
                yield return new WaitForSeconds(1f);
                s = true;
            }
        }
        for (int i = 0; i < Onliner.othernames.Length; i++)
        {
            playerbars[i].SetActive(true);
            theirnames[i].text = Onliner.othernames[i];
        }
        newgametext.text = "START";
        startgamebutton.gameObject.SetActive(true);
    }
    
    public void WaitforOthers()
    {
        StartCoroutine(waitforplayers());
    }
    void Update()
    {
        RotateButton(kalogo);
        if(onlineReady)
        {
            if(Onliner.GameSet)
            {
                onlineReady = false;
                GoLocal();
                WaitforOthers();
            }
        }
    }
}
