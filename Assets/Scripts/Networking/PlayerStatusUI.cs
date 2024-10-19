using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private int playerIndex;

    [SerializeField]
    private GameObject connected, waiting;

    [SerializeField]
    private TextMeshProUGUI playerName, playerScore;
    [SerializeField]
    private Image playerIcon;

    private Player player;

    private void Start()
    {
        // We default at a waiting state
        ShowConnectionUI(false);
        PlayerNumbering.OnPlayerNumberingChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        //Check if there is an available player with our index
        if (playerIndex <= PhotonNetwork.PlayerList.Length - 1)
        {
            player = PhotonNetwork.PlayerList[playerIndex];
            int playerNumber = player.GetPlayerNumber();
            //It's possible to get -1 as player number when it is not fully initialized
            //So let's add a check
            if (playerNumber == -1) return;

            ShowConnectionUI(true);
            playerName.text = player.NickName;
            playerIcon.sprite = NetworkManager.Instance.GetPlayerIcon(playerNumber);
        }
        else
        {
            // Player disconnected 
            ShowConnectionUI(false);
        }
    }

    private void ShowConnectionUI(bool isConnected)
    {
        waiting.SetActive(!isConnected);
        connected.SetActive(isConnected);
    }
}
