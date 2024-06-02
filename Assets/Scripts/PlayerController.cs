using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Drive ds;
    float lastTimeMoving = 0;
    Vector3 lastPosition;
    Quaternion lastRotation;
    CheckPointManager cpManager;
    float finishSteer;
    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0;
        this.GetComponent<Ghost>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        ds = this.GetComponent<Drive>();
        this.GetComponent<Ghost>().enabled = false;
        lastPosition = ds.rb.gameObject.transform.position;
        lastRotation = ds.rb.gameObject.transform.rotation;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void Update()
    {
        if (cpManager == null)
            cpManager = ds.rb.GetComponent<CheckPointManager>();

        if (cpManager.lap == RaceMonitor.totalLaps + 1) {
            ds.highAccel.Stop();
            ds.Go(0,finishSteer,0);
            return;
        }

        Debug.Log("OFF ROAD TIMEOUT TIME "+ Time.time +"    "+ lastTimeMoving);
        float a  = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)[1];
        float s = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)[0];
        //float a = Input.GetAxis("Vertical");
        //float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        if (ds.rb.velocity.magnitude > 1 || !RaceMonitor.racing)
            lastTimeMoving = Time.time;

        RaycastHit hit;
        if (Physics.Raycast(ds.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "road") {
                Debug.Log("OFF ROAD TIMEOUT ONROADPOS "+ lastPosition);
                lastPosition = ds.rb.gameObject.transform.position;
                lastRotation = ds.rb.gameObject.transform.rotation;
            }
        }

        if (Time.time > lastTimeMoving + 4 || ds.rb.gameObject.transform.position.y < -5 ) {

            
            Debug.Log("OFF ROAD TIMEOUT");
            ds.rb.gameObject.transform.position = cpManager.LastCP.transform.position + Vector3.up *2;
            ds.rb.gameObject.transform.rotation = cpManager.LastCP.transform.rotation;
            ds.rb.gameObject.layer = 8;
            this.GetComponent<Ghost>().enabled = true;
            lastTimeMoving = Time.time;
            Invoke("ResetLayer", 3);
        }

        if (RaceMonitor.racing == false) a = 0;
        ds.Go(a, s, b);

        ds.CheckForSkid();
        ds.CalculateEngineSound();
    }
}
