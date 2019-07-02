using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


namespace Game
{
    namespace Tool
    {
#if UNITY_EDITOR

        public class ToolDijkstraUI : EditorWindow
        {
            //マウス楮遺体
            public struct Mouse
            {
                public Vector2 startPos; //スタート位置
                public Vector2 endPos;   //エンド位置
                public Vector2 worldPos; //座標
            }
            protected Mouse mouse_;

            //ダイクストラのデーブデータ
            protected Save.DijkstraSave dijkstraDate_;

            //ダイクストラを使って移動するオブジェクト
            private GameObject object_;

            //リンク
            [SerializeField]
            protected Link[] link_;
            //ルート
            [SerializeField]
            protected List<PostRoot> postRoot_;

            protected static bool init_;

            [MenuItem("Tools/DijkstraUI %d")]
            static void Init()
            {
                init_ = false;
                EditorWindow.GetWindow<ToolDijkstraUI>(true, "DijkstraUI");
            }

            protected void OnGUI()
            {
                dijkstraDate_ = EditorGUILayout.ObjectField("LOAD", dijkstraDate_, typeof(Save.DijkstraSave), true) as Save.DijkstraSave;
                object_ = EditorGUILayout.ObjectField("OBJECT_LOAD", object_, typeof(GameObject), true) as GameObject;
                if (dijkstraDate_ == null)
                {
                    BeginWindows();
                    EndWindows();
                    init_ = false;
                    return;
                }
                else
                {
                    if(init_ == false)
                    {
                        InitDIjkstra();
                    }
                    else if(link_ != null)
                    {
                        InitDIjkstra();
                    }
                    init_ = true;
                    BeginWindows();
                    MouseUpdate();
                    Vector2 move = MouseDragLength();

                    Repaint();

                    foreach (var obj in link_)
                    {
                        if (obj != null)
                        {
                            obj.ToolUpdate(move);
                            obj.ToolDraw();
                        }

                    }
                    if (object_ != null)
                    {
                        object_.GetComponent<Character.BaseCharacter>().ToolUpdate(move);
                        object_.GetComponent<Character.BaseCharacter>().ToolDraw();
                    }
                    if (Event.current.type == EventType.MouseDrag)
                    {
                        mouse_.startPos = mouse_.endPos;
                    }
                    EndWindows();
                }
            }

            private void InitDIjkstra()
            {
                link_ = GameObject.FindObjectsOfType<Link>();

                Array.Sort(link_, (a, b) => a.LinkID - b.LinkID);
                //配列を確保
                postRoot_ = new List<Game.PostRoot>(dijkstraDate_.LinkSave.Count);


                for (int cntObj = 0; cntObj < link_.Length; cntObj++)
                {
                    postRoot_.Add(new PostRoot());
                    postRoot_[cntObj].StartRootObject = link_[cntObj].gameObject;
                    postRoot_[cntObj].RootGoalObject = new List<PostGoal>(link_.Length);
                    for (int cnt = 0; cnt < link_.Length; cnt++)
                    {
                        postRoot_[cntObj].RootGoalObject.Add(new PostGoal());
                        postRoot_[cntObj].RootGoalObject[cnt].GoalRootObject = new List<GameObject>(dijkstraDate_.LinkSave[cntObj].RootGoalObject[cntObj].GoalRootObject.Count);
                        foreach (var indexObj in dijkstraDate_.LinkSave[cntObj].RootGoalObject[cnt].GoalRootObject)
                        {
                            postRoot_[cntObj].RootGoalObject[cnt].GoalRootObject.Add(link_[indexObj - 1].gameObject);
                        }
                    }
                }
            }



            //マウスの更新処理
            protected void MouseUpdate()
            {
                mouse_.worldPos = Event.current.mousePosition;
                //マウスが動いている状態
                if (Event.current.type == EventType.MouseDown)
                {
                    
                    mouse_.endPos = mouse_.startPos = mouse_.worldPos;
                }
                //マウスがドラッグした状態
                else if (Event.current.type == EventType.MouseDrag)
                {
                    mouse_.endPos = Event.current.mousePosition;
                }
            }

            //ドラッグ中の移動距離
            protected Vector2 MouseDragLength()
            {
                return mouse_.endPos - mouse_.startPos;
            }
        }

#endif
    }
}
