using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    

    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button firstSelectedSettingsButton;
    [SerializeField] private Button settingsPanelFirstSelectedButton;

    [Header("Credits")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button firstSelectedCreditsButton;
    [SerializeField] private Button creditsPanelFirstSelectedButton;

    [Header("Scene Name")]
    [SerializeField] private string nameLoadScene;
    [SerializeField] private string nameUnloadScene;

    private bool isSettingsPanelopen;
    private bool isCreditsPanelOpen;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(nameLoadScene);
        SceneManager.UnloadSceneAsync(nameUnloadScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsPanel()
    {
        if (!isSettingsPanelopen)
        {
            settingsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(settingsPanelFirstSelectedButton.gameObject);
            isSettingsPanelopen = true;
        }
        else
        {
            settingsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(firstSelectedSettingsButton.gameObject);
            isSettingsPanelopen = false;
        }
    }

    public void CreditsPanel()
    {
        if (!isCreditsPanelOpen)
        {
            creditsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsPanelFirstSelectedButton.gameObject);
            isCreditsPanelOpen = true;
        }
        else
        {
            creditsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(firstSelectedCreditsButton.gameObject);
            isCreditsPanelOpen = false;
        }
    }
}
