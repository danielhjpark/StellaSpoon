using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI gameTimeText; // 게임 시간을 표시할 UI 텍스트
    public TextMeshProUGUI gameDaysText; // 게임 일수를 표시할 UI 텍스트

    public float gameTime = 0f; // 게임 내 시간 (단위: 초)
    private const string lastSavedTimeKey = "LastSavedTime"; // PlayerPrefs 키
    private const string gameTimeKey = "GameTime"; // PlayerPrefs 키
    private const string gameDaysKey = "GameDays"; // PlayerPrefs 키

    public int startHour = 0; // 초기 게임 시각 (예: 0시)
    public int startMinute = 0;
    public int startDays = 0;

    public int gameHours; //게임 시간
    public int gameMinutes; //게임 분
    public int gameDays = 0; //게임 일

    private bool shouldLoadSavedTime = true; // 기본은 저장된 시간 불러오기

    private void Start()
    {
        //저장된 시간 가져오기
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
        // 실제 시간의 흐름을 기반으로 게임 시간 증가
        gameTime += Time.deltaTime * 60f; // 1초 실제 시간 = 60초 게임 시간

        //24시간 갱신
        if (gameTime >= 86400f) //24시간 = 86400초
        {
            gameTime -= 86400f;
            gameDays++; //게임 일 증가
            //해당 부분에서 날짜 계산 가능
        }

        // 게임 시간 계산
        gameHours = (int)(gameTime / 3600) % 24; // 게임 시간 (시)
        gameMinutes = (int)(gameTime / 60) % 60; // 게임 시간 (분)

        // 게임 시간 텍스트 업데이트
        if (gameTimeText != null)
        {
            gameTimeText.text = $"Time\n{gameHours:D2}:{gameMinutes:D2}";
        }
        if (gameDaysText != null)
        {
            gameDaysText.text = $"{gameDays}일";
        }
    }
    private void OnApplicationQuit()
    {
        // 게임 종료 시 시간 저장
        SaveGameTime();
    }

    private void SaveGameTime()
    {
        // 현재 시간 기록
        PlayerPrefs.SetString(lastSavedTimeKey, DateTime.Now.ToString());
        PlayerPrefs.SetFloat(gameTimeKey, gameTime);
        PlayerPrefs.SetInt(gameDaysKey, gameDays); // 게임 일수 저장
        PlayerPrefs.Save();
    }

    private void LoadGameTime()
    {
        // 마지막 저장된 시간 가져오기
        if (PlayerPrefs.HasKey(lastSavedTimeKey) && PlayerPrefs.HasKey(gameTimeKey))
        {
            /*string lastSavedTimeString = PlayerPrefs.GetString(LastSavedTimeKey);*/
            float savedGameTime = PlayerPrefs.GetFloat(gameTimeKey);


            /*게임 종료시에도 시간이 흐르는걸 원치 않으면 지우면 됨*//*
            //lastSavedTimeString가 필요 없음
            // 저장된 시간과 현재 시간의 차이 계산
            DateTime lastSavedTime = DateTime.Parse(lastSavedTimeString);
            TimeSpan timeDifference = DateTime.Now - lastSavedTime;

            // 경과된 실제 시간을 게임 시간으로 변환
            float elapsedGameTime = (float)timeDifference.TotalSeconds * 60f;
            *//*여기까지*/


            // 게임 시간 복구
            gameTime = savedGameTime;

            // 24시간을 초과하면 초기화
            gameTime %= 86400f;

            if (PlayerPrefs.HasKey(gameDaysKey))
            {
                gameDays = PlayerPrefs.GetInt(gameDaysKey); // 게임 일수 복구
            }
            else
            {
                gameDays = 0; // 게임 일수 초기화
            }
        }
        else
        {
            // 저장된 데이터가 없으면 초기화
            gameTime = (startHour * 3600) + (startMinute * 60);
        }
    }

    public void AddTime(int minutes)
    {
        gameTime += minutes * 60f;
    }
    
    public void RestaurantOpenTime()
    {
        gameTime = (18 * 3600); // 18시간 = 8 * 3600초
        gameDays = 1;

        gameHours = 18;
        gameMinutes = 0;

        Debug.Log("식당 오픈시간 18시 설정");
    }

    public void InitializeNewGameTime()
    {
        // 1일차 오전 8시 00분 (총 초 계산)
        gameTime = (8 * 3600); // 8시간 = 8 * 3600초
        gameDays = 1;

        gameHours = 8;
        gameMinutes = 0;

        Debug.Log("New Game 시간 초기화: 1일 08:00");
    }
    public void DisableAutoLoad()
    {
        shouldLoadSavedTime = false;
    }
}
