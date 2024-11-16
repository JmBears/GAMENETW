using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    public GameObject PlayerList;

    [Header("Options")] 
    public float refreshRate = 1.0f;

    [Header("UI")] 
    public GameObject[] slots;

    [Space] 
    public TextMeshProUGUI[] scoreText;
    public TextMeshProUGUI[] playerText;

    private void Start()
    {

    }
    
    public void Refresh()
    {
        photonView.RPC("RPCRefresh", RpcTarget.AllBuffered);
    }

    [PunRPC]

    public void RPCRefresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            if (player.NickName == "")
            {
                player.NickName = "Nobody";
            }

            playerText[i].text = player.NickName;
            scoreText[i].text = player.GetScore().ToString();

            i++;
        }
    }

    
}

