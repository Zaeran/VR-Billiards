using UnityEngine;
using System.Collections;

public class CueShootScript : MonoBehaviour {

    public GameObject LeftHand;
    public GameObject RightHand;

    SixenseHand LeftHandCode;
    SixenseHand RightHandCode;

    private Vector3 prevPosition;
    private Vector3 velocity;

    private Vector3 RightHandPrevPosition;

    bool positionLocked;
    public static bool resetting;

	// Use this for initialization
	void Start () {
        LeftHandCode = LeftHand.GetComponent<SixenseHand>();
        RightHandCode = RightHand.GetComponent<SixenseHand>();
        resetting = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        positionLocked = false;
        if (RightHandCode.isActive)
        {
            if (LeftHandCode.m_controller.GetButton(SixenseButtons.TRIGGER))
            {
                transform.LookAt(GameObject.Find("Hand - Left").transform.position);
                if (RightHandCode.m_controller.GetButtonDown(SixenseButtons.TRIGGER))
                {
                    RightHandPrevPosition = RightHand.transform.position;
                }
                if (RightHandCode.m_controller.GetButton(SixenseButtons.TRIGGER))
                {
                    Vector3 RightHandInCueReferenceMovement = transform.InverseTransformDirection(RightHand.transform.position - RightHandPrevPosition);
                    transform.Translate(new Vector3(0, 0, RightHandInCueReferenceMovement.z));
                    positionLocked = true;
                    RightHandPrevPosition = RightHand.transform.position;
                    if (!resetting)
                        GetComponent<BoxCollider>().enabled = true;
                }
            }
            else
            {
                transform.localEulerAngles = RightHand.transform.eulerAngles + new Vector3(0, 270, 0);
                GetComponent<BoxCollider>().enabled = false;
            }

            if (RightHandCode.m_controller.GetButtonDown(SixenseButtons.THREE))
            {
                GameObject.Find("WhiteBall").transform.position = new Vector3(0, 1.026f, 0);
                GameObject.Find("WhiteBall").rigidbody.velocity = new Vector3(0, 0, 0);
                GameObject.Find("WhiteBall").rigidbody.angularVelocity = Vector3.zero;
            }
            if (!positionLocked)
            {
                transform.position = RightHand.transform.position;
            }
            velocity = (transform.position - prevPosition) / Time.deltaTime;
            prevPosition = transform.position;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        c.rigidbody.AddForce((velocity / 1.5f) * 20);
        GetComponent<BoxCollider>().enabled = false;
        resetting = true;
    }
}
