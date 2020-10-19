using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalZone : MonoBehaviour {

    public GameObject youWin;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.SetActive(false);
            youWin.SetActive(true);
        }
    }
}
