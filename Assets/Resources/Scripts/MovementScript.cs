using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

    public GameObject LeftHand;
    public GameObject RightHand;

    SixenseHand LeftHandCode;
    SixenseHand RightHandCode;

	// Use this for initialization
	void Start () {
        LeftHandCode = LeftHand.GetComponent<SixenseHand>();
        RightHandCode = RightHand.GetComponent<SixenseHand>();
	}
	
	// Update is called once per frame
	void Update () {
        //zero velocities
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        if (RightHandCode.isActive)
        {
            //get new velocities
            rigidbody.AddRelativeForce(new Vector3(LeftHandCode.m_controller.JoystickX * 500, 0, LeftHandCode.m_controller.JoystickY * 500));
            rigidbody.AddTorque(new Vector3(0, RightHandCode.m_controller.JoystickX * 100, 0));
        }
	}
}
