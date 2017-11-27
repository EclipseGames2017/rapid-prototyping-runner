﻿using System.Collections;
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
    public float currentTime;
    public float startTime;

    public GameObject FailScreen;

    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private float dragDistance;


    // Use this for initialization
    void Start()
    {
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);

        currentTime = Time.timeSinceLevelLoad;

        startTime = Time.time;

        dragDistance = Screen.height * 15 / 100; //Drag distance is 15% height of the screen.
    }

    // Update is called once per frame
    void Update()
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
                    Debug.Log("Tap");
                }
            }
        }

        // Phase swipe, replace with touch swipe
       /* if (Input.GetButtonDown("Fire1"))
        {
            isLayerA = !isLayerA;
            collisionObject.layer = isLayerA ? layerA : layerB;

            m_Anim.SetBool("isZoneA", isLayerA);
        }*/

        currentTime = Time.time;
        float meters = (currentTime - startTime) * moveSpeed;
        distanceText.text = "Meters: " + meters.ToString("f0");

    }

    private void FixedUpdate()
    {

        // Movement
        // replace jump with tap and hold.
        // add mario jump mechanic
        /*if (Input.GetButtonDown("Jump") && canJump)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }*/

        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
