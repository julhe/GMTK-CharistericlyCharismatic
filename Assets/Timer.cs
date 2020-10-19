using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int timerStart;

    private bool multiplayer;
    private float timerNow;
    private Text timerText;
    private int currentTimerText;

    public GameObject gameOver;
    public GameObject player;

	// Use this for initialization
	void Start ()
	{
	    timerText = GetComponentInChildren<Text>();
	    if (timerStart < 30)
	    {
	        timerStart = 30;
	    }
	    timerNow = timerStart;
	    currentTimerText = timerStart;
	}
	

	void Update () {
	    if (!multiplayer)
	    {
	        timerNow -= Time.deltaTime;
	        if (currentTimerText >= timerNow)
	        {
	            currentTimerText -= 1;
	            timerText.text = ((currentTimerText - currentTimerText % 60)/60).ToString() + ":" + 
	                             (currentTimerText % 60).ToString();
	        }
	        if (timerNow <= 0)
	        {
                player.SetActive(false);
	            gameOver.SetActive(true);
	        }
        }
	}

    public void Setmultiplayer(bool value)
    {
        multiplayer = value;
    }
}
