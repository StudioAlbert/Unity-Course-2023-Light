using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private Inventory _inventory;

    private void Update()
    {
        _scoreText.text = _inventory.Coins.ToString();
    }

    public void TriggerMenu(InputAction.CallbackContext ctx)
    {
        
        Debug.Log("Trigger Menu : " + ctx.phase);
        
        if (ctx.phase == InputActionPhase.Performed)
            TriggerMenuOn();

        if (ctx.phase == InputActionPhase.Disabled)
            TriggerMenuOff();
        
    }

    private void TriggerMenuOn()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void TriggerMenuOff()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 0;
    }

}
