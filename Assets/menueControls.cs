using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menueControls : MonoBehaviour {
	
	void Update () {
	    if (Input.GetButtonDown("Cancel"))
	    {
            print("quit");
	        Application.Quit();
	    }
	    if (Input.GetButtonDown("Start"))
	    {
            Debug.Log("Start Pressed");
	        SceneManager.LoadScene(1);
	    }
	}
}
