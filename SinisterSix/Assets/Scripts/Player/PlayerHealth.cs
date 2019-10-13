using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "ScriptableObjects/Player/Health")]
public class PlayerHealth : ScriptableObject
{

    /// <summary>
    /// Initial player health.
    /// </summary>
    [SerializeField]
    private float _initialHealth = 100;

    /// <summary>
    /// Reference to the current player health.
    /// </summary>
    [SerializeField]
    private float _currentHealth = 100;

    /// <summary>
    /// Get reference to the current health.
    /// </summary>
    public float Health => this._currentHealth;

    /// <summary>
    /// Reset the player's health.
    /// </summary>
    public void ResetHealth() => this._currentHealth = this._initialHealth;

    /// <summary>
    /// Damage the player by input amount.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void Damage(float damage = 0.0f) => this._currentHealth -= damage;

}