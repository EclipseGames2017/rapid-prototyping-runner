using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public int jumpForce = 20;
    public int moveSpeed = 20;

    private bool canJump = false;
    public bool canSpeedUp = true;

    public int layerA = 8, layerB = 9;

    bool isLayerA = true;

    public GameObject collisionObject;

    public IE_DualCameraBlend cameraEffect;

    public Rigidbody2D m_Rigid;
    public Animator m_Anim;

    public Text distanceText;
    public float distanceTravelled = 0;
    public float distanceCounter = 0;
    Vector2 lastPosition;
    Vector2 lastCounter;

    // these can be spent
    private int spendableTimeCrystals = 5;
    // these are the total, doesn't go down when spending
    private int totalTimeCrystals;

    public Text spendableCrystalsText;

    public GameObject FailScreen;

    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private float dragDistance;


    // Use this for initialization
    void Start()
    {
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);

        lastPosition = transform.position;

        lastCounter = transform.position;

        dragDistance = Screen.height * 5 / 100; //Drag distance is 5% height of the screen.
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        distanceText.text = "Meters: " + distanceTravelled.ToString("f0");

        distanceCounter += Vector2.Distance(transform.position, lastCounter);
        lastCounter = transform.position;

        spendableCrystalsText.text = "Crystals: " + spendableTimeCrystals;

        canSpeedUp = true;

        if (distanceCounter >= 500 && canSpeedUp == true)
        {
            SpeedUp();
        }

        //handle input for the platform it's on
#if UNITY_ANDROID || UNITY_IOS
        DoTouchInput();
        #else
        DoPCInput();
#endif
    
    }

    private void DoPCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (spendableTimeCrystals >= 1)
            {
                DoTimeSwipe();
            }
        }
    }

    private void SpeedUp()
    {
            moveSpeed += 2;
            canSpeedUp = false;
            distanceCounter -= 500;
    }

    private void DoTouchInput()
    {
        if (Input.touchCount == 1) //1 finger is touching screen.
        {
            Touch touch = Input.GetTouch(0); //Gets Touch.
            if (touch.phase == TouchPhase.Began) //Checks for the first touch.
            {
                firstTouch = touch.position;
                lastTouch = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) //Updates last Touch.
            {
                lastTouch = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //Checks if finger is off screen.
                lastTouch = touch.position;

            if (Mathf.Abs(lastTouch.x - firstTouch.x) > dragDistance || Mathf.Abs(lastTouch.y - firstTouch.y) > dragDistance) //it's a drag.
            {
                if (Mathf.Abs(lastTouch.x - firstTouch.x) > Mathf.Abs(lastTouch.y - firstTouch.y)) //Checks if horizontal or vertical.
                {
                    if ((lastTouch.x > firstTouch.x)) //Checks if drag was to the right.
                    {
                        Debug.Log("Right Swipe");
                    }
                    else
                    {
                        if (spendableTimeCrystals >= 1)
                        {
                            DoTimeSwipe();
                        }
                    }
                }
                else
                {
                    if (lastTouch.y > firstTouch.y) //Checks if drag was Up.
                    {
                        Debug.Log("Up Swipe");
 
                    }
                    else
                    {
                        Debug.Log("Down Swipe");
                    }
                }
            }

            else
            {
                if (canJump)
                {
                    m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    canJump = false;
                }
                Debug.Log("Tap");
            }
        }
    }

    private void DoTimeSwipe()
    {
        Debug.Log("Left Swipe");
        isLayerA = !isLayerA;
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);
        spendableTimeCrystals--;
    }

    private void FixedUpdate()
    {

        // Movement
        // replace jump with tap and hold.
        // add mario jump mechanic
        #if UNITY_DESKTOP || true
        if (Input.GetButtonDown("Jump") && canJump)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
        #endif

        // Move Right
        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // bad ground checking
        canJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Time Crystal":
                CollectCrystal();
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private void CollectCrystal()
    {
        spendableTimeCrystals++;
        totalTimeCrystals++;
    }
}
