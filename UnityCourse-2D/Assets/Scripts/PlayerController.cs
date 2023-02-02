using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] float _speed;

    private Vector2 _moveValue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_moveValue * _speed * Time.deltaTime);
    }

    public void HandleMove(InputAction.CallbackContext ctx)
    {
        _moveValue = ctx.ReadValue<Vector2>();
    }

    public void HandleShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("Shoot ready !!!!!!!!!!!");
            _spriteRenderer.color = Color.yellow;
        }

        if (ctx.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Shoot...............................");
            _spriteRenderer.color = Color.green;
        }
        
    }
    
}
