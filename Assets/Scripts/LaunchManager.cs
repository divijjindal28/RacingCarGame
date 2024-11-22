using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;


public class LaunchManager : MonoBehaviourPunCallbacks
{

    byte maxPlayersPerRoom = 4;
    bool iConnecting;
    public TMP_InputField playerName;
    public TextMeshProUGUI feedbackText;
    string gameVersion = "1";


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("PlayerName"))
            playerName.text = PlayerPrefs.GetString("PlayerName");
    }

    public void ConnectNetwork() {
        feedbackText.text = "";
        iConnecting = true;

        PhotonNetwork.NickName = playerName.text;
        if (PhotonNetwork.IsConnected)
        {
            feedbackText.text += "\n JoiningRoom...";
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            feedbackText.text += "\n Connecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SetName(string name) {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void ConnectSingle() {
        SceneManager.LoadScene("CartRacing");
    }


    //////Network Callbacks
    ///
    public override void OnConnectedToMaster()
    {
        if (iConnecting) {
            feedbackText.text += "\n OnConnectedToMaster...";
            PhotonNetwork.JoinRandomRoom();
            
        }
    }

    public override void OnJoinRandomFailed(short returnCode,string message)
    {
        feedbackText.text += "\n FailedToJoinRandomRoom...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedbackText.text += "\n Disconnected because + "+ cause;
        iConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text += "\n Joined Room with + " + PhotonNetwork.CurrentRoom.PlayerCount + " players.";
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("CartRacing");
    }

    void OnLevelWasLoaded(int level)
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
}
