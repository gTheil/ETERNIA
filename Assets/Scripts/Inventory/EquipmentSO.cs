using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public int equipID;
    public EquipType equipType;
    public int swordAtk, bowAtk, shieldDef, shieldBlockPoint;
    public float atkPush, projectileSpeed, shieldRecoverySpeed, shieldRecoveryCooldown, shieldPushResistance, shieldBreakOffset;

    public bool EquipItem() {
        Player player = GameManager.instance.GetPlayer().GetComponent<Player>();

        if (player.GetEquipment(equipType) == null || player.GetEquipment(equipType).equipID != this.equipID) {
             player.SetEquipment(this);
             return true;       
        }

        return false;
    }
}

public enum EquipType {
    sword,
    bow,
    shield,
    none,
};
