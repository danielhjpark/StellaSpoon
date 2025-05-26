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

    public enum Interact
    {
        TurnOnMovePlanet,
        MovePlanet,
    }

    public enum Display
    {
        Display_Menu_Button,
        Button,
        Drop_Item,
        Move_Item,
        Receipe_Select,
    }

    public enum DailyMenu
    {
        Daily_Menu_Button,
        Daily_Menu_Complete_Button,
        Button_,
    }

    public enum Store
    {
        Daily_Menu_Button,
        Button,
    }

    //audio clip ���� �� �ִ� �迭
    [SerializeField] public AudioClip[] bgms;
    [SerializeField] public AudioClip[] sfxs;
    [SerializeField] AudioClip[] displays;
    [SerializeField] AudioClip[] dailyMenus;
    [SerializeField] AudioClip[] stores;
    [SerializeField] AudioClip[] interacts;

    //�÷����ϴ� AudioSource
    [Header("Audio Source")]
    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;


    [Header("���� ����")]
    public AudioMixer masterMixer;
    public Slider bgmAudioSlider;
    public Slider sfxAudioSlider;
    public Slider masterAudioSlider;


    private void Start()
    {
        bgmAudioSlider = GameObject.Find("Canvas/PARENT_Option(DeactivateThis)/OptionBase/SoundBase/BGMSlider").GetComponent<Slider>();
        sfxAudioSlider = GameObject.Find("Canvas/PARENT_Option(DeactivateThis)/OptionBase/SoundBase/SFXSlider").GetComponent<Slider>();
        masterAudioSlider = GameObject.Find("Canvas/PARENT_Option(DeactivateThis)/OptionBase/SoundBase/MasterSlider").GetComponent<Slider>();

        bgmAudioSlider.onValueChanged.AddListener(delegate { AudioBGMControl(); });
        sfxAudioSlider.onValueChanged.AddListener(delegate { AudioSFXControl(); });
        masterAudioSlider.onValueChanged.AddListener(delegate { AudioMASTERControl(); });
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    public void PlaySound(Display display)
    {
        audioSfx.PlayOneShot(displays[(int)display]);
    }

    public void PlaySound(DailyMenu dailyMenu)  
    {
        audioSfx.PlayOneShot(dailyMenus[(int)dailyMenu]);
    }
    
    public void PlaySound(Interact interact)
    {
        audioSfx.PlayOneShot(interacts[(int)interact]);
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
