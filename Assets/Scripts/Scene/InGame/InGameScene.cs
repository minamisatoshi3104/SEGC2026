using Game;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// インゲームシーン
/// </summary>
public partial class InGameScene : SceneBase
{
    public enum EStateType
    {
        None,
        Menu,
        MenuClose,
        ToMapSelect,
    }

    [SerializeField] private CinemachineCamera _playerCamera;   // プレイヤーカメラ

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
            // ネットワーク接続
            GameManager.Instance.NetworkManager.Connect();
        }

        public override void OnUpdate()
        {
            var scene = _stateMachine.Owner as InGameScene;
            // 入室に成功したら画面表示ステートへ遷移
            if (GameManager.Instance.NetworkManager.State == NetworkManager.NetworkState.JoinedRoom)
            {
                // プレイヤーを生成
                var position = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
                var player = PhotonNetwork.Instantiate("Player", position, Quaternion.identity);
                // カメラのターゲットをプレイヤーに設定
                scene._playerCamera.Follow = player.transform;
                // 入室成功ダイアログを表示
                DialogManager.Instance.Open(new DialogManager.Option()
                {
                    Type = DialogManager.DialogType.Time,
                    Title = "入室成功",
                    Content = "入室に成功しました。"
                });
                // 画面表示ステートへ遷移
                _stateMachine.ChangeState<EnterState>();
            }
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

    /// <summary>
    /// 何もないステート
    /// </summary>
    public class EmptyState : StateBase
    {
    }   
}
