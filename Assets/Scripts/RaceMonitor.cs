using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    CheckPointManager[] carsCPM;
    public static bool racing = false;
    public static int totalLaps = 1;
    public GameObject[] carPrefabs;
    public Transform[] spawnPos;
    public GameObject gameOverPannel;
    public GameObject HUD;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject g in countDownItems)
            g.SetActive(false);
        StartCoroutine(PlayCountDown());
        gameOverPannel.SetActive(false);

        foreach (Transform t in spawnPos) {
            GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)]);
            car.transform.position = t.position;
            car.transform.rotation = t.rotation;
        }

        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        carsCPM = new CheckPointManager[cars.Length];
        for(int i = 0; i< cars.Length; i++)
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
