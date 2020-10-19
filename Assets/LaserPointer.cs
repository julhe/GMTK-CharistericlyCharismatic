using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    public Transform laserPointerHit;
    // Use this for initialization
    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(new Ray(GetComponent<PlayerController>().rightRopeStartPoint.position, GetComponent<PlayerController>().rightRopeStartPoint.up), out hit, GetComponent<PlayerController>().maxGrapplingLength-1))
        {
            if(hit.transform.tag != "Player")
            {
                laserPointerHit.gameObject.SetActive(true);
                laserPointerHit.position = hit.point;
            }
        }
        else
        {
            laserPointerHit.gameObject.SetActive(false);
        }
    }
}
