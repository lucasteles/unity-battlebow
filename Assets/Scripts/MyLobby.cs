using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Linq;

public class MyLobby : MonoBehaviourPunCallbacks
{
    public InputField namefield;
    public GameObject roomPanel;

    public GameObject joinButton;
    public GameObject gameRoomContent;
    public List<PlayerUIRoom> playerUIRooms = new List<PlayerUIRoom>();
    public PhotonView pview;

    bool started = false;

    void Awake()
    {
        pview = GetComponent<PhotonView>();
    }

    void Start()
    {
        InvokeRepeating(nameof(CheckAllReady), 1, 1);
    }

    public void PlayGame()
    {
        var field = namefield.text;
        if (string.IsNullOrWhiteSpace(field))
            field = $"Fulano {Guid.NewGuid()}";

        PhotonNetwork.LocalPlayer.NickName = field;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom()
    {
        joinButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster() => roomPanel.SetActive(true);

    public override void OnJoinedRoom()
    {
        var ob = PhotonNetwork.Instantiate("PlayerUIRoom", Vector3.zero, Quaternion.identity);
        ob.transform.SetParent(gameRoomContent.transform, false);
        Invoke(nameof(FixName), 2);

        print($"Conectado na room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => Invoke(nameof(FixName), 1);

    void FixName()
    {
        print($"searching {gameRoomContent.name}");
        var obs = GameObject.FindGameObjectsWithTag("UIPlayer");
        foreach (var ob in obs)
        {
            ob.transform.SetParent(gameRoomContent.transform, false);
            var component = ob.GetComponent<PlayerUIRoom>();
            if (!playerUIRooms.Contains(component))
                playerUIRooms.Add(component);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Não tem nenhuma sala, criando uma...");
        var name = $"Sala {Guid.NewGuid()}";
        var op = new RoomOptions {MaxPlayers = 6};
        PhotonNetwork.CreateRoom(name, op, null);
        print($"Nome da Sala: {name}");
    }

    void CheckAllReady()
    {
        var allready = false;
        if (playerUIRooms.Any())
            allready = playerUIRooms.All(x => x.bready); //Soluçao LucasTeles

        if (!allready || started) return;
        started = true;
        pview.RPC(nameof(BroadcastLoadScene), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void BroadcastLoadScene() => PhotonNetwork.LoadLevel("Arena");
}