using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public int jumpForce = 20;
    public int moveSpeed = 20;

    public int layerA = 8, layerB = 9;

    bool isLayerA = true;

    public GameObject collisionObject;

    public IE_DualCameraBlend cameraEffect;

    public Rigidbody2D m_Rigid;
    public Animator m_Anim;

    // Use this for initialization
    void Start()
    {
        collisionObject.layer = isLayerA ? layerA : layerB;

        m_Anim.SetBool("isZoneA", isLayerA);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            isLayerA = !isLayerA;
            collisionObject.layer = isLayerA ? layerA : layerB;

            m_Anim.SetBool("isZoneA", isLayerA);
        }

        if (Input.GetButtonDown("Jump"))
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        m_Rigid.velocity = m_Rigid.velocity.y * Vector2.up + moveSpeed * Vector2.right;

    }
}
