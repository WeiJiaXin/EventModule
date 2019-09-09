using System.Collections;
using System.Collections.Generic;
using Lowy.Event;
using UnityEngine;

public class Test : MonoBehaviour
{
    class EventData:IEvent
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<EventData>(CallBack1);
        EventManager.AddListener<EventData>(CallBack2);
        EventManager.Dispatch(new EventData());
    }

    private void CallBack2(EventData obj)
    {
        print("call this<button onclick=\"copyText()\">复制文本</button>");
    }

    private void CallBack1(EventData obj)
    {
        Call2();
    }

    private void Call2()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
