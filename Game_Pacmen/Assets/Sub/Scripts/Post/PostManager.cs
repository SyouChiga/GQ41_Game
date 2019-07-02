using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game
{
    //ルート
    [Serializable]
    public class PostRoot
    {
        [SerializeField]
        //始まり
        private GameObject rootObj_;
        public GameObject StartRootObject
        {
            set
            {
                rootObj_ = value;
            }
            get
            {
                return rootObj_;
            }
        }
        
        [SerializeField]
        private List<PostGoal> rootGoalObj_;
        public List<PostGoal> RootGoalObject
        {
            set
            {
                rootGoalObj_ = value;
            }
            get
            {
                return rootGoalObj_;
            }
        }



    }
    //ルートゴール
    [Serializable]
    public class PostGoal
    {
        [SerializeField]
        //終わりまでのルート
        private List<GameObject> goalRootObj_;
        public List<GameObject> GoalRootObject
        {
            set
            {
                goalRootObj_ = value;
            }
            get
            {
                return goalRootObj_;
            }
        }
        [SerializeField]
        private List<PostGoalSub> postGoalSubObj_ = new List<PostGoalSub>();
        public List<PostGoalSub> PostGoalSubObject
        {
            set
            {
                postGoalSubObj_ = value;
            }
            get
            {
                return postGoalSubObj_;
            }
        }
        [SerializeField]
        private bool use = false;
        public bool PostGoalUse
        {
            set
            {
                use = value;
            }
            get
            {
                return use;
            }
        }

    }
    [Serializable]
    public class PostGoalSub
    {
        [SerializeField]
        private List<PostGoalSubRoot> goalSubObj_ = new List<PostGoalSubRoot>();
        public List<PostGoalSubRoot> GoalSubObject
        {
            set
            {
                goalSubObj_ = value;
            }
            get
            {
                return goalSubObj_;
            }
        }


    }
    [Serializable]
    public class PostGoalSubRoot
    {
        [SerializeField]
        private List<GameObject> goalSubRootObj_ = new List<GameObject>();
        public List<GameObject> GoalSubRootObject
        {
            set
            {
                goalSubRootObj_ = value;
            }
            get
            {
                return goalSubRootObj_;
            }
        }

    }

    public class PostManager : MonoBehaviour
    {
        //リンク
        [SerializeField]
        Link[] link_;
        public Link[] Link
        {
            get
            {
                return link_;
            }
        }
        //ルート
        [SerializeField]
        List<PostRoot> postRoot_;
        public List<PostRoot> PostRoot
        {
            get
            {
                return postRoot_;
            }
        }

        //ダイクストラのデータ
        [SerializeField]
        private Game.Save.DijkstraSave dijkstraDate_;


        // Start is called before the first frame update
        void Awake()
        {
            link_ = GameObject.FindObjectsOfType<Link>();

            Array.Sort(link_, (a, b) => a.LinkID - b.LinkID);

            Dijkstra();
          
        }

        // Update is called once per frame
        void Update()
        {

        }

        //ダイクストラ
        void Dijkstra()
        {
            //配列を確保
            postRoot_ = new List<Game.PostRoot>(dijkstraDate_.LinkSave.Count);

            int obstacleIndex = 0;
            foreach(var obj in link_)
            {
                if(obj.Obstacle == true)
                {
                    obstacleIndex += 1;
                }
            }
 
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
                for (int cnt = 0; cnt < link_.Length; cnt++)
                {
                    if (dijkstraDate_.LinkSave[cntObj].RootGoalObject[cnt].PostGoalUse == true)
                    {

                        for (int obstacle = 0; obstacle < obstacleIndex; obstacle++)
                        {
                            postRoot_[cntObj].RootGoalObject[cnt].PostGoalSubObject.Add(new PostGoalSub());
                            int cntObstacleIndex = 0;
                            foreach (var obj in dijkstraDate_.LinkSave[cntObj].RootGoalObject[cnt].PostGoalSubObject[obstacle].GoalSubObject)
                            {
                                postRoot_[cntObj].RootGoalObject[cnt].PostGoalSubObject[obstacle].GoalSubObject.Add(new PostGoalSubRoot());
                                foreach (var objs in obj.GoalSubRootObject)
                                {
                                    postRoot_[cntObj].RootGoalObject[cnt].PostGoalSubObject[obstacle].GoalSubObject[cntObstacleIndex].GoalSubRootObject.Add(link_[objs - 1].gameObject);
                                }
                                cntObstacleIndex++;
                            }


                        }
                    }
                }
            }
            
          
        }

        //ポストルート初期化
        void PostRootInit()
        {
            //ポストルートのサイズを、Link変数分用意する
            foreach(var save in link_)
            {
                postRoot_.Add(new PostRoot());
            }
            int cnt = 0;
            foreach(var postRootObj in postRoot_)
            {
                postRootObj.StartRootObject = link_[cnt].gameObject;
                postRootObj.RootGoalObject = new List<PostGoal>();
                for (int nCnt = 0 ; nCnt < link_.Length; nCnt++)
                {
                    postRootObj.RootGoalObject.Add(new PostGoal());
                    postRootObj.RootGoalObject[nCnt].GoalRootObject = new List<GameObject>();
                }
                cnt++;
            }
            PostRootRetrieval();
        }
        //ポストルートの検索
        void PostRootRetrieval()
        {
            foreach(var post in postRoot_)
            {
                int cnt = 0;
                foreach(var posGoal in postRoot_)
                {
                   

                    float length_, lenght1_;
                    bool triger = false;
                    length_ = lenght1_ = 0.0f;
                    List<GameObject> goalSaveRootObj_ = new List<GameObject>();
                    goalSaveRootObj_.Add(post.StartRootObject);
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
                        if(linkObj == posGoal.StartRootObject)
                        {
                            one = true;
                            post.RootGoalObject[cnt].GoalRootObject.Add(linkObj);
                            break;
                        }
                    }

                    if(one == false)PostRootGoalReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject,post.RootGoalObject[cnt].GoalRootObject, goalSaveRootObj_, ref length_, ref lenght1_, ref triger);
                    cnt++; 

                }
            }
        }
        //再帰　繋がっている先があるかどうか
        //trueなら次があり、falseは次がない
        bool PostRootGoalReturn(Link rLink,GameObject goal,List<GameObject> postRootGoal, List<GameObject> postRootNewGoal,ref float length,ref float newLength,ref bool triger)
        {
            bool safe = true;
            int index = 0;
            foreach (var linkObj in rLink.LinkObject)
            {
                safe = true;
                int cnt = 0;
                foreach(var returnObj in postRootNewGoal)
                {
                    if(linkObj == returnObj)
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
                if(goal == linkObj)
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

        static public bool PostRootFGoalObstacleReturn(Link rLink, GameObject goal, GameObject[] obstacleGame, List<GameObject> postRootGoal, List<GameObject> postRootNewGoal, ref float length, ref float newLength, ref bool triger)
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
                    if (obstacleUse)
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

                    if (PostRootFGoalObstacleReturn(linkObj.GetComponent<Link>(), goal, obstacleGame, postRootGoal, postRootNewGoal, ref length, ref newLength, ref triger))
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



    }
}
