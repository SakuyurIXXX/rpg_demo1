using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.PackageManager.UI;

// ��Unity�༭������ʱִ�е��Զ�����Ͳ���ģʽ�����
[InitializeOnLoad]
public class AutoSaveAndPlayMode : EditorWindow
{
    private static float saveTime = 120; // Ĭ�ϵ��Զ�����ʱ��������λΪ��
    private static float nextSave = 0; // ��һ���Զ������ʱ���
    private string userInput = "120"; // �û������ʱ������Ĭ��Ϊ 300 ��

    // ��̬���캯�������౻����ʱע�Ქ��ģʽ״̬�ı��¼�
    static AutoSaveAndPlayMode()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    // �˵�����˲�� -> �Զ�����
    [MenuItem("���˲��/�Զ�����")]
    static void Init()
    {

        // ���� AutoSaveAndPlayMode ����ʵ��
        AutoSaveAndPlayMode window = (AutoSaveAndPlayMode)EditorWindow.GetWindowWithRect(typeof(AutoSaveAndPlayMode), new Rect(0, 0, 300, 80));
        window.Show();
    }

    // �ڴ��ڽ��л���
    void OnGUI()
    {
        // ��EditorPrefs�ж�ȡ�����ֵ�����û����ʹ��Ĭ��ֵ
        saveTime = EditorPrefs.GetFloat("AutoSave_saveTime", 300);

        // ����һ�α����ʱ���Ϊ 0�����ʼ��Ϊ��ǰʱ������Զ�����ʱ����
        if (nextSave == 0)
        {
            nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
        }
        // ��ʾ�Զ�����ʱ��������һ�α���ʱ��
        float timeToSave = nextSave - (float)EditorApplication.timeSinceStartup;
        EditorGUILayout.LabelField("��һ�α���:", timeToSave.ToString() + " ��");

        // �û�������뵥λ
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("�Զ�������:");
        userInput = EditorGUILayout.TextField(userInput, GUILayout.Width(50));
        GUILayout.Label("��", GUILayout.Width(30));
        GUILayout.EndHorizontal();

        // ���û����»س���ʱִ�б���
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return)
        {
            if (float.TryParse(userInput, out float newSaveTime))
            {
                saveTime = newSaveTime;
                nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
                // �����û������ֵ��EditorPrefs
                EditorPrefs.SetFloat("AutoSave_saveTime", saveTime);
                SaveScene();
            }
            else
            {
                Debug.LogError("��Ч��ʱ��������������Ч�����֡�");
            }
        }
        else
        {
            SaveScene();
        }

        // ʵʱ���´���
        Repaint();
    }

    // �ֶ����浱ǰ�����ķ���
    void SaveScene()
    {
        // �������ڲ���ģʽ���ҵ�ǰʱ�䳬����һ�α����ʱ���
        if (!EditorApplication.isPlaying && EditorApplication.timeSinceStartup > nextSave)
        {
            // ֱ�ӱ��浱ǰ���д򿪵ĳ���
            bool saveOK = EditorSceneManager.SaveOpenScenes();
            // �ڿ���̨���������ɹ���ʧ�ܵ���Ϣ
            Debug.Log("���泡�� " + (saveOK ? "�ɹ�" : "ʧ�ܣ�"));
            // ������һ�α����ʱ���
            nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
        }
    }

    // ����ģʽ״̬�ı�ʱ�Ļص�����
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // �ڼ����л�������ģʽʱִ�еĴ���
            bool shouldSaveScene = EditorUtility.DisplayDialog("���� ����", "�Ƿ�Ҫ�ڽ��벥��ģʽ֮ǰ���泡��?", "��", "��");

            if (shouldSaveScene)
            {
                EditorSceneManager.SaveOpenScenes();
            }
        }
    }
}