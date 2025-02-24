using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;

public class ShowStateText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private TextMeshProUGUI defenseText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI speedText;

    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    void Update()
    {

        if(InventoryManager.instance.isWeaponRifle == true)
        {
            attackText.text = RifleManager.instance.attackDamage.ToString();
        }
        else
        {
            attackText.text = 0.ToString();
        }
        defenseText.text = thirdPersonController.Def.ToString();
        healthText.text = thirdPersonController.MaxHP.ToString();
        speedText.text = thirdPersonController.MoveSpeed.ToString();
    }
}
