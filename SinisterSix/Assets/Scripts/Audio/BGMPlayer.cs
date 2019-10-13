using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;
using NaughtyAttributes;
using UnityEngine.Animations;

[RequireComponent(typeof(BoxCollider))]
public class BGMPlayer : MonoBehaviour
{

    /// <summary>
    /// Player to target.
    /// </summary>
    [Required]
    public GameObject player = null;

    /// <summary>
    /// All clips to play for when this player's trigger volume is entered.
    /// </summary>
    [Label("Audio Clips")]
    public List<AudioClip> clips = null;

    /// <summary>
    /// Audio cache.
    /// </summary>
    private List<Audio> _audioCache = null;

    /// <summary>
    /// Time it takes for volume fade to take effect.
    /// </summary>
    public float fadeTransition = 1.0f;

    /// <summary>
    /// Fade state.
    /// </summary>
    public float fadeState = 0.0f;

    /// <summary>
    /// Track volume.
    /// </summary>
    [Slider(0.0f, 1.0f)]
    public float maxTrackVolume = 1.0f;

    /// <summary>
    /// The current track volume.
    /// </summary>
    private float _currentTrackVolume = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        EazySoundManager.GlobalVolume = 1.0f;

        // Prepare cache.
        this._audioCache = this._audioCache ?? new List<Audio>();

        if(this.clips != null && this.clips.Count > 0)
        {
            foreach(AudioClip clip in this.clips)
            {
                // Fill the cache.
                int clipID = EazySoundManager.PrepareMusic(clip, 0.0f, true, false, this.fadeTransition, this.fadeTransition);
                this._audioCache.Add(EazySoundManager.GetMusicAudio(clipID));
            }

            this.PlayTracksAsync(1.0f);
        }

    }

    /// <summary>
    /// Volume fade coroutine.
    /// </summary>
    /// <returns></returns>
    public IEnumerator VolumeFade()
    {
        float fadeTime = 0.0f;
        float fadeTarget = this.maxTrackVolume * this.fadeState;
        AnimationCurve curve = AnimationCurve.Linear(0.0f, this._currentTrackVolume, this.fadeTransition, fadeTarget);
        while (fadeTime < this.fadeTransition)
        {
            fadeTime += Time.deltaTime; 
            this._currentTrackVolume = curve.Evaluate(fadeTime);
            yield return null;
        }
        this._currentTrackVolume = fadeTarget;
        this.UpdateVolume();
    }

    public IEnumerator PlayTracksAsync(float afterAllSeconds)
    {
        yield return new WaitForSeconds(afterAllSeconds);
        this.PlayTracks();
    }

    /// <summary>
    /// Play all tracks.
    /// </summary>
    public void PlayTracks()
    {
        foreach(Audio audio in this._audioCache)
        {
            this.PlayClip(audio);
        }
    }

    public void PauseAllTracks()
    {
        foreach (Audio audio in this._audioCache)
        {
            audio.Pause();
        }
    }

    public void ResumeAllTracks()
    {
        foreach (Audio audio in this._audioCache)
        {
            audio.UnPause();
        }
    }

        /// <summary>
        /// Stop all tracks.
        /// </summary>
        public void StopAllTracks()
    {
        foreach (Audio audio in this._audioCache)
        {
            audio.Stop();
        }
    }
    
    /// <summary>
    /// Play clip if it isn't null.
    /// </summary>
    /// <param name="clip"></param>
    public void PlayClip(Audio bgm)
    {
        if(bgm != null && !bgm.IsPlaying)
        {
            Debug.Log("Playing new track.");
            bgm.Play();
        }
    }

    /// <summary>
    /// Update the volume for tracks.
    /// </summary>
    public void UpdateVolume()
    {
        foreach(Audio bgm in this._audioCache)
        {
            Debug.Log("Updating volumes.");
            bgm.SetVolume(this._currentTrackVolume);
        }
    }

    /// <summary>
    /// Start the fade in.
    /// </summary>
    public void StartFadeIn()
    {
        this.PlayTracks();
        this.fadeState = 1.0f;
        this.StartCoroutine(this.VolumeFade());
    }

    /// <summary>
    /// Start the fade out.
    /// </summary>
    public void StartFadeOut()
    {
        this.fadeState = 0.0f;
        this.StartCoroutine(this.VolumeFade());
    }

    /// <summary>
    /// When entering the volume, transition fade in.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.player)
        {
            // Stop all coroutines on this object.
            this.StopAllCoroutines();

            // Start transition.
            this.StartFadeIn();
        }
    }

    /// <summary>
    /// When exiting the volume, transition fade out.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == this.player)
        {
            // Stop all coroutines on this object.
            this.StopAllCoroutines();

            // Start transition.
            this.StartFadeOut();
        }
    }
}
