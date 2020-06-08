﻿//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] Button AudioTabButton = null;
    [SerializeField] Button VideoTabButton = null;
    [SerializeField] Button ControlTabButton = null;

    [SerializeField] GameObject AudioTabPanel = null;
    [SerializeField] GameObject VideoTabPanel = null;
    [SerializeField] GameObject ControlTabPanel = null;

    [SerializeField] TextMeshProUGUI ControlsText = null;

    Dictionary<GameObject, Button> PanelToButton = new Dictionary<GameObject, Button>();

    public System.Action closeCallback;

    public string[] controls = new string[]
    {
        "<b>CONTROLS</b>\n-------",
        "<b>MOUSE</b> - Look",
        "<b>W/S</b> - Move forward/back",
        "<b>A/D</b> - Move left/right",
        "<b>Q/E</b> - Rotate left/right",
        "<b>SPACE/CTRL</b> - Move up/down",
        "<b>1</b> - Select salvage tool",
        "<b>TAB</b> - Target hovered object",
        "<b>LEFT CLICK</b> - Salvage target",
        "<b>ESC</b> - Pause"

    };

    public void OnEsc(InputValue value)
    {
        Close();
    }

    public void Close()
    {
        closeCallback();

        closeCallback = null;
    }

    void Awake()
    {
        PanelToButton[AudioTabPanel] = AudioTabButton;
        PanelToButton[VideoTabPanel] = VideoTabButton;
        PanelToButton[ControlTabPanel] = ControlTabButton;

        SwitchActiveTab(AudioTabPanel);
    }

    private void Start()
    {
        string text = "";
        foreach (var control in controls)
        {
            text = text + $"\n{control}";
        }
        ControlsText.SetText(text);
    }

    public void ClickAudioTab()
    {
        SwitchActiveTab(AudioTabPanel);
    }

    public void ClickVideoTab()
    {
        SwitchActiveTab(VideoTabPanel);
    }

    public void ClickControlsTab()
    {
        SwitchActiveTab(ControlTabPanel);
    }

    public void SwitchViewTo(GameObject newPanel)
    {
        newPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SwitchActiveTab(GameObject active_panel)
    {
        foreach (var kvp in PanelToButton)
        {
            if (kvp.Key == active_panel)
            {
                kvp.Key.SetActive(true);
                kvp.Value.GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                kvp.Key.SetActive(false);
                kvp.Value.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }
}