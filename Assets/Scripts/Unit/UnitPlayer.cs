using UnityEngine;

/// <summary>
/// プレイヤーモデル制御
/// </summary>
public class UnitPlayer : MonoBehaviour
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

    private Animator _animator;    // アニメーター
    private Rigidbody _rigidbody;

    private State _currentState = State.Idle;   // 現在の状態

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        
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
