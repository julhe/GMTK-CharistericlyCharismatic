using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    Quaternion xQuaternion;
    Quaternion yQuaternion;
    Quaternion zQuaternion;
    public float horizontalSensitivity;
    public float verticalSensitivity;
    public Transform rightRopeTip;
    public Transform rightRopeStartPoint;
    public Transform leftRopeTip;
    public Transform leftRopeStartPoint;
    public Transform hoverBoard;
    public float ropeFlySpeed;
    public float ropeShorteningSpeed;
    public float maxGrapplingLength;
    private string rightGrappleShot = "Fire1";
    private string rightGrappleShortener = "Bumper1";
    private string leftGrappleShot = "Fire2";
    private string leftGrappleShortener = "Bumper2";
    private string rightGrappleShotKeyboard = "KeyboardFire1";
    private string rightGrappleShortenerKeyboard = "KeyboardBumper1";
    private string leftGrappleShotKeyboard = "KeyboardFire2";
    private string leftGrappleShortenerKeyboard = "KeyboardBumper2";
    bool hasLeftGrapplingHook;
    bool leftIsShooting = false;
    bool rightIsShooting = false;
    public SpringJoint leftSpringJoint;
    public SpringJoint rightSpringJoint;
    // Use this for initialization
    void Start () {
        StartCoroutine(PointGraplingRopes());
        StartCoroutine(KeepGraplingAnchors());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        rightRopeTip.GetComponent<BoxCollider>().enabled = rightIsShooting;
        if (hasLeftGrapplingHook)
        {
            leftRopeTip.GetComponent<BoxCollider>().enabled = leftIsShooting;
        }
        if ((Input.GetAxis(rightGrappleShot) > 0.1f || Input.GetButton(rightGrappleShotKeyboard))&& !rightIsShooting)
        {
            rightIsShooting = true;
            StopCoroutine(ShootRightGraplingHook());
            StartCoroutine(ShootRightGraplingHook());
        }
        if (Input.GetAxis(rightGrappleShot) < 0.1f && !Input.GetButton(rightGrappleShotKeyboard))
        {
            rightIsShooting = false;
        }
        if ((Input.GetButton(rightGrappleShortener) || Input.GetButton(rightGrappleShortenerKeyboard)) && rightRopeTip.GetComponent<RopeController>().hasHitSomething)
        {
            rightSpringJoint.maxDistance = rightSpringJoint.maxDistance - ropeShorteningSpeed * Time.deltaTime;
        }
        if (hasLeftGrapplingHook)
        {
            if ((Input.GetAxis(leftGrappleShot) > 0.1f || Input.GetButton(leftGrappleShotKeyboard)) && !leftIsShooting)
            {
                leftIsShooting = true;
                StopCoroutine(ShootLeftGraplingHook());
                StartCoroutine(ShootLeftGraplingHook());
            }
            if (Input.GetAxis(leftGrappleShot) < 0.1f && !Input.GetButton(leftGrappleShotKeyboard))
            {
                leftIsShooting = false;
            }
            if ((Input.GetButton(leftGrappleShortener) || Input.GetButton(leftGrappleShortenerKeyboard)) && rightRopeTip.GetComponent<RopeController>().hasHitSomething)
            {
                leftSpringJoint.maxDistance = leftSpringJoint.maxDistance - ropeShorteningSpeed * Time.deltaTime;
            }
        }
        float horizontalValue = Input.GetAxis("Horizontal") * horizontalSensitivity;
        float verticalValue = Input.GetAxis("Vertical") * verticalSensitivity;
        if(Vector3.Angle(transform.forward, Vector3.up) < 5 && verticalValue > 0)
        {
            verticalValue = 0;
        }
        if (Vector3.Angle(transform.forward, Vector3.down) < 5 && verticalValue < 0)
        {
            verticalValue = 0;
        }
        horizontalValue *= (Vector3.Angle(transform.forward, Vector3.up) / 180);
        xQuaternion = Quaternion.AngleAxis(horizontalValue, Vector3.up);
        yQuaternion = Quaternion.AngleAxis(verticalValue, Vector3.left);
        zQuaternion = Quaternion.AngleAxis(-transform.rotation.eulerAngles.z, Vector3.forward);
        transform.rotation *= xQuaternion * yQuaternion * zQuaternion;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator ShootRightGraplingHook()
    {
        rightRopeTip.parent = transform.parent;
        rightRopeTip.localScale = Vector3.one;
        rightRopeTip.GetComponent<Rigidbody>().isKinematic = false;
        rightRopeTip.position = rightRopeStartPoint.position;
        rightRopeTip.rotation = rightRopeStartPoint.rotation;
        rightSpringJoint.maxDistance = maxGrapplingLength * 2;
        rightRopeTip.GetComponent<RopeController>().SetColor(Color.white);
        Vector3 shotDirection = transform.forward;
        rightRopeTip.GetComponent<RopeController>().hasHitSomething = false;
        while ((Input.GetAxis(rightGrappleShot) > 0.1f || Input.GetButton(rightGrappleShotKeyboard)) && !rightRopeTip.GetComponent<RopeController>().hasHitSomething && (Vector3.Distance(rightRopeStartPoint.position, rightRopeTip.position) < maxGrapplingLength))
        {
            rightRopeTip.GetComponent<Rigidbody>().velocity = shotDirection * ropeFlySpeed;
            
            yield return new WaitForEndOfFrame();
        }
        if (rightRopeTip.GetComponent<RopeController>().hasHitSomething)
        {
            rightSpringJoint.connectedBody = rightRopeTip.GetComponent<Rigidbody>();
            rightSpringJoint.maxDistance = Vector3.Distance(transform.position, rightRopeTip.position);
            
        }
        else
        {
            while (Vector3.Distance(rightRopeTip.position, rightRopeStartPoint.position) >= 1)
            {
                rightRopeTip.GetComponent<Rigidbody>().velocity = Vector3.Normalize(rightRopeStartPoint.position - rightRopeTip.transform.position) * ropeFlySpeed * 2;
                yield return new WaitForEndOfFrame();
            }
            rightRopeTip.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rightRopeTip.position = rightRopeStartPoint.position;

        }
    }

    private IEnumerator ShootLeftGraplingHook()
    {
        leftRopeTip.parent = transform.parent;
        leftRopeTip.localScale = Vector3.one;
        leftRopeTip.GetComponent<Rigidbody>().isKinematic = false;
        leftRopeTip.position = leftRopeStartPoint.position;
        leftRopeTip.rotation = leftRopeStartPoint.rotation;

        leftSpringJoint.maxDistance = maxGrapplingLength * 2;
        Vector3 shotDirection = transform.forward;
        leftRopeTip.GetComponent<RopeController>().hasHitSomething = false;
        while ((Input.GetAxis(leftGrappleShot) > 0.1f || Input.GetButton(leftGrappleShotKeyboard)) && !leftRopeTip.GetComponent<RopeController>().hasHitSomething && (Vector3.Distance(transform.position, leftRopeTip.position) < maxGrapplingLength))
        {
            leftRopeTip.GetComponent<Rigidbody>().velocity = shotDirection * ropeFlySpeed;
            
            yield return new WaitForEndOfFrame();
        }
        if (leftRopeTip.GetComponent<RopeController>().hasHitSomething)
        {
            leftSpringJoint.connectedBody = leftRopeTip.GetComponent<Rigidbody>();
            leftSpringJoint.maxDistance = Vector3.Distance(transform.position, leftRopeTip.position);
            
        }
        else
        {
            while (Vector3.Distance(leftRopeTip.position, leftRopeStartPoint.position) >= 1)
            {
                leftRopeTip.GetComponent<Rigidbody>().velocity = Vector3.Normalize(leftRopeStartPoint.position - leftRopeTip.transform.position) * ropeFlySpeed * 2;
                yield return new WaitForEndOfFrame();
            }
            leftRopeTip.GetComponent<Rigidbody>().velocity = Vector3.zero;
            leftRopeTip.position = leftRopeStartPoint.position;
        }
    }

    private IEnumerator PointGraplingRopes()
    {
        while (true)
        {
            rightRopeTip.GetComponent<LineRenderer>().SetPosition(1, rightRopeTip.position);
            rightRopeTip.GetComponent<LineRenderer>().SetPosition(0, rightRopeStartPoint.position - transform.forward);
            if (hasLeftGrapplingHook)
            {
                leftRopeTip.GetComponent<LineRenderer>().SetPosition(1, leftRopeTip.position);
                leftRopeTip.GetComponent<LineRenderer>().SetPosition(0, leftRopeStartPoint.position);
            }            
            if (!rightIsShooting && !rightRopeTip.GetComponent<RopeController>().hasHitSomething)
            {
                rightRopeTip.position = rightRopeStartPoint.position;
                rightRopeTip.rotation = rightRopeStartPoint.rotation;
            }
            if (hasLeftGrapplingHook)
            {
                if (!leftIsShooting && !leftRopeTip.GetComponent<RopeController>().hasHitSomething)
                {
                    leftRopeTip.position = leftRopeStartPoint.position;
                    leftRopeTip.rotation = leftRopeStartPoint.rotation;
                }
            }
            //GetComponent<Rigidbody>().AddForce(Vector3.up * 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator KeepGraplingAnchors()
    {
        while (true)
        {
            rightSpringJoint.anchor = rightRopeStartPoint.localPosition;
            hoverBoard.position = transform.position + Vector3.down * 1f;
            hoverBoard.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
            if (hasLeftGrapplingHook)
            {
                leftSpringJoint.anchor = leftRopeStartPoint.localPosition;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
