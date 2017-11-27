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
    public float currentTime;
    public float startTime;

    public GameObject FailScreen;


    // Use this for initialization
    void Start()
    {
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);

        currentTime = Time.timeSinceLevelLoad;

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        // Phase swipe, replace with touch swipe
        if (Input.GetButtonDown("Fire1"))
        {
            isLayerA = !isLayerA;
            collisionObject.layer = isLayerA ? layerA : layerB;

            m_Anim.SetBool("isZoneA", isLayerA);
        }

        currentTime = Time.time;
        float meters = (currentTime - startTime) * moveSpeed;
        distanceText.text = "Meters: " + meters.ToString("f0");

    }

    private void FixedUpdate()
    {

        // Movement
        // replace jump with tap and hold.
        // add mario jump mechanic
        if (Input.GetButtonDown("Jump") && canJump)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }

        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
}
