﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject CodexPrefab;
    [SerializeField] GameObject OptionsPrefab;
    [SerializeField] GameObject ConfirmationPrefab;

    [SerializeField] string menuScene;

    public void OnClickResume()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCodex()
    {
        SwitchViewTo(CodexPrefab);
    }

    public void OnClickOptions()
    {
        var options = OptionsPrefab.GetComponent<Options>();

        options.closeCallback = DoClose;

        SwitchViewTo(OptionsPrefab);
    }

    //used by the Options screen to close itself and reopen the pause menu
    void DoClose()
    {
        gameObject.SetActive(true);
        OptionsPrefab.SetActive(false);
    }

    public void OnClickMainMenu()
    {
        var confirmation = ConfirmationPrefab.GetComponent<Confirmation>();

        confirmation.titleText.SetText("Main Menu?");

        confirmation.bodyText.GetComponent<TMPro.TMP_Text>().SetText("Are you sure you want to return to the main menu?");

        confirmation.clickConfirmCallback = DoMainMenu;

        ConfirmationPrefab.SetActive(true);
    }

    //used by the Confirmation screen
    void DoMainMenu()
    {
        SceneManager.LoadScene($"{menuScene}");
    }

    public void OnClickQuit()
    {
        var confirmation = ConfirmationPrefab.GetComponent<Confirmation>();

        confirmation.titleText.SetText("Quit?");

        confirmation.bodyText.GetComponent<TMPro.TMP_Text>().SetText("Are you sure you want to quit to desktop?");

        confirmation.clickConfirmCallback = DoQuit;

        ConfirmationPrefab.SetActive(true);
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

    public void OnEsc(InputValue value)
    {
        gameObject.SetActive(false);
    }

    public void SwitchViewTo(GameObject newPanel)
    {
        newPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
