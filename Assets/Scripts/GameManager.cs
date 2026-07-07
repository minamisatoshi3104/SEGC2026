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

        // ゲーム中のデータ
        public GameData GameData { get; private set; } = new GameData();
        public class UpdateGameDataEvent : UnityEvent<GameData> { }
        public UpdateGameDataEvent OnUpdateGameData = new UpdateGameDataEvent();

        // SOData
        public SOGameData SOGameData;

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
            }
            Load();
        }

        private void OnDestroy()
        {
            Save();
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

        /// <summary>
        /// セーブ
        /// </summary>
        private void Save()
        {
            //PlayerPrefs.SetInt("Score", GameData.Score);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// ロード
        /// </summary>
        private void Load()
        {
            //GameData.Score = PlayerPrefs.GetInt("Score", 0);
        }
    }

    /// <summary>
    /// ゲーム中のデータ
    /// </summary>
    public class GameData
    {
        //public int Score { get; set; }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset()
        {
            //Score = 0;
        }
    }
}