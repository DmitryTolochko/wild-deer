using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ApplicationQuit()
    {
        print("quit");
        Application.Quit();
    }

    public void StartGame()
    {
        print("start");
        SceneManager.LoadScene("SampleScene");  
    }
}
