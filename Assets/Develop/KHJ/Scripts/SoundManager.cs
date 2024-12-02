using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // BGM types
    public enum E_BGM
    {
        LOGIN,
        LOBBY,
        ROOM,

        FARM,
        TOMATO,
        ICE_VILLAGE,
        PIRATE,
        FACTORY,
        FOREST,
    }

    // SFX typs
    public enum E_SFX
    {
        CLICK,

        GET_ITEM,
        BOMB_SET,
        BOMB_EXPLOSION,
        BOMB_LOCKED,
        BOMB_RESCUED,
        BOMB_DEAD,

        START,
        WIN,
        LOSE,
        DRAW,
    }

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] _bgms;
    [SerializeField] AudioClip[] _sfxs;

    [Header("Audio Source")]
    [SerializeField] AudioSource _audioBgm;
    [SerializeField] AudioSource _audioSfx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���ϴ� BGM�� ����մϴ�.
    /// </summary>
    /// <param name="bgmIdx">����� ���ϴ� BGM</param>
    public void PlayBGM(E_BGM bgmIdx)
    {
        _audioBgm.clip = _bgms[(int)bgmIdx];
        _audioBgm.Play();
    }

    /// <summary>
    /// ��� ���� BGM�� �����մϴ�.
    /// </summary>
    public void StopBGM()
    {
        _audioBgm.Stop();
    }

    /// <summary>
    /// ���ϴ� SFX�� �� �� ����մϴ�.
    /// </summary>
    /// <param name="sfxIdx">����� ���ϴ� SFX</param>
    /// <param name="volumeScale">���� ���� [0, 1]</param>
    public void PlaySFX(E_SFX sfxIdx, float volumeScale = 1f)
    {
        _audioSfx.PlayOneShot(_sfxs[(int)sfxIdx], volumeScale);
    }
}