using UnityEngine;
using UnityEngine.InputSystem;

public class EntityPlayer : MonoBehaviour
{
    private InputAction _move; // à⁄ìÆ
    private UnitPlayer _unitPlayer; // ÉÇÉfÉãêßå‰ÉNÉâÉX

    private void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
        _unitPlayer = GetComponent<UnitPlayer>();
    }

    private void OnEnable()
    {
        _move?.Enable();
    }

    private void OnDisable()
    {
        _move?.Disable();
    }

    private void Update()
    {
        if(_move != null)
        {
            Vector2 value = _move.ReadValue<Vector2>();
            _unitPlayer.Move(value);
        }
    }
}
