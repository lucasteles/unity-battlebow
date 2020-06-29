using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIRoom : MonoBehaviour
{
    public PhotonView pview;
    public Text text;
    public Toggle ready;
    public bool bready = false;

    void Start()
    {
        if (!pview.IsMine)
        {
            ready.interactable = false;
        }

        text.text = pview.Owner.NickName;
    }


    public void ReadyChange()
    {
        bready = ready.isOn;
        pview.RPC(nameof(StatusChanged), RpcTarget.OthersBuffered, bready);
    }

    [PunRPC]
    void StatusChanged(bool mybready)
    {
        bready = mybready;
        ready.isOn = bready;
    }
}