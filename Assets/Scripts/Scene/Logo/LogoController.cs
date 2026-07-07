using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ロゴの表示管理
/// </summary>
public class LogoController : MonoBehaviour
{
    [Serializable]
    class LogoData
    {
        public Image LogoImg;   // ロゴの画像
        public bool CanSkip;    // スキップ可能か
    }

    enum EState
    {
        None,
        Preparing,  // 準備
        Playing,    // 再生中
    }

    readonly int AnimHash_TriggerStart = Animator.StringToHash("Start");
    readonly int AnimHash_TriggerEnd = Animator.StringToHash("End");

    public bool IsBusy { get { return _state != EState.None; } }

    [SerializeField] Animator _animator;    // アニメーター
    [SerializeField] List<LogoData> _logoDataList = new List<LogoData>();   // ロゴデータリスト
    [SerializeField] float _fadeTime = 0.5f;    // フェード時間
    [SerializeField] float _displayTime = 2.0f; // ロゴ表示時間

    EState _state = EState.None;    // 状態
    int _currentLogoIndex;  // 現在のロゴIndex
    Coroutine _logoCoroutine;   // ロゴ表示コルーチン

    private void Awake()
    {
        // ロゴ画像を透明にする
        for (int i = 0; i < _logoDataList.Count; i++)
        {
            Color color = _logoDataList[i].LogoImg.color;
            color.a = 0f;
            _logoDataList[i].LogoImg.color = color;
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case EState.None:
                break;
            case EState.Preparing:
                LogoData logoData = GetLogoData();
                if (logoData != null)
                {
                    // ロゴ表示処理開始
                    _logoCoroutine = GameManager.Instance.StartCoroutine(LogoViewProc(logoData));
                    _state = EState.Playing;
                }
                else
                {
                    // 終了
                    _animator.SetTrigger(AnimHash_TriggerEnd);
                    _state = EState.None;
                }
                break;
            case EState.Playing:
                // ロゴ表示処理終了チェック
                if (_logoCoroutine == null)
                {
                    // 次のロゴへ
                    _currentLogoIndex++;
                    _state = EState.Preparing;
                }
                break;
        }
    }

    /// <summary>
    /// 処理開始
    /// </summary>
    public void Play()
    {
        // パラメータ初期化
        _currentLogoIndex = 0;
        // 開始
        _animator.SetTrigger(AnimHash_TriggerStart);
        _state = EState.Preparing;
    }

    /// <summary>
    /// ロゴデータ取得
    /// </summary>
    /// <returns></returns>
    private LogoData GetLogoData()
    {
        LogoData result = null;
        if (0 <= _currentLogoIndex && _currentLogoIndex < _logoDataList.Count)
        {
            result = _logoDataList[_currentLogoIndex];
        }
        return result;
    }

    /// <summary>
    /// ロゴ表示処理
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator LogoViewProc(LogoData data)
    {
        if (data == null) yield break;
        // フェードイン
        float fadeInSec = 0f;
        while (fadeInSec < _fadeTime)
        {
            fadeInSec += Time.deltaTime;
            float rate = Mathf.Clamp01(fadeInSec / _fadeTime);
            Color color = data.LogoImg.color;
            color.a = Mathf.Lerp(0f, 1f, rate);
            data.LogoImg.color = color;
            yield return null;
        }
        // 待機
        float wait = 0f;
        while (wait < _displayTime)
        {
            wait += Time.deltaTime;
            // スキップ
            if (data.CanSkip && Input.anyKeyDown)
            {
                break;
            }
            yield return null;
        }
        // フェードアウト
        float fadeOutSec = 0f;
        while (fadeOutSec < _fadeTime)
        {
            fadeOutSec += Time.deltaTime;
            float rate = Mathf.Clamp01(fadeOutSec / _fadeTime);
            Color color = data.LogoImg.color;
            color.a = Mathf.Lerp(1f, 0f, rate);
            data.LogoImg.color = color;
            yield return null;
        }
        // 終了処理
        _logoCoroutine = null;
    }
}
