using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableItem : ScriptableObject
{
    //public int itemID;
    public int restoreHealth;
    public int attackBonus;
    public int defenseBonus;
    public float buffDuration;
    public bool pushImmune;

    public bool UseItem() {
        Player player = GameManager.instance.GetPlayer().GetComponent<Player>();

        if (restoreHealth > 0) {
            if (player.hitPoint == player.hitPointMax) {
                return false;
            } else {
                player.RestoreHealth(restoreHealth);
                return true;
            }
        }

        if (attackBonus > 0) {
            if (player.attackBonus > 0) {
                return false;
            } else {
                player.IncreaseStat("attack", attackBonus, buffDuration);
                return true;
            }
        }

        if (defenseBonus > 0) {
            if (player.defenseBonus > 0) {
                return false;
            } else {
                player.IncreaseStat("defense", defenseBonus, buffDuration);
                return true;
            }
        }

        if (pushImmune) {
            if (player.pushImmune == true) {
                return false;
            } else {
                player.IncreaseStat("pushImmune", 0, buffDuration);
                return true;
            }
        }

        return false;
    }
    
}
