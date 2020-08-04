using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioSubscriber : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioSource audioSource => _audioSource ?? (_audioSource = GetComponent<AudioSource>());
    public bool audioOn = true;

    public abstract void Init(Transform transform);
    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void UpdateVolumn();
}

public class SfxSubscriber : AudioSubscriber
{
    public void SFXPlayOneShot(AudioClip audioClip, bool stopPrevious = false)
    {
        if (!AudioManager.muteSFX && audioOn)
        {
            if (stopPrevious)
                audioSource.Stop();
            audioSource.PlayOneShot(audioClip, AudioManager.sfxVol);
        }
    }
    protected override void OnEnable() { AudioManager.instance.UpdateSfxVolumn += UpdateVolumn; }
    protected override void OnDisable() { AudioManager.instance.UpdateSfxVolumn -= UpdateVolumn; }
    protected override void UpdateVolumn() { audioSource.mute = AudioManager.muteSFX; audioSource.volume = AudioManager.sfxVol; }

    public override void Init(Transform transform)
    {
        this.name = this.GetType().ToString();
        this.transform.parent = transform;
        this.transform.localPosition = Vector3.zero;
    }
}

public class MusicSubscriber : AudioSubscriber
{
    private Coroutine musicCorou;

    public void MusicPlay(AudioClip audioClip, bool loop)
    {
        StopAllCorou();
        if (audioOn) PlayMusic(audioClip, loop);
    }

    public void MusicPlay(AudioClip audioClip, bool loop, float durationCrossfade)
    {
        StopAllCorou();
        if (audioOn) VolLerp(AudioManager.musicVol, durationCrossfade, () => PlayMusic(audioClip, loop));
    }

    private void PlayMusic(AudioClip audioClip, bool loop = true)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = AudioManager.musicVol;
        audioSource.Play();
    }

    public void MusicStop()
    {
        StopAllCorou();
        StopMusic();
    }

    public void MusicStop(float durationCrossfade)
    {
        StopAllCorou();
        VolLerp(0f, durationCrossfade, () => StopMusic());
    }

    private void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.volume = AudioManager.musicVol;
    }

    private void VolLerp(float tarVol, float duration, Action onDone = null)
    {
        StopAllCorou();
        musicCorou = StartCoroutine(VolLerpIE(tarVol, duration, onDone));
    }

    private IEnumerator VolLerpIE(float tarVol, float duration, Action onDone = null)
    {
        float t = 0f;
        float curVol = audioSource.volume;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(curVol, tarVol, t);
            yield return GameManager.WaitForEndOfFrame;
        }
        audioSource.volume = tarVol;
        onDone?.Invoke();
        yield break;
    }

    private void StopAllCorou() { if (musicCorou != null) StopCoroutine(musicCorou); }

    protected override void OnEnable() { AudioManager.instance.UpdateMusicVolumn += UpdateVolumn; }
    protected override void OnDisable() { AudioManager.instance.UpdateMusicVolumn -= UpdateVolumn; StopAllCorou(); }
    protected override void UpdateVolumn() { audioSource.mute = AudioManager.muteMusic; audioSource.volume = AudioManager.musicVol; }

    public override void Init(Transform transform)
    {
        this.name = this.GetType().ToString();
        this.transform.parent = transform;
        this.transform.localPosition = Vector3.zero;
    }
}
