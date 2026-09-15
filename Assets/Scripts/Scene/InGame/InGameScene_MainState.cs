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
            // 接続が切れたらタイトルへ戻る
            if (GameManager.Instance.NetworkManager.State == NetworkManager.NetworkState.Disconnected)
            {
                _stateMachine.ChangeState<ViewDialogState>();
            }
            // ステート変更
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

    /// <summary>
    /// ダイアログ表示ステート
    /// </summary>
    public class ViewDialogState : StateBase
    {
        public override void OnEnter()
        {
            DialogManager.Instance.Open(new DialogManager.Option()
            {
                Type = DialogManager.DialogType.OK,
                Title = "接続切断",
                Content = "接続が切断されました。\nタイトルへ戻ります。",
                Callback = (result) =>
                {
                    GameManager.Instance.SetNextScene("Title");
                    _stateMachine.ChangeState<ExitState>();
                }
            });
        }
    }
}
