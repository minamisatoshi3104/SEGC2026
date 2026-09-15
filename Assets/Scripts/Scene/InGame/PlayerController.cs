using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー制御
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 状態
    /// </summary>
    public enum State
    {
        Idle,
        Dash,
        Jamp,
    }

    private readonly int AnimHashTriggerJamp = Animator.StringToHash("Jamp");

    [SerializeField] private float _forceRate = 1f; // 加速度の倍率

    private InputAction _move; // 移動
    private Animator _animator;    // アニメーター
    private Rigidbody _rigidbody;

    private State _currentState = State.Idle;   // 現在の状態

    private void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();    
    }

    private void OnEnable()
    {
        _move?.Enable();
    }

    private void OnDisable()
    {
        _move?.Disable();
    }

    void Update()
    {
        if (_move != null && photonView.IsMine)
        {
            Vector2 value = _move.ReadValue<Vector2>();
            Move(value);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="value"></param>
    public void Move(Vector2 value)
    {
        value *= _forceRate;
        _rigidbody.AddForce(new Vector3(value.x, 0f, value.y));
    }

    /// <summary>
    /// ステート切り替え
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(State state)
    {
        _currentState = state;
    }
}
