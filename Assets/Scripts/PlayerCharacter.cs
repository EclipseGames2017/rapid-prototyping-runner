using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    public int jumpForce = 20;
    public int moveSpeed = 20;

    private bool canJump = false;
    public bool canSpeedUp = true;

    public int layerA = 8, layerB = 9;

    bool isLayerA = true;

    public bool GameOver;

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

    private Vector2 touchStart;
    //private Vector2 touchEnd;

    private float dragDistance;


    // Use this for initialization
    void Start()
    {
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);

        lastPosition = transform.position;

        lastCounter = transform.position;

        dragDistance = Screen.height * 20 / 100; //Drag distance is x% height of the screen.

        GameOver = false;
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

        if (GameOver == true)
        {
            SceneManager.LoadScene("GameOverTest", LoadSceneMode.Single);
        }

        HandleInput();
    }

    private void HandleInput()
    {
        //handle input for the platform it's on
#if !UNITY_EDITOR
#if UNITY_ANDROID || UNITY_IOS
        DoTouchInput();
#else
        DoPCInput();
#endif
#else
        DoTouchInput();
#endif


    }

    private void DoPCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (spendableTimeCrystals >= 1)
            {
                HandleSwipeInput();
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
        if (Input.touches.Length > 0)
        {
            Touch touch0 = Input.touches[0];
            switch (touch0.phase)
            {
                case TouchPhase.Began:
                    touchStart = touch0.position;
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:

                    float distance = Vector2.Distance(touchStart, touch0.position);

                    if (distance > dragDistance)
                    {
                        HandleSwipeInput();
                    }
                    else
                    {
                        HandleTapInput();
                    }
                    touchStart = Vector2.zero;
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleTapInput()
    {
        if (canJump)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void HandleSwipeInput()
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

        if (collision.gameObject.name == "WallQuad")
        {
            GameOver = true;
            Time.timeScale = 0;
            Debug.Log("HIT");
        }
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
