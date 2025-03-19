using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health, sanity, strength, agility, intellect, luck;

    void Start()
    {
        RollStats();
    }

    public void RollStats()
    {
        health = Random.Range(3, 9);
        sanity = Random.Range(3, 9);
        strength = Random.Range(3, 9);
        agility = Random.Range(3, 9);
        intellect = Random.Range(3, 9);
        luck = Random.Range(3, 9);
        // 총합 조정 로직은 나중에 추가 가능
    }

    public int GetStatValue(string stat)
    {
        switch (stat)
        {
            case "health": return health;
            case "sanity": return sanity;
            case "strength": return strength;
            case "agility": return agility;
            case "intellect": return intellect;
            case "luck": return luck;
            default: return 0;
        }
    }
}
