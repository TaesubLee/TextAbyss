using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text mainText;
    public Button[] optionButtons;
    public TextAsset jsonFile;
    private EventList eventList;
    private PlayerStats playerStats;

    [System.Serializable]
    public class EventData
    {
        public string eventId;
        public string description;
        public OptionData[] options;
    }

    [System.Serializable]
    public class OptionData
    {
        public string text;
        public string requiredStat;
        public int successChance;
        public string successEvent;
        public string failEvent;
    }

    [System.Serializable]
    public class EventList
    {
        public EventData[] events;
    }

    void Start()
    {
        Debug.Log("GameManager Start ȣ��");
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null) Debug.LogError("PlayerStats�� ã�� �� �����ϴ�!");
        eventList = JsonUtility.FromJson<EventList>(jsonFile.text);
        if (eventList == null || eventList.events == null)
        {
            Debug.LogError("JSON �ε� ���� �Ǵ� �����Ͱ� ��� �ֽ��ϴ�!");
        }
        else
        {
            Debug.Log($"JSON �ε� ����: {eventList.events.Length}���� �̺�Ʈ �ε�");
            foreach (var evt in eventList.events)
            {
                Debug.Log($"�̺�Ʈ Ȯ��: {evt.eventId}");
            }
        }
        LoadEvent("start");
    }

    public void LoadEvent(string eventId)
    {
        Debug.Log($"LoadEvent ȣ��: {eventId}");
        EventData currentEvent = System.Array.Find(eventList.events, e => e.eventId == eventId);
        if (currentEvent == null)
        {
            Debug.LogError($"�̺�Ʈ ID {eventId}�� ã�� �� �����ϴ�!");
            Debug.Log("���� �ε�� �̺�Ʈ ���:");
            foreach (var evt in eventList.events)
            {
                Debug.Log($"- {evt.eventId}");
            }
            return;
        }
        mainText.text = currentEvent.description;
        Debug.Log($"�̺�Ʈ �ε�: {currentEvent.description}");
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentEvent.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = currentEvent.options[i].text;
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(currentEvent.options[index]));
                Debug.Log($"��ư {i} ����: {currentEvent.options[i].text}");
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }
    void OnOptionSelected(OptionData option)
    {
        Debug.Log($"OnOptionSelected ȣ��: {option.text}");
        if (string.IsNullOrEmpty(option.requiredStat))
        {
            Debug.Log($"�ܼ� �̵�: {option.successEvent}");
            LoadEvent(option.successEvent);
        }
        else
        {
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats�� null�Դϴ�!");
                return;
            }
            int successChance = option.successChance + (playerStats.GetStatValue(option.requiredStat) * 5);
            string nextEventId = (Random.Range(0, 100) < successChance) ? option.successEvent : option.failEvent;
            Debug.Log($"Ȯ�� ���: {successChance}%, ���� �̺�Ʈ: {nextEventId}");
            LoadEvent(nextEventId);
        }
    }
}