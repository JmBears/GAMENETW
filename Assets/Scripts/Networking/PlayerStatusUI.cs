using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour, IOnEventCallback
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

    // Make sure to add/remove the callback target so we can receive photon raise events
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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

    public void OnEvent(EventData photonEvent)
    {
        // Filter out the byte code that we want to listen to
        byte eventCode = photonEvent.Code;
        Debug.Log("Received event Code: " + eventCode);
        if (eventCode != NetworkManager.SCORE_UPDATED_EVENT_CODE) return;
        // Get the data from the paramets passed from the RaiseEvent
        object[] data = (object[])photonEvent.CustomData;
        Debug.Log("Received data: " + data.Length);
        Player receivedPlayer = (Player)data[0];
        Debug.Log("Received player: " + receivedPlayer);
        if (receivedPlayer != null)
        {
            UpdateScore(receivedPlayer);
        }
    }

    private void UpdateScore(Player player)
    {
        // Check whether the player who scored has the same index
        if(player.GetPlayerNumber() == playerIndex)
        {
            playerScore.text = player.GetScore().ToString();
        }
    }

    private void ShowConnectionUI(bool isConnected)
    {
        waiting.SetActive(!isConnected);
        connected.SetActive(isConnected);
    }

   
}
