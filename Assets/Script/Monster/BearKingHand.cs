using UnityEngine;

public class BearKingHand : MonoBehaviour
{

    public int attackDamage = 10;      // ���� ������


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}�� ������ �¾ҽ��ϴ�!");

            // �÷��̾ ������ ó�� ����
            
        }
    }
}