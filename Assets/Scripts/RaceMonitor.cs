using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class RaceMonitor : MonoBehaviourPunCallbacks
{
    public GameObject mainCamera;
    public GameObject[] countDownItems;
    CheckPointManager[] carsCPM;
    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject[] carPrefabs;
    public Transform[] spawnPos;
    public GameObject gameOverPannel;
    public GameObject HUD;
    public GameObject StartRace;
    public GameObject WaitingText;
    int playerCar;

    GameObject pcar = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDelayMethod());


    }

    IEnumerator StartDelayMethod() {
        yield return new WaitForSeconds(1f);
        mainCamera.GetComponent<SmoothFollowNew>().enabled = true;
        foreach (GameObject g in countDownItems)
            g.SetActive(false);

        gameOverPannel.SetActive(false);

        StartRace.SetActive(false);
        WaitingText.SetActive(false);
        playerCar = PlayerPrefs.GetInt("PlayerCar");
        int randomSTartPos = Random.Range(0, spawnPos.Length);
        Vector3 startPos = spawnPos[randomSTartPos].position;
        Quaternion startRot = spawnPos[randomSTartPos].rotation;

        if (PhotonNetwork.IsConnected)
        {

            startPos = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].position;
            startRot = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation;
            byte num = (byte)(PhotonNetwork.CurrentRoom.PlayerCount - 1);
            if (NetworkedPlayer.LocalPlayerInstance == null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    pcar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPos, startRot, 0);
                }
                else
                {
                    
                    pcar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPos, startRot, 0);
                }

            }
            if (PhotonNetwork.IsMasterClient)
            {
                StartRace.SetActive(true);
            }
            else
            {
                WaitingText.SetActive(true);
            }
        }
        else
        {

            pcar = Instantiate(carPrefabs[playerCar]);
            pcar.transform.position = startPos;
            pcar.transform.rotation = startRot;

            foreach (Transform t in spawnPos)
            {
                if (t == spawnPos[randomSTartPos]) continue;
                GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)]);
                car.transform.position = t.position;
                car.transform.rotation = t.rotation;
            }

            StartGame();
        }

        //yield return new WaitUntil(()=> pcar != null);
        SmoothFollowNew.PlayerCar = pcar.gameObject.GetComponent<Drive>().rb.transform;
        pcar.GetComponent<AIController>().enabled = false;
        pcar.GetComponent<Drive>().enabled = true;
        pcar.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator  InstanciateDelay(Vector3 startPos, Quaternion startRot) {
        yield return new WaitForSeconds(5.0f);
        pcar = PhotonNetwork.Instantiate(carPrefabs[playerCar].name, startPos, startRot, 0);
    }

    public void BeginGame() {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
    }

    [PunRPC]
    public void StartGame() {
        StartCoroutine(PlayCountDown());
        StartRace.SetActive(false);
        WaitingText.SetActive(false);
        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        carsCPM = new CheckPointManager[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsCPM[i] = cars[i].GetComponent<CheckPointManager>();
        }
    }

    IEnumerator PlayCountDown() {
        yield return new WaitForSeconds(2);
        foreach (GameObject g in countDownItems)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }
        racing = true;

    }

    public void RestartLevel() {
        racing = false;
        SceneManager.LoadScene("CartRacing");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!racing) return;
        int finishedCount = 0;
        foreach (CheckPointManager cpm in carsCPM) {
            if (cpm.lap == totalLaps + 1)
                finishedCount++;
            if (finishedCount == carsCPM.Length) {
                HUD.SetActive(false);
                gameOverPannel.SetActive(true);
            }
        }
    }
}
