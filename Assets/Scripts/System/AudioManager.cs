using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _sfxBall;
    [SerializeField] private AudioClip _sfxScore;
    [SerializeField] private AudioClip _sfxPowerup;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this as AudioManager;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        _musicAudioSource.volume = .05f;
        _sfxAudioSource.volume = .05f;
    }

    private void OnApplicationQuit()
    {
        instance = null;
        Destroy(gameObject);
    }

    public void PlayMenuMusic()
    {
        if (_musicAudioSource.clip != _menuMusic)
            _musicAudioSource.clip = _menuMusic;
        _musicAudioSource.Play();
    }

    public void PlayGameMusic()
    {
        if (_musicAudioSource.clip != _gameMusic)
            _musicAudioSource.clip = _gameMusic;
        _musicAudioSource.Play();
    }

    public void PlaySfxBallSound()
    {
        if (_sfxAudioSource.clip != _sfxBall)
            _sfxAudioSource.clip = _sfxBall;
        _sfxAudioSource.PlayOneShot(_sfxAudioSource.clip);
    }

    public void PlaySfxScoreSound()
    {
        if (_sfxAudioSource.clip != _sfxScore)
            _sfxAudioSource.clip = _sfxScore;
        _sfxAudioSource.PlayOneShot(_sfxAudioSource.clip);
    }

    public void PlaySfxPowerupSound()
    {
        if (_sfxAudioSource.clip != _sfxPowerup)
            _sfxAudioSource.clip = _sfxPowerup;
        _sfxAudioSource.PlayOneShot(_sfxAudioSource.clip);
    }

    public void SetMusicVolume(float value)
    {
        _musicAudioSource.volume = value;
    }

    public void SetSfxVolume(float value)
    {
        _sfxAudioSource.volume = value;
    }

    public void MuteMusic(bool mute)
    {
        _musicAudioSource.mute = mute;
    }

    public void MuteSfx(bool mute)
    {
        _sfxAudioSource.mute = mute;
    }

    public void MuteMaster(bool mute)
    {
        MuteSfx(mute);
        MuteMusic(mute);
    }
}
