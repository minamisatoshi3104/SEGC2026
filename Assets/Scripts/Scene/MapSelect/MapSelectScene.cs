using Game;
using UnityEngine;

/// <summary>
/// マップ選択シーン
/// </summary>
public partial class MapSelectScene : SceneBase
{
    public enum EStateType
    {
        None,
        MenuClose,
        Menu,
        ToInGame,
        ToTitle,
    }

    void Start()
    {
        // ロードステートから開始
        ChangeState<LoadState>();
    }

    /// <summary>
    /// ロードステート
    /// </summary>
    public class LoadState : StateBase
    {
        public override void OnEnter()
        {
            // 最初は暗転状態で開始
            FadeManager.Instance.FadeOut(0f);
            // ロードがないので即座に画面表示ステートへ遷移
            _stateMachine.ChangeState<EnterState>();
        }
    }

    /// <summary>
    /// 画面表示ステート
    /// </summary>
    public class EnterState : StateBase
    {
        public override void OnEnter()
        {
            FadeManager.Instance.FadeIn();
        }

        public override void OnUpdate()
        {
            if (!FadeManager.Instance.IsFading)
            {
                _stateMachine.ChangeState<MainState>();
            }
        }
    }

    /// <summary>
    /// 画面非表示ステート
    /// </summary>
    public class ExitState : StateBase
    {
        public override void OnEnter()
        {
            FadeManager.Instance.FadeOut();
        }

        public override void OnUpdate()
        {
            if (!FadeManager.Instance.IsFading)
            {
                _stateMachine.ChangeState<EmptyState>();
            }
        }

        public override void OnExit()
        {
            // シーン切り替え
            GameManager.Instance.ChangeScene();
        }
    }   
}
