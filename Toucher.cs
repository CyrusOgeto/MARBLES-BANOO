using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toucher : MonoBehaviour
{
    //Initial bundle name was com.DefaultCompany.pm2020
   
    private MovementScript m;
    public Text ShowText;
    public Touch YouTouch,youi;
    private int AnaTouch;
    float itsTorque;
    float a, b , c;
    //public float TouchEnded;
    //private float displayTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // print("Niko");
        //  FingerNumber();
        //TouchPeriod();
          DetectSwipe();        
    }
   //This method only detects one finger and cannot detect the number of finger(s) on the screen
    void FingerNumber()
    {
        //this is you becoming  a script kiddie
        var fingerCount = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                fingerCount++;
                ShowText.text = "User has " + fingerCount + " finger(s) touching the screen";
            }
        }
        if (fingerCount > 0)
        {
            print("User has " + fingerCount + " finger(s) touching the screen");
        }
    }
    void TouchPeriod()
    {       
        if (Input.touchCount != 0)
        {
            YouTouch = Input.GetTouch(0);
            if (YouTouch.phase!=TouchPhase.Stationary)
            {
               // YouTouch = Input.GetTouch(0);
                if (YouTouch.phase != TouchPhase.Ended)
                {
                    AnaTouch++;
                }
                print(AnaTouch);
                itsTorque = (float)AnaTouch;
                m.DipSpeed = 5.0f;//itsTorque;
                m.roll();
            }
        }
        else
        {
            AnaTouch = 0;
        }
      //  print(AnaTouch);
    }
    void DetectSwipe()
    {
        if (Input.touchCount != 0)
        {
            if (Input.GetTouch(0).phase != TouchPhase.Stationary)
            {
                youi = Input.GetTouch(0);
                if (youi.phase == TouchPhase.Began)
                {
                    Debug.Log("Amegusa");
                    a = youi.position.y;
                }
                if (youi.phase == TouchPhase.Ended)
                {
                    Debug.Log("Ameachilia");
                    b = youi.position.y;
                    if (a > b)
                    {
                        Debug.Log("Ameshuka");
                        Debug.Log(a - b);
                        c = Mathf.Round((a - b) / 100);
                        Debug.Log(c);
                        m.DipSpeed = 1.0f;
                        m.shootspeed = c;
                        m.roll();
                    }
                    else
                    {
                        Debug.Log("Amepanda");
                        Debug.Log(b - a);
                        c = Mathf.Round((b - a) / 100);
                        Debug.Log(c);
                        m.DipSpeed = 5.0f;
                        m.shootspeed = c;
                        m.roll();
                    }
                }
            }
            else
            {
                Debug.Log("Hakasongii!");
            }
        
        }
        //  print(AnaTouch);
    }
    void pcdetectswipe()
    {
        if(Input.GetKey(KeyCode.M))
        {
            m.DipSpeed = 0.1f;
            m.shootspeed = 0.5f;
            m.roll();
        }

    }
}
