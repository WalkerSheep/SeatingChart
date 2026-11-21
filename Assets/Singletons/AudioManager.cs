using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private ObjectPoolController spawner => ObjectPoolController.instance;

    public GameObject audioHolder;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioSource musicSourceTwo;
    public float musicFadeSpeed = 1f;
    public float musicSpeed = 1f;

    [Header("Volume")]
    public float[] soundEffectVolumes;
    public float[] musicVolumes;
    public int soundEffectVolume;
    public int musicVolume;

    [Header("Sound Effect Cooldown")]
    public float sameSoundCooldown = 0.2f;

    private Dictionary<AudioClip, float> lastSoundTimes =
        new Dictionary<AudioClip, float>();

    private AudioSource currentMusicSource;

    void Awake()
    {
        soundEffectVolume = PlayerPrefs.GetInt("SoundEffectVolume", 5);
        musicVolume = PlayerPrefs.GetInt("MusicVolume", 3);

        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("SoundEffectVolume", soundEffectVolume);

        UpdateMusicSource(musicSource);
        UpdateMusicSource(musicSourceTwo);

        musicSource.pitch = Mathf.Lerp(
            musicSource.pitch,
            musicSpeed,
            Time.unscaledDeltaTime
        );

        musicSourceTwo.pitch = Mathf.Lerp(
            musicSourceTwo.pitch,
            musicSpeed,
            Time.unscaledDeltaTime
        );
    }

    private void UpdateMusicSource(AudioSource source)
    {
        if (source == null || source.clip == null)
            return;

        float targetVolume = source == currentMusicSource
            ? musicVolumes[musicVolume]
            : 0f;

        source.volume = Mathf.MoveTowards(
            source.volume,
            targetVolume,
            musicFadeSpeed * Time.unscaledDeltaTime
        );

        if (source != currentMusicSource &&
            source.volume <= 0f &&
            source.isPlaying)
        {
            source.Pause();
        }
    }

    public void SetSoundEffectVolume(int amount)
    {
        soundEffectVolume += amount;

        if (soundEffectVolume < 0)
        {
            soundEffectVolume = 0;
        }
        else if (soundEffectVolume > soundEffectVolumes.Length - 1)
        {
            soundEffectVolume = soundEffectVolumes.Length - 1;
        }
    }

    public void SetMusicVolume(int amount)
    {
        musicVolume += amount;

        if (musicVolume < 0)
        {
            musicVolume = 0;
        }
        else if (musicVolume > musicVolumes.Length - 1)
        {
            musicVolume = musicVolumes.Length - 1;
        }
    }

    public void SetSpeed(float speed)
    {
        musicSpeed = speed;
    }

    public void PlayMusic(AudioClip music, float point = 0f)
    {
        if (music == null)
            return;

        if (currentMusicSource != null &&
            currentMusicSource.clip == music)
        {
            return;
        }

        AudioSource existingSource = GetSourceWithClip(music);

        if (existingSource != null)
        {
            currentMusicSource = existingSource;
            existingSource.UnPause();

            return;
        }

        AudioSource newSource = GetNextMusicSource();

        if (newSource == null)
            return;

        newSource.clip = music;
        newSource.time = Mathf.Clamp(point, 0f, music.length);
        newSource.volume = 0f;
        newSource.pitch = musicSpeed;
        newSource.Play();

        currentMusicSource = newSource;
    }

    private AudioSource GetSourceWithClip(AudioClip clip)
    {
        if (musicSource.clip == clip)
            return musicSource;

        if (musicSourceTwo.clip == clip)
            return musicSourceTwo;

        return null;
    }

    private AudioSource GetNextMusicSource()
    {
        if (musicSource.clip == null)
            return musicSource;

        if (musicSourceTwo.clip == null)
            return musicSourceTwo;

        if (currentMusicSource == musicSource)
            return musicSourceTwo;

        return musicSource;
    }

    public void StopMusic()
    {
        currentMusicSource = null;

        musicSource.Stop();
        musicSourceTwo.Stop();

        musicSource.volume = 0f;
        musicSourceTwo.volume = 0f;

        musicSource.clip = null;
        musicSourceTwo.clip = null;
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();

        if (musicSourceTwo.isPlaying)
            musicSourceTwo.Pause();
    }

    public void ResumeMusic()
    {
        if (currentMusicSource != null)
        {
            currentMusicSource.UnPause();
        }
    }

    private bool CanPlaySound(AudioClip sound)
    {
        if (sound == null)
            return false;

        float currentTime = Time.unscaledTime;

        if (lastSoundTimes.TryGetValue(sound, out float lastTime))
        {
            if (currentTime - lastTime < sameSoundCooldown)
            {
                return false;
            }
        }

        lastSoundTimes[sound] = currentTime;

        return true;
    }

    public AudioSource PlaySound(AudioClip sound, Vector3 position, float pitchVar = 0.1f)
    {
        if (!CanPlaySound(sound))
            return null;

        float chosenPitch =
            1f + Random.Range(-pitchVar, pitchVar);

        GameObject spawned = spawner.SpawnObject(
            audioHolder,
            position,
            Quaternion.identity,
            sound.length + 1f
        );

        AudioSource source =
            spawned.GetComponent<AudioSource>();

        source.volume =
            soundEffectVolumes[soundEffectVolume];

        source.spatialBlend = 1f;
        source.pitch = chosenPitch;
        source.clip = sound;
        source.Play();

        return source;
    }

    public void PlaySoundFlat(AudioClip sound, float pitchVar = 0.1f)
    {
        if (!CanPlaySound(sound))
            return;

        float chosenPitch =
            1f + Random.Range(-pitchVar, pitchVar);

        GameObject spawned = spawner.SpawnObject(
            audioHolder,
            Vector3.zero,
            Quaternion.identity,
            sound.length + 1f
        );

        AudioSource source =
            spawned.GetComponent<AudioSource>();

        source.volume =
            soundEffectVolumes[soundEffectVolume];

        source.spatialBlend = 0f;
        source.pitch = chosenPitch;
        source.clip = sound;
        source.Play();
    }
}