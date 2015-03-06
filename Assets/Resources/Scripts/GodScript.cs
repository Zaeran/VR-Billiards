using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodScript : MonoBehaviour {

    static int playerTurn;

    static string[] playerBall;

    static bool ballHasBeenPocketed;
    static bool turnContinues;
    static bool penalty;
    static bool gameStarted;
    static bool whiteBallPocketed;
    static bool wrongBallPocketed;
    static bool noBallsHit;
    static bool wrongBallHit;

    static int turnCooldownTimer;

    static Transform playerLocation;

    static List<GameObject> movingBalls;

	// Use this for initialization
	void Start () {
        movingBalls = new List<GameObject>();
        gameStarted = false;
        whiteBallPocketed = false;
        wrongBallPocketed = false;
        noBallsHit = false;
        playerTurn = 0; 
        playerBall = new string[2];
        turnCooldownTimer = 0;
        playerLocation = GameObject.Find("CenterEyeAnchor").transform;
	}

    void Update()
    {
        if (turnCooldownTimer != 0)
        {
            turnCooldownTimer--;
        }
        transform.LookAt(playerLocation);
        transform.eulerAngles += new Vector3(0, 180, 0);
    }

    public static void TurnOver()
    {
        bool overrideText = false;
        if (turnCooldownTimer == 0)
        {
            if (whiteBallPocketed)
            {
                playerTurn = (playerTurn + 1) % 2;
                penalty = true;
                whiteBallPocketed = false;
            }
            else if (wrongBallPocketed)
            {
                playerTurn = (playerTurn + 1) % 2;
                penalty = true;
            }
            else if (noBallsHit)
            {
                playerTurn = (playerTurn + 1) % 2;
                SetText("No balls hit\nPlayer " + (playerTurn + 1) + "'s turn");
                overrideText = true;
                penalty = true;
            }
            else if (wrongBallHit)
            {
                playerTurn = (playerTurn + 1) % 2;
                penalty = true;
            }
            else if (turnContinues)
            {
                SetText("Player " + (playerTurn + 1) + " continues");
                overrideText = true;
            }
            else if (penalty)
            {
                penalty = false;
                SetText("Player " + (playerTurn + 1) + " has\npenalty shot");
                overrideText = true;
            }
            else
            {
                playerTurn = (playerTurn + 1) % 2;
            }

            if (!overrideText)
            {
                SetText("Player " + (playerTurn + 1) + "'s turn");
            }
            turnContinues = false;
            wrongBallHit = false;
            noBallsHit = false;
            wrongBallPocketed = false;

            CueShootScript.resetting = false;

            turnCooldownTimer = 100;
        }
    }

    public static void AddMovingBall(GameObject ball)
    {
        if (gameStarted)
        {
            if (!movingBalls.Contains(ball))
                movingBalls.Add(ball);
        }
    }

    public static void BallStopped(GameObject ball)
    {
        if (gameStarted)
        {
            if (movingBalls.Contains(ball))
            {
                movingBalls.Remove(ball);
            }
            if (movingBalls.Count == 0)
            {
                TurnOver();
            }
        }
    }

    public static void BallPocketed(GameObject ball)
    {
        if (ball.name == "WhiteBall")
        {
            SetText("White Ball\nPocketed");
            whiteBallPocketed = true;

            ball.GetComponent<SphereCollider>().enabled = true;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            ball.rigidbody.velocity = Vector3.zero;
            ball.rigidbody.angularVelocity = Vector3.zero;
            ball.transform.position = new Vector3(0, 2.35f, -10.29f);
        }
        else if (ball.name == "BlackBall")
        {
            SetText("GAME OVER");
        }
        else if (!ballHasBeenPocketed)
        {
            if (ball.name == "YellowBall")
            {
                playerBall[playerTurn] = "YellowBall";
                playerBall[(playerTurn + 1) % 2] = "RedBall";
            }
            else
            {
                playerBall[playerTurn] = "RedBall";
                playerBall[(playerTurn + 1) % 2] = "YellowBall";
            }

            SetText("Player 1: " + playerBall[0].Substring(0, playerBall[0].Length - 4) + "\nPlayer 2: " + playerBall[1].Substring(0, playerBall[1].Length - 4)); 

            ballHasBeenPocketed = true;
            turnContinues = true;
        }
        else
        {
            if (ball.name != playerBall[playerTurn])
            {
                //penalty = true;
                SetText("Wrong ball\npocketed");
                wrongBallPocketed = true;
            }
            else
            {
                turnContinues = true;
                SetText(ball.name.Substring(0, ball.name.Length - 4) + "ball\npocketed");
            }
        }
    }

    public static void GameStart()
    {
        gameStarted = true;
    }

    public static bool GameHasStarted()
    {
        return gameStarted;
    }

    public static void NoBallsHit()
    {
        noBallsHit = true;
    }

    public static string GetPlayerBallColour()
    {
        return playerBall[playerTurn];
    }

    public static bool PlayersHaveColours()
    {
        return ballHasBeenPocketed;
    }

    public static void WrongBallHit()
    {
        wrongBallHit = true;
        SetText("Wrong ball\nwas hit");
    }

    private static void SetText(string text)
    {
        GameObject.FindGameObjectWithTag("GOD").GetComponent<TextMesh>().text = text;
    }
}
