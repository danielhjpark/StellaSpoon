using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM ������
    public enum EBgm
    {
        BGM_TITLE,
        BGM_GAME,
    }

    //SFX ������
    public enum ESfx
    {
        SFX_BUTTON,
        SFX_OPENBOX,
        SFX_GUNSHOT,
    }

    //audio clip ���� �� �ִ� �迭
    [SerializeField]
    public AudioClip[] bgms;
    [SerializeField]
    public AudioClip[] sfxs;

    //�÷����ϴ� AudioSource
    [SerializeField]
    public AudioSource audioBgm;
    [SerializeField]
    public AudioSource audioSfx;

    [Header("���� ����")]
    public AudioMixer masterMixer;
    public Slider bgmAudioSlider;
    public Slider sfxAudioSlider;
    public Slider masterAudioSlider;



    private void Start()
    {
        bgmAudioSlider.onValueChanged.AddListener(delegate { AudioBGMControl(); });
        sfxAudioSlider.onValueChanged.AddListener(delegate { AudioSFXControl(); });
        masterAudioSlider.onValueChanged.AddListener(delegate { AudioMASTERControl(); });
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // EBgm �������� �Ű������� �޾� �ش��ϴ� ��� ���� Ŭ���� ���
    public void PlayBGM(EBgm bgmIdx)
    {
        //enum int������ ����ȯ ����
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    // ���� ��� ���� ��� ���� ����
    public void StopBGM()
    {
        audioBgm.Stop();
    }

    // ESfx �������� �Ű������� �޾� �ش��ϴ� ȿ���� Ŭ���� ���
    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }

    //��뿹��
    /*public void OnClickBack()
    {
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BUTTON);
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_TITLE);

        PhotonNetwork.LoadLevel("GameLobbyScene");
    }*/


    public void AudioBGMControl()
    {
        float sound = bgmAudioSlider.value;

        if (sound == -40f)
        {
            masterMixer.SetFloat("BGM", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM", sound);
        }
    }

    public void AudioSFXControl()
    {
        float sound = sfxAudioSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("SFX", -80);
        }
        else
        {
            masterMixer.SetFloat("SFX", sound);
        }
    }
    public void AudioMASTERControl()
    {
        float sound = masterAudioSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("Master", -80);
        }
        else
        {
            masterMixer.SetFloat("Master", sound);
        }
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}
