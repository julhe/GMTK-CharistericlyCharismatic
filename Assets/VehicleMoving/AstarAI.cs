using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour
{

    public AstarAISettings AstarAiSettings;
    public Transform ModelTransform;
    private Seeker seeker;
    private CharacterController controller;
    private Rigidbody rigidbody;
    public Path path;


    public AstarAIMovement AstarAiMovement = AstarAIMovement.CharacterController;
    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public Transform targetsRoot;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        switch (AstarAiMovement)
        {
            case AstarAIMovement.CharacterController:
                controller = GetComponent<CharacterController>();
                break;
            case AstarAIMovement.Rigidbody:
                rigidbody = GetComponent<Rigidbody>();
                break;
        }

        NewRandomTarget();
        // If you are writing a 2D game you can remove this line
        // and use the alternative way to move sugggested further below.

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update()
    {
        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
      
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * AstarAiSettings.Speed * speedFactor ;

        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        
        switch (AstarAiMovement)
        {
            case AstarAIMovement.CharacterController:
                controller.SimpleMove(velocity);
                if (controller.velocity.magnitude < 00.01f && !reachedEndOfPath)
                {
                    // controller is probably stuck, so we "shake" him a little...
                    Vector3 shake = Random.insideUnitSphere * 0.1f;
                    shake.y = 0f;

                    transform.position += shake;
                }; 
                break;
            case AstarAIMovement.Rigidbody:
                rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
                //if (rigidbody.velocity.magnitude < 00.01f && !reachedEndOfPath)
                //{
                //    // controller is probably stuck, so we "shake" him a little...
                //    Vector3 shake = Random.insideUnitSphere * 0.1f;
                //    shake.y = 0f;

                //    transform.position += shake;
                //};
                break;
        }


        if (reachedEndOfPath)
        {
            NewRandomTarget();
        }
        else
        { // assing rotation to model
            ModelTransform.rotation = Quaternion.RotateTowards(ModelTransform.rotation, Quaternion.LookRotation(dir), AstarAiSettings.VisualTurningSpeed * Time.fixedDeltaTime);
        }
       
        // If you are writing a 2D game you may want to remove the CharacterController and instead use e.g transform.Translate
        // transform.position += velocity * Time.deltaTime;
    }

    void NewRandomTarget()
    {
        int possibleTargets = targetsRoot.childCount;
       var targetPosition = targetsRoot.GetChild(Random.Range(0, possibleTargets));
        path = null; // remove only path
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete); // issues a new path
       
    }
}

public enum AstarAIMovement
{
    CharacterController,
    Rigidbody
}