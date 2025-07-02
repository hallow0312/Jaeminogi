using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    SelectionSystem _selection;
    PlayerMovement _movement;
    PlayerInput _inputActions;
    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _inputActions = new PlayerInput();
    }
    void Start()
    {
        
    }
    private void OnEnable()
    {
        InitializeKeyInput();
    }
    private void OnDisable()
    {
        DiasbleKeyInput();
    }

    #region KeyInput
    void InitializeKeyInput()
    {
        _inputActions.Player.Move.performed += OnPlayerMove;
        _inputActions.Player.Move.canceled += OnPlayerMoveCanceled;
        _inputActions.Player.Jump.performed += OnPlayerJump;
        _inputActions.Player.Interact.performed += OnPlayerInteract;
        _inputActions.Enable();
    }

   

    private void DiasbleKeyInput()
    {
        _inputActions.Player.Move.performed -= OnPlayerMove;
        _inputActions.Player.Move.canceled -= OnPlayerMoveCanceled;
        _inputActions.Player.Jump.performed -= OnPlayerJump;
        _inputActions.Player.Interact.performed -= OnPlayerInteract;
        _inputActions.Player.Disable();
    }
    private void OnPlayerJump(InputAction.CallbackContext context)
    {
        _movement.Jump();
    }

    
    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0f, input.y);
        _movement.SetMoveInput(moveDir);
    }
  
    private void OnPlayerMoveCanceled(InputAction.CallbackContext context)
    {
       _movement.SetMoveInput(Vector3.zero);
    }
    private void OnPlayerInteract(InputAction.CallbackContext context)
    {
       // _selection?.TryToInteract();
    }
    #endregion
}
