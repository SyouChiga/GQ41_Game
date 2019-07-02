using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Link))]
    public class LinkInspector : Editor
    {
        //折り畳み  
        bool folding = false;
        //ID
        int ID = 0;
        public override void OnInspectorGUI()
        {
            Link link = target as Link;

            GameObject[] linkObj = link.LinkObject;

            EditorGUILayout.LabelField("ID");
            link.LinkID = EditorGUILayout.IntField(link.LinkID, GUILayout.Width(48));
            EditorGUILayout.LabelField("接続ID");
            link.linkIDNext = EditorGUILayout.IntField(link.linkIDNext, GUILayout.Width(48));
            if (GUILayout.Button("接続追加"))
            {
                GameObject[] saveObj =  new GameObject[linkObj.Length + 1];
                for(int cnt = 0; cnt < linkObj.Length; cnt++)
                {
                    saveObj[cnt] = linkObj[cnt];
                }
                GameObject[] post = GameObject.FindGameObjectsWithTag("post_tag");
                foreach(var obj in post)
                {
                    if(link.linkIDNext == obj.GetComponent<Link>().LinkID)
                    {
                        saveObj[linkObj.Length] = obj;
                    }
                }
                if (saveObj[linkObj.Length] != null)
                {
                    linkObj = saveObj;
                }
               
            }

            if (folding = EditorGUILayout.Foldout(folding, "接続先"))
            {
                // リスト表示
                for (int i = 0; i < linkObj.Length; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    linkObj[i] = EditorGUILayout.ObjectField(linkObj[i], typeof(GameObject), true, GUILayout.Width(100)) as GameObject;
                    if (GUILayout.Button("接続解除"))
                    {
                        GameObject[] saveObj = new GameObject[linkObj.Length - 1];
                        int index = 0;
                        int indexNext = 0;
                        for (int cnt = 0; cnt < linkObj.Length; cnt++)
                        {
                            if(i == cnt)
                            {
                                //接続先の方も消す
                                Link nextLink = linkObj[i].GetComponent<Link>();
                                GameObject[] saveNextObj = new GameObject[nextLink.LinkObject.Length - 1];
                                GameObject[] nextObj = nextLink.LinkObject;
                                for (int cntNext = 0; cntNext < nextLink.LinkObject.Length; cntNext++)
                                {
                                    if (link.gameObject == nextObj[cntNext])
                                    {
                                        continue;
                                    }
                                    saveNextObj[indexNext] = nextObj[cntNext];
                                    indexNext++;
                                }
                                linkObj[i].GetComponent<Link>().LinkObject = saveNextObj;
                                continue;
                            }
                            saveObj[index] = linkObj[cnt];
                            index++;
                        }
                        linkObj = saveObj;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("障害物かどうか");
            link.Obstacle = EditorGUILayout.Toggle(link.Obstacle, GUILayout.Width(48));
            EditorGUILayout.EndHorizontal();
            if (link.Obstacle)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("障害物発生か");
                link.UseObstacle = EditorGUILayout.Toggle(link.UseObstacle, GUILayout.Width(48));
                EditorGUILayout.EndHorizontal();
            }
            link.LinkObject = linkObj;
        }
    }
#endif

}