using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject buttonScreen;
    private bool isScreenVisible = false;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || isScreenVisible) return;
        buttonScreen.SetActive(true);
        isScreenVisible = true;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        print("Application.Quit()");
    }
}
