﻿using UnityEngine;
using UnityEngine.InputSystem;

public class RepairableScreenRange : MonoBehaviour
{
    [SerializeField] GameFrameworkManager GameManager = null;
    [SerializeField] private Playing playing = null;
    [SerializeField] public StationRepair StationRepairScreen = null;
    [SerializeField] private float AntiSpamDelay = 0.2f;

    private bool canOpen = false;
    private Collider currentSat = null;
    private float LastPressedTime;

    public void OnRepairScreenHotkey(InputAction.CallbackContext context)
    {
        if (LastPressedTime + AntiSpamDelay > Time.unscaledTime) return; //anti spam

        LastPressedTime = Time.unscaledTime;

        //Debug.LogError("Pressed");
        if (canOpen && !GameManager.isPaused)
        {
            RepairableComponent satComponent = currentSat.GetComponentInParent<RepairableComponent>();
            //Debug.LogWarning("Can Open");
            //EVAN - menu open sound
            if (satComponent != null)
            {
                StationRepairScreen.OpenScreen(satComponent);
                GameManager.Pause();
                Debug.Log("Paused");
            }
            else
            {
                Debug.LogError("Did not find Repairable Component on passed collision");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        canOpen = playing.ActivePlayer.RepairComponentsInRange(out currentSat);
    }
}