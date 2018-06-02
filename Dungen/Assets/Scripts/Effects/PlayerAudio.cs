using UnityEngine;
using System.Collections;
using System;

public class PlayerAudio : MonoBehaviour
{

    public static PlayerAudio _this;

    [Serializable]
    public struct SoundEffects
    {
        public string name;
        public AudioClip audioClip;
    }
    public SoundEffects[] mSoundEffects;

    public AudioClip PickupAudioClip;

    [SerializeField] private AudioSource mWalkingAudioSource;
    [SerializeField] private AudioSource mAttackAudioSource;
    private int mCurrentState = 0;

    private void Awake()
    {
        _this = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!mWalkingAudioSource && !mAttackAudioSource)
        {
            Debug.LogError(name + ": Make sure both audio source references are set.");
            return;
        }

        mCurrentState = PlayerDirector._this.ReturnCurrentState();

        switch (mCurrentState)
        {
            case 0:
                if (!mWalkingAudioSource.isPlaying) { mWalkingAudioSource.Play(); }
                break;
            case 2:
                mWalkingAudioSource.enabled = false;
                mAttackAudioSource.enabled = false;
                break;
            case 3:
                if (mWalkingAudioSource.isPlaying) { mWalkingAudioSource.Stop(); }
                break;
            default:
                break;
        }

        if (Time.timeScale <= 0)
        {
            mWalkingAudioSource.Pause();
        }
        else if (Time.timeScale > 0)
        {
            mWalkingAudioSource.UnPause();
        }
    }

    public void PlayAttackSound()
    {
        mAttackAudioSource.pitch = UnityEngine.Random.Range(1, 3);
        mAttackAudioSource.PlayOneShot(mSoundEffects[1].audioClip);
    }

    public void PlayPickupSound()
    {
        mAttackAudioSource.PlayOneShot(PickupAudioClip);
    }
}
