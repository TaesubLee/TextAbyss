using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TMP_Text mainText;
    public TextAsset jsonFile; // 인스펙터에서 JSON 파일 연결


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
        public string nextEvent;
    }

    [System.Serializable]
    public class EventList
    {
        public EventData[] events;
    }

    void Start()
    {
        EventList eventList = JsonUtility.FromJson<EventList>(jsonFile.text);
        LoadEvent(eventList.events[0]);
    }

    void LoadEvent(EventData eventData)
    {
        mainText.text = eventData.description;
        // 옵션은 다음 단계에서 구현
    }
}
