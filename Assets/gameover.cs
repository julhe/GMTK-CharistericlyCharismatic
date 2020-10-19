using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameover : MonoBehaviour {


	void Update () {
	    if (Input.GetButtonDown("Start") || Input.GetButtonDown("Cancel"))
	    {
	        SceneManager.LoadScene(0);
	    }
	}
}
