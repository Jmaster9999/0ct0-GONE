﻿using UnityEngine;

public class Damaged : MonoBehaviour
{
    [SerializeField] private float flashTime;
    [SerializeField] GameFrameworkManager gameManager = null;
    [SerializeField] private FillBar healthBar = null;

    private bool isLowFuel = false;

    private void Update()
    {
        if (gameManager.ActiveGameState.GetType() != typeof(Playing))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (!isLowFuel)
        {
            healthBar.Damaged();
            //EVAN - octo hurt sound, already buffered so cant be spammed
            AkSoundEngine.PostEvent("Damage", gameObject);
            StartCoroutine(BufferTime(flashTime));
        }
    }

    System.Collections.IEnumerator BufferTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.SetActive(false);
    }
}
