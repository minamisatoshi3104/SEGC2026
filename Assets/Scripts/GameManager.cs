using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace Game
{
    /// <summary>
    /// ゲームの管理
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // シーン管理
        public string PrevSceneName { get; private set; }
        private string _nextSceneName;

        // イベント
        public class UpdateGameDataEvent : UnityEvent<SaveData> { }
        public UpdateGameDataEvent OnUpdateGameData = new UpdateGameDataEvent();

        // SOData
        public SOGameData SOGameData;

        // 管理クラス
        private NetworkManager _networkManager;
        private SaveDataManager _saveDataManager;
        public NetworkManager NetworkManager => _networkManager;
        public SaveDataManager SaveDataManager => _saveDataManager;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            // コンポーネント取得
            _networkManager = GetComponent<NetworkManager>();
            _saveDataManager = GetComponent<SaveDataManager>();
            // セーブデータ読み込み
            _saveDataManager.Load();
        }

        private void OnDestroy()
        {
            // セーブデータ保存
            _saveDataManager.Save();
        }

        /// <summary>
        /// 次のシーン名を設定する
        /// </summary>
        /// <param name="sceneName"></param>
        public void SetNextScene(string sceneName)
        {
            _nextSceneName = sceneName;
        }

        /// <summary>
        /// シーンを切り替える
        /// </summary>
        public void ChangeScene()
        {
            PrevSceneName = _nextSceneName;
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}