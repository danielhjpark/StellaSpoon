using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI gameTimeText; // ���� �ð��� ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI gameDaysText; // ���� �ϼ��� ǥ���� UI �ؽ�Ʈ

    public float gameTime = 0f; // ���� �� �ð� (����: ��)
    private const string lastSavedTimeKey = "LastSavedTime"; // PlayerPrefs Ű
    private const string gameTimeKey = "GameTime"; // PlayerPrefs Ű
    private const string gameDaysKey = "GameDays"; // PlayerPrefs Ű

    public int startHour = 0; // �ʱ� ���� �ð� (��: 0��)
    public int startMinute = 0;
    public int startDays = 0;

    public int gameHours; //���� �ð�
    public int gameMinutes; //���� ��
    public int gameDays = 0; //���� ��

    private bool shouldLoadSavedTime = true; // �⺻�� ����� �ð� �ҷ�����

    private void Start()
    {
        //����� �ð� ��������
        if (shouldLoadSavedTime)
        {
            LoadGameTime();
        }

        if (gameTimeText == null)
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
            gameDays++; //���� �� ����
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
        if (gameDaysText != null)
        {
            gameDaysText.text = $"{gameDays}��";
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
        PlayerPrefs.SetString(lastSavedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.SetFloat(gameTimeKey, gameTime);
        PlayerPrefs.SetInt(gameDaysKey, gameDays); // ���� �ϼ� ����
        PlayerPrefs.Save();
    }

    private void LoadGameTime()
    {
        // ������ ����� �ð� ��������
        if (PlayerPrefs.HasKey(lastSavedTimeKey) && PlayerPrefs.HasKey(gameTimeKey))
        {
            /*string lastSavedTimeString = PlayerPrefs.GetString(LastSavedTimeKey);*/
            float savedGameTime = PlayerPrefs.GetFloat(gameTimeKey);


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

            if (PlayerPrefs.HasKey(gameDaysKey))
            {
                gameDays = PlayerPrefs.GetInt(gameDaysKey); // ���� �ϼ� ����
            }
            else
            {
                gameDays = 0; // ���� �ϼ� �ʱ�ȭ
            }
        }
        else
        {
            // ����� �����Ͱ� ������ �ʱ�ȭ
            gameTime = (startHour * 3600) + (startMinute * 60);
        }
    }

    public void AddTime(int minutes)
    {
        gameTime += minutes * 60f;
    }
    
    public void RestaurantOpenTime()
    {
        gameTime = (18 * 3600); // 18�ð� = 8 * 3600��
        gameDays = 1;

        gameHours = 18;
        gameMinutes = 0;

        Debug.Log("�Ĵ� ���½ð� 18�� ����");
    }

    public void InitializeNewGameTime()
    {
        // 1���� ���� 8�� 00�� (�� �� ���)
        gameTime = (8 * 3600); // 8�ð� = 8 * 3600��
        gameDays = 1;

        gameHours = 8;
        gameMinutes = 0;

        Debug.Log("New Game �ð� �ʱ�ȭ: 1�� 08:00");
    }
    public void DisableAutoLoad()
    {
        shouldLoadSavedTime = false;
    }
}
