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


    private void OnDrawGizmosSelected()
    {

    }

}
