using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {
    public bool hasHitSomething = false;

    /*
    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player")
        {
            hasHitSomething = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(other.transform, true);
            SetGlobalScale(transform, Vector3.one);
        }        
    }
    */
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag != "Player")
        {
            hasHitSomething = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(collision.transform.transform, true);
            SetGlobalScale(transform, Vector3.one);
            SetColor(Color.yellow);
        }
    }

    public void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    public void SetColor(Color color)
    {
        GetComponent<LineRenderer>().startColor = color;
        GetComponent<LineRenderer>().endColor = color;
    }
}
