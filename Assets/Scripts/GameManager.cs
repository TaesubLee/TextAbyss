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
        Debug.Log("GameManager Start 호출");
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null) Debug.LogError("PlayerStats를 찾을 수 없습니다!");
        eventList = JsonUtility.FromJson<EventList>(jsonFile.text);
        if (eventList == null || eventList.events == null)
        {
            Debug.LogError("JSON 로드 실패 또는 데이터가 비어 있습니다!");
        }
        else
        {
            Debug.Log($"JSON 로드 성공: {eventList.events.Length}개의 이벤트 로드");
            foreach (var evt in eventList.events)
            {
                Debug.Log($"이벤트 확인: {evt.eventId}");
            }
        }
        LoadEvent("start");
    }

    public void LoadEvent(string eventId)
    {
        Debug.Log($"LoadEvent 호출: {eventId}");
        EventData currentEvent = System.Array.Find(eventList.events, e => e.eventId == eventId);
        if (currentEvent == null)
        {
            Debug.LogError($"이벤트 ID {eventId}를 찾을 수 없습니다!");
            Debug.Log("현재 로드된 이벤트 목록:");
            foreach (var evt in eventList.events)
            {
                Debug.Log($"- {evt.eventId}");
            }
            return;
        }
        mainText.text = currentEvent.description;
        Debug.Log($"이벤트 로드: {currentEvent.description}");
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentEvent.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = currentEvent.options[i].text;
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(currentEvent.options[index]));
                Debug.Log($"버튼 {i} 설정: {currentEvent.options[i].text}");
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }
    void OnOptionSelected(OptionData option)
    {
        Debug.Log($"OnOptionSelected 호출: {option.text}");
        if (string.IsNullOrEmpty(option.requiredStat))
        {
            Debug.Log($"단순 이동: {option.successEvent}");
            LoadEvent(option.successEvent);
        }
        else
        {
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats가 null입니다!");
                return;
            }
            int successChance = option.successChance + (playerStats.GetStatValue(option.requiredStat) * 5);
            string nextEventId = (Random.Range(0, 100) < successChance) ? option.successEvent : option.failEvent;
            Debug.Log($"확률 계산: {successChance}%, 다음 이벤트: {nextEventId}");
            LoadEvent(nextEventId);
        }
    }
}