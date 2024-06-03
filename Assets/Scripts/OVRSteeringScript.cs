using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRSteeringScript : MonoBehaviour
{
    float leftControllerPressCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.Get(OVRInput.Button.One))
        {
            if(leftControllerPressCount < 1)
                leftControllerPressCount += 1f * Time.deltaTime;
            Debug.Log("Left Controller: Primary button (X) pressed. Total presses: " + leftControllerPressCount);
            // Handle left controller primary button press
        }
        else
        {
            if (leftControllerPressCount > 0.1f)
                leftControllerPressCount -= 1f * Time.deltaTime;
            Debug.Log("Left Controller: Primary button (X) pressed. Total presses: " + leftControllerPressCount);
            // Handle left controller primary button press
        }
        float leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        Debug.Log("PrimaryIndexTrigger " + leftTriggerValue);
        Debug.Log("Left Joystick: " + leftJoystick);
        Debug.Log("Right Joystick: " + rightJoystick);
    }
}
