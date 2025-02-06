using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI gameTimeText; // ���� �ð��� ǥ���� UI �ؽ�Ʈ

    private float gameTime = 0f; // ���� �� �ð� (����: ��)
    private const string LastSavedTimeKey = "LastSavedTime"; // PlayerPrefs Ű
    private const string GameTimeKey = "GameTime"; // PlayerPrefs Ű

    public int startHour = 0; // �ʱ� ���� �ð� (��: 0��)
    public int startMinute = 0;

    public int gameHours; //���� �ð�
    public int gameMinutes; //���� ��

    
    private void Start()
    {
        //����� �ð� ��������
        LoadGameTime();

        if(gameTimeText == null)
        {
            gameTimeText = GameObject.Find("Canvas/Time").GetComponent<TextMeshProUGUI>();
        }
    }
    void Update()
    {
        // ���� �ð��� �帧�� ������� ���� �ð� ����
        gameTime += Time.deltaTime * 60f; // 1�� ���� �ð� = 60�� ���� �ð�

        //24�ð� ����
        if (gameTime >= 86400f) //24�ð� = 86400��
        {
            gameTime -= 86400f;
            //�ش� �κп��� ��¥ ��� ����
        }

        // ���� �ð� ���
        gameHours = (int)(gameTime / 3600) % 24; // ���� �ð� (��)
        gameMinutes = (int)(gameTime / 60) % 60; // ���� �ð� (��)

        // ���� �ð� �ؽ�Ʈ ������Ʈ
        if (gameTimeText != null)
        {
            gameTimeText.text = $"Time\n{gameHours:D2}:{gameMinutes:D2}";
        }
    }
    private void OnApplicationQuit()
    {
        // ���� ���� �� �ð� ����
        SaveGameTime();
    }

    private void SaveGameTime()
    {
        // ���� �ð� ���
        PlayerPrefs.SetString(LastSavedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.SetFloat(GameTimeKey, gameTime);
        PlayerPrefs.Save();
    }

    private void LoadGameTime()
    {
        // ������ ����� �ð� ��������
        if (PlayerPrefs.HasKey(LastSavedTimeKey) && PlayerPrefs.HasKey(GameTimeKey))
        {
            /*string lastSavedTimeString = PlayerPrefs.GetString(LastSavedTimeKey);*/
            float savedGameTime = PlayerPrefs.GetFloat(GameTimeKey);


            /*���� ����ÿ��� �ð��� �帣�°� ��ġ ������ ����� ��*//*
            //lastSavedTimeString�� �ʿ� ����
            // ����� �ð��� ���� �ð��� ���� ���
            DateTime lastSavedTime = DateTime.Parse(lastSavedTimeString);
            TimeSpan timeDifference = DateTime.Now - lastSavedTime;

            // ����� ���� �ð��� ���� �ð����� ��ȯ
            float elapsedGameTime = (float)timeDifference.TotalSeconds * 60f;
            *//*�������*/


            // ���� �ð� ����
            gameTime = savedGameTime;

            // 24�ð��� �ʰ��ϸ� �ʱ�ȭ
            gameTime %= 86400f;
        }
        else
        {
            // ����� �����Ͱ� ������ �ʱ�ȭ
            gameTime = (startHour * 3600) + (startMinute * 60);
        }
    }
}
