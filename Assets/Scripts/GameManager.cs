using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour, IPunObservable
{
    public GameObject prefabPlayer;
    public Transform spawnPoint;

    [SerializeField]
    Dictionary<string, int> score = new Dictionary<string, int>();

    public static GameManager Instance;

    public string winningPlayer;
    
    void Awake() => Instance = this;

    void Start()
    {
        PhotonNetwork
            .Instantiate(prefabPlayer.name, spawnPoint.position, Quaternion.identity, 0);
    }

    private void Update()
    {
        winningPlayer = score
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .FirstOrDefault();
    }

    public void AddScore(ArcherController controller)
    {
        if (score.ContainsKey(controller.Id))
            score[controller.Id]++;
        else
            score.Add(controller.Id, 0);
    }
    
    public void Reset(ArcherController controller)
    {
        if (score.ContainsKey(controller.Id))
            score[controller.Id] = 0;
        else
            score.Add(controller.Id, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (Dictionary<string,int>)stream.ReceiveNext();
            print("received!!");
        }
    }
}