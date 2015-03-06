using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

    bool hasStopped;
    int stoppedCooldown;
    bool hasHitBall;

    void Start()
    {
        hasStopped = true;
        stoppedCooldown = 0;
        hasHitBall = false;
    }
    void OnCollisionEnter(Collision c)
    {
        hasStopped = false;
        GodScript.AddMovingBall(gameObject);
        stoppedCooldown = 20;
        if (gameObject.name == "WhiteBall")
        {
            if (c.collider.tag == "Ball")
            {
                if (GodScript.PlayersHaveColours() && !hasHitBall)
                {
                    if (GodScript.GetPlayerBallColour() != c.gameObject.name)
                    {
                        GodScript.WrongBallHit();
                    }
                }
                hasHitBall = true;
            }
        }
    }

    void Update()
    {
        if (!hasStopped)
        {
            if (rigidbody.velocity == Vector3.zero)
            {
                stoppedCooldown--;
                if (stoppedCooldown == 0)
                {
                    hasStopped = true;
                    if (gameObject.name == "WhiteBall")
                    {
                        if (!hasHitBall && GodScript.GameHasStarted())
                        {
                            GodScript.NoBallsHit();
                        }
                    }
                    hasHitBall = false;
                    GodScript.BallStopped(gameObject);
                }
            }
            else
            {
                stoppedCooldown = 20;
            }
        }
    }


    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Pocket")
        {
            RigidbodyConstraints newConstraints = new RigidbodyConstraints();
            newConstraints = RigidbodyConstraints.None;
            rigidbody.constraints = newConstraints;
            Debug.Log("POCKET ENTERED");
        }
        else if (c.tag == "PocketSensor")
        {
            rigidbody.velocity = Vector3.zero;
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GodScript.BallPocketed(gameObject);
        }
    }

    void OnTriggerExit(Collider c)
    {
        RigidbodyConstraints newConstraints = new RigidbodyConstraints();
        newConstraints = RigidbodyConstraints.FreezePositionY;
        rigidbody.constraints = newConstraints;
    }
}
