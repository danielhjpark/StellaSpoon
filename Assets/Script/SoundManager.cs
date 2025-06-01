using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM 종류들
    public enum EBgm
    {
        BGM_RESTAURANT,
        BGM_SHOP,
        BGM_SCALIA,
        BGM_SERENOXIA,
    }

    //SFX 종류들
    public enum ESfx
    {
        SFX_BUTTON,
        SFX_OPENBOX,
        SFX_GUNSHOT,
    }
    public enum EPlayerSfx
    {
        GlassFootstep,
        Error,
    }
    public enum Interact
    {
        TurnOnMovePlanet,
        MovePlanet,
        Door,
    }

    public enum Display
    {
        Display_Menu_Button,
        Button,
        Drop_Item,
        Move_Item,
        Receipe_Select,
        Item_Pick_Up,
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
        Error,
    }

    public enum Gun
    {
        Button,
        Shot
    }

    public enum Player
    {
        restfoot,
        foot,
        jump,
        hit,
        land,
        roll,
        reload,
    }

    //audio clip 담을 수 있는 배열
    [SerializeField] public AudioClip[] bgms;
    [SerializeField] public AudioClip[] sfxs;
    [SerializeField] AudioClip[] displays;
    [SerializeField] AudioClip[] dailyMenus;
    [SerializeField] AudioClip[] stores;
    [SerializeField] AudioClip[] interacts;
    [SerializeField] AudioClip[] guns;
    [SerializeField] AudioClip[] player;
    [SerializeField] private AudioClip[] playerSfxs;

    //플레이하는 AudioSource
    [Header("Audio Source")]
    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;
    [SerializeField] AudioSource audioInteract;

    [Header("사운드 조절")]
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(PlanetManager.selectedPlanet);
        Debug.Log((int)EBgm.BGM_RESTAURANT);
        string[] sceneNames = { "WokMergeTest", "FryingPanMergeTest", "PotMergeTest", "CuttingBoardMergeTest" };
        foreach (string sceneName in sceneNames) if (scene.name == sceneName) return;
        switch (PlanetManager.selectedPlanet)
        {
            case PlanetManager.PlanetType.Restaurant:
                PlayBGM(EBgm.BGM_RESTAURANT);

                break;
            case PlanetManager.PlanetType.Shop:
                PlayBGM(EBgm.BGM_SHOP);
                break;
            case PlanetManager.PlanetType.aRedForest:
                PlayBGM(EBgm.BGM_SCALIA);
                break;
            case PlanetManager.PlanetType.Serenoxia:
                PlayBGM(EBgm.BGM_SERENOXIA);
                break;
            default:

                return;
        }
    }

    // EBgm 열거형을 매개변수로 받아 해당하는 배경 음악 클립을 재생
    public void PlayBGM(EBgm bgmIdx)
    {
        //enum int형으로 형변환 가능
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    // 현재 재생 중인 배경 음악 정지
    public void StopBGM()
    {
        audioBgm.Stop();
    }

    // ESfx 열거형을 매개변수로 받아 해당하는 효과음 클립을 aaa재생
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
        if ((int)interact <= 1) audioSfx.PlayOneShot(interacts[(int)interact], 0.7f);
        audioSfx.PlayOneShot(interacts[(int)interact]);
    }
    public void PlaySound(Store store)
    {
        audioSfx.PlayOneShot(stores[(int)store]);
    }
    public void Play_GunChangeSound(Gun gun)
    {
        audioSfx.PlayOneShot(guns[(int)gun], 0.6f);
    }
    public void PlayPlayerSound(Player playerType)
    {
        audioSfx.PlayOneShot(player[(int)playerType], 0.3f);
    }
    public void PlayPlayerSFX(EPlayerSfx sfx)
    {
        int index = (int)sfx;
        if (index >= 0 && index < playerSfxs.Length && playerSfxs[index] != null)
        {
            audioSfx.PlayOneShot(playerSfxs[index], 0.3f);
        }
        else
        {
            Debug.LogWarning($"PlayerSFX [{sfx}] is missing or not assigned.");
        }
    }
    public void PlayGunSound(AudioClip clip)
    {
        audioSfx.PlayOneShot(clip, 0.3f);
    }

    //사용예시
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
