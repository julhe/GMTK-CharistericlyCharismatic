using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour {

    public Transform goal;
    public Transform playerCamera;

    public GameObject timer;
	// Use this for initialization
	void Start () {
        StartCoroutine(introCam());
	}
	
	private IEnumerator introCam()
    {
        
        transform.position = goal.position + new Vector3(10, 0, 10);
        Vector3 start = transform.position;
        Vector3 end = goal.position + new Vector3(0, 25, 0);
        float time = 3;
        float passedTime = 0;
        while(Vector3.Distance(transform.position, end) > 1)
        {
            transform.LookAt(goal);
            passedTime += Time.deltaTime / time;
            transform.position = Vector3.Slerp(start ,end, passedTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        passedTime = 0;
        time = 7.5f;
        end = playerCamera.position;
        start = transform.position;
        Quaternion startRotation = transform.rotation;
        while (Vector3.Distance(transform.position, end) > 1)
        {
            passedTime += Time.deltaTime / time;
            transform.rotation = Quaternion.Slerp(startRotation, playerCamera.rotation, passedTime);
            transform.position = Vector3.Slerp(start, end, passedTime);
            yield return new WaitForEndOfFrame();
        }
        StartTheGameNow();
    }

    public void StartTheGameNow()
    {
        playerCamera.gameObject.SetActive(true);
        timer.SetActive(true);
        gameObject.SetActive(false);
    }
}
