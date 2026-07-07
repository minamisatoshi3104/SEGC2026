using Game;
using UnityEngine;

/// <summary>
/// ロゴシーン
/// </summary>
public partial class LogoScene
{
    [SerializeField] LogoController _logoController;

    /// <summary>
    /// メインステート
    /// </summary>
    public class MainState : StateBase
    {
        public override void OnEnter()
        {
            var scene = _stateMachine.Owner as LogoScene;
            scene._logoController.Play();
        }

        public override void OnUpdate()
        {
            var scene = _stateMachine.Owner as LogoScene;
            if (!scene._logoController.IsBusy)
            {
                // タイトルへ
                GameManager.Instance.SetNextScene("Title");
                _stateMachine.ChangeState<ExitState>();
            }
        }

        public override void OnExit()
        {
        }
    }
}
