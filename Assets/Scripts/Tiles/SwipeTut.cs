using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTut : MonoBehaviour {

    public PlayerCharacter PlayerCharacterRef;
    public GameObject SwipeAnim;
    public bool swipeAnimShown;
    private PlayerCharacter playerCharacter;

    // Use this for initialization
    void Start()
    {

        playerCharacter = GameObject.Find("TutPlayer").GetComponent<PlayerCharacter>();

    }

    // Update is called once per frame
    void Update()
    {

        if (swipeAnimShown == true)
        {
            StartCoroutine(playerMove());
        }
    }

    IEnumerator playerMove()
    {
        yield return new WaitForSeconds(1);

        if (PlayerCharacterRef.IsLayerA == true)
        {
            Debug.Log("Jump");
            PlayerCharacterRef.HandleSwipeInput();
            PlayerCharacterRef.m_Rigid.simulated = true;
            Destroy(GetComponent<SwipeTut>());
        }
        Destroy(GetComponent<BoxCollider2D>());
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "TutPlayer")
        {
            Debug.Log("hit");
            PlayerCharacterRef.m_Rigid.simulated = false;
            SwipeAnim.SetActive(true);
            swipeAnimShown = true;
        }
    }
}
