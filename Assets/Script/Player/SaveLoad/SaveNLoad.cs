using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StarterAssets;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    [Header("플레이어")]
    public Vector3 playerPos;
    public Vector3 playerRot;
    public float playerHp;

    [Header("씬")]
    public string currentSceneName;

    [Header("인벤토리")]
    public List<int> invenArrayNumber = new List<int>(); 
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    [Header("냉장고")]
    public List<int> refriArrayNumber = new List<int>();
    public List<string> refriItemName = new List<string>();
    public List<int> refriItemNumber = new List<int>();

    //[Header("상자")]

    [Header("변수")]
    public int equippedWeaponIndex;
    public bool gunTempest;
    public bool gunInferno;
    public bool stage1Clear;
    public bool stage2Clear;
}
public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private ThirdPersonController thePlayer;
    private RifleManager rifleManager;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private RefrigeratorInventory theRefrigeratorInventory;

    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject inventoryObject = GameObject.Find("PARENT_InventoryBase(DeactivateThis)");
        GameObject refriInventoryObject = GameObject.Find("PARENT_RefrigeratorBase(DeactivateThis)");

        if (inventoryObject != null)
        {
            theInventory = inventoryObject.GetComponent<Inventory>();
            theRefrigeratorInventory = refriInventoryObject.GetComponent<RefrigeratorInventory>();
        }
        else
        {
            Debug.LogWarning("Inventory 오브젝트를 찾지 못했습니다");
        }
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<ThirdPersonController>();
        rifleManager = FindObjectOfType<RifleManager>();

        if (theInventory == null)
        {
            Debug.LogWarning("Inventory를 찾을 수 없습니다. SaveData 중단됨.");
            return;
        }
        // 플레이어
        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;
        saveData.playerHp = thePlayer.curHP;

        // 변수
        saveData.equippedWeaponIndex = rifleManager.CurrentWeaponLevel;
        saveData.gunTempest = rifleManager.tempestFang;
        saveData.gunInferno = rifleManager.infernoLance;
        saveData.stage1Clear = Manager.stage_01_clear;
        saveData.stage2Clear = Manager.stage_02_clear;

        // 씬 이름
        saveData.currentSceneName = SceneManager.GetActiveScene().name;

        // 리스트 초기화
        saveData.invenArrayNumber.Clear();
        saveData.invenItemName.Clear();
        saveData.invenItemNumber.Clear();

        saveData.refriArrayNumber.Clear();
        saveData.refriItemName.Clear();
        saveData.refriItemNumber.Clear();

        Slot[] slots = theInventory.GetSlots();
        if (slots == null)
        {
            Debug.LogError("슬롯 배열이 null입니다.");
            return;
        }
        Debug.Log("슬롯 개수: " + slots.Length);

        for (int i = 0; i < slots.Length; i++) // 인벤토리 저장
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
                Debug.Log($"슬롯 {i}: {slots[i].item.itemName} ({slots[i].itemCount}) 저장됨");
            }
            else
            {
                Debug.Log($"슬롯 {i}은 비어 있음");
            }
        }

        RefrigeratorSlot[] refriSlots = theRefrigeratorInventory.refrigeratorSlots; // 냉장고 저장
        if (refriSlots == null)
        {
            Debug.LogError("냉장고 슬롯 배열이 null입니다.");
            return;
        }
        Debug.Log("냉장고 슬롯 개수: " + refriSlots.Length);

        for (int i = 0; i < refriSlots.Length; i++)
        {
            if (refriSlots[i].item != null)
            {
                saveData.refriArrayNumber.Add(i);
                saveData.refriItemName.Add(refriSlots[i].item.itemName);
                saveData.refriItemNumber.Add(refriSlots[i].itemCount);
                Debug.Log($"냉장고 슬롯 {i}: {refriSlots[i].item.itemName} ({refriSlots[i].itemCount}) 저장됨");
            }
            else
            {
                Debug.Log($"냉장고 슬롯 {i}은 비어 있음");
            }
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("세이브 완료:\n" + json);
    }


    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<ThirdPersonController>();
            rifleManager = FindObjectOfType<RifleManager>();

            if (thePlayer != null)
            {
                // 플레이어
                thePlayer.transform.position = saveData.playerPos;
                thePlayer.transform.eulerAngles = saveData.playerRot;
                // HP
                thePlayer.curHP = saveData.playerHp;
                thePlayer._hpBar.value = thePlayer.curHP;
                // 총 변수 로드
                rifleManager.tempestFang = saveData.gunTempest;
                rifleManager.infernoLance = saveData.gunInferno;
                rifleManager.SwitchWeapon(saveData.equippedWeaponIndex);

                // 일반 변수 로드
                Manager.stage_01_clear = saveData.stage1Clear;
                Manager.stage_02_clear = saveData.stage2Clear;

                // 인벤토리 로드
                for (int i = 0; i < saveData.invenItemName.Count; i++)
                {
                    theInventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
                    Debug.Log("인벤토리 로드 완료");
                }
                Debug.Log("로드 완료");
                // 냉장고 로드
                for (int i = 0; i < saveData.refriItemName.Count; i++)
                {
                    theRefrigeratorInventory.LoadToInven(saveData.refriArrayNumber[i], saveData.refriItemName[i], saveData.refriItemNumber[i]);
                }
                Debug.Log("냉장고 로드 완료");
            }
            else
            {
                Debug.LogWarning("ThirdPersonController를 찾을 수 없습니다. LoadData 중단됨.");
            }
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
