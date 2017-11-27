using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    public GameObject IGM;
    public GameObject FailScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGame()
    {
        Application.LoadLevel("GeneratedLevel");
    }

    public void LoadMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void ShowIGM()
    {
        IGM.SetActive(true);

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }

    public void hideIGM()
    {
        IGM.SetActive(false);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
