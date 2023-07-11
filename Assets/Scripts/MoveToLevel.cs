using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToLevel : MonoBehaviour
{
    public int sceneBuildIndex = 0;
    public void ChangeLevel()
    {
        SetLevel(sceneBuildIndex);
    }
    
    public static void SetLevel(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
    public static void SetLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
