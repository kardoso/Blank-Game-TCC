using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class SoundManager : MonoBehaviour
{

	/*
		To play a sound effect:
		SoundManager.PlaySFX(audioClip);
	
		To play background music or fade into a new track:
		SoundManager.PlayBGM(audioClip, shouldFade, fadeDuration);
	*/


    // Static instance
    static SoundManager _instance;
    public static SoundManager GetInstance()
    {
        if (!_instance)
        {
            GameObject soundManager = new GameObject("SoundManager");
			_instance = soundManager.AddComponent<SoundManager>();
			_instance.Initialize();
        }
        return _instance;
    }

	const float MaxVolume_BGM = 1f;
	const float MaxVolume_SFX = 1f;
	static float CurrentVolumeNormalized_BGM = 1f;
	static float CurrentVolumeNormalized_SFX = 1f;
	static bool isMuted = false;
	List<AudioSource> sfxSources;
	AudioSource bgmSource;

	void Initialize()
	{
		// add our bgm sound source
		bgmSource = gameObject.AddComponent<AudioSource>();
		bgmSource.loop = true;
		bgmSource.playOnAwake = false;
		bgmSource.volume = GetBGMVolume();
		DontDestroyOnLoad(gameObject);
	}

	// ==================== Volume Getters =====================
	public static float GetBGMVolume () {
		return isMuted ? 0f : MaxVolume_BGM * CurrentVolumeNormalized_BGM;
	}
	public static float GetSFXVolume () {
	return isMuted ? 0f : MaxVolume_SFX * CurrentVolumeNormalized_SFX;
	}

	// ====================== BGM Utils ======================
	void FadeBGMOut (float fadeDuration)
	{
		SoundManager soundMan = GetInstance();
		float delay = 0f;
		float toVolume = 0f;
		if(soundMan.bgmSource.clip == null){
		Debug.LogError("Error: Could not fade BGM out as BGM AudioSource has no currently playing clip.");
		}
		StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
	}
	void FadeBGMIn (AudioClip bgmClip, float delay, float fadeDuration)
	{
		SoundManager soundMan = GetInstance();
		soundMan.bgmSource.clip = bgmClip;
		soundMan.bgmSource.Play();
		float toVolume = GetBGMVolume();
		StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
	}
	IEnumerator FadeBGM(float fadeToVolume, float delay, float duration)
	{
		yield return new WaitForSeconds (delay);
		SoundManager soundMan = GetInstance();
		float elapsed = 0f;
		while (duration > 0) {
			float t = (elapsed / duration);
			//float volume = Mathf.Lerp (0f, fadeToVolume*CurrentVolumeModifier_BGM, t);
			float volume = Mathf.Lerp (0f, fadeToVolume*0.1f, t);
			soundMan.bgmSource.volume = volume;
			elapsed += Time.deltaTime;
			yield return 0;
		}
	}

	// ====================== BGM Functions ======================
	public static void PlayBGM(AudioClip bgmClip, bool fade, float fadeDuration)
	{
		SoundManager soundMan = GetInstance();
		if (fade) {
			if (soundMan.bgmSource.isPlaying) {
				// fade out, then switch and fade in
				soundMan.FadeBGMOut(fadeDuration/2);
				soundMan.FadeBGMIn (bgmClip, fadeDuration / 2, fadeDuration / 2);
			} else {
			// just fade in
			float delay = 0f;
			soundMan.FadeBGMIn(bgmClip, delay, fadeDuration);
			}
		} else {
			// play immediately
			soundMan.bgmSource.volume = GetBGMVolume ();
			soundMan.bgmSource.clip = bgmClip;
			soundMan.bgmSource.Play ();
		}
	}
	public static void StopBGM(bool fade, float fadeDuration)
	{
		SoundManager soundMan = GetInstance();
		if (soundMan.bgmSource.isPlaying) {
			// fade out, then switch and fade in
			if(fade){
				soundMan.FadeBGMOut(fadeDuration);
			} else {
				soundMan.bgmSource.Stop();
			}
		}
	}

	public static void PauseBGM()
	{
		SoundManager soundMan = GetInstance();
		if (soundMan.bgmSource.isPlaying) {
			soundMan.bgmSource.Pause();
		}
	}

	public static void UnPauseBGM()
	{
		SoundManager soundMan = GetInstance();
		if (soundMan.bgmSource.isPlaying) {
			soundMan.bgmSource.UnPause();
		}
	}

	// ======================= SFX Utils ====================================
	AudioSource GetSFXSource()
	{
		// set up a new sfx sound source for each new sfx clip
		AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
		sfxSource.loop = false;
		sfxSource.playOnAwake = false;
		sfxSource.volume = GetSFXVolume();
		if (sfxSources == null)
		{
			sfxSources = new List<AudioSource>();
		}
		sfxSources.Add(sfxSource);
		return sfxSource;
	}
	IEnumerator RemoveSFXSource(AudioSource sfxSource)
	{
		yield return new WaitForSeconds(sfxSource.clip.length);
		sfxSources.Remove(sfxSource);
		Destroy(sfxSource);
	}
	IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length)
	{
		yield return new WaitForSeconds(length);
		sfxSources.Remove(sfxSource);
		Destroy(sfxSource);
	}

	// ====================== SFX Functions =================================
	public static void PlaySFX(AudioClip sfxClip)
	{
		SoundManager soundMan = GetInstance();
		AudioSource source = soundMan.GetSFXSource();
		source.volume = GetSFXVolume ();
		source.clip = sfxClip;
		source.Play();
		soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
	}
	public static void PlaySFXRandomized(AudioClip sfxClip)
	{
		SoundManager soundMan = GetInstance();
		AudioSource source = soundMan.GetSFXSource();
		source.volume = GetSFXVolume ();
		source.clip = sfxClip;
		source.pitch = Random.Range(0.85f, 1.2f);
		source.Play();
		soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
	}
	public static void PlaySFXFixedDuration(AudioClip sfxClip, float duration, float volumeMultiplier = 1.0f)
	{
		SoundManager soundMan = GetInstance();
		AudioSource source = soundMan.GetSFXSource();
		source.volume = GetSFXVolume() * volumeMultiplier;
		source.clip = sfxClip;
		source.loop = true;
		source.Play();
		soundMan.StartCoroutine(soundMan.RemoveSFXSourceFixedLength(source, duration));
	}

	// ==================== Volume Control Functions ==========================
	public static void DisableSoundImmediate()
	{
		SoundManager soundMan = GetInstance();
		soundMan.StopAllCoroutines();
		if (soundMan.sfxSources != null)
		{
			foreach (AudioSource source in soundMan.sfxSources)
			{
				source.volume = 0;
			}
		}
		soundMan.bgmSource.volume = 0f;
		isMuted = true;
	}
	public static void EnableSoundImmediate()
	{
		SoundManager soundMan = GetInstance();
		if (soundMan.sfxSources != null)
		{
			foreach (AudioSource source in soundMan.sfxSources)
			{
				source.volume = GetSFXVolume();
			}
		}
		soundMan.bgmSource.volume = GetBGMVolume();
		isMuted = false;
	}
	public static void SetGlobalVolume(float newVolume)
	{
		if(newVolume <= 0){
			newVolume = 0;
		}
		else if(newVolume >= 1){
			newVolume = 1;
		}
		CurrentVolumeNormalized_BGM = newVolume;
		CurrentVolumeNormalized_SFX = newVolume;
		AdjustSoundImmediate();
	}
	public static void SetSFXVolume(float newVolume){
		if(newVolume <= 0){
			newVolume = 0;
		}
		else if(newVolume >= 1){
			newVolume = 1;
		}
		CurrentVolumeNormalized_SFX = newVolume;
		AdjustSoundImmediate();
	}
	public static void SetBGMVolume(float newVolume){
		if(newVolume <= 0){
			newVolume = 0;
		}
		else if(newVolume >= 1){
			newVolume = 1;
		}
		CurrentVolumeNormalized_BGM = newVolume;
		AdjustSoundImmediate();
	}

	public static void AdjustSoundImmediate()
	{
		SoundManager soundMan = GetInstance();
		if (soundMan.sfxSources != null)
		{
			foreach (AudioSource source in soundMan.sfxSources)
			{
				source.volume = GetSFXVolume();
			}
		}
		Debug.Log("BGM Volume: "+ GetBGMVolume());
		soundMan.bgmSource.volume = GetBGMVolume();
		Debug.Log("BGM Volume is now: " + GetBGMVolume());
	}

	// ==================== Volume Increase/Decrease Functions ==========================
	public static void decreaseFXVolume(){
		SetSFXVolume(GetSFXVolume() - 0.1f);
	}

	public static void increaseFXVolume(){
		SetSFXVolume(GetSFXVolume() + 0.1f);
	}

	public static void decreaseBGMVolume(){
		SetBGMVolume(GetBGMVolume() - 0.1f);
	}

	public static void increaseBGMVolume(){
		SetBGMVolume(GetBGMVolume() + 0.1f);
	}
}