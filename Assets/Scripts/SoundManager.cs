using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] List<AudioSource> seAudioSources;

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeMasterVolume(float volume)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.audioClip == bgmAudioSource.clip);
        if(data != null)
            bgmAudioSource.volume = data.volume * bgmMasterVolume * volume;
        masterVolume = volume;
    }
    public void ChangeBgmVolume(float volume)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.audioClip == bgmAudioSource.clip);
        if (data != null)
            bgmAudioSource.volume = data.volume * volume * masterVolume;
        bgmMasterVolume = volume;
    }
    public void ChangeSeVolume(float volume)
    {
        seMasterVolume = volume;
    }

    private float bgmTime;
    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        bgmAudioSource.pitch = data.pitch;
        bgmTime = 0;
        bgmAudioSource.time = 0;
        bgmAudioSource.Play();
    }
    public void StopBGM()
    {
        bgmTime = bgmAudioSource.time;
        bgmAudioSource.Stop();
    }
    public void RePlayBGM()
    {
        bgmAudioSource.time = bgmTime;
        bgmAudioSource.Play();
    }


    public void PlaySE(SESoundData.SE se, AudioSource extSource = null)
    {
        SESoundData data = seSoundDatas.Find(d => d.se == se);
        if (data == null) return;

        AudioSource source;

        if (extSource != null)
            source = extSource;
        else
        {
            source = seAudioSources.Find(s => !s.isPlaying);
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                seAudioSources.Add(source);
                source.playOnAwake = false;
            }
        }

        source.volume = data.volume * seMasterVolume * masterVolume;
        source.pitch = data.pitch;
        source.PlayOneShot(data.audioClip);
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        StageBGM,
        MainMenuBGM,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
    public float pitch = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        Damage,
        Jump,
        Land,
        StepRock,
        Cancel,
        Hover,
        Select,
        Pause,
        Clear,
        BigSpikeNotice,
        BigSpikeActive,
        SpikeBall,
        Gun,

    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
    [Range(0, 3)]
    public float pitch = 1;
}