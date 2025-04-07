using StarterAssets;
using UnityEngine;

public class BearKingHand : MonoBehaviour
{

    public int attackDamage = 40;      // ���� ������

    public ThirdPersonController thirdPersonController; // �÷��̾��� ThirdPersonController ��ũ��Ʈ ����

    private void Start()
    {
        thirdPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}�� ������ �¾ҽ��ϴ�!");

            thirdPersonController.TakeDamage(attackDamage, transform.position); //�÷��̾� ������

        }
    }
}