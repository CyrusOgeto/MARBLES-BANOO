using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.UI;

public class kiFirebase : MonoBehaviour
{ 

    //Firebase variables
    FirebaseAuth auth;
    DatabaseReference Dreference;
    FirebaseUser user;
    private bool GoLogin;
    public bool SignedIN;
    ///public GameObject OnlineNextScreen;
    public Dropdown playerno;
    public Text creatinggame; 
    public string playerpos; 
    private bool GameIDPresent = false;

    public InputField theEmail;
    public InputField LoginMail;
    public InputField LoginPass;
    public InputField theUsername;
    public InputField thepassword;
    public InputField confpassword;
    public InputField GivenGameID;
    public Button createGamebtn;
    public Button joinGamebtn;
    public Button GameCodeBtn;


    // public Text confirmation;
    public Text UserShower;
    private string KaUser;
    public string TheUserId;
    public string FoundGame;
    public GameObject gameCan;
    public GameObject AftergameCan;
    public string foundUsername;
    public Text FoundGameID;
    public bool GameSet = false;

    private static kiFirebase instance;

    //Foreigner variables
    public string movingplayer;
    public int playerAmount;
    public bool CurrentTurn;
    public bool ForeignTurn;

    public float rotator;
    public float shooter;
    public float dipper;
    //
    public int remainingplayers;
    private string[] otherplayers;
    public string[] othernames;
    // Start is called before the first frame update
    void Awake()
    {

        //gameCan.SetActive(false);
        //Object to the database
        if (instance != null)
        {
            // DontDestroyOnLoad(transform.gameObject);
            Destroy(gameObject);
        }
        else
        {
            GoLogin = false;
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        Dreference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

    }
    public void AddAUser()
    {
        string emai = theEmail.text.ToString();
        string passwor = confpassword.text.ToString();
        string ipasswor = thepassword.text.ToString();
        string jina = theUsername.text.ToString();

        if (jina == "")
        {
            Debug.Log("Weka Jina Bwana");
            return;
        }

        if (emai != "" && passwor != "")
        {
            if (ipasswor == passwor)
            {
                AddUser();
            }
            else
            {
                Debug.Log("Hizo password zinafanana kweli??");
            }
        }
        else
        {
            Debug.Log("Andika kitu bwana");
        }

    }

    //This is to add a user to firebase
    void AddUser()
    {
        string email = theEmail.text.ToString();
        string password = confpassword.text.ToString();

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Dreference.Child("Users").Child(newUser.UserId).Child("UserName").SetValueAsync(theUsername.text.ToString());
            GoLogin = true;

            //KaUser = newUser.DisplayName;
            TheUserId = newUser.UserId;


            //gameCan.SetActive(true);
            //AftergameCan.SetActive(false);
        });

    }


    //end of add function

    // Update is called once per frame
    void Update()
    {
        if (GoLogin)
        {
            hasSignedUp();
            calG();
            GoLogin = false;
        }
        /*
          if (auth.CurrentUser != user)
          {
              bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
              if (!signedIn && user != null)
              {
                  Debug.Log("Signed out " + user.UserId);
              }
              user = auth.CurrentUser;
              if (signedIn)
              {
                  Debug.Log("Signed in " + user.UserId);
              }
          }
        */
    }

    private void JoinGame()
    {
        Dreference.Child("CurrentlyOnline").Child("Gamers").Child(TheUserId).SetValueAsync(GivenGameID.text.ToString());
        Debug.Log("Added GameID");
        StartCoroutine(MoveToGame(GivenGameID.text.ToString()));
    }
    public void CheckForGame()
    {
        if(GivenGameID.text.ToString() == "")
        {
            return;
        }
        else
        {
           GameCodeBtn.gameObject.SetActive(false);
           joinGamebtn.interactable = false;
           StartCoroutine(joinAGame(GivenGameID.text.ToString()));
        }      
    }
    public void changethestring()
    {
        string h = GivenGameID.text.ToString();
        h = h.ToUpper();
        GivenGameID.text = h;
    }
    public void CreateGame()
    {
        string IdToFind = "";
        createGamebtn.interactable = false;
         if (!GameIDPresent)
         {
             Dreference.Child("CurrentlyOnline").Child("Gamers").Child(TheUserId).SetValueAsync("NoID");
             Debug.Log("Generated NoID");             
             StartCoroutine(SetUpGame());
         }
         else
         {
            Debug.Log("Game iko tayari!!");
            Dreference.Child("CurrentlyOnline").Child("Gamers").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;                    
                    IdToFind = snapshot.Child(TheUserId).Value.ToString();                   

                    if (IdToFind != "NoID")
                    {
                        Debug.Log(IdToFind);
                    }
                }
            });
            DatabaseWaiter(5);
            FoundGameID.text = IdToFind;
            StartCoroutine(MoveToGame(IdToFind));
         }
         
    }
    void SignIN()
    {

        string email = LoginMail.text.ToString();
        string password = LoginPass.text.ToString();
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);            
            TheUserId = newUser.UserId;
            //calG();
            SignedIN = true;
            
        });
       
    }
    public void PressSignIn()
    {
        SignIN();
    }
    public void calG()
    {        
        Dreference.Child("Users").Child(TheUserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Debug.Log(snapshot.Child("UserName").Value.ToString());
                KaUser = snapshot.Child("UserName").Value.ToString();
                UserShower.text = KaUser;
                Debug.Log(KaUser);
            }
            
        });
       // UserShower.text = KaUser;
    }
   
    void hasSignedUp()
    {
            gameCan.SetActive(true);
            AftergameCan.SetActive(false);
            Debug.Log("UshaSign in boss");
           // UserShower.text = TheUserId;
    }
    IEnumerator joinAGame(string gameidyahapa)
    {
        Dreference.Child("ActiveGames").Child(gameidyahapa).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log("Kuna Game");
                    GameIDPresent = true;
                }
                else
                {
                    Debug.Log("Hakuna Game");
                    GameIDPresent = false;
                }
            }

        });  
        yield return new WaitForSeconds(5);
        Debug.Log(GameIDPresent);
        if(GameIDPresent)
        {
            JoinGame();
        }
        else
        {
            joinGamebtn.interactable = true;
            GameCodeBtn.gameObject.SetActive(true);
        }
    }
    IEnumerator SetUpGame()
    {
        int playersNumber = int.Parse(playerno.options[playerno.value].text);
        playerno.gameObject.SetActive(false);
        creatinggame.text = "Creating Game...";
        Debug.Log("Watu kama " + playersNumber);        
        Debug.Log("Nipee kaa 20 seconds");
        yield return new WaitForSeconds(10);
        string TheGameId = "";
        Dreference.Child("CurrentlyOnline").Child("Gamers").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Debug.Log(snapshot.Child("UserName").Value.ToString());
                TheGameId = snapshot.Child(TheUserId).Value.ToString();
                // Debug.Log(TheGameId);
                if (TheGameId != "NoID")
                {
                    GameIDPresent = true;
                    Debug.Log(TheGameId);
                }
            }
        });
        yield return new WaitForSeconds(5);
        if (GameIDPresent) { 
        Dreference.Child("ActiveGames").Child(TheGameId).Child("Number of Players").SetValueAsync(playersNumber-1);
        Debug.Log("Game ID status is " + GameIDPresent);
            }
        yield return new WaitForSeconds(5);
        FoundGameID.text = TheGameId;
        Debug.Log("Nimengoja kaa 20 seconds hivi");
        StartCoroutine(MoveToGame(TheGameId));
    }
    IEnumerator MoveToGame(string setAsFoundGame)
    {
        yield return new WaitForSeconds(5);
        FoundGame = setAsFoundGame;
        GetMyTag();
        yield return new WaitForSeconds(3);
        if (FoundGame!="")
        {
            GameSet = true;
        }
    }
    IEnumerator DatabaseTime(int TimeToWait)
    {
        Debug.Log("Chill for " + TimeToWait + " seconds");
        yield return new WaitForSeconds(TimeToWait);
        Debug.Log("Chilled for " + TimeToWait + " seconds");
    }
    private void DatabaseWaiter(int howlong)
    {
        StartCoroutine(DatabaseTime(howlong));
    }
    public void GetMyTag()
    {
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                playerAmount = (int)snapshot.ChildrenCount;
                foreach(var item in snapshot.Children)
                {
                   if(item.Child("UserID").Value.ToString() == TheUserId)
                    {
                        playerpos = item.Key;
                        Debug.Log(playerpos);
                    }
                  
                }
            }
        });
    }
    //listener for turn
    private void SuscbribeToTurn() {
     FirebaseDatabase.DefaultInstance
        .GetReference("ActiveGames/" + FoundGame + "/Turn/")
        .ValueChanged += HandleTurnChange;
    }
    void HandleTurnChange(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if(args.Snapshot.Value.ToString() == playerpos)
        {
            Debug.Log("Ni Turn yako");
            CurrentTurn = true;
            movingplayer = playerpos;
           
        }
        else if(args.Snapshot.Value.ToString() != "next")
        {
            movingplayer = args.Snapshot.Value.ToString();
            CurrentTurn = false;
            Debug.Log("Ni Turn ya " + movingplayer);
           
        }
        else
        {
            CurrentTurn = false;
            movingplayer = "";
            Debug.Log("Seeking Player");
        }

    }
    public void UnsubscribeToTurn()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("ActiveGames/" + FoundGame + "/Turn/")
        .ValueChanged -= HandleTurnChange;
    }
    private void CheckforMove()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference(GetTurnLocation())
        .ValueChanged += HandleMoveUpdate;
    }
    public void Uncheckformove()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference(GetTurnLocation())
        .ValueChanged -= HandleMoveUpdate;
    }
    void HandleMoveUpdate(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (args.Snapshot.Child("Status").Value.ToString() == "Ready")
        {
            rotator = float.Parse(args.Snapshot.Child("CurrentMove").Child("Rotation").Value.ToString());
            shooter = float.Parse(args.Snapshot.Child("CurrentMove").Child("ShootSpeed").Value.ToString());
            dipper = float.Parse(args.Snapshot.Child("CurrentMove").Child("DipSpeed").Value.ToString());
            Debug.Log("These are the Rotator: " + rotator + "Shooter: " + shooter + "Dipper: " + dipper);
        }
    }
    string GetTurnLocation()
    {
        string Turnlocation = "";
        if (movingplayer != "")
        {
            Turnlocation = "ActiveGames/" + FoundGame + "/Players/" + movingplayer;            
        }
        Debug.Log("The Turn Location is..." + Turnlocation);
        return Turnlocation;
    }

    public void Turner()
    {
        if (!CurrentTurn)
        {
            GetTurnLocation();
            CheckforMove();
        }
        else
        {
            Debug.Log("Turn Location ni useless");
            Uncheckformove();
        }        
    }
    public void ChangeTurn()
    {
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Turn").SetValueAsync("next");
    }
    public void MakeMove(float ss, float ds, float rot)
    {
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").Child(playerpos).Child("CurrentMove").Child("ShootSpeed").SetValueAsync(ss);
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").Child(playerpos).Child("CurrentMove").Child("Rotation").SetValueAsync(ds);
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").Child(playerpos).Child("CurrentMove").Child("DipSpeed").SetValueAsync(rot);
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").Child(playerpos).Child("Status").SetValueAsync("Ready");
    }
    public void startsesh()
    {
        if(playerpos == "Player1")
        {
            Dreference.Child("ActiveGames").Child(FoundGame).Child("Turn").SetValueAsync("Player1");
        }
        SuscbribeToTurn();
    }
    public void EndSesh()
    {
        if (playerpos == "Player1")
        {
            Dreference.Child("ActiveGames").Child(FoundGame).RemoveValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("You have deleted the game");
                }
            });
        }
    }
    IEnumerator waitforplayers()
    {
       
        checkwaitingplayers();
        yield return new WaitForSeconds(1f);
        fillplayers();
        yield return new WaitForSeconds(1f);
        GetUserNames();
        while (remainingplayers != 0)
        {
            checkwaitingplayers();
            fillplayers();
            Debug.Log("Waiting for other player");
            yield return new WaitForSeconds(1f);
            GetUserNames();
        }

    }
    private void checkwaitingplayers()
    {
        Dreference.Child("ActiveGames").Child(FoundGame).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                remainingplayers = int.Parse(snapshot.Child("Number of Players").Value.ToString());
            }
        });
    }
    private void fillplayers()
    {
        Dreference.Child("ActiveGames").Child(FoundGame).Child("Players").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int x = (int)snapshot.ChildrenCount;
                otherplayers = new string[x];
                int i = 0;
                foreach (var item in snapshot.Children)
                {
                    otherplayers[i] = item.Child("UserID").Value.ToString();
                    i++;
                }
            }
        });
    }
    private void GetUserNames()
    {
        if (othernames.Length != otherplayers.Length)
        {
            othernames = new string[otherplayers.Length];
        }
        for (int i=0;i<otherplayers.Length;i++)
        {
             PutUsername(i);
        }
    }
    public void PutUsername(int someindex)
    {
        if (othernames[someindex] != null)
        {
            Debug.Log("Already filled");
        }
        else
        {
            Dreference.Child("Users").Child(otherplayers[someindex]).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    othernames[someindex] = snapshot.Child("UserName").Value.ToString();
                }
            });
        }
    }
    public void loadplayers()
    {
        StartCoroutine(waitforplayers());
    }
    public void LogOut()
    {
        auth.SignOut();
    }
   
}
