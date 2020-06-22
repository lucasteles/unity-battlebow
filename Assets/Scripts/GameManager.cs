using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefabPlayer;
    public Transform spawnPoint;

    public static GameManager Instance;

    public IList<ArcherController> players = new List<ArcherController>();
    
    public string winningPlayer;
    
    void Awake() => Instance = this;
    public void AddPlayer(ArcherController player) => players.Add(player);
    
    void Start()
    {
        PhotonNetwork
            .Instantiate(prefabPlayer.name, spawnPoint.position, Quaternion.identity, 0);
    }

    private void Update()
    {
        winningPlayer = players
            .Where(x => x.score > 0)
            .OrderByDescending(x => x.score)
            .Select(x => x.Id)
            .FirstOrDefault();
    }

    
}