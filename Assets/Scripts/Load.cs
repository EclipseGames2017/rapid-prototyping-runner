using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FallingSloth;

public class Load : MonoBehaviour {

    public GameObject IGM;
    public bool SetActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGame()
    {
        SceneManager.LoadScene("GeneratedLevel", LoadSceneMode.Single);
        //Application.LoadLevel("GeneratedLevel");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        //Application.LoadLevel("MainMenu");
    }

    public void LoadTutorial()
    {
        if (SaveDataManager<RunnerSaveData>.data.tutorialSeen)
        {
            LoadGame();
        }
        else
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
        }
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
