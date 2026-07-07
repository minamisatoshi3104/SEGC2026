using Game;
using UnityEngine;

/// <summary>
/// インゲームシーン
/// </summary>
public partial class InGameScene
{
    [SerializeField] InGameController _inGameController;

    /// <summary>
    /// メインステート
    /// </summary>
    public class MainState : StateBase
    {
        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            var scene = _stateMachine.Owner as InGameScene;
            switch (scene._inGameController.NextState)
            {
                case EStateType.Menu: // メニュー
                    break;
                case EStateType.MenuClose: // メニューを閉じる
                    break;
                case EStateType.ToMapSelect:  // タイトルへ
                    GameManager.Instance.SetNextScene("Title");
                    _stateMachine.ChangeState<ExitState>();
                    break;
            }
        }

        public override void OnExit()
        {
        }
    }
}
