﻿//Kristin Ruff-Frederickson | Copyright 2020©
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class FlashWhileActive : MonoBehaviour
{
    [SerializeField] private GameObject flashObject;
    [SerializeField] private float flashSpeed;
    [SerializeField] private float maxTextOpacity = 1f;
    [SerializeField] private float maxImageOpacity = .6f;
    [SerializeField] private float minOpacity = 0f;
    [SerializeField] private bool disableTextFlash;
    [SerializeField] private bool disableImageFlash;

    private float endAlpha = 0f;

    List<Image> imageColours = new List<Image>();
    List<TextMeshProUGUI> textColours = new List<TextMeshProUGUI>();

    private bool isDisabling = false;
    private bool isEnabling = false;

    private enum FlashType
    {   
        //flash in
        fIn = 1,
        //flash out
        fOut = -1,
    }

    private FlashType flash = FlashType.fOut;
    private float opacityMax;

    // Start is called before the first frame update
    void Awake()
    {
        opacityMax = Math.Max(maxTextOpacity, maxImageOpacity);

        Image[] imageChildren = flashObject.GetComponentsInChildren<Image>();
        foreach (Image child in imageChildren)
        {
            if (child.color != null)
            {
                imageColours.Add(child);
            }
        }

        TextMeshProUGUI[] textChildren = flashObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in textChildren)
        {
            if (child.color != null)
            {
                textColours.Add(child);
            }
        }
    }

    public void FlashImage()
    {
        for (int i = 0; i < imageColours.Count; i++)
        {
            CheckDirection(imageColours[i].color.a);
            if (flash == FlashType.fIn)
            {
                endAlpha = maxImageOpacity;
            }
            else
            {
                endAlpha = minOpacity;
            }

            Color lerpToColor = new Color(imageColours[i].color.r, imageColours[i].color.g, imageColours[i].color.b, endAlpha);

            imageColours[i].color = Color.Lerp(imageColours[i].color, lerpToColor, Time.deltaTime * 1f / flashSpeed);
        }
    }
    public void FlashText()
    {
        for (int i = 0; i < textColours.Count; i++)
        {
            CheckDirection(textColours[i].color.a);
            if (flash == FlashType.fIn)
            {
                endAlpha = maxTextOpacity;
            }
            else
            {
                endAlpha = minOpacity;
            }

            Color lerpToColor = new Color(textColours[i].color.r, textColours[i].color.g, textColours[i].color.b, endAlpha);

            textColours[i].color = Color.Lerp(textColours[i].color, lerpToColor, Time.deltaTime * 1f / flashSpeed);
        }
    }

    void CheckDirection(float opacity)
    {
        //going toward 1, switches to going toward 0
        if(flash == FlashType.fIn && opacity >= (opacityMax - 0.1f))
        {
            flash = FlashType.fOut;
        }
        //going toward 0, switches to going toward 1
        else if(flash == FlashType.fOut && opacity <= (minOpacity + 0.1f))
        {
            flash = FlashType.fIn;
        }
    }

    public void EnableFlash()
    {

    }

    public void DisableFlash()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabling == true)
        {
            EnableFlash();
            return;
        }
        if (isDisabling == true)
        {
            DisableFlash();
            return;
        }

        if (disableImageFlash == false)
        {
            FlashImage();
        }
        if (disableTextFlash == false)
        {
            FlashText();
        }
    }
}
