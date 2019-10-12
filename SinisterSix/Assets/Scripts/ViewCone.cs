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
    /// Called when the trigger is entered.
    /// </summary>
    /// <param name="other">The other collider.</param>
    public void OnTriggerStay(Collider other)
    {
        // If target is currently being seen, do nothing.
        if (this.targetSeen)
        {
            // Do nothing, target is already being tracked.
            Debug.Log("Target is already being tracked.");
            return;
        }

        // Check target against collider.
        if(!this.TargetExists || other.gameObject != this.target)
        {
            // Do nothing if not the appropriate target.
            Debug.Log("Trigger activated by non-targeted game object.");
            return;
        }

        // Check if the target object is being searched for.
        if (!this.isSearching)
        {
            // Do nothing, because not in search mode.
            Debug.Log("Is not searching for target at the moment.");
            return;
        }

        // Check if the target is in the view cone.
        if (this.IsTargetInCone())
        {
            this.onTargetSeen.Invoke();
        }   
    }

    /// <summary>
    /// If the target is no longer being seen, set the flag appropriately.
    /// </summary>
    /// <param name="other">Other collider.</param>
    public void OnTriggerExit(Collider other)
    {
        if(this.isSearching && this.TargetExists && other.gameObject == this.target)
        {
            this.onTargetLost.Invoke();
        }        
    }

    // Check if in cone.
    bool IsTargetInCone()
    {
        // If the target exists.
        if (this.TargetExists)
        {
            Vector3 direction = this.target.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit hit;


            }
        }

        // If not true, return false.
        return false;
    }

    // Draw view cone reference.
    private void OnDrawGizmos()
    {
        // Set the gizmo color.
        Gizmos.color = this.gizmoColor;

        float halfFOV = this.fieldOfView / 2.0F;

        Quaternion leftRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftDirection = leftRotation * transform.forward * this.range;
        Vector3 rightDirection = rightRotation * transform.forward * this.range;

        // Draw left, right, and forward rays.
        Gizmos.DrawRay(transform.position, transform.forward * this.range);
        Gizmos.DrawRay(transform.position, leftDirection);
        Gizmos.DrawRay(transform.position, rightDirection);

    }

}
