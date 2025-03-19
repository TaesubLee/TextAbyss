using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TMP_Text mainText;
    public Button[] optionButtons; // 최대 4개 버튼
    public TextAsset jsonFile;
    private EventList eventList;
    private PlayerStats playerStats;

    [System.Serializable]
    public class EventData {
        public string eventId;
        public string description;
        public OptionData[] options;
    }

    [System.Serializable]
    public class OptionData {
        public string text;
        public string requiredStat;
        public int successChance;
        public string successEvent;
        public string failEvent;
    }

    [System.Serializable]
    public class EventList {
        public EventData[] events;
    }

    void Start() {
        playerStats = FindObjectOfType<PlayerStats>();
        eventList = JsonUtility.FromJson<EventList>(jsonFile.text);
        LoadEvent("start"); // 시작 이벤트 로드
    }

    public void LoadEvent(string eventId) {
        EventData currentEvent = System.Array.Find(eventList.events, e => e.eventId == eventId);
        mainText.text = currentEvent.description;
        for (int i = 0; i < optionButtons.Length; i++) {
            if (i < currentEvent.options.Length) {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = currentEvent.options[i].text;
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(currentEvent.options[index]));
            } else {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnOptionSelected(OptionData option) {
        if (string.IsNullOrEmpty(option.requiredStat)) {
            LoadEvent(option.successEvent); // 단순 이동 이벤트
        } else {
            int successChance = option.successChance + (playerStats.GetStatValue(option.requiredStat) * 5);
            string nextEventId = (Random.Range(0, 100) < successChance) ? option.successEvent : option.failEvent;
            LoadEvent(nextEventId);
        }
    }
}
