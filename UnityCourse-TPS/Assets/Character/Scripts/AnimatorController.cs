using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatorController : MonoBehaviour
{
    
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private Animator _gunAnimator;
    
    private MoveController _moveController;
    private TPS_Character_InputsWrapper _inputs;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_characterAnimator == null)
            _characterAnimator = GetComponent<Animator>();
        
        _moveController = GetComponent<MoveController>();
        _inputs = GetComponent<TPS_Character_InputsWrapper>();
        if(_inputs != null)
            _inputs.ShootEvent += InputsOnShootEvent;

    }

    private void InputsOnShootEvent(bool shootValue)
    {
        _gunAnimator.SetBool("Shoot", shootValue);
    }

    // Update is called once per frame
    void Update()
    {
        _characterAnimator.SetFloat("Speed", _moveController.Velocity.magnitude);
        _characterAnimator.SetBool("Aim", _inputs.IsAiming);
        _gunAnimator.SetBool("Aim", _inputs.IsAiming);
    }
}
