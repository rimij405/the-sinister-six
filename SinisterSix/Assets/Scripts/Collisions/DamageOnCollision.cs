using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// Entities with this class will damage targets on collision.
/// </summary>
public class DamageOnCollision : MonoBehaviour
{

    /// <summary>
    /// The target to check.
    /// </summary>
    public DamageOnCollisionTarget target;

    /// <summary>
    /// Event fired when collision with target is successful.
    /// </summary>
    public UnityEvent onTargetCollision;

    /// <summary>
    /// Amount of damage to cause on collision.
    /// </summary>
    public float damage = 100;

    public void Awake()
    {
        this.SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    public void SetTarget(GameObject _target)
    {
        if(_target != null)
        {
            DamageOnCollisionTarget component = _target.GetComponent<DamageOnCollisionTarget>();
            if (component)
            {
                target = component;
            }
        }
    }

    /// <summary>
    /// On trigger enter, attempt to damage the target.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            bool isColliding = false;

            // If target is the matching game object in the collision, attempt to damage it.
            if (target.gameObject == other.gameObject)
            {
                target.Damage(this);
                isColliding = true;
            }

            target.SetCollisionColor(isColliding);
        }
    }

    /// <summary>
    /// No longer colliding.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (target != null)
        {
            target.SetCollisionColor(false);
        }
    }

}