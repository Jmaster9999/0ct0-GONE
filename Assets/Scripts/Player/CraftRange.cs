﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftRange : MonoBehaviour
{
    [SerializeField] GameFrameworkManager GameManager = null;
    [SerializeField] GameObject CraftingPrefab = null;
    [SerializeField] Player Player = null;

    private bool canCraft = false;
    // Start is called before the first frame update
    void Start()
    {
        canCraft = false;
    }

    public void OnCraftHotkey(InputValue value)
    {
        canCraft = Player.CanCraft(canCraft);
        

        if(canCraft == false)
        {
            return;
        }
        else if(!GameManager.isPaused)
        {
            CraftingPrefab.SetActive(true);
            GameManager.Pause();
            Debug.Log("Paused");
        }
    }

    public void OpenCrafting()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
