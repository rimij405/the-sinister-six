using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{

    // Health object for player to reset.
    public PlayerHealth health;

    // Game over event is invoked.
    public UnityEvent onGameOver;

    public float transitionDuration = 2.0f;

    public ImageFade gameOverFade = null;

    // Start is called before the first frame update
    void Start()
    {
        // On game start, reset health.
        health.ResetHealth();

        Debug.Log(health.Health);

        onGameOver = this.onGameOver ?? new UnityEvent();
        onGameOver.AddListener(LoadGameOverScene);
    }
    
    public void LoadGameOverScene()
    {
        this.StartCoroutine(Transition(2.0f));
    }

    public IEnumerator Transition(float duration)
    {
        if (gameOverFade)
        {
            gameOverFade.FadeIn(duration);
        }
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("End");
    }

    public void EndGame()
    {
        onGameOver.Invoke();
    }





}
