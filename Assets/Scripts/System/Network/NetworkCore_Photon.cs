using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkCore_Photon : INetworkCore
{
    public INetworkCore.NetworkState State { get; private set; }

    /// <summary>
    /// サーバーに接続
    /// </summary>
    /// <param name="address"></param>
    /// <param name="port"></param>
    public void Connect(string address, int port)
    {
        State = INetworkCore.NetworkState.Connecting;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// サーバーから切断
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// ルーム作成
    /// </summary>
    /// <param name="roomName"></param>
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    /// <summary>
    /// ルームに参加
    /// </summary>
    /// <param name="roomName"></param>
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    /// <summary>
    /// ルームから退出
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// メッセージ送信
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(string message)
    {
        PhotonNetwork.RaiseEvent(0, message, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }
}
