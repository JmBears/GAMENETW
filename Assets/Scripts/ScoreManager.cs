using Photon.Realtime;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ScoreManager : SingletonPUN<ScoreManager>
{
    public void AddScore(int value, Player player)
    {
        player.AddScore(value);
        // We pass in the data of the player in the event
        object[] data = new[] { player };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        Debug.Log($"Raise Event: {NetworkManager.SCORE_UPDATED_EVENT_CODE}");
        PhotonNetwork.RaiseEvent(NetworkManager.SCORE_UPDATED_EVENT_CODE, data, options, SendOptions.SendUnreliable);
    }

    
}
