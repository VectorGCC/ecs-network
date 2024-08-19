using System;
using System.Collections.Generic;
using Ragon.Client;
using Ragon.Client.Unity;
using Ragon.Protocol;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameNetwork : MonoBehaviour, IRagonListener
{
    private void Start()
    {
        RagonNetwork.AddListener(this);
        RagonNetwork.Connect();
    }

    public void OnAuthorizationSuccess(RagonClient client, string playerId, string playerName)
    {
        Debug.Log("Authorized");
        RagonNetwork.Session.CreateOrJoin("RagonNetworkScene", 1, 20);
    }

    public void OnAuthorizationFailed(RagonClient client, string message)
    {
        Debug.Log("Unauthorized");
    }

    public void OnConnected(RagonClient client)
    {
        Debug.Log("Connected");

        var randomName = $"Player {Random.Range(100, 999)}";
        RagonNetwork.Session.AuthorizeWithKey("defaultkey", randomName, string.Empty);
    }

    public void OnDisconnected(RagonClient client, RagonDisconnect reason)
    {
        Debug.Log("Disconnected");
    }

    public void OnFailed(RagonClient client, string message)
    {
        Debug.Log("Failed");
    }

    public void OnJoined(RagonClient client)
    {
        Debug.Log("Joined");
    }

    public void OnLeft(RagonClient client)
    {
        Debug.Log("Left");
    }

    public void OnOwnershipChanged(RagonClient client, RagonPlayer player)
    {
        Debug.Log($"Room ownership changed");
    }

    public void OnPlayerJoined(RagonClient client, RagonPlayer player)
    {
        Debug.Log($"Player joined");
    }

    public void OnPlayerLeft(RagonClient client, RagonPlayer player)
    {
        Debug.Log($"Player left");
    }

    public void OnSceneLoaded(RagonClient client)
    {
        Debug.Log($"Level: {client.Room.Scene}");
        RagonNetwork.Room.SceneLoaded();
    }

    public void OnRoomListUpdate(RagonClient client, IReadOnlyList<RagonRoomInformation> roomsInfos)
    {
        Debug.Log("OnRoomListUpdate");
    }

    public void OnUserDataUpdated(RagonClient client, IReadOnlyList<string> changes)
    {
        Debug.Log("OnUserDataUpdated");
    }

    public void OnPlayerUserDataUpdated(RagonClient client, RagonPlayer player, IReadOnlyList<string> changes)
    {
        Debug.Log("OnPlayerUserDataUpdated");
    }
}