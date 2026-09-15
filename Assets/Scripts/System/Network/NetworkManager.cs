using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public enum NetworkState
    {
        None,
        Connecting,
        Connected,
        JoiningRoom,
        JoinedRoom,
        Disconnected,
    }
    public NetworkState State { get; private set; } = NetworkState.None;

    public void Connect()
    {
        //マスターサーバーに接続
        State = NetworkState.Connecting;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        // マスターサーバーから切断
        State = NetworkState.Disconnected;
    }

    public void SendMessage()
    {

    }

    /// <summary>
    /// マスターサーバーに接続成功した時に呼ばれる
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // 接続完了
        State = NetworkState.Connected;
        //Roomという名前のルームを作成する、既存の場合は参加する
        State = NetworkState.JoiningRoom;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    /// <summary>
    /// ルームへの接続が成功したら呼ばれる
    /// </summary>
    public override void OnJoinedRoom()
    {
        // 入室完了
        State = NetworkState.JoinedRoom;
        //Playerを生成する座量をランダムに決める
        var position = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
        //Resourcesフォルダから"Player"を探してきてそれを生成
        PhotonNetwork.Instantiate("Player", position, Quaternion.identity);
    }
}
