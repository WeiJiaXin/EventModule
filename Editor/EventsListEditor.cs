using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lowy.Event;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public partial class EventManagerWindow
{
    private ReorderableList eventList;
    private List<string> data;

    private void DrawEventList()
    {
        if (eventList == null)
            InitData();
        eventList.drawElementCallback = DrawEventsItem;
        eventList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Events");
        eventList.displayAdd = false;
        eventList.displayRemove = false;
        eventList.onSelectCallback=SelectEventItem;
        eventList.DoLayoutList();
    }

    private void SelectEventItem(ReorderableList list)
    {
        
    }

    private void InitData()
    {
        data = new List<string> {_content.cs_path,"asdasd"};
        eventList = new ReorderableList(data, typeof(string));
    }

    private void DrawEventsItem(Rect rect,int index,bool isActive,bool isFocused)
    {
        var element = data[index];
        rect.height -= 4;
        rect.width /= 5f;
        rect.width *= 3;
        rect.y += 2;
        EditorGUI.TextField(rect, element);
        rect.width /= 3f;
        rect.x += rect.width * 3;
        GUI.Button(rect, "Editor");
        rect.x += rect.width;
        GUI.Button(rect, "Remove");
        Type t = typeof(EventManager);
        var fs = t.Assembly.GetFiles();
    }
}