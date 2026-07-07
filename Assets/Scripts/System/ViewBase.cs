using UnityEngine;
using ZZ;

public class ViewBase : MonoBehaviour
{
    void Awake()
    {
        // ボタン登録
        var buttons = GetComponentsInChildren<ZZButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClickWithType.AddListener(OnButtonClick);
        }
    }

    /// <summary>
    /// ボタンがクリックされた
    /// </summary>
    /// <param name="buttonType"></param>
    protected virtual void OnButtonClick(EButtonType buttonType) { }
}
