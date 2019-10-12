using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCone : MonoBehaviour
{

    /// <summary>
    /// Target to check for.
    /// </summary>
    public GameObject target = null;

    /// <summary>
    /// Field of view angle in degrees.
    /// </summary>
    public float fieldOfView = 110f;

    /// <summary>
    /// Range in meters of the vision.
    /// </summary>
    public float range = 10.0f;

    /// <summary>
    /// Is the target in sight.
    /// </summary>
    public bool targetInSight = false;

    /// <summary>
    /// Check if target exists.
    /// </summary>
    public bool TargetExists => (this.target != null);

    /// <summary>
    ///  Check if the target is within view.
    /// </summary>
    public bool TargetInSight => (this.TargetExists && this.targetInSight);
       
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Draw view cone reference.
    private void OnDrawGizmosSelected()
    {

        float halfFOV = this.fieldOfView / 2.0F;

        Quaternion leftRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftDirection = leftRotation * transform.forward;
        Vector3 rightDirection = rightRotation * transform.forward;

        // Draw left, right, and forward rays.
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.DrawRay(transform.position, leftDirection);
        Gizmos.DrawRay(transform.position, rightDirection);

    }

}
