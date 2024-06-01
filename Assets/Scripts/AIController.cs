using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public Circuit circuit;
    Drive ds;
    public float steeringSenstivity = 0.01f;
    public float brakeSentivity = 1;
    public float accelSentivity = 0.3f;
    Vector3 target;
    float totalDistanceToTarget;

    GameObject tracker;
    int currentTrackerWP = 0;
    public float lookAhead = 10.0f;

    Ghost ghostScript;


    float lastTimeMoving = 0;
    CheckPointManager cpManager;
    float finishSteer;
    // Start is called before the first frame update
    void Start()
    {
        if (circuit == null) {
            circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>();
        }
        ds = this.GetComponent<Drive>();
        target = circuit.waypoints[currentTrackerWP].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = ds.rb.gameObject.transform.position;
        tracker.transform.rotation = ds.rb.gameObject.transform.rotation;

        ghostScript = gameObject.GetComponent<Ghost>();
        ghostScript.enabled = false;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }


    void ProgressTracker() {
        Debug.DrawLine(ds.rb.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(ds.rb.gameObject.transform.position, tracker.transform.position) > lookAhead) { return;}
        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f);

        if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.waypoints.Length)
            {
                currentTrackerWP = 0;
            }
            //target = circuit.waypoints[currentTrackerWP].transform.position;
        }
    }


    void ResetLayer() {
        ds.rb.gameObject.layer = 0;
        ghostScript.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (RaceMonitor.racing == false) {
            lastTimeMoving = Time.time;
            return;
        }

        if (cpManager == null)
        {
            cpManager = ds.rb.GetComponent<CheckPointManager>();
        }

        if (cpManager.lap == RaceMonitor.totalLaps + 1) {
            ds.highAccel.Stop();
            ds.Go(0, finishSteer, 0);
            return;
        }
        ProgressTracker();
        Vector3 localTarget;
        float targetAngle;

        if (ds.rb.velocity.magnitude > 1)
            lastTimeMoving = Time.time;

        if (Time.time > (lastTimeMoving + 4) || ds.rb.gameObject.transform.position.y < -5) {
            
            ds.rb.gameObject.transform.position = cpManager.LastCP.transform.position + Vector3.up * 2;
            ds.rb.gameObject.transform.rotation = cpManager.LastCP.transform.rotation;
            //ds.rb.gameObject.transform.position = circuit.waypoints[currentTrackerWP].transform.position + Vector3.up * 2;
            Debug.Log("Waypoint : " + ds.rb.gameObject.transform.root.name + " " + currentTrackerWP);
            tracker.transform.position = cpManager.transform.position;
            ds.rb.gameObject.layer = 8;
            ghostScript.enabled = true;
            Invoke("ResetLayer",3);
            lastTimeMoving = Time.time;
        }

        if (Time.time < ds.rb.GetComponent<AvoidDetector>().avoidTime)
        {
            localTarget = tracker.transform.right * ds.rb.GetComponent<AvoidDetector>().avoidPath;
        }
        else {
            localTarget = ds.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        }

        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSenstivity,-1,1) * Mathf.Sign(ds.currentSpeed);

        float speedFactor = ds.currentSpeed / ds.maxSpeed;
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90;

        float brake = 0;
        if (corner > 10 && speedFactor > 0.1f) {
            brake = Mathf.Lerp(0, 1 + speedFactor * brakeSentivity, cornerFactor);
        }

        float accel = 1f;
        if (corner > 20 && speedFactor > 0.2f)
        {
            accel = Mathf.Lerp(0, 1  * accelSentivity, 1- cornerFactor);
        }

        float prevTorque = ds.torque;
        if (speedFactor < 0.3f && ds.rb.gameObject.transform.forward.y > 1.0f) {
            ds.torque *= 3.0f;
            accel = 1;
            brake = 0;
        }


        ds.Go(accel,steer,brake);

        Debug.Log("RigidBody Velocity : "+ ds.rb.velocity.magnitude);
        ds.CheckForSkid();
        ds.CalculateEngineSound();
        ds.torque = prevTorque;
    }
}
