using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class WindowContent
{
    public int tab = 0;
    public string cs_path = "Scripts/Event";
    //
    public string eventName = "NewEventEve";
}

public partial class EventManagerWindow : EditorWindow
{
    private WindowContent _content;

    [MenuItem("Window/Lowy/EventManage %e")]
    public static void CreateView()
    {
        //创建窗口
        EventManagerWindow window = GetWindow();
        window.Show();
    }

    public static EventManagerWindow GetWindow()
    {
        Rect wr = new Rect(0, 0, 500, 550);
        return (EventManagerWindow) EditorWindow.GetWindowWithRect(typeof(EventManagerWindow), wr, false,
            "Event Manage Window");
    }

    private void OnEnable()
    {
        _content = GetContent();
    }

    private void OnDisable()
    {
        SaveContent(_content);
    }

    private void OnGUI()
    {
        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        {
            _content.tab = GUILayout.Toggle(_content.tab == 0, "首选项", EditorStyles.toolbarButton) ? 0 : _content.tab;
            _content.tab = GUILayout.Toggle(_content.tab == 1, "管理Event", EditorStyles.toolbarButton) ? 1 : _content.tab;
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        switch (_content.tab)
        {
            case 0:
                SettingGUI();
                break;
            case 1:
                ManagerEventGUI();
                break;
        }
    }

    private void SettingGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        EditorGUILayout.TextField("File Path：", _content.cs_path);
        GUI.enabled = true;
        if (GUILayout.Button("Select Event File Path", GUILayout.MaxWidth(150)))
        {
            string path = SelectPath();
            if (!string.IsNullOrEmpty(path) && path.Contains(Application.dataPath + "/"))
            {
                _content.cs_path = path.Replace(Application.dataPath + "/", "");
            }
            else if(path != null && path.Contains(Application.dataPath + "/"))
            {
                EditorUtility.DisplayDialog("EventManager",
                    $"'{path}' Path invalid, path need has '{Application.dataPath}/'", "ok");
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ManagerEventGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            DrawEventList();
        }
        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("请尽量使用 <Eve> 作为事件后缀");
            _content.eventName = EditorGUILayout.TextField("Event Name:", _content.eventName);
            if (GUILayout.Button("Add Event", GUILayout.Height(30)))
            {
                SaveContent(_content);
                if (CanCreate(_content))
                {
                    CreateEventFile(_content);
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "Event 已存在", "ok");
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private bool CanCreate(WindowContent content)
    {
        return !File.Exists($"{Application.dataPath}/{content.cs_path}/{content.eventName}.cs");
    }

    private string SelectPath()
    {
        return EditorUtility.SaveFolderPanel("Select path", Application.dataPath, "Assets");
    }

    private WindowContent GetContent()
    {
        if (PlayerPrefs.HasKey("EVENT_MANAGER_WINDOW_CONTENT_KEY"))
            return JsonUtility.FromJson<WindowContent>(PlayerPrefs.GetString("EVENT_MANAGER_WINDOW_CONTENT_KEY"));
        return new WindowContent();
    }

    private void SaveContent(WindowContent content)
    {
        PlayerPrefs.SetString("EVENT_MANAGER_WINDOW_CONTENT_KEY", JsonUtility.ToJson(content));
    }

    private void CreateEventFile(WindowContent content)
    {
        string path = $"{Application.dataPath}/{content.cs_path}";
        CreatePath(path);
        string text = $"public class {_content.eventName} : Lowy.Event.IEvent {{ }}";

        //
        File.WriteAllText($"{path}/{content.eventName}.cs",
            text);
    }

    private void CreatePath(string s)
    {
        if (!Directory.Exists(s))
            Directory.CreateDirectory(s);
    }
}