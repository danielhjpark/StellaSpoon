using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetInfo
{
    public string planetName; //행성 이름
    public string description; //행성 설명
    public string weather; //행성 날씨
    public float gravity; //중력 정도
    public List<string> monsters; //등장 몬스터 목록
}
