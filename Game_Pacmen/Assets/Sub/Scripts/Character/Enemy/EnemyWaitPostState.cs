using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWaitPostState : BaseEnemyState
            {
                //ゴールまでの道筋
                [SerializeField]
                List<GameObject> postGoal_;
                public List<GameObject> postGoal
                {
                    set
                    {
                        postGoal = value;
                    }
                }
                //最初のインデックス
                int indexStart_;
                public int IndexStart
                {
                    set
                    {
                        indexStart_ = value;
                    }
                }
                //ゴールするところ
                int indexGoal_;
                public int IndexGoal
                {
                    set
                    {
                        indexGoal_ = value;
                    }
                }

                // Start is called before the first frame update
                void Start()
                {
                  
                    GoalInit();
                }

                // Update is called once per frame
                void Update()
                {
                    Object.Destroy(GetComponent<BaseEnemy>().State);
                    EnemyWalkPostState walk = gameObject.AddComponent<EnemyWalkPostState>();
                    walk.postGoal = postGoal_;
                    walk.CntGoal = 0;
                    GetComponent<BaseEnemy>().State = walk;
                    GetComponent<BaseEnemy>().Animation.Walk = true;
                }

                private void GoalInit()
                {
                    indexStart_ = transform.gameObject.GetComponent<BaseEnemy>().IndexPost;

                    //ゴールまでの道筋をpostManagerからもらう
                    postGoal_ = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().PostRoot[indexStart_].RootGoalObject[indexGoal_].GoalRootObject;


                }
            }
        }
    }
}
