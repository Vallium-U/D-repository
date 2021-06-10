using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _2nd_Part
{
    public class Reload : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R)){
                Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene("Manage/ManagePJ", LoadSceneMode.Single);
            }
        }

        public void GeneratePlanets()
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        public void QuitToMenu()
        {
            SceneManager.LoadScene("Manage/ManagePJ", LoadSceneMode.Single);
        }
    }
}