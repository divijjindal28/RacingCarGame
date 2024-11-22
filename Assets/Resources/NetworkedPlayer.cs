using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class NetworkedPlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    public GameObject playerNamePrefab;
    public Rigidbody rb;
    public Renderer jeepMesh;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        else {
            GameObject playerName = Instantiate(playerNamePrefab);
            playerName.GetComponent<NameUIController1>().target = rb.gameObject.transform;
            playerName.GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
            playerName.GetComponent<NameUIController1>().carRend = jeepMesh;
        }
    }
}
