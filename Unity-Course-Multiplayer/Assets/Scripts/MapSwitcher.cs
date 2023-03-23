using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapSwitcher : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerInput>(out var playerInputs))
        {
            playerInputs.currentActionMap = playerInputs.actions.FindActionMap("StreetFight");
            Debug.Log("Switched Map !!!!!!!!!!!!!!" + playerInputs.currentActionMap);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<PlayerInput>(out var playerInputs))
        {
            playerInputs.currentActionMap = playerInputs.actions.FindActionMap("Others");
            Debug.Log("Switched Map !!!!!!!!!!!!!!" + playerInputs.currentActionMap);
        }
    }

}
