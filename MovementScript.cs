using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class MovementScript : MonoBehaviour
{

    public float DipSpeed;
    public float shootspeed;
    private bool itsRigid;
    public Vector3 itsPos;
    private bool TumiaScreeen;
    public bool DonePitted;
    Rigidbody thisball;
    public Transform y;
    public int score;
    public bool myturn;
    public bool hitsBall;
    private bool PittedIt;
    private Vector3 uio;
    public bool alert;
    public string whatAlert;
    private TheARManager arm;
    private bool unplayed = true;
    public bool resetit=false;
    public float rotval;

    public bool alipit;
    public Vector3 onecorner;
    public Vector3 twocorner;

    private float swipetime = 0.0f;
    //public GameObject a;
    public Vector3 otherpos;
    public GameObject otherball;
    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.FindGameObjectWithTag("Meneja").GetComponent<TheARManager>();

        PittedIt = false;        
        hitsBall = false;
        myturn = true;
        thisball = GetComponent<Rigidbody>();
        thisball.useGravity = false;
        thisball.isKinematic = false;
        itsPos = transform.localPosition;     

    }

    public void Control(Vector2 bonyeza)
    {
        if (bonyeza != Vector2.zero)
        {
          //  transform.eulerAngles = Vector3.up * Mathf.Atan2(bonyeza.x, bonyeza.y) * Mathf.Rad2Deg;
        }
        /*  ballspeed = bonyeza.magnitude;
          transform.Translate(transform.forward * ballspeed * Time.deltaTime, Space.World);
        */
    }
    public void roll()
    {
        if (!arm.amepause)
        {
        swipetime = Time.time;
        unplayed = false;
        thisball.useGravity = true;  
        thisball.AddForce(transform.up * DipSpeed + transform.forward * shootspeed, ForceMode.Impulse);
        arm.laini.gameObject.SetActive(true);
        arm.laini.SetPosition(0, transform.localPosition);
        thisball.AddTorque(transform.right * 20f);            
        }
    }
    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.name == "Cube")
        {
            //facer(y);
            Debug.Log("Mesh Renderer amesema Ametoka");
            itsPos.x = transform.localPosition.x;
            itsPos.z = transform.localPosition.z;
            resetit = true;
        }
    }
    
    private void OnTriggerEnter(Collider sother)
    {
        
        if (sother.gameObject.name == "Shimo")
        {
            if (DonePitted == false)
            {
                alert = true;
                whatAlert = "Pitt!!";
                PittedIt = true;
                itsPos.x = transform.localPosition.x;
                itsPos.z = transform.localPosition.z;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BALL")
        {
            otherball = collision.gameObject;
            otherpos = otherball.transform.localPosition;
            hitsBall = true;
            alert = true;
            whatAlert = "Hit!!";          
        }
    }
    public void scorer()
    {
        if (PittedIt == true)
        {
            score += 11;
        }
        else if(hitsBall == true)
        {
            score += 13;
        }
        else
        {
            score += 1;
        }
               
    }
    // Update is called once per frame
    void Update()
    {
        
        uio = arm.usercamera.transform.forward;        
        uio.y = 0f;
        if (unplayed)
        {  
            transform.rotation = Quaternion.LookRotation(uio);
            rotval = transform.localEulerAngles.y;
            // transform.localEulerAngles= new Vector3(0, 45, 0);
        }        
        else
        {
            //transform.Rotate(shootspeed*20, 0f, 0f);
            arm.laini.gameObject.SetActive(false);            
            //arm.laini.SetPosition(1, transform.localPosition + transform.forward *4f);
            if (Time.time - swipetime >=7 && Time.time - swipetime <=8)
            {
                swipetime = 0.0f;
                thisball.isKinematic = true;                
                ResetPosition(transform.localPosition);
            }
        }
        
    }
    
    public void ResetPosition(Vector3 exitpoint)
    {
        alipit = PittedIt;
        this.transform.gameObject.SetActive(false);
        if (resetit)
        {
            float a = Mathf.Clamp(itsPos.x, -1.75f, 1.75f);
            float b = Mathf.Clamp(itsPos.z, -1.75f, 1.75f);
            itsPos.x = a;
            itsPos.z = b;
            Debug.Log("Its Position is..." + itsPos);
            transform.localPosition = new Vector3(itsPos.x, itsPos.y, itsPos.z);
            if (hitsBall)
            {
                otherpos.y = itsPos.y;
                otherball.transform.localPosition = new Vector3(otherpos.x, otherpos.y, otherpos.z);
                Destroy(otherball.GetComponent<Rigidbody>());
            }
            myturn = false;
        }
        else
        {
            transform.localPosition = new Vector3(itsPos.x, itsPos.y, itsPos.z);
            myturn = false;
        }

    }
}

