using Game;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using ZZ;

public class DialogManager : MonoBehaviour
{
    /// <summary>
    /// ダイアログの種類
    /// </summary>
    public enum DialogType
    {
        None,
        Time,
        OK,
        YesNo,
    }

    /// <summary>
    /// ダイアログのオプション
    /// </summary>
    public class Option
    {
        public DialogType Type = DialogType.None;
        public float DisplaySec = 3f;
        public string Title = "";
        public string Content = "";
        public UnityAction<EButtonType> Callback = null;
    }

    public static DialogManager Instance { get; private set; }

    private readonly int AnimHash_BoolIsOpen = Animator.StringToHash("IsOpen"); // 開いているか

    public bool IsOpen { get; private set; } = false;   // ダイアログが開いているか
    public EButtonType ResultType { get; private set; } = EButtonType.None; // ダイアログの結果のボタンの種類

    [SerializeField] private ZZButton _okButton;    // OKボタン
    [SerializeField] private ZZButton _yesButton;   // YESボタン
    [SerializeField] private ZZButton _noButton;    // NOボタン
    [SerializeField] private GameObject _titleRoot; // タイトルルート
    [SerializeField] private TextMeshProUGUI _titleText;    // タイトルテキスト
    [SerializeField] private TextMeshProUGUI _contentText;  // 内容テキスト

    private Animator _animator; // アニメーター
    private Option _currentOption;  // 現在のダイアログのオプション
    private float _displayTimer;   // 表示タイマー

    void Awake()
    {
        // シングルトン化
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
        _animator = GetComponent<Animator>();
        // ボタンのクリックイベント登録
        _okButton.onClickWithType.AddListener(OnClickButton);
        _yesButton.onClickWithType.AddListener(OnClickButton);
        _noButton.onClickWithType.AddListener(OnClickButton);
    }

    private void Update()
    {
        if (IsOpen && _currentOption != null)
        {
            // タイマー処理
            if (_currentOption.Type == DialogType.Time)
            {
                _displayTimer -= Time.deltaTime;
                if (_displayTimer <= 0f)
                {
                    ResultType = EButtonType.None;
                    Close();
                }
            }
        }
    }

    /// <summary>
    /// ダイアログを開く
    /// </summary>
    public void Open(Option option)
    {
        // すでに開いている または 設定が不十分の場合は無視
        if (IsOpen || option.Type == DialogType.None)
        {
            Close();
            return;
        }
        // オプションを保存
        _currentOption = option;
        _okButton.gameObject.SetActive(option.Type == DialogType.OK);
        _yesButton.gameObject.SetActive(option.Type == DialogType.YesNo);
        _noButton.gameObject.SetActive(option.Type == DialogType.YesNo);
        _titleRoot.SetActive(!string.IsNullOrEmpty(option.Title));
        _titleText.text = option.Title;
        _contentText.text = option.Content;
        _displayTimer = option.DisplaySec;
        // ダイアログを開く
        IsOpen = true;
        _animator.SetBool(AnimHash_BoolIsOpen, true);
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    public void Close()
    {
        // コールバックを呼び出す
        _currentOption.Callback?.Invoke(ResultType);
        // 初期化
        _currentOption = null;
        // ダイアログを閉じる
        IsOpen = false;
        _animator.SetBool(AnimHash_BoolIsOpen, false);
    }

    /// <summary>
    /// ボタンがクリックされた
    /// </summary>
    /// <param name="buttonType"></param>
    private void OnClickButton(EButtonType buttonType)
    {
        // 結果のボタンの種類を保存して閉じる
        ResultType = buttonType;
        Close();
    }
}
