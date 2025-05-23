using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StarterAssets;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

}
public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private ThirdPersonController thePlayer;
    [SerializeField]
    private Inventory theInventory;

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
        if (inventoryObject != null)
        {
            theInventory = inventoryObject.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogWarning("Inventory ������Ʈ�� ã�� ���߽��ϴ�");
        }
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<ThirdPersonController>();
        //theInventory = FindObjectOfType<Inventory>();

        if (theInventory == null)
        {
            Debug.LogWarning("Inventory�� ã�� �� �����ϴ�. SaveData �ߴܵ�.");
            return;
        }

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        // ����Ʈ �ʱ�ȭ
        saveData.invenArrayNumber.Clear();
        saveData.invenItemName.Clear();
        saveData.invenItemNumber.Clear();

        Slot[] slots = theInventory.GetSlots();
        if (slots == null)
        {
            Debug.LogError("���� �迭�� null�Դϴ�.");
            return;
        }
        Debug.Log("���� ����: " + slots.Length);

        for (int i = 0; i < slots.Length; i++)
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
            //theInventory = FindObjectOfType<Inventory>();

            if (thePlayer != null)
            {
                thePlayer.transform.position = saveData.playerPos;
                thePlayer.transform.eulerAngles = saveData.playerRot;

                for(int i = 0; i < saveData.invenItemName.Count; i++)
                {
                    theInventory.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
                    Debug.Log("�κ��丮 �ε� �Ϸ�");
                }
                Debug.Log("�ε� �Ϸ�");
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
