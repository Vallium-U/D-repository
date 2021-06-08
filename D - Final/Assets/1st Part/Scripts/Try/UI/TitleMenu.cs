using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class TitleMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject settingsObject;

    public Settings settings;

    [Header("Main Menu UI Elements")] 
    public TMP_InputField seedField;
    public TMP_InputField overallOffsetField;

    [Header("Settings Menu UI Elements")] 
    public Slider viewDistanceSlider;
    public TextMeshProUGUI viewDistanceText;
    public Slider mouseSlider;
    public TextMeshProUGUI mouseTextSlider;
    public Toggle threadingToggle;
    

    private void Awake()
    {
        //if (!File.Exists(Application.dataPath + "/1st Part/Resources/settings.cfg"))
            if (!File.Exists(Application.dataPath + "/Resources/settings.cfg"))
        {
            Debug.Log("No settings file found, creating new one.");

            settings = new Settings();
            string jsonExport = JsonUtility.ToJson(settings);
            //File.WriteAllText(Application.dataPath + "/1st Part/Resources/settings.cfg", jsonExport);
            File.WriteAllText(Application.dataPath + "/Resources/settings.cfg", jsonExport);
        }
        else
        {
            Debug.Log("Setting file found, loading settings!");
            // string jsonImport = File.ReadAllText(Application.dataPath + "/1st Part/Resources/settings.cfg");
            string jsonImport = File.ReadAllText(Application.dataPath + "/Resources/settings.cfg");
            settings = JsonUtility.FromJson<Settings>(jsonImport);
        }
    }

    private void Start()
    {
        seedField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        overallOffsetField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        seedField.characterLimit = 9;
        overallOffsetField.characterLimit = 9;
    }

    public void StartGame()
    {
        // VoxelData.seed = Mathf.Abs(seedField.text.GetHashCode()) / VoxelData.WorldSizeInChunks;
        // VoxelData.overalOffset = Mathf.Abs(seedField.text.GetHashCode()) / VoxelData.WorldSizeInChunks;
        
        VoxelData.seed = int.Parse(seedField.text);
        
        VoxelData.overalOffset = int.Parse(overallOffsetField.text);
        

        SceneManager.LoadScene("1st Part/Scenes/Main", LoadSceneMode.Single);
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Manage/ManagePJ", LoadSceneMode.Single);
    }

    public void EnterSettings()
    {
        viewDistanceSlider.value = settings.viewDistance;
        UpdateViewDistanceSlider();
        mouseSlider.value = settings.mouseSensetivity;
        UpdateMouseSlider();
        threadingToggle.isOn = settings.enableThreading;
        
        mainMenuObject.SetActive(false);
        settingsObject.SetActive(true);
    }

    public void LeaveSettings()
    {
        settings.viewDistance = (int) viewDistanceSlider.value;
        settings.mouseSensetivity = mouseSlider.value;
        settings.enableThreading = threadingToggle.isOn;

        string jsonExport = JsonUtility.ToJson(settings);
        //File.WriteAllText(Application.dataPath + "/1st Part/Resources/settings.cfg", jsonExport);
        File.WriteAllText(Application.dataPath + "/Resources/settings.cfg", jsonExport);
        
        mainMenuObject.SetActive(true);
        settingsObject.SetActive(false);
    }

    public void UpdateViewDistanceSlider()
    {
        viewDistanceText.text = "View Distance: " + viewDistanceSlider.value;
    }

    public void UpdateMouseSlider()
    {
        mouseTextSlider.text = "Mouse Sensetivity: " + mouseSlider.value.ToString("F1");
    }
    

}
