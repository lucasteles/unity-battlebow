using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System;

public class MyLobby : MonoBehaviourPunCallbacks
{
    public InputField namefield;
    public GameObject roomPanel;

    public void PlayGame()
    {
        var field = namefield.text;
        if (string.IsNullOrWhiteSpace(field))
            field = $"Indigente {Guid.NewGuid()}";
            
        PhotonNetwork.LocalPlayer.NickName = field;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom() => PhotonNetwork.JoinRandomRoom();

    public override void OnConnectedToMaster() => roomPanel.SetActive(true);

    public override void OnJoinedRoom()
    {
        print($"Conectado na room: {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LoadLevel("Arena");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Não tem nenhuma sala, criando uma...");
        var name = "Sala " + Guid.NewGuid();
        RoomOptions op;
        op = new RoomOptions();
        op.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(name, op, null);
        print($"NomedaSala: {name}");
    }

}
