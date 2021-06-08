using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manage : MonoBehaviour
{
    public void StartFirstDemo()
    {
        SceneManager.LoadScene("1st Part/Scenes/MainMenu",LoadSceneMode.Single);
    }   
    public void StartSecondDemo()
    {
        SceneManager.LoadScene("2nd Part/Scenes/PlanetGen",LoadSceneMode.Single);
    }    
    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
