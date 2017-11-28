using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public int jumpForce = 20;
    public int moveSpeed = 20;

    private bool canJump = false;

    public int layerA = 8, layerB = 9;

    bool isLayerA = true;

    public GameObject collisionObject;

    public IE_DualCameraBlend cameraEffect;

    public Rigidbody2D m_Rigid;
    public Animator m_Anim;

    public Text distanceText;
    public float distanceTravelled = 0;
    Vector2 lastPosition;

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

        dragDistance = Screen.height * 5 / 100; //Drag distance is 5% height of the screen.
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        distanceText.text = "Meters: " + distanceTravelled.ToString("f0");

        #if UNITY_ANDROID || UNITY_IOS
        DoTouchInput();
        #endif
        #if UNITY_DESKTOP || true
        DoPCInput();
        #endif

    }

    private void DoPCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isLayerA = !isLayerA;
            collisionObject.layer = isLayerA ? layerA : layerB;

            m_Anim.SetBool("isZoneA", isLayerA);
        }
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
                        Debug.Log("Left Swipe");
                        isLayerA = !isLayerA;
                        collisionObject.layer = isLayerA ? layerA : layerB;

                        m_Anim.SetBool("isZoneA", isLayerA);
                    }
                }
                else
                {
                    if (lastTouch.y > firstTouch.y) //Checks if drag was Up.
                    {
                        Debug.Log("Up Swipe");
                        if (canJump)
                        {
                            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                            canJump = false;
                        }
                    }
                    else
                    {
                        Debug.Log("Down Swipe");
                    }
                }
            }

            else
            {
                /*if (canJump)
                {
                    m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    canJump = false;
                }*/
                Debug.Log("Tap");
            }
        }
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

        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
