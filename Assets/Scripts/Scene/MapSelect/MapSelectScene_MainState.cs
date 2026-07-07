using Game;
using UnityEngine;

/// <summary>
/// マップ選択シーン
/// </summary>
public partial class MapSelectScene
{
    [SerializeField] MapSelectController _mapSelectController;

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
            var scene = _stateMachine.Owner as MapSelectScene;
            switch (scene._mapSelectController.NextState)
            {
                case EStateType.Menu: // メニュー
                    break;
                case EStateType.MenuClose: // メニューを閉じる
                    break;
                case EStateType.ToInGame: // インゲームへ
                    GameManager.Instance.SetNextScene("InGame");
                    _stateMachine.ChangeState<ExitState>();
                    break;
                case EStateType.ToTitle: // タイトルへ
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
