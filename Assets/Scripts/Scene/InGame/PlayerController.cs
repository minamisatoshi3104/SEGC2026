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

    private const float WALK_SPEED = 5f;    // 移動速度
    private const float SPRINT_SPEED = 10f;   // ダッシュ速度

    private const float ROTATION_LERP_RATE = 0.3f;  // 回転補間率

    private readonly int AnimHashTriggerJamp = Animator.StringToHash("Jamp");

    private InputAction _move;      // 移動
    private InputAction _sprint;    // ダッシュ

    private Animator _animator;    // アニメーター
    private Rigidbody _rigidbody;

    private State _currentState = State.Idle;   // 現在の状態

    private void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
        _sprint = InputSystem.actions.FindAction("Sprint");
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();    
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _move?.Enable();
        _sprint?.Enable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _move?.Disable();
        _sprint?.Disable();
    }

    void Update()
    {
        // 自分のプレイヤーのみ操作可能
        if (photonView.IsMine)
        {
            // 移動
            Vector2 moveDir = _move.ReadValue<Vector2>();
            bool isSprint = _sprint.ReadValue<float>() > 0f;
            if (moveDir != Vector2.zero)
            {
                Move(moveDir, isSprint);
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isSprint"></param>
    public void Move(Vector2 value, bool isSprint)
    {
        // 移動
        float speed = isSprint ? SPRINT_SPEED : WALK_SPEED;
        _rigidbody.linearVelocity = new Vector3(value.x, 0f, value.y) * speed;
        // 回転
        Quaternion rotation = Quaternion.LookRotation(new Vector3(value.x, 0f, value.y));
        _rigidbody.transform.rotation = Quaternion.Lerp(_rigidbody.transform.rotation.normalized, rotation, ROTATION_LERP_RATE);
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
