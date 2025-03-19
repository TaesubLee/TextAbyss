using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health, sanity, strength, agility, intellect, luck;
    void Start()
    {
        health = Random.Range(3, 9);
        sanity = Random.Range(3, 9);
        strength = Random.Range(3, 9);
        agility = Random.Range(3, 9);
        intellect = Random.Range(3, 9);
        luck = Random.Range(3, 9);
    }
}
