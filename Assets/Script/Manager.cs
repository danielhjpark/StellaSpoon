using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Manager : MonoBehaviour
{
    public static bool Initialized { get; set; } = false;

    private static Manager _instance;

    public static Manager Instance {  get { return _instance; } }

    private GameTimeManager _timer = new GameTimeManager();


    public static GameTimeManager Timer
    {
        get
        {
            Init();
            if (Instance?._timer == null)
            {
                // GameTimeManager가 없으면 추가
                Instance._timer = Instance.gameObject.GetComponent<GameTimeManager>()
                                  ?? Instance.gameObject.AddComponent<GameTimeManager>();
            }
            return Instance._timer;
        }
    }
    private static void Init()
    {
        if (_instance == null && Initialized == false)
        {
            Initialized = true;

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Manager>();
            }
            if (go.GetComponent<GameTimeManager>() == null)
            {
                go.AddComponent<GameTimeManager>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Manager>();
        }
    }
}
