using Game;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZZ;

public class SceneBase : MonoBehaviour
{
    protected StateMachine _stateMachine;   // ステートマシン

    void Awake()
    {
        // ステートマシン生成
        _stateMachine = new StateMachine(this);
    }

    void Update()
    {
        // ステートマシン更新
        _stateMachine?.Update();
    }

    /// <summary>
    /// ステート変更（シーンから開始
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected async void ChangeState<T>() where T : StateBase, new()
    {
#if UNITY_EDITOR
        // なければ生成
        if (!GameManager.Instance)
        {
            var handle = Addressables.InstantiateAsync("GameManager");
            await handle.Task;
        }
#endif
        _stateMachine.ChangeState<T>();
    }

    /// <summary>
    /// 何もないステート
    /// </summary>
    public class EmptyState : StateBase
    {
    }
}
