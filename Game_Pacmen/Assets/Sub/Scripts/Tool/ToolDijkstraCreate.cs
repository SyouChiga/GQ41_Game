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
        public class ToolDijkstraCreate : EditorWindow
        {
            //リンク
            [SerializeField]
            static Link[] link_;
            //ルート
            [SerializeField]
            static List<PostRoot> postRoot_;
            //ダイクストラのセーブ
            Save.DijkstraSave dijkstraSave_;
            //障害物の数
            static int obstacleCnt = 0;
            //障害物の数2
            int obstacleIDNum = 0;
            static GameObject[] obstacleObjs_;
            [MenuItem("Tools/DijkstraCreate %d")]
            static void Init()
            {
                EditorWindow.GetWindow<ToolDijkstraCreate>(true, "DijkstraCreate");
            }
            void OnGUI()
            {
                dijkstraSave_ = EditorGUILayout.ObjectField("SAVE", dijkstraSave_, typeof(Save.DijkstraSave), true) as Save.DijkstraSave;
                if (GUILayout.Button("Create"))
                {
                    CreateInit();
                }
                if(dijkstraSave_ != null) EditorUtility.SetDirty(dijkstraSave_);
            }

            void CreateInit()
            {
                link_ = GameObject.FindObjectsOfType<Link>();

                Array.Sort(link_, (a, b) => a.LinkID - b.LinkID);
                int id = 0;

                obstacleIDNum = 0;

                int one = 1;
                foreach (var link in link_)
                {
                    if(link.Obstacle == true)
                    {
                        link.ObstacleID = id;
                        id++;
                        obstacleIDNum += one;
                        
                        
                    }
                }
                obstacleObjs_ = new GameObject[id];
                id = 0;
                foreach (var link in link_)
                {
                    if (link.Obstacle == true)
                    {
                       
                        obstacleObjs_[id] = link.gameObject;
                        id++;
                    }
                }
                Dijkstra();
                DijkstraSave();
            }

            //ダイクストラ
            void Dijkstra()
            {
                PostRootInit();
            }

            //ポストルート初期化
            void PostRootInit()
            {
                postRoot_ = new List<PostRoot>(link_.Length);
                int obsta = 0;
                //ポストルートのサイズを、Link変数分用意する
                foreach (var save in link_)
                {
                    postRoot_.Add(new PostRoot());
                    if(save.Obstacle == true)
                    {
                        obsta++;
                    }
                }
                obstacleCnt = obsta;
                int cnt = 0;

                foreach (var postRootObj in postRoot_)
                {
                    postRootObj.StartRootObject = link_[cnt].gameObject;
                    postRootObj.RootGoalObject = new List<PostGoal>();
                    for (int nCnt = 0; nCnt < link_.Length; nCnt++)
                    {
                        postRootObj.RootGoalObject.Add(new PostGoal());
                        postRootObj.RootGoalObject[nCnt].GoalRootObject = new List<GameObject>();
                    }
                    cnt++;
                }
                PostRootRetrieval(obsta);
            }
            //ポストルートの検索
            void PostRootRetrieval(int obstacleCnt)
            {
                
                foreach (var post in postRoot_)
                {
                    int cnt = 0;
                    foreach (var posGoal in postRoot_)
                    {


                        float length_, lenght1_;
                        bool triger = false;
                        length_ = lenght1_ = 0.0f;
                        List<GameObject> goalSaveRootObj_ = new List<GameObject>();
                        goalSaveRootObj_.Add(post.StartRootObject);
                        post.RootGoalObject[cnt].PostGoalSubObject = new List<PostGoalSub>(obstacleCnt + obstacleCnt - 1);
                       
                      // post.RootGoalObject[cnt].PostGoalSubObject[0].GoalSubObject.Add(post.StartRootObject);
                        post.RootGoalObject[cnt].GoalRootObject.Add(post.StartRootObject);
                        bool one = false;
                        //同じなら処理しない
                        if (post == posGoal)
                        {
                            cnt++;
                            continue;
                        }
                        //隣にあるかどうか調べる
                        foreach (var linkObj in post.StartRootObject.GetComponent<Link>().LinkObject)
                        {
                            if (linkObj == posGoal.StartRootObject)
                            {
                                one = true;
                                post.RootGoalObject[cnt].GoalRootObject.Add(linkObj);
                                break;
                            }
                        }

                        if (one == false)
                        {
                            PostRootGoalReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject, post.RootGoalObject[cnt].GoalRootObject, goalSaveRootObj_, ref length_, ref lenght1_, ref triger);
                            
                            bool useObstacke = false;
                            foreach(var obstacleObjes in obstacleObjs_ )
                            {
                                if(posGoal.StartRootObject == obstacleObjes)
                                {
                                    useObstacke = true;
                                }
                            }

                            if (useObstacke == true)
                            {
                                cnt++;
                                continue;
                            }
                            int index = 0;
                            int cntObs = 0;
                            int cntObsNext = obstacleCnt - 1;
                            int size = 2;

                            for(int cntObsIndex = 0; cntObsIndex < obstacleObjs_.Length; cntObsIndex++)
                            {
                                post.RootGoalObject[cnt].PostGoalUse = true;
                                post.RootGoalObject[cnt].PostGoalSubObject.Add(new PostGoalSub());
                                post.RootGoalObject[cnt].PostGoalSubObject[cntObsIndex].GoalSubObject.Add(new PostGoalSubRoot());

                                GameObject[] objGoal = new GameObject[1];
                                objGoal[0] = obstacleObjs_[cntObsIndex];
                                PostRootFGoalObstacleReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject, objGoal, post.RootGoalObject[cnt].PostGoalSubObject[cntObsIndex].GoalSubObject[0].GoalSubRootObject, goalSaveRootObj_, ref length_, ref lenght1_, ref triger);


                                length_ = 0.0f;
                                lenght1_ = 0.0f;
                                triger = false;
                                size = 2;
                                for(int cntObstacle = 0; cntObstacle < obstacleObjs_.Length - (cntObsIndex + 1); cntObstacle++)
                                {
                                    int sizeObjeObject = size;
                                    
                                    GameObject[] objGoals = new GameObject[sizeObjeObject];
                                    size++;
                                    objGoals[0] = obstacleObjs_[cntObsIndex];
                                    int cntObstacleObjs = 0;
                                    post.RootGoalObject[cnt].PostGoalSubObject[cntObsIndex].GoalSubObject.Add(new PostGoalSubRoot());

                                    int indexObstacle = cntObsIndex;
                                    foreach (var obj in objGoals)
                                    {
                                        if (cntObstacleObjs == 0)
                                        {
                                            cntObstacleObjs += 1;
                                            continue;
                                        }

                                        indexObstacle += 1;
                                        objGoals[cntObstacleObjs] = obstacleObjs_[indexObstacle];
                                       
                                        cntObstacleObjs += 1;
                                    }
                                    PostRootFGoalObstacleReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject, objGoals, post.RootGoalObject[cnt].PostGoalSubObject[cntObsIndex].GoalSubObject[cntObstacle + 1].GoalSubRootObject, goalSaveRootObj_, ref length_, ref lenght1_, ref triger);
                                    length_ = 0.0f;
                                    lenght1_ = 0.0f;
                                    triger = false;
                                }
                                size = 0;
                            }

                        }
                        
                        cnt++;

                    }
                }
               
               
            }
            //再帰　繋がっている先があるかどうか
            //trueなら次があり、falseは次がない
            bool PostRootGoalReturn(Link rLink, GameObject goal, List<GameObject> postRootGoal, List<GameObject> postRootNewGoal, ref float length, ref float newLength, ref bool triger)
            {
                bool safe = true;
                int index = 0;
                foreach (var linkObj in rLink.LinkObject)
                {
                    safe = true;
                    int cnt = 0;

                    Link link = linkObj.GetComponent<Link>();
                    foreach (var returnObj in postRootNewGoal)
                    {
                        if (linkObj == returnObj)
                        {
                            if (linkObj == postRootNewGoal[0])
                            {
                                safe = false;
                            }
                            else
                            {
                                safe = false;
                                break;
                            }
                        }
                        cnt++;
                    }
                    if (goal == linkObj)
                    {
                        if (index >= 1)
                        {
                            if (postRootNewGoal[postRootNewGoal.Count - 1] == rLink.LinkObject[index - 1])
                            {
                                if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                        postRootNewGoal.Add(linkObj);
                        return true;
                    }
                    if (safe == true)
                    {
                        postRootNewGoal.Add(linkObj);

                        if (PostRootGoalReturn(linkObj.GetComponent<Link>(), goal, postRootGoal, postRootNewGoal, ref length, ref newLength, ref triger))
                        {

                            for (int cntObj = postRootNewGoal.Count - 1; cntObj > 0; cntObj--)
                            {
                                newLength += (postRootNewGoal[cntObj - 1].transform.position - postRootNewGoal[cntObj].transform.position).magnitude;
                            }

                            //前回の経路探索を見比べる
                            if (newLength <= length || triger == false)
                            {
                                triger = true;
                                postRootGoal.Clear();
                                foreach (var saveObj in postRootNewGoal)
                                {
                                    postRootGoal.Add(saveObj);
                                }
                                length = newLength;


                            }
                            newLength = 0.0f;
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                        }
                        else
                        {

                            if (postRootNewGoal.Count - 1 != 0 && rLink.gameObject != postRootNewGoal[postRootNewGoal.Count - 1])
                            {
                                postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                    }
                    index++;


                }
                if (postRootNewGoal.Count - 1 != 0)
                {
                    postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                }

                return false;
            }

            bool PostRootFGoalObstacleReturn(Link rLink, GameObject goal, GameObject[] obstacleGame,List<GameObject> postRootGoal, List<GameObject> postRootNewGoal, ref float length, ref float newLength, ref bool triger)
            {
                bool safe = true;
                int index = 0;
                bool obstacleUse = false;
                foreach (var linkObj in rLink.LinkObject)
                {
                    safe = true;
                    int cnt = 0;

                    Link link = linkObj.GetComponent<Link>();
                    GameObject obstacleObj = null;
                    foreach (var returnObj in postRootNewGoal)
                    {
                        if (linkObj == returnObj)
                        {
                            if (linkObj == postRootNewGoal[0])
                            {
                                safe = false;
                            }
                            else
                            {
                                safe = false;
                                break;
                            }
                        }
                        foreach (var obstacle in obstacleGame)
                        {
                            if (obstacle == linkObj)
                            {
                                obstacleObj = obstacle;
                                safe = false;
                                obstacleUse = true;
                                break;

                            }
                        }
                        if(obstacleUse)
                        {
                            break;
                        }
                        cnt++;
                    }

                    bool a = false;
                    if (obstacleObj != null)
                    {
                        if (obstacleObj.name == goal.name) a = true;
                    }
                    if (a == true)
                    {
                        a = false;
                    }
                     
                    else if (goal == linkObj)
                    {
                        if (index >= 1)
                        {
                            if (postRootNewGoal[postRootNewGoal.Count - 1] == rLink.LinkObject[index - 1])
                            {
                                if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                         postRootNewGoal.Add(linkObj);
                         return true;
                    }
                    if (safe == true)
                    {
                        postRootNewGoal.Add(linkObj);

                        if (PostRootFGoalObstacleReturn(linkObj.GetComponent<Link>(), goal, obstacleGame,postRootGoal, postRootNewGoal, ref length, ref newLength, ref triger))
                        {

                            for (int cntObj = postRootNewGoal.Count - 1; cntObj > 0; cntObj--)
                            {
                                newLength += (postRootNewGoal[cntObj - 1].transform.position - postRootNewGoal[cntObj].transform.position).magnitude;
                            }

                            //前回の経路探索を見比べる
                            if (newLength <= length || triger == false)
                            {
                                triger = true;
                                postRootGoal.Clear();
                                foreach (var saveObj in postRootNewGoal)
                                {
                                    postRootGoal.Add(saveObj);
                                }
                                length = newLength;


                            }
                            newLength = 0.0f;
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                        }
                        else
                        {

                            if (postRootNewGoal.Count - 1 != 0 && rLink.gameObject != postRootNewGoal[postRootNewGoal.Count - 1])
                            {
                                postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                    }
                    index++;


                }
                if (postRootNewGoal.Count - 1 != 0)
                {
                    postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                }
                return false;
            }

            //ダイクストラセーブ
            void DijkstraSave()
            {
                List<Save.PostRootINT> postRootSave_;
                postRootSave_ = dijkstraSave_.LinkSave;

                postRootSave_.Clear();
                bool tr = false;
                //リンクIDを入れる
                for (int cnt = 0; cnt < postRoot_.Count; cnt++)
                {
                    postRootSave_.Add(new Save.PostRootINT());
                    postRootSave_[cnt].StartRootObject = postRoot_[cnt].StartRootObject.GetComponent<Link>().LinkID;
                    postRootSave_[cnt].RootGoalObject = new List<Save.PostGoalINT>(postRoot_.Count);
                    postRootSave_[cnt].RootGoalObject = new List<Save.PostGoalINT>(1);



                    for (int cntIndex = 0; cntIndex < postRoot_.Count; cntIndex++)
                    {
                        postRootSave_[cnt].RootGoalObject.Add(new Save.PostGoalINT());
                        postRootSave_[cnt].RootGoalObject[cntIndex] = new Save.PostGoalINT();
                        postRootSave_[cnt].RootGoalObject[cntIndex].GoalRootObject = new List<int>();
                        postRootSave_[cnt].RootGoalObject[cntIndex].PostGoalSubObject = new List<Save.PostGoalSubINT>();

                    }
                    int index = 0;
                    foreach (var linkObj in postRoot_[cnt].RootGoalObject)
                    {
                        foreach (var goalObj in linkObj.GoalRootObject)
                        {
                            postRootSave_[cnt].RootGoalObject[index].GoalRootObject.Add(goalObj.GetComponent<Link>().LinkID);

                            
                            


                        }
                       
                        for (int cntObs = 0; cntObs < obstacleIDNum; cntObs++)
                        {
                            bool s;
                            s = postRoot_[cnt].RootGoalObject[index].PostGoalUse;
                            postRootSave_[cnt].RootGoalObject[index].PostGoalUse = s;

                            if (s == true)
                            {
                                postRootSave_[cnt].RootGoalObject[index].PostGoalSubObject.Add(new Save.PostGoalSubINT());

                                int cntObstacleIndex = 0;
                                foreach (var obj in postRoot_[cnt].RootGoalObject[index].PostGoalSubObject[cntObs].GoalSubObject)
                                {
                                    postRootSave_[cnt].RootGoalObject[index].PostGoalSubObject[cntObs].GoalSubObject.Add(new Save.PostGoalSubRootINT());

                                    foreach (var objs in postRoot_[cnt].RootGoalObject[index].PostGoalSubObject[cntObs].GoalSubObject[cntObstacleIndex].GoalSubRootObject)
                                    {

                                        Link link = objs.GetComponent<Link>();
                                        postRootSave_[cnt].RootGoalObject[index].PostGoalSubObject[cntObs].GoalSubObject[cntObstacleIndex].GoalSubRootObject.Add(link.LinkID);
                                    }
                                    cntObstacleIndex += 1;
                                }

                            }
                        }
                        
                        index++;
                    }

                }


            }



        }
#endif

    }
}

