using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class PotSystem : MonoBehaviour
{
    [SerializeField] PlayableDirector lidTimeline;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private CinemachineTrackedDolly dolly;
    private bool movingForward = true;
    public float speed = 2f;
    private bool isRewinding = false;
    private float rewindSpeed = 1f; // ����� �ӵ� (1���)


    // Start is called before the first frame update
    void Start()
    {
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_PathPosition = 0;
    }


    void Update()
    {
        if (isRewinding)
        {
            lidTimeline.time -= Time.deltaTime * rewindSpeed; // time ���� ���� ����
            if (lidTimeline.time <= 0)
            {
                lidTimeline.time = 0;
                isRewinding = false;
            }
            lidTimeline.Evaluate(); // ��� ����
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            ViewControll();
        }

    }

    private void ViewControll() {
        if(movingForward) {
            StartCoroutine(TopView());
        }
        else {
            StartCoroutine(FrontView());
        }
    }
    public void PlayForward()
    {
        lidTimeline.Play();
        lidTimeline.time = 0; // Ÿ�Ӷ��� ������
        lidTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1); // ������ ���
        
    }

    public void PlayBackward()
    {
        isRewinding = true;
        lidTimeline.Pause();
    }


    IEnumerator TopView() {
        PlayForward();
        while(movingForward) {
            dolly.m_PathPosition += speed * Time.deltaTime;
            if (dolly.m_PathPosition >= 2) // �ִ� WayPoint ����
                movingForward = false;
            yield return null;
        }
    }

    IEnumerator FrontView() {
        PlayBackward();
        while(!movingForward) {
            dolly.m_PathPosition -= speed * Time.deltaTime;
            if (dolly.m_PathPosition <= 0) // �ּ� WayPoint ����
                movingForward = true;
            yield return null;
        }
    }

    public void SetDirection(bool forward)
    {
        movingForward = forward;
    }


}
