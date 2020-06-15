﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameFrameworkManager GameManager = null;
    [SerializeField] GameObject OptionsPrefab = null;
    [SerializeField] GameObject CreditsPrefab = null;
    [SerializeField] GameObject ConfirmationPrefab = null;
    [SerializeField] GameObject AudioReferences = null;

    [SerializeField] string nextScene = null;

    public void OnClickPlay()
    {
        if(AudioReferences == null)
        {
            Debug.LogError("A sound reference has not been hooked up on the UI Main Menu prefab");
        }
        else
        {
            AkSoundEngine.PostEvent("MUS_Stop", AudioReferences);
        }
        GameManager.LoadScene($"{nextScene}");
    }

    public void OnClickOptions()
    {
        var options = OptionsPrefab.GetComponent<Options>();

        options.closeCallback = DoClose;

        SwitchViewTo(OptionsPrefab);
    }

    //used by the Options screen to close itself and reopen the main menu
    void DoClose()
    {
        gameObject.SetActive(true);
        OptionsPrefab.SetActive(false);
    }

    public void OnClickCredits()
    {
        SwitchViewTo(CreditsPrefab);
    }

    //used by the Confirmation screen
    void DoQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();   
#endif

    }

    public void OnClickQuit()
    {
        var confirmation = ConfirmationPrefab.GetComponent<Confirmation>();

        confirmation.titleText.SetText("Quit?");

        confirmation.bodyText.SetText("Are you sure you want to quit to desktop?");

        confirmation.clickConfirmCallback = DoQuit;

        ConfirmationPrefab.SetActive(true);
    }

    public void SwitchViewTo(GameObject newPanel)
    {
        newPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Cursor.visible = true;
    }
}
