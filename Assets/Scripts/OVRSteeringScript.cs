using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRSteeringScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Debug.Log("Left Joystick: " + leftJoystick);
        Debug.Log("Right Joystick: " + rightJoystick);
    }
}
