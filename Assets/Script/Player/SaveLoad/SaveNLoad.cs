using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StarterAssets;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    [Header("�÷��̾�")]
    public Vector3 playerPos;
    public Vector3 playerRot;
    public float playerHp;

    [Header("��")]
    public string currentSceneName;

    [Header("�κ��丮")]
    public List<int> invenArrayNumber = new List<int>(); 
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    [Header("�����")]
    public List<int> refriArrayNumber = new List<int>();
    public List<string> refriItemName = new List<string>();
    public List<int> refriItemNumber = new List<int>();

    [Header("���� 1, 2")]
    public List<int> chest1ArrayNumber = new List<int>();
    public List<string> chest1ItemName = new List<string>();
    public List<int> chest1ItemNumber = new List<int>();
    public List<int> chest2ArrayNumber = new List<int>();
    public List<string> chest2ItemName = new List<string>();
    public List<int> chest2ItemNumber = new List<int>();

    [Header("����")]
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
    [SerializeField]
    private Inventory chest1Inventory;
    [SerializeField]
    private Inventory chest2Inventory;


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
        GameObject chest1InventoryObject = GameObject.Find("PARENT_RestaurantChestBase (1)");
        GameObject chest2InventoryObject = GameObject.Find("PARENT_RestaurantChestBase (2)");

        if (inventoryObject != null)
        {
            theInventory = inventoryObject.GetComponent<Inventory>();
            theRefrigeratorInventory = refriInventoryObject.GetComponent<RefrigeratorInventory>();
        }
        else
        {
            Debug.LogWarning("Inventory ������Ʈ�� ã�� ���߽��ϴ�");
        }
        if (chest1InventoryObject != null)
            chest1Inventory = chest1InventoryObject.GetComponent<Inventory>();

        if (chest2InventoryObject != null)
            chest2Inventory = chest2InventoryObject.GetComponent<Inventory>();
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<ThirdPersonController>();
        rifleManager = FindObjectOfType<RifleManager>();

        if (theInventory == null)
        {
            Debug.LogWarning("Inventory�� ã�� �� �����ϴ�. SaveData �ߴܵ�.");
            return;
        }
        // �÷��̾�
        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;
        saveData.playerHp = thePlayer.curHP;

        // ����
        saveData.equippedWeaponIndex = rifleManager.CurrentWeaponLevel;
        saveData.gunTempest = rifleManager.tempestFang;
        saveData.gunInferno = rifleManager.infernoLance;
        saveData.stage1Clear = Manager.stage_01_clear;
        saveData.stage2Clear = Manager.stage_02_clear;

        // �� �̸�
        saveData.currentSceneName = SceneManager.GetActiveScene().name;

        // ����Ʈ �ʱ�ȭ
        saveData.invenArrayNumber.Clear();
        saveData.invenItemName.Clear();
        saveData.invenItemNumber.Clear();

        saveData.refriArrayNumber.Clear();
        saveData.refriItemName.Clear();
        saveData.refriItemNumber.Clear();

        Slot[] slots = theInventory.GetSlots();
        if (slots == null)
        {
            Debug.LogError("���� �迭�� null�Դϴ�.");
            return;
        }
        Debug.Log("���� ����: " + slots.Length);

        for (int i = 0; i < slots.Length; i++) // �κ��丮 ����
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
                Debug.Log($"���� {i}: {slots[i].item.itemName} ({slots[i].itemCount}) �����");
            }
            else
            {
                Debug.Log($"���� {i}�� ��� ����");
            }
        }

        RefrigeratorSlot[] refriSlots = theRefrigeratorInventory.refrigeratorSlots; // ����� ����
        if (refriSlots == null)
        {
            Debug.LogError("����� ���� �迭�� null�Դϴ�.");
            return;
        }
        Debug.Log("����� ���� ����: " + refriSlots.Length);

        for (int i = 0; i < refriSlots.Length; i++)
        {
            if (refriSlots[i].item != null)
            {
                saveData.refriArrayNumber.Add(i);
                saveData.refriItemName.Add(refriSlots[i].item.itemName);
                saveData.refriItemNumber.Add(refriSlots[i].itemCount);
                Debug.Log($"����� ���� {i}: {refriSlots[i].item.itemName} ({refriSlots[i].itemCount}) �����");
            }
            else
            {
                Debug.Log($"����� ���� {i}�� ��� ����");
            }
        }
        // ����1 ����
        Slot[] chest1Slots = chest1Inventory.GetSlots();
        for (int i = 0; i < chest1Slots.Length; i++)
        {
            if (chest1Slots[i].item != null)
            {
                saveData.chest1ArrayNumber.Add(i);
                saveData.chest1ItemName.Add(chest1Slots[i].item.itemName);
                saveData.chest1ItemNumber.Add(chest1Slots[i].itemCount);
                Debug.Log($"����1 ���� {i}: {chest1Slots[i].item.itemName} ({chest1Slots[i].itemCount}) �����");
            }
        }
        // ����2 ����
        Slot[] chest2Slots = chest2Inventory.GetSlots();
        for (int i = 0; i < chest2Slots.Length; i++)
        {
            if (chest2Slots[i].item != null)
            {
                saveData.chest2ArrayNumber.Add(i);
                saveData.chest2ItemName.Add(chest2Slots[i].item.itemName);
                saveData.chest2ItemNumber.Add(chest2Slots[i].itemCount);
                Debug.Log($"����2 ���� {i}: {chest2Slots[i].item.itemName} ({chest2Slots[i].itemCount}) �����");
            }
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("���̺� �Ϸ�:\n" + json);
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
                // �÷��̾�
                thePlayer.transform.position = saveData.playerPos;
                thePlayer.transform.eulerAngles = saveData.playerRot;
                // HP
                thePlayer.curHP = saveData.playerHp;
                thePlayer._hpBar.value = thePlayer.curHP;
                // �� ���� �ε�
                rifleManager.tempestFang = saveData.gunTempest;
                rifleManager.infernoLance = saveData.gunInferno;
                rifleManager.SwitchWeapon(saveData.equippedWeaponIndex);

                // �Ϲ� ���� �ε�
                Manager.stage_01_clear = saveData.stage1Clear;
                Manager.stage_02_clear = saveData.stage2Clear;

                // �κ��丮 �ε�
                for (int i = 0; i < saveData.invenItemName.Count; i++)
                {
                    theInventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
                    Debug.Log("�κ��丮 �ε� �Ϸ�");
                }
                Debug.Log("�ε� �Ϸ�");
                // ����� �ε�
                for (int i = 0; i < saveData.refriItemName.Count; i++)
                {
                    theRefrigeratorInventory.LoadToInven(saveData.refriArrayNumber[i], saveData.refriItemName[i], saveData.refriItemNumber[i]);
                }
                Debug.Log("����� �ε� �Ϸ�");

                for (int i = 0; i < saveData.chest1ItemName.Count; i++)
                {
                    chest1Inventory.LoadToInven(saveData.chest1ArrayNumber[i], saveData.chest1ItemName[i], saveData.chest1ItemNumber[i]);
                }
                Debug.Log("����1 �ε� �Ϸ�");

                for (int i = 0; i < saveData.chest2ItemName.Count; i++)
                {
                    chest2Inventory.LoadToInven(saveData.chest2ArrayNumber[i], saveData.chest2ItemName[i], saveData.chest2ItemNumber[i]);
                }
                Debug.Log("����2 �ε� �Ϸ�");
            }
            else
            {
                Debug.LogWarning("ThirdPersonController�� ã�� �� �����ϴ�. LoadData �ߴܵ�.");
            }
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�.");
        }
    }
}
