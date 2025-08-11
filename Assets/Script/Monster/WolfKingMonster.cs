using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WolfKingMonster : MonsterBase
{
    private Coroutine currentPatternCoroutine;

    private int nextPattern = 0;

    private static readonly int THROW = 0; //������
    private static readonly int PILLAR = 1; //��ռ����
    private static readonly int SPAWN = 2; //����

    public static bool isThrowWarning = false; //������ ��� ����

    [SerializeField]
    private float PatternRange = 15.0f;

    [Header("Throw")]
    [SerializeField]
    private int waitThrowTime; //������ ��ٸ��� �ð�
    [SerializeField]
    private Transform[] throwPosition; //��ô�� ��ġ
    [SerializeField]
    private GameObject throwObjectPrefab; //��ô�� ������Ʈ ������
    private int throwcount = 0; //��ô�� Ƚ��
    [SerializeField]
    private GameObject warningPrefab; // ���� ��� ������

    [Header("Pillar")]
    [SerializeField]
    private float pillarRange = 5.0f; //��� ����� ����
    [SerializeField]
    private GameObject pillarPrefab; //��� ������
    private int pillarCount = 0; //��� ����� Ƚ��
    [SerializeField]
    private float pillarTime = 7.0f; //��� ������ �ð�

    [Header("Spawn")]
    [SerializeField]
    private GameObject silverSpawnPrefab; //�ǹ����� ���� ������
    [SerializeField]
    private GameObject novaSpawnPrefab; //���ٿ��� ���� ������
    [SerializeField]
    private float spawnRange = 5f; //���� ����
    private int silverSpawnCount = 2; //�ǹ����� ���� Ƚ��
    private int novaSpawnCount = 1; //��ٿ��� ���� Ƚ��

    [Header("Health UI")]
    [SerializeField]
    private GameObject bossHealthUI;
    [SerializeField]
    private Slider bossHealthSlider;

    private bool isPlayerInRange = false;

    protected override void Start()
    {
        base.Start();

        if (bossHealthUI != null)
        {
            bossHealthUI.SetActive(false);
        }

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        base.Update();

        if (distanceToPlayer <= playerDetectionRange && !isPlayerInRange)
        {
            isPlayerInRange = true;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(true);
            }
        }
        else if (distanceToPlayer > playerDetectionRange && isPlayerInRange)
        {
            isPlayerInRange = false;
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(false);
            }
        }
        // ���� ���� ����
        if (inAttackRange && !isAttack)
        {
            HandleAttack(); // �÷��̾ ���� ������ �ٽ� ������ ����
        }
        else if (!inAttackRange && isAttack)
        {
            // ���� ����� ���� ���� ���� �� ���� �ʱ�ȭ
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);
                currentPatternCoroutine = null;
            }
            isAttack = false;
        }
    }

    protected override void HandleAttack()
    {
        if (isAttack || currentPatternCoroutine != null) return; // �̹� ���� ���̸� ����

        animator.SetBool("Walk", false);

        isAttack = true;
        currentPatternCoroutine = StartCoroutine(Throw());

    }
    private IEnumerator Throw()
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        animator.SetBool("Walk", false);
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����

        foreach (Transform pos in throwPosition)
        {
            //��� ȿ�� ǥ��
            StartCoroutine(ShowThrowGroundEffect(pos));

        }
        throwcount++;
        if (throwcount < 2)
        {
            yield return new WaitForSeconds(6.0f); //3�� ���
            nextPattern = THROW;
            currentPatternCoroutine = null;
            nextPatternPlay();
        }
        else
        {
            throwcount = 0;
            yield return new WaitForSeconds(4.0f); //4�� ���
            nextPattern = PILLAR;
            currentPatternCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
            nextPatternPlay();
        }
    }

    private IEnumerator ShowThrowGroundEffect(Transform pos)
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        Vector3 targetPosition = player.transform.position;//�غ� �� �÷��̾��� ���� ��ġ ����
        Vector3 middlePosition = transform.position + ((targetPosition - pos.position) / 2);//�÷��̾�� �߰����� ���
        yield return new WaitForSeconds(0.7f);
        isThrowWarning = true;

        GameObject warning = Instantiate(warningPrefab, new Vector3(pos.position.x, transform.position.y + 0.01f, pos.position.z), pos.rotation);//��� ������Ʈ ����
        Vector3 warnRot = warning.transform.rotation.eulerAngles;
        warnRot.x = 90; //X�� ȸ������ 90���� ����
        warning.transform.rotation = Quaternion.Euler(warnRot);
        warning.transform.localScale = new Vector3(1.5f, 10, 1);//���� ����

        yield return new WaitForSeconds(waitThrowTime);

        ThrowObjectSpawn(pos);//��ô�� ����
        Destroy(warning);

        StartCoroutine(StartAttack01());
    }

    private IEnumerator StartAttack01()
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        animator.SetTrigger("Attack1");
        // �ִϸ��̼��� ���� ������ ���
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            yield return null;
    }

    private void ThrowObjectSpawn(Transform pos)
    {
        if (isDead) return; //�׾����� �ڷ�ƾ ����
        // ���� ��ô�� ����
        GameObject throwObject = Instantiate(throwObjectPrefab, pos.position, pos.rotation);
        throwObject.GetComponent<LunaWolfKingBullet>().thirdPersonController = thirdPersonController;
        throwObject.GetComponent<LunaWolfKingBullet>().damage = damage;

        StartCoroutine(WaitThrowDelay(1f));
    }

    private IEnumerator WaitThrowDelay(float delayTime)
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        yield return new WaitForSeconds(delayTime);
        isThrowWarning = false;
    }

    private IEnumerator Pillar()
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        animator.SetBool("Walk", false);
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        pillarCount = Random.Range(4, 6); //��� ����� Ƚ��

        yield return new WaitForSeconds(2.0f); //2�� ���
        animator.SetTrigger("Attack4");//�ִϸ��̼� ���

        for (int i = 0; i < pillarCount; i++)
        {
            Debug.Log(pillarCount);
            Vector3 randomPos = new Vector3(Random.Range(-pillarRange, pillarRange), 0, Random.Range(-pillarRange, pillarRange));
            GameObject pillar = Instantiate(pillarPrefab, transform.position + randomPos, Quaternion.identity);
            Destroy(pillar, pillarTime); //5�� �� ��� ����
        }
        yield return new WaitForSeconds(5.0f); //5�� ���
        nextPattern = SPAWN;
        currentPatternCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
        nextPatternPlay();
    }

    private IEnumerator Spawn()
    {
        if (isDead) yield break; //�׾����� �ڷ�ƾ ����
        animator.SetBool("Walk", false);
        if (!inAttackRange) yield break; //���� ���� �ȿ� �÷��̾ ������ �������� ����
        yield return new WaitForSeconds(2.0f); //2�� ���
        animator.SetTrigger("Buff");//�ִϸ��̼� ���
        yield return new WaitForSeconds(1.0f); //1�� ���
        for (int i = 0; i < silverSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject silverWolf = Instantiate(silverSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        for (int i = 0; i < novaSpawnCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject novaWolf = Instantiate(novaSpawnPrefab, transform.position + randomPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(3.0f); //3�� ���
        nextPattern = THROW;
        currentPatternCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
        nextPatternPlay();
    }
    protected void nextPatternPlay()
    {
        if (isDead) return;

        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            currentPatternCoroutine = null;
        }

        switch (nextPattern)
        {
            case 0:
                currentPatternCoroutine = StartCoroutine(Throw());
                Debug.Log("������ ���� ����");
                break;
            case 1:
                currentPatternCoroutine = StartCoroutine(Pillar());
                Debug.Log("��� ����� ���� ����");
                break;
            case 2:
                currentPatternCoroutine = StartCoroutine(Spawn());
                Debug.Log("���� ���� ����");
                break;
        }
    }

    protected override void OnDrawGizmosSelected()//���ý� ���̰� //�׻� ���̰� OnDrawGizmos
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.white; //��� ����
        Gizmos.DrawWireSphere(transform.position, pillarRange);
    }
}
