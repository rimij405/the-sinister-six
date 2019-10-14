using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using NaughtyAttributes;

public class ImageFade : MonoBehaviour
{

    public Image image;

    public AnimationCurve opacity = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    private float currentOpacity = 0.0f;

    [OnValueChanged("UpdateState")]
    [Slider(0.0f, 1.0f)]
    public float state = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        state = 0.0f;
        image = image ?? this.GetComponent<Image>();
        this.UpdateState();
    }

    public void SetState(float _state)
    {
        this.state = Mathf.Clamp(_state, 0.0f, 1.0f);
        this.UpdateState();
    }

    public void Flash(float duration)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Fade(1.0f, duration));
    }





    public void UpdateState()
    {
        this.currentOpacity = this.opacity.Evaluate(this.state);
        this.UpdateOpacity();
    }

    public void FadeIn(float duration = 1.0f)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Fade(1.0f, duration));
    }
    public void FadeOut(float duration = 1.0f)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Fade(0.0f, duration));
    }

    public void UpdateOpacity()
    {
        if (this.image)
        {
            Color temp = this.image.color;
            this.image.color = new Color(temp.r, temp.g, temp.b, this.currentOpacity);
        }
    }

    public IEnumerator Fade(float targetOpacity, float duration)
    {
        // Set the animation curve.
        opacity = AnimationCurve.EaseInOut(0.0f, this.currentOpacity, 1.0f, targetOpacity);
        float time = 0.0f;

        while (time <= duration)
        {
            // Update state.
            time += Time.deltaTime;
            this.state = time / duration;
            this.UpdateState();
            yield return null;
        }

        this.state = 1.0f;
        this.UpdateState();
    }

}
