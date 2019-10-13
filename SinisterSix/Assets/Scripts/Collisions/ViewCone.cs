using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// View cone for an enemy.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ViewCone : MonoBehaviour
{

    /// <summary>
    /// Target to check for.
    /// </summary>
    [SerializeField]
    private GameObject target = null;

    /// <summary>
    /// Check if the view cone is in search mode.
    /// </summary>
    [SerializeField]
    private bool isSearching = false;

    /// <summary>
    /// Field of view angle in degrees.
    /// </summary>
    [Slider(0.5f, 360.0f)]
    [SerializeField]
    private float fieldOfView = 110f;

    /// <summary>
    /// Range in meters of the vision.
    /// </summary>
    [OnValueChanged("UpdateColliderRadius")]
    [Slider(0.0f, 10.0f)]
    [SerializeField]
    private float range = 10.0f;

    /// <summary>
    /// Vision collider.
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private SphereCollider visionCollider = null;

    /// <summary>
    /// Target sighted color.
    /// </summary>
    [SerializeField]
    private Color gizmoColor = Color.white;

    /// <summary>
    /// Target sighted color.
    /// </summary>
    [SerializeField]
    private Color targetLostColor = Color.yellow;

    /// <summary>
    /// Target sighted color.
    /// </summary>
    [SerializeField]
    private Color targetSeenColor = Color.red;

    /// <summary>
    /// Is the target currently being seen?
    /// </summary>
    [SerializeField]
    private bool targetSeen = false;

    /// <summary>
    /// Check if the target has just been seen.
    /// </summary>
    [SerializeField]
    public UnityEvent onTargetSeen;

    /// <summary>
    /// Check if the target has just been lost.
    /// </summary>
    [SerializeField]
    public UnityEvent onTargetLost;

    /// <summary>
    /// Get the collider.
    /// </summary>
    public SphereCollider Collider => this.visionCollider = (this.visionCollider == null) ? this.GetComponent<SphereCollider>() : this.visionCollider;

    /// <summary>
    /// Check if target exists.
    /// </summary>
    public bool TargetExists => (this.target != null);

    /// <summary>
    /// Return the player or find if null.
    /// </summary>
    private GameObject Target => (this.target = this.target ?? GameObject.FindGameObjectWithTag("Player"));

    // Start is called before the first frame update.
    void Start()
    {
        // Grab reference to the internal collider.
        this.visionCollider = this.GetComponent<SphereCollider>();

        // Set up the events.
        this.onTargetSeen = this.onTargetSeen ?? new UnityEvent();
        this.onTargetLost = this.onTargetLost ?? new UnityEvent();

        // Add events for when view cone sees target. 
        this.onTargetSeen.AddListener(this.TargetSeen);
        this.onTargetSeen.AddListener(this.UpdateGizmoColor);

        // Add events for when view cone loses target.
        this.onTargetLost.AddListener(this.TargetLost);
        this.onTargetLost.AddListener(this.UpdateGizmoColor);

        // First frame update.
        this.UpdateGizmoColor();
        this.UpdateColliderRadius();

        // Load the target.
        this.target = this.Target;
    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// The target has been found.
    /// </summary>
    public void TargetSeen() => this.targetSeen = true;

    /// <summary>
    /// The target has been lost.
    /// </summary>
    public void TargetLost() => this.targetSeen = false;

    /// <summary>
    /// Update the gizmo color.
    /// </summary>
    public void UpdateGizmoColor() => this.gizmoColor = (this.targetSeen) ? this.targetSeenColor : this.targetLostColor;

    /// <summary>
    /// Update the collider radius.
    /// </summary>
    public void UpdateColliderRadius() => this.Collider.radius = this.range;

    /// <summary>
    /// On trigger enter, check if in view.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        // Check target against collider.
        if (!this.TargetExists || other.gameObject != this.Target)
        {
            // Do nothing if not the appropriate target.
            Debug.Log("Trigger activated by non-targeted game object.");
            return;
        }
        else
        {
            Debug.Log("Trigger entered by target.");
        }


        // Check if the target is in the view cone.
        if (this.IsTargetInView())
        {
            // If in cone, invoke seen target event.
            this.onTargetSeen.Invoke();
        }
        else
        {
            // If not in cone, invoke lost target event.
            this.onTargetLost.Invoke();
        }
    }

    /// <summary>
    /// On trigger stay, check raycasts.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(Collider other)
    {
        // Check if the target object is being searched for.
        if (!this.isSearching)
        {
            // Do nothing, because not in search mode.
            Debug.Log("OnTriggerStay: Viewer is not in search mode.");
            return;
        }

        // Does the target exist? If not, nothing to find.
        if (!this.TargetExists || other.gameObject != this.Target)
        {
            // Do nothing, because no target exists.
            Debug.Log("OnTriggerStay: No target exists.");
            return;
        }

        // Check if the target is in the view cone.
        if (this.IsTargetInView())
        {
            // If in view, and not currently seen, invoke the seen event.
            if (!this.targetSeen)
            {
                this.onTargetSeen.Invoke();
            }
        }
        else
        {
            // If not in view, and currently seen, invoke the lost event.
            if (this.targetSeen)
            {
                this.onTargetLost.Invoke();
            }
        }
    }

    /// <summary>
    /// If the target is no longer being seen, set the flag appropriately.
    /// </summary>
    /// <param name="other">Other collider.</param>
    public void OnTriggerExit(Collider other)
    {
        if (this.targetSeen && this.TargetExists && other.gameObject == this.Target)
        {
            Debug.Log("Trigger exited by the target.");
            this.onTargetLost.Invoke();
        }
    }

    // Check if in cone.
    bool IsTargetInView()
    {
        // If the target exists.
        if (this.TargetExists)
        {
            // Measure the angles.
            float halfFOV = this.fieldOfView * 0.5f;
            Vector3 targetRay = this.Target.transform.position - this.transform.position;
            float targetAngle = Vector3.Angle(targetRay, this.transform.forward);

            // Check if it's in range and in angle.
            if ((targetRay.magnitude < this.Collider.radius * 0.75f) && (targetAngle < halfFOV))
            {
                // If within bounds, perform raycast.
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, targetRay.normalized, out hit, this.Collider.radius * 2.0f))
                {
                    if (hit.collider.gameObject == this.Target)
                    {
                        Debug.Log("Target hit by raycast.");
                        return true;
                    }
                    else
                    {
                        Debug.Log("Non-target hit by raycast.");
                    }
                }
                else
                {
                    Debug.Log("Nothing hit by raycast.");
                }
            }
            Debug.Log("Target is not in view.");
            return false;
        }

        // If not true, return false.
        return false;
    }

    // Draw view cone reference.
    private void OnDrawGizmos()
    {
        // Set the gizmo color.
        Gizmos.color = this.gizmoColor;

        // Measure the angles.
        float halfFOV = this.fieldOfView * 0.5f;
        Vector3 targetRay = this.Target.transform.position - this.transform.position;
        float targetAngle = Vector3.Angle(targetRay, this.transform.forward);

        // Direction rays.
        Vector3 forwardRay = this.transform.forward * this.Collider.radius * 0.5f;
        Vector3 leftRay = Quaternion.AngleAxis(-halfFOV, Vector3.up) * forwardRay;
        Vector3 rightRay = Quaternion.AngleAxis(halfFOV, Vector3.up) * forwardRay;

        // Draw the direction rays.
        Gizmos.DrawRay(this.transform.position, forwardRay);
        Gizmos.DrawRay(this.transform.position, leftRay);
        Gizmos.DrawRay(this.transform.position, rightRay);

        // Check if the target is within the range and angles.
        bool isInRange = (targetRay.magnitude < this.Collider.radius * 0.5f);
        bool isInAngle = (targetAngle < halfFOV);

        // Change ray color if target is within angles.
        Gizmos.color = (isInRange && isInAngle) ? Color.cyan : this.gizmoColor;

        // Draw target ray and target sphere.
        Gizmos.DrawRay(this.transform.position, targetRay);
        Gizmos.DrawWireSphere(this.Target.transform.position, 0.5f);

    }

    /// <summary>
    /// Target to assign.
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            this.target = target;
        }
    }

}