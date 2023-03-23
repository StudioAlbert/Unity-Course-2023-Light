using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickAnimator : MonoBehaviour
{
    [SerializeField] private List<AnimatorOverrideController> _skins;

    private PlayerInput _input;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        
        _animator.runtimeAnimatorController = _skins[_input.playerIndex];
        
        Debug.Log("New challenger : " + _input.playerIndex + " : " + _input.currentControlScheme + " !!!!!!!!!!!!!");

    }

}
