#if UNITY_ANDROID || UNITY_IOS
#define TOUCHMODE
#else
#undef TOUCHMODE
#endif

//#undef TOUCHMODE // Force PC mode


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FallingSloth;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    const string HIGH_SCORE_KEY = "High Score";

    [Header("Movement")]
    [Range(10, 50)]
    public int jumpForce = 20;
    public float moveSpeed = 10;

    [Range(1, 30), Tooltip("Time in seconds to transition to the new speed.")]
    public float speedupTime;
    [Tooltip("Distance Traveled, Speed at that distance.")]
    public Vector2[] distanceSpeedMap;

    // Jump Stuff
    private bool doJump;
    private bool canJump = false;
    //public bool canSpeedUp = true;
    //private float movespeedHard = 15;

    // Transforms for drawing raycasts between
    [Header("Collision Check handles")]
    public Transform jumpCheckStart;
    public Transform jumpCheckEnd;
    public Transform wallCheckStart;
    public Transform wallCheckEnd;

    [Header("Personal component Referances")]
    public Rigidbody2D m_Rigid;
    public Animator m_Anim;
    public GameObject collisionObject;
    public IE_DualCameraBlend cameraEffect;

    // Distance Stuff
    [Header("GUI Referances")]
    public Text[] scoreText;
    public Text highScoreText;
    public Text highScoreBText;
    public GameObject FailScreenA;
    public GameObject FailScreenB;
    public GameObject CanvasA;
    public GameObject CanvasB;

    // Distance and Score
    private float distanceTravelled = 0;
    private float distanceCounter = 0;

    public Text spendableCrystalsText;

    // Layer Settings
    private int layerA = 8, layerB = 9;
    private bool isLayerA = true;

    [HideInInspector]
    public float highScore;

    Vector2 lastPosition;
    Vector2 lastCounter;

    // Time Crystal Stuff
    private int spendableTimeCrystals = 5;
    private int totalTimeCrystals;

    // Properties
    public bool IsLayerA { get { return isLayerA; } }

    public float DistanceTravelled { get { return distanceTravelled; } }

    public bool DoJump { get { return doJump; } }

#if TOUCHMODE
    [Header("Touch Settings")]
    private float dragDistance;
    #region Stuff Jamie Added
    int fingerID = -1;
    Vector2 touchStartPosition = -Vector2.one;
    float touchStartTime = -1;
    float timeForTap = 0.075f;
    float maxDistanceForTap = 20f;
    #endregion

#endif
    // Use this for initialization
    void Start()
    {
        highScore = SaveDataManager<RunnerSaveData>.data.highscore;

        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);

        lastPosition = transform.position;

        lastCounter = transform.position;

#if TOUCHMODE
        dragDistance = Screen.height * 20 / 100; //Drag distance is x% height of the screen.
#endif

        highScoreText.text = "High Score: " + highScore.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        scoreText[0].text = distanceTravelled.ToString("N0");
        scoreText[1].text = distanceTravelled.ToString("N0");

        if (distanceTravelled >= 0.1f)
        {
            highScoreText.text = "HS: " + highScore.ToString("F0");
        }

        if (distanceTravelled >= 0.1f)
        {
            highScoreBText.text = "HS: " + highScore.ToString("F0");
        }

        if (distanceTravelled >= highScore)
        {
            highScore = distanceTravelled;
            SaveDataManager<RunnerSaveData>.data.highscore = highScore;
            SaveDataManager<RunnerSaveData>.SaveData();
        }

        distanceCounter += Vector2.Distance(transform.position, lastCounter);
        lastCounter = transform.position;

        spendableCrystalsText.text = "CanJump: " + canJump;

        //canSpeedUp = true;

        for (int i = 1; i < distanceSpeedMap.Length; i++)
        {
            if (distanceTravelled > distanceSpeedMap[i - 1].x && distanceTravelled < distanceSpeedMap[i].x)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, distanceSpeedMap[i - 1].y, Time.deltaTime / speedupTime);
                break;
            }
        }

        //if (distanceCounter >= 500 && canSpeedUp == true)
        //{
        //    SpeedUp();
        //}

        HandleInput();
    }

    //private void SpeedUp()
    //{
    //    moveSpeed += 2;
    //    canSpeedUp = false;
    //    distanceCounter -= 500;
    //}

    private void CollectCrystal()
    {
        spendableTimeCrystals++;
        totalTimeCrystals++;
    }

#region Input
    private void HandleInput()
    {
        //handle input for the platform it's on
#if TOUCHMODE
        DoTouchInput();
#else
        DoPCInput();
#endif


    }

    public void DoPCInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            HandleSwipeInput();
        }
        if (Input.GetButtonDown("Jump") && canJump)
        {
            doJump = true;
        }
    }

#if TOUCHMODE
    public void DoTouchInput()
    {
    #region Stuff Jamie Added
        if (fingerID == -1) // If we have no finger ID...
        {
            if (Input.touchCount > 0) // ...and we have at least one touch...
            {
                foreach (Touch t in Input.touches) // ...check each touch...
                {
                    if (t.phase == TouchPhase.Began) // ...until we get one with a phase of Began.
                    {
                        fingerID = t.fingerId; // Store the finger ID
                        touchStartPosition = t.position; // Store the start position
                        touchStartTime = Time.time; // Store the start time of the touch
                        break;
                    }
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch t in Input.touches)
                {
                    if (t.fingerId == fingerID)
                    {
                        if (t.phase == TouchPhase.Canceled)
                        {
                            ResetTouchInput();
                        }

                        else if (t.phase == TouchPhase.Ended)
                        {
                            float distance = Vector2.Distance(touchStartPosition, t.position);

                            if (distance <= maxDistanceForTap)
                                HandleTapInput();
                            else
                                HandleSwipeInput();

                            ResetTouchInput();
                        }

                        else if (Time.time - touchStartTime >= timeForTap)
                        {
                            float distance = Vector2.Distance(touchStartPosition, t.position);

                            if (distance <= maxDistanceForTap)
                                HandleTapInput();
                            else
                                HandleSwipeInput();

                            ResetTouchInput();
                        }

                        break;
                    }
                }
            }
            else // If we have a finger ID but no touches, reset.
            {
                ResetTouchInput();
            }
        }
    }
    void ResetTouchInput()
    {
        fingerID = -1;
        touchStartPosition = -Vector2.one;
        touchStartTime = -1;
    }
    #endregion

    public void HandleTapInput()
    {
        if (canJump)
        {
            doJump = true;

        }
    }
#endif

    public void HandleSwipeInput()
    {
        Debug.Log("Left Swipe");
        isLayerA = !isLayerA;
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);
        spendableTimeCrystals--;
    }

#endregion

#region Physics
    private void FixedUpdate()
    {
        // DoGroundCheck
        //Vector2 line = jumpCheckEnd.position - jumpCheckStart.position;
        Debug.DrawLine(jumpCheckStart.position, jumpCheckEnd.position);
        if (Physics2D.Linecast(jumpCheckStart.position, jumpCheckEnd.position, 1 << collisionObject.layer))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        //Vector2 wall = wallCheckEnd.position - wallCheckStart.position;
        Debug.DrawLine(wallCheckStart.position, wallCheckEnd.position);
        if (Physics2D.Linecast(wallCheckStart.position, wallCheckEnd.position, 1 << collisionObject.layer) && isLayerA)
        {
            Debug.Log("Dead");
            FailScreenA.SetActive(true);
            m_Rigid.simulated = false;
        }

        if (Physics2D.Linecast(wallCheckStart.position, wallCheckEnd.position, 1 << collisionObject.layer) && !isLayerA)
        {

            FailScreenB.SetActive(true);
            m_Rigid.simulated = false;

        }


        // wold be nice to make this use Mario Jump
        if (doJump)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            doJump = false;
        }

        // Move Right
        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // bad ground checking
        // canJump = true;

        /*if (collision.gameObject.name == "WallQuad")
        {
            GameOver = true;
            Time.timeScale = 0;
            Debug.Log("HIT");
        }*/
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
#endregion
}
