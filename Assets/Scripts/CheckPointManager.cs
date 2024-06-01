using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public int lap = 0;
    public int checkPoint = -1;
    int checkPointCount;
    int nextCheckpoint;
    public GameObject LastCP;
    public float timeEntered;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] cps = GameObject.FindGameObjectsWithTag("CheckPoint");
        checkPointCount = cps.Length;

        foreach (GameObject cp in cps) {
            if (cp.name == "0") {
                LastCP = cp;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "CheckPoint") {
            int thisCPNumber = int.Parse(col.gameObject.name);
            if (thisCPNumber == nextCheckpoint) {

                LastCP = col.gameObject;
                checkPoint = thisCPNumber;
                timeEntered = Time.time;
                if (checkPoint == 0) lap++;

                nextCheckpoint++;
                if (nextCheckpoint >= checkPointCount)
                    nextCheckpoint = 0;
            }
        }
    }
}
