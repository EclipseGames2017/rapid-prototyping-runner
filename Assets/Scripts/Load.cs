using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    public GameObject IGM;
    public GameObject FailScreen;
    public bool SetActive = false;

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
        SetActive = true;
    }

    public void hideIGM()
    {
        IGM.SetActive(false);
        SetActive = false;

        if (Time.timeScale == 0)
        {
            SetActive = false;
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        if(SetActive == true || Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }
}
