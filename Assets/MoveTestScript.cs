using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.forward * .1f;
	}
}
