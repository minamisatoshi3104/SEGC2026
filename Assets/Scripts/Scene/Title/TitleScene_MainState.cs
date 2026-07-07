using Game;
using UnityEngine;

/// <summary>
/// タイトルシーン
/// </summary>
public partial class TitleScene
{
    [SerializeField] TitleController _titleController;

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
            var scene = _stateMachine.Owner as TitleScene;
            switch (scene._titleController.NextState)
            {
                case EStateType.Start: // マップ選択へ
                    GameManager.Instance.SetNextScene("MapSelect");
                    _stateMachine.ChangeState<ExitState>();
                    break;
            }
        }

        public override void OnExit()
        {
        }
    }
}
