using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefabPlayer;
    public Transform spawnPoint;
    
    void Start()
    {
         PhotonNetwork
            .Instantiate(prefabPlayer.name, spawnPoint.position, Quaternion.identity, 0);
    }

    void Update()
    {
        
    }
}
