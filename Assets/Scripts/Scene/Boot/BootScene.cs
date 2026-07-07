using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 起動シーン
/// </summary>
public partial class BootScene : SceneBase
{
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
        List<AsyncOperationHandle> _handleList = new List<AsyncOperationHandle>();

        public override void OnEnter()
        {
            // 最初は暗転状態で開始
            FadeManager.Instance.FadeOut(0f);
            // 読み込み開始
            _handleList.Add(Addressables.InstantiateAsync("GameManager"));
        }

        public override void OnUpdate()
        {
            // 読み込み完了チェック
            if (_handleList.All(item => item.IsDone))
            {
                // ロゴシーンへ
                GameManager.Instance.SetNextScene("Logo");
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
