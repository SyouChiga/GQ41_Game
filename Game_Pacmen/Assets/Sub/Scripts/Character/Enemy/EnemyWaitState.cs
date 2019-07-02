using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWaitState : BaseEnemyState
            {
                [SerializeField]
                private GameObject goalPost_;
                [SerializeField]
                private float waitTime_;
                // Start is called before the first frame update
                void Start()
                {
                   
                    PostRoot();
                }
                //デバックゴール
                public int debugGoal_ = 1;

                // Update is called once per frame
                void Update()
                {
                    waitTime_ += Time.deltaTime;
                    if(waitTime_ >= 3.0f)
                    {
                        int range = Random.Range(0, 100);
                        Object.Destroy(GetComponent<BaseEnemy>().State);

                        if (range < 50)
                        {
                            if (range > 25)
                            {
                                Link[] link = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;
                                EnemyWaitPostState walk = gameObject.AddComponent<EnemyWaitPostState>();
                                walk.IndexGoal = debugGoal_ -1;
                                GetComponent<BaseEnemy>().State = walk;
                                GetComponent<BaseEnemy>().Animation.Walk = true;
                            }
                            else
                            {
                                Link[] link = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;
                                EnemyWaitPostState walk = gameObject.AddComponent<EnemyWaitPostState>();
                                walk.IndexGoal = debugGoal_-1;
                                GetComponent<BaseEnemy>().State = walk;
                                GetComponent<BaseEnemy>().Animation.Walk = true;
                            }
                        }
                        else
                        {
                            if (range < 75)
                            {
                                GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyAttackState>();
                                GetComponent<BaseEnemy>().Animation.Attack = true;
                            }
                            else
                            {
                                Link[] link = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;
                                EnemyWaitPostState walk = gameObject.AddComponent<EnemyWaitPostState>();
                                walk.IndexGoal = debugGoal_ -1;
                                GetComponent<BaseEnemy>().State = walk;
                                GetComponent<BaseEnemy>().Animation.Walk = true;
                            }
                        }
                    }
                }

                //目的地を決める
                private void PostRoot()
                {
                    BaseEnemy enemy = GetComponent<BaseEnemy>();

                    GameObject[] postObj = enemy.PostObject;

                    int range = Random.Range(0, postObj.Length);

                    goalPost_ = postObj[range];
                }
            }
        }
    }
}
