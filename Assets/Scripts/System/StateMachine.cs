using System;
using System.Collections.Generic;

/// <summary>
/// ステートマシン
/// </summary>
public class StateMachine
{
    public StateBase CurrentState { get; private set; } // 現在のステート
    public object Owner { get; private set; }   // シーンクラス

    private Dictionary<Type, StateBase> _stateDict = new(); // ステートのキャッシュ

    public StateMachine(object owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// ステート変更
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ChangeState<T>() where T : StateBase, new()
    {
        // 既存ステート検索
        Type type = typeof(T);
        if (!_stateDict.TryGetValue(type, out StateBase state))
        {
            // 新規ステート生成
            state = new T();
            state.SetStateMachine(this);
            _stateDict.Add(type, state);
        }

        // ステート切り替え
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState?.OnEnter();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        CurrentState?.OnUpdate();
    }
}

public abstract class StateBase
{
    protected StateMachine _stateMachine;

    /// <summary>
    /// ステートマシン設定
    /// </summary>
    /// <param name="stateMachine"></param>
    public void SetStateMachine(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}
