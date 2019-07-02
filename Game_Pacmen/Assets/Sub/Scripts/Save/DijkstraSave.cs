using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Game
{
    namespace Save
    {

        //ルート
        [Serializable]
        public class PostRootINT
        {
            [SerializeField]
            //始まり
            private int rootObj_;
            public int StartRootObject
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
            private List<PostGoalINT> rootGoalObj_;
            public List<PostGoalINT> RootGoalObject
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
        public class PostGoalINT
        {
            [SerializeField]
            //終わりまでのルート
            private List<int> goalRootObj_ = new List<int>();
            public List<int> GoalRootObject
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
            private List<PostGoalSubINT> postGoalSubObj_ = new List<PostGoalSubINT>();
            public List<PostGoalSubINT> PostGoalSubObject
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
        public class PostGoalSubINT
        {
            [SerializeField]
            private List<PostGoalSubRootINT> goalSubObj_ = new List<PostGoalSubRootINT>();
            public List<PostGoalSubRootINT> GoalSubObject
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
        public class PostGoalSubRootINT
        {
            [SerializeField]
            private List<int> goalSubRootObj_ = new List<int>();
            public List<int> GoalSubRootObject
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

        //================================================
        //ダイクストラの保存
        //================================================

        [CreateAssetMenu(menuName = "MyGame/Create Dijkstra", fileName = "DijkstraTable")]
        public class DijkstraSave : ScriptableObject
        {


            //リンク
            [SerializeField]
            List<PostRootINT> postRootINT_ = new List<PostRootINT>();
            public List<PostRootINT> LinkSave
            {
                get
                {
                    return postRootINT_;
                }
                set
                {
                    postRootINT_ = value;
                }

            }
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }

        }

    }
}

