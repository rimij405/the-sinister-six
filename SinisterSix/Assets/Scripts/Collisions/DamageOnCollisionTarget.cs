using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// Entities with this will be damaged when hit.
/// </summary>
public class DamageOnCollisionTarget : MonoBehaviour
{

    /// <summary>
    /// Event fired when damage is successfully registered.
    /// </summary>
    public UnityEvent onDamage;

    /// <summary>
    /// If dead, trigger this event.
    /// </summary>
    public UnityEvent onDeath;

    /// <summary>
    /// Health tracker.
    /// </summary>
    public PlayerHealth healthTracker;

    /// <summary>
    /// Cooldown value. Collision possible when equal to zero.
    /// </summary>
    [ReadOnly]
    [SerializeField]
    private float _cooldown = 0.0f;

    /// <summary>
    /// Gizmo color.
    /// </summary>
    private Color gizmoColor = Color.white;

    /// <summary>
    /// Color drawn when colliding.
    /// </summary>
    private Color onCollisionColor = Color.red;

    /// <summary>
    /// Color drawn when not colliding.
    /// </summary>
    private Color offCollisionColor = Color.blue;

    /// <summary>
    /// Cooldown duration.
    /// </summary>
    [Slider(0.0f, 10.0f)]
    [SerializeField]
    private float _cooldownDuration = 10.0f;

    /// <summary>
    /// Set up the class.
    /// </summary>
    public void Start()
    {
        this.onDamage = this.onDamage ?? new UnityEvent();
        this.onDeath = this.onDeath ?? new UnityEvent();
        this.onDamage.AddListener(StartCooldown);
    }

    /// <summary>
    /// Start the coroutine for the cooldown.
    /// </summary>
    public void StartCooldown() => this.StartCoroutine(this.Cooldown());

    /// <summary>
    /// Coroutine for cooldown.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Cooldown()
    {
        // Reset cooldown.
        this._cooldown = this._cooldownDuration;

        // Continue to cooldown while not paused.
        while (this._cooldown > 0.0f)
        {
            // Subtract time since last frame.
            this._cooldown -= Time.deltaTime;
            yield return null;
        }

        // Set cooldown to zero once the end has been reached.
        this._cooldown = 0.0f;
    }

    /// <summary>
    /// Cause damage using the input actor.
    /// </summary>
    /// <param name="actor"></param>
    public void Damage(DamageOnCollision actor)
    {
        if (this._cooldown == 0.0f)
        {
            this.onDamage.Invoke();
            healthTracker.Damage(actor.damage);

            if (healthTracker.Health <= 0.0f)
            {
                this.onDeath.Invoke();
            }

        }
        else
        {
            Debug.Log("No damage because i-frame cooldown has not yet resolved.");
        }
    }

    /// <summary>
    /// Set the collision color.
    /// </summary>
    /// <param name="isColliding"></param>
    public void SetCollisionColor(bool isColliding)
    {
        this.gizmoColor = isColliding ? this.onCollisionColor : this.offCollisionColor;
    }

    /// <summary>
    /// Draw the gizmos.
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = this.gizmoColor;
        Gizmos.DrawSphere(this.transform.position, 0.5f);
    }

}