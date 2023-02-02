using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject _reticlePanel;

    private TPS_Character_InputsWrapper _inputs;
    
    private void Start()
    {
        _reticlePanel.SetActive(false);
        _inputs = GetComponent<TPS_Character_InputsWrapper>();
    }

    private void Update()
    {
        _reticlePanel.SetActive(_inputs.IsAiming);
    }

}
