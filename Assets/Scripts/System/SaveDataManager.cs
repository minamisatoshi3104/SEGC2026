using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public SaveData Data { get; private set; } = new SaveData();

    /// <summary>
    /// セーブ
    /// </summary>
    public void Save()
    {
        //PlayerPrefs.SetInt("Score", GameData.Score);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ロード
    /// </summary>
    public void Load()
    {
        //GameData.Score = PlayerPrefs.GetInt("Score", 0);
    }
}


/// <summary>
/// セーブデータ
/// </summary>
public class SaveData
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