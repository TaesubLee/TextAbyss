using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int birthNum,allstats;
    public int statMin, statMax;
    public int health, sanity, strength, agility, intellect, luck;

    void Start()
    {
        RollStats();
    }

    public void RollStats()
    {
        birthNum = Random.Range(1, 5);
        switch (birthNum)
        {
            case 1:
                allstats = 50;
                statMin = 5;
                statMax = 20;
                break;
            case 2:
                allstats = 60;
                statMin = 8;
                statMax = 25;
                break;
            case 3:
                allstats = 70;
                statMin = 10;
                statMax = 30;
                break;
            case 4:
                allstats = 80;
                statMin = 12;
                statMax = 35;
                break;
            case 5:
                allstats = 90;
                statMin = 15;
                statMax = 40;
                break;
        }
        health = Random.Range(50, 100);
        sanity = Random.Range(50, 100);
        // 능력치 분배
        int[] stats = GetConstrainedRandomStats(allstats, 4, statMin, statMax);

        strength = stats[0];
        agility = stats[1];
        intellect = stats[2];
        luck = stats[3];

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
    int[] GetConstrainedRandomStats(int total, int count, int minVal, int maxVal)
    {
        int[] result = new int[count];
        int remaining = total;

        // 우선 각 스탯에 최소값을 할당
        for (int i = 0; i < count; i++)
        {
            result[i] = minVal;
            remaining -= minVal;
        }

        // 이제 남은 값을 랜덤하게 분배 (각각 최대값까지)
        while (remaining > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (remaining <= 0) break;

                int maxAdd = Mathf.Min(remaining, maxVal - result[i]);
                int add = Random.Range(0, maxAdd + 1);
                result[i] += add;
                remaining -= add;
            }
        }

        // 섞어서 랜덤성 보장
        Shuffle(result);

        return result;
    }

    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
