using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public PlayerCharacter PlayerCharacterRef;
    public GameObject TutAnim;
    public GameObject TutAnim2;
    public bool tapAnimShown;
    private PlayerCharacter playerCharacter;
    public int jumpForce = 0;
    
    // Use this for initialization
    void Start () {

        playerCharacter = GameObject.Find("TutPlayer").GetComponent <PlayerCharacter>();

    }
	
	// Update is called once per frame
	void Update () {
		
        if(tapAnimShown == true)
        {
            StartCoroutine(playerMove());
        }
	}

    IEnumerator playerMove()
    {
        yield return new WaitForSeconds(1);

        if (playerCharacter.DoJump == true)
        {
            Debug.Log("Jump");
            PlayerCharacterRef.m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            PlayerCharacterRef.m_Rigid.simulated = true;
        }
        Destroy(GetComponent<BoxCollider2D>());
    }   

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "TutPlayer")
        {
            Debug.Log("hit");
            PlayerCharacterRef.m_Rigid.simulated = false;
            TutAnim.SetActive(true);
            TutAnim2.SetActive(true);
            tapAnimShown = true;
        }
    }
}
