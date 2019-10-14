using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using NaughtyAttributes;

/// <summary>
/// Floating effect will float the target in place.
/// </summary>
public class FloatingEffect : MonoBehaviour
{

    /// <summary>
    /// The skulltarget to move.
    /// </summary>
    [SerializeField]
    private GameObject _target = null;

    [Header("Oscilliation Settings")]

    /// <summary>
    /// Local offset to apply.
    /// </summary>
    public Vector3 oscillationOffset = Vector3.zero;

    /// <summary>
    /// Maximum distance.
    /// </summary>
    [Slider(0.0f, 1.0f)]
    public float maxDistance = 1.0f;

    /// <summary>
    /// Oscilliation weight.
    /// </summary>
    [Slider(0.0f, 1.0f)]
    public float weight = 0.5f;

    /// <summary>
    /// Period.
    /// </summary>
    public float period = 0.0f;

    [Slider(0.0f, 10.0f)]
    public float duration = 1.0f;

    private bool flip = false;

    [Header("Weighted Animation Curves")]

    public AnimationCurve xWeight = AnimationCurve.Constant(0.0f, 1.0f, 1.0f);
    public AnimationCurve yWeight = AnimationCurve.Constant(0.0f, 1.0f, 1.0f);
    public AnimationCurve zWeight = AnimationCurve.Constant(0.0f, 1.0f, 1.0f);

    /// <summary>
    /// Fixed update will move target.
    /// </summary>
    public void FixedUpdate()
    {
        this.StartCoroutine(BounceTime());
        if(_target != null)
        {
            this._target.transform.localPosition = oscillationOffset + this.EvaluateSine(Time.time * weight);
        }
    }

    public IEnumerator BounceTime()
    {
        while (true)
        {
            period += (flip) ? Time.deltaTime : -Time.deltaTime;
            if(Mathf.Abs(period) >= duration)
            {
                flip = !flip;
            }
            yield return null;
        }

    }

    /// <summary>
    /// Evaluate the sine for all dimensions at given time T.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 EvaluateSine(float t) => new Vector3(
        Mathf.Sin(t) * maxDistance * xWeight.Evaluate(period),
        Mathf.Sin(t) * maxDistance * yWeight.Evaluate(period),
        Mathf.Sin(t) * maxDistance * zWeight.Evaluate(period)        
        );






}
