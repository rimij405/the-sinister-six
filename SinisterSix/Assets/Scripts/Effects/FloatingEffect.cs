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

    /// <summary>
    /// Local offset to apply.
    /// </summary>
    public Vector3 localOffset = Vector3.zero;
    
    [Header("Sine Animation Curves")]

    /// <summary>
    /// Sine multiplier to weight.
    /// </summary>
    [Slider(0.0f, 1.0f)]
    public float sinMultiplier = 1.0f;
    public AnimationCurve xSinWeight;
    public AnimationCurve ySinWeight;
    public AnimationCurve zSinWeight;

    [Header("Cosine Animation Curves")]

    /// <summary>
    /// Cosine multiplier to weight.
    /// </summary>
    [Slider(0.0f, 1.0f)]
    public float cosMultiplier = 1.0f;
    public AnimationCurve xCosWeight;
    public AnimationCurve yCosWeight;
    public AnimationCurve zCosWeight;

    /// <summary>
    /// Fixed update will move target.
    /// </summary>
    public void FixedUpdate()
    {
        if (sinMultiplier > 0.0f)
        {
            this._target.transform.localPosition += this.EvaluateSine(Time.time);
        }

        if (cosMultiplier > 0.0f)
        {
            this._target.transform.localPosition += this.EvaluateCosine(Time.time);
        }
    }

    /// <summary>
    /// Calculate the sine at given time 't'.
    /// </summary>
    /// <returns></returns>
    public Vector3 EvaluateSine(float t) =>
        new Vector3(
            this.GetSine(this.xSinWeight.Evaluate(t)),
            this.GetSine(this.ySinWeight.Evaluate(t)),
            this.GetSine(this.zSinWeight.Evaluate(t))            
            ) * this.sinMultiplier;

    /// <summary>
    /// Calculate the cosine at given time 't'.
    /// </summary>
    public Vector3 EvaluateCosine(float t) =>
        new Vector3(
            this.GetCosine(this.xCosWeight.Evaluate(t)),
            this.GetCosine(this.yCosWeight.Evaluate(t)),
            this.GetCosine(this.zCosWeight.Evaluate(t))
            ) * this.cosMultiplier;    
       
    /// <summary>
    /// Return cosine value for given theta.
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    private float GetCosine(float theta) => Mathf.Cos(theta);

    /// <summary>
    /// Return sine value for given theta.
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    private float GetSine(float theta) => Mathf.Sin(theta);






}
