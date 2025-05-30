using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetInfo
{
    public string planetName; //행성 이름
    public string type; //유형
    public string location; //위치
    public string description; //설명
    public List<string> monsters; //등장 몬스터 목록
    public List<string> plant; //자생식물
}
