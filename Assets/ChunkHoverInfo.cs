﻿using UnityEngine;
using TMPro;

public class ChunkHoverInfo : MonoBehaviour
{
    [SerializeField] private GameObject GO = null;
    [SerializeField] private TextMeshProUGUI title = null;
    [SerializeField] private TextMeshProUGUI slotAmount = null;

    public string titleText;
    public string amountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnHover()
    {
        title.SetText(titleText);
        slotAmount.SetText(amountText);
        GO.SetActive(true);
    }

    public void OnHoverEnd()
    {
        GO.SetActive(false);
    }

    private void OnDisable()
    {
        GO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
