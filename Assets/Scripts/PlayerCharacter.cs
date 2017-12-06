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
        [Header("Movement")]
        public int jumpForce = 20;
        public float moveSpeed = 10;

        public float speedupTime;
        public Vector2[] distanceSpeedMap;

        private float movespeedHard = 15;

        private bool doJump;

        private bool canJump = false;
        public bool canSpeedUp = true;

        public Transform jumpCheckStart, jumpCheckEnd;
        public Transform wallCheckStart, wallCheckEnd;

        [Header("Personal component Referances")]
        public Rigidbody2D m_Rigid;
        public Animator m_Anim;
        public GameObject collisionObject;
        public IE_DualCameraBlend cameraEffect;

        // Layer Settings
        private int layerA = 8, layerB = 9;
        public bool isLayerA = true;

        // Distance Stuff
        public Text[] scoreText;
        public Text highScoreText;
        public float distanceTravelled = 0;
        public float distanceCounter = 0;

        [HideInInspector]
        public float highScore;

        const string highScoreKey = "High Score";

        Vector2 lastPosition;
        Vector2 lastCounter;

        // Time Crystal Stuff
        private int spendableTimeCrystals = 5;
        private int totalTimeCrystals;

        public Text spendableCrystalsText;

        public GameObject FailScreenA;
        public GameObject FailScreenB;
        public GameObject CanvasA;
        public GameObject CanvasB;

        // Touch Stuff
        private Vector2 touchStart;
        private float dragDistance;

        public bool IsLayerA { get { return isLayerA; } }


        // Use this for initialization
        void Start()
        {
            highScore = SaveDataManager<RunnerSaveData>.data.highscore;

            collisionObject.layer = isLayerA ? layerA : layerB;

            m_Anim.SetBool("isZoneA", isLayerA);

            lastPosition = transform.position;

            lastCounter = transform.position;

            dragDistance = Screen.height * 20 / 100; //Drag distance is x% height of the screen.

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

            if (distanceTravelled >= highScore)
            {
                highScore = distanceTravelled;
                SaveDataManager<RunnerSaveData>.data.highscore = highScore;
                SaveDataManager<RunnerSaveData>.SaveData();
            }

            distanceCounter += Vector2.Distance(transform.position, lastCounter);
            lastCounter = transform.position;

            spendableCrystalsText.text = "CanJump: " + canJump;

            canSpeedUp = true;

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

        private void SpeedUp()
        {
            moveSpeed += 2;
            canSpeedUp = false;
            distanceCounter -= 500;
        }

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

        private void DoPCInput()
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
                doJump = true;

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

        #endregion

        #region Physics
        private void FixedUpdate()
        {
            // DoGroundCheck
            Vector2 line = jumpCheckEnd.position - jumpCheckStart.position;
            Debug.DrawLine(jumpCheckStart.position, jumpCheckEnd.position);
            if (Physics2D.Linecast(jumpCheckStart.position, jumpCheckEnd.position, 1 << collisionObject.layer))
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }

            Vector2 wall = wallCheckEnd.position - wallCheckStart.position;
            Debug.DrawLine(wallCheckStart.position, wallCheckEnd.position);
            if (Physics2D.Linecast(wallCheckStart.position, wallCheckEnd.position, 1 << collisionObject.layer) && isLayerA)
            {
                //SceneManager.LoadScene("GameOverTest", LoadSceneMode.Single);
                //FailScreen.SetActive(true);
                FailScreenA.SetActive(true);
                CanvasA.SetActive(false);
            }
            else
            {
            FailScreenB.SetActive(true);
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
