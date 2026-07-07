using UnityEngine;

public interface INetworkCore
{
    public enum NetworkState
    {
        Disconnected,
        Connecting,
        Connected,
        JoiningRoom,
        InRoom,
    }
    public NetworkState State { get; }

    /// <summary>
    /// サーバーに接続
    /// </summary>
    /// <param name="address"></param>
    /// <param name="port"></param>
    public void Connect(string address, int port);

    /// <summary>
    /// サーバーから切断
    /// </summary>
    public void Disconnect();

    /// <summary>
    /// ルーム作成
    /// </summary>
    /// <param name="roomName"></param>
    public void CreateRoom(string roomName);

    /// <summary>
    /// ルームに参加
    /// </summary>
    /// <param name="roomName"></param>
    public void JoinRoom(string roomName);

    /// <summary>
    /// ルームから退出
    /// </summary>
    public void LeaveRoom();

    /// <summary>
    /// メッセージ送信
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(string message);
}
