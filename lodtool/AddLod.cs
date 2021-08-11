using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddLod : EditorWindow
{
    float loddistance;
    Vector2 _scrollPos;

    public GameObject myPrefab;
    public GameObject prefabGenNode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Tools/LodTool")]
    static void LodTool()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(AddLod));
        window.Show();
    }

    void OnGUI()
    {
        float originalValue = EditorGUIUtility.labelWidth;

        EditorGUILayout.Space(20);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("********* LOD���ù��� *********");

        loddistance = EditorGUILayout.FloatField("LOD ֵ��", 0.01f);
        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("��������ģ��")) 
        {
            Debug.Log("Add Lod All");
            AddStart();
            Debug.Log("Done!");
        }

        if (GUILayout.Button("����ѡ��ģ��"))
        {
            Debug.Log("Add Lod One");
            AddOneLodStart();
            Debug.Log("Done!");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(40);

        EditorGUILayout.LabelField("********* Ԥ�����滻���� *********");

        myPrefab = (GameObject)EditorGUILayout.ObjectField("Ԥ���壺",myPrefab, typeof(GameObject), false);
        prefabGenNode = (GameObject)EditorGUILayout.ObjectField("���ɵ���", prefabGenNode, typeof(GameObject), true);
        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("����ȫ��"))
        {
            //AddOneLodStart();
            spawnAll();
        }

        if (GUILayout.Button("����ѡ��"))
        {
            spawnGameObject((GameObject)Selection.activeObject);
        }

        if (GUILayout.Button("ȫ�����"))
        {
            spawnClear();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    void AddStart()
    {
        Transform tParent = ((GameObject)Selection.activeObject).transform;
        AddMyLod(tParent);
    }

    void AddMyLod(Transform tParent)
    {
        

        //Object tempPrefab;
        //int i = 0;
        foreach (Transform t in tParent)
        {
            //tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Prefab/prefab" + i + ".prefab");
            //tempPrefab = PrefabUtility.ReplacePrefab(t.gameObject, tempPrefab);
            Debug.Log(t.name);
            AddMyLod(t);
            AddOneLod(t);
            //i++;
        }
    }

    void AddOneLodStart()
    {
        Transform tParent = ((GameObject)Selection.activeObject).transform;
        AddOneLod(tParent);
    }

    void AddOneLod(Transform t)
    {
        Renderer r = t.GetComponent<Renderer>();
        if (r == null)
            return;

        //Debug.Log("Add One" + tParent.name);
        LODGroup lg = t.GetComponent<LODGroup>();
        if(lg != null)
        {
            //lg = t.gameObject.AddComponent<LODGroup>();
            DestroyImmediate(lg);
        }

        lg = t.gameObject.AddComponent<LODGroup>();

        LOD[] lods = new LOD[1];
        Renderer[] rs = new Renderer[1];
        rs[0] = r;
        lods[0] = new LOD(loddistance, rs);
        lg.SetLODs(lods);
    }

    void spawnAll()
    {
        Transform tParent = ((GameObject)Selection.activeObject).transform;

        foreach (Transform t in tParent)
        {
            Debug.Log(t.name);
            spawnGameObject(t.gameObject);
        }
    }

    void spawnGameObject(GameObject A)
    {
        GameObject go = Instantiate(myPrefab);
        go.transform.SetParent(prefabGenNode.transform);
        go.transform.localPosition = A.transform.localPosition;
        go.transform.localRotation = A.transform.localRotation;
        go.transform.localScale = A.transform.localScale;
        go.name = A.name;
    }

    void spawnClear()
    {
        /*foreach (Transform t in prefabGenNode.transform)
        {
            Debug.Log("ɾ�� :" + t.name);
            //spawnGameObject(t.gameObject);
            DestroyImmediate(t.gameObject);
        }*/

        int count = prefabGenNode.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(prefabGenNode.transform.GetChild(0).gameObject);
        }

    }

}
