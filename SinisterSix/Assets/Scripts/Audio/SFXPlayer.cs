using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;
using NaughtyAttributes;

public class SFXPlayer : MonoBehaviour
{

    /// <summary>
    /// Clip to use.
    /// </summary>
    private AudioClip clip = null;

    private Audio clipCache = null;

    [Slider(0.0f, 1.0f)]
    public float volume;

    /// <summary>
    /// Random sampling pitch change on play.
    /// </summary>
    [Slider(0.0f, 5.0f)]
    public float pitchRange = 1.0f;

    public float _cooldownDuration = 10.0f;

    private float _cooldown = 0.0f;

    public void Start()
    {
        this.PrepareAudio(this.clip);
    }

    public void SetAudioClip(AudioClip _clip) => this.SetAudioClip(_clip, this.volume, this.pitchRange, this._cooldownDuration);

    public void SetAudioClip(AudioClip _clip, float _volume = 1.0f, float _pitchRange = 1.0f, float _padding = 0.4f)
    {
        this.clip = _clip;
        this.volume = _volume;
        this.pitchRange = _pitchRange;
        this._cooldownDuration = _padding;
        this.clipCache = null;

        this.PrepareAudio(this.clip);
    }

    public void PrepareAudio(AudioClip _clip)
    {
        if (_clip != this.clip)
        {
            this.clip = _clip;
            clipCache = null;
        }

        if (clipCache == null)
        {
            int id = EazySoundManager.PrepareSound(this.clip, this.volume, false, this.transform);
            this.clipCache = EazySoundManager.GetSoundAudio(id);
        }
    }

    public void UpdateVolume()
    {
        if (clipCache != null)
        {
            Audio sfx = this.GetAudio(this.clip);
            sfx.SetVolume(this.volume);
        }
    }
    
    public Audio GetAudio(AudioClip _clip)
    {
        if(clipCache == null)
        {
            this.PrepareAudio(_clip);
        }
        return this.clipCache;            
    }

    public void PlaySoundOnce(AudioClip _clip)
    {
        if(clip != _clip)
        {
            this.SetAudioClip(_clip);
        }

        if(clip != null)
        {
            Audio sfx = this.GetAudio(this.clip);
            if (!sfx.IsPlaying && _cooldown <= 0.0f)
            {
                float basePitch = sfx.Pitch;
                sfx.Pitch = basePitch + Random.Range(-pitchRange, pitchRange);
                sfx.Play();
                this.StartCoroutine(this.Cooldown());
                sfx.Pitch = basePitch;
            }
        }
    }

    public IEnumerator Cooldown()
    {
        this._cooldown = this._cooldownDuration;
        while(this._cooldown > 0.0f)
        {
            this._cooldown -= Time.deltaTime;
            yield return null;
        }
    }











}
