using Game;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DisplayGameDataText : MonoBehaviour
{
    enum GameDataType
    {
        Score,
        AddScoreSec,
        AddScoreTap,
    }

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameDataType _type;

    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.OnUpdateGameData.AddListener(Display);
        Display(GameManager.Instance.SaveDataManager.Data);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUpdateGameData.RemoveListener(Display);
    }

    public void Display(SaveData data)
    {
        switch (_type)
        {
            case GameDataType.Score:
                //_text.text = data.Score.ToString();
                break;
        }
    }
}
