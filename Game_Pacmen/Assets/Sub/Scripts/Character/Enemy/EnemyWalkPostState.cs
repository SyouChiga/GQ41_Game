using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWalkPostState : BaseEnemyState
            {
                //ゴールまでの道筋
                [SerializeField]
                List<GameObject> postGoal_;
                public List<GameObject> postGoal
                {
                    set
                    {
                        postGoal_ = value;
                    }
                    get
                    {
                        return postGoal_;
                    }
                }
                //ゴールまでのカウント
                int cntGoal_;
                public int CntGoal
                {
                    set
                    {
                        cntGoal_ = value;
                    }
                }
                //障害物のオブジェクト
                private List<GameObject> obstacleObj_ = new List<GameObject>();
                private List<GameObject> ObstacleObj
                {
                    set
                    {
                        obstacleObj_ = value;
                    }
                }


                //障害物のカウント
                private int obstacleNum_;
                private int obstacleCnt_;
                public  int ObstacleCnt
                {
                    set
                    {
                        obstacleCnt_ = value;
                    }
                }

                // Start is called before the first frame update
                void Start()
                {

                }

                // Update is called once per frame
                void Update()
                {
                    if(Walk())
                    {
                        cntGoal_++;
                        if(UpdateObstacle()) return;
                        //まだ目的地についていないなら
                        if (cntGoal_ < postGoal_.Count)
                        {
                            Object.Destroy(GetComponent<BaseEnemy>().State);
                            EnemyWalkPostState walk = gameObject.AddComponent<EnemyWalkPostState>();
                            walk.postGoal = postGoal_;
                            walk.CntGoal = cntGoal_;
                            walk.ObstacleObj = obstacleObj_;
                            walk.ObstacleCnt = 0;
                            GetComponent<BaseEnemy>().State = walk;
                            GetComponent<BaseEnemy>().Animation.Walk = true;
                        }
                        else
                        {
                            Object.Destroy(GetComponent<BaseEnemy>().State);
                            GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyWaitState>();
                            GetComponent<BaseEnemy>().Animation.Walk = false;
                        }
                    }
                }

                //目的地まで向かう
                bool Walk()
                {
                    Vector3 goalVectorXZ = new Vector3(postGoal_[cntGoal_].transform.position.x, 0.0f, postGoal_[cntGoal_].transform.position.z);
                    Vector3 enemyVectorXZ = new Vector3(transform.position.x, 0.0f, transform.position.z);

                    Vector3 goalVector = (goalVectorXZ - enemyVectorXZ);

                    transform.position += goalVector.normalized * 0.05f;


                    //if (transform.position.y != postGoal_[cntGoal_].transform.position.y)
                    //{
                    //    postGoal_[cntGoal_].transform.position = new Vector3(postGoal_[cntGoal_].transform.position.x, transform.position.y, postGoal_[cntGoal_].transform.position.z);
                    //}
                    Quaternion targetRotation = Quaternion.LookRotation(postGoal_[cntGoal_].transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.05f);
                    if (goalVector.magnitude < 1.0f)
                    {
                        return true;
                    }


                    return false;
                }

                bool UpdateObstacle()
                {
                    //障害物発見
                    if(Obstacle())
                    {
                        BaseEnemy enemy = GetComponent<BaseEnemy>();

                        int indexStart = enemy.IndexPost;
                        int indexGoal = postGoal_[postGoal.Count - 1].GetComponent<Link>().LinkID - 1;

                        GameObject[] objs = new GameObject[obstacleObj_.Count];
                        for(int x = 0; x < obstacleObj_.Count; x++)
                        {
                            objs[x] = obstacleObj_[x];
                        }

                        List<GameObject> objRoot = new List<GameObject>();
                        List<GameObject> objNewRoot = new List<GameObject>();

                        float length = 0.0f;
                        float length01 = 0.0f;
                        bool check = false;

                        Link[] link = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;
                        PostManager.PostRootFGoalObstacleReturn(link[enemy.IndexPost],
                                                                postGoal[postGoal.Count - 1],
                                                                objs,
                                                                objRoot,
                                                                objNewRoot,
                                                                ref length,ref length01,ref check);


                        Object.Destroy(GetComponent<EnemyWalkPostState>());
                        EnemyWalkPostState walk = gameObject.AddComponent<EnemyWalkPostState>();
                        walk.postGoal = objRoot;
                        walk.CntGoal = 0;
                        walk.ObstacleObj = obstacleObj_;
                        walk.ObstacleCnt = obstacleCnt_;
                        GetComponent<BaseEnemy>().State = walk;
                        GetComponent<BaseEnemy>().Animation.Walk = true;

                        return true;
                    }
                    return false;
                }

                //目的地までに障害物が発生してしまった場合
                bool Obstacle()
                {
                    List<GameObject> obstacleSaveObj = new List<GameObject>();
                  

                    Link[] objectObstacle = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;
                    foreach (var obj in objectObstacle)
                    {
                       
                        if (obj.Obstacle == true)
                        {
                           
                            obstacleNum_++;
                            if (obj.UseObstacle == false)
                            {
                                continue;
                            }
                            obstacleSaveObj.Add(obj.gameObject);

                        }
                        
                    }

                    bool safe = true;
                    if (obstacleObj_.Count > 0)
                    {
                        for (int cnt = 0; cnt < obstacleSaveObj.Count; cnt++)
                        {
                            for (int cntSave = 0; cntSave < obstacleObj_.Count; cntSave++)
                            {
                                if (obstacleObj_[cntSave] == obstacleSaveObj[cnt])
                                {
                                    safe = true;
                                    break;
                                }
                                else
                                {
                                    safe = false;
                                }
                            }

                        }
                        if (safe == true)
                        {
                           
                            return false;
                        }
                    }


                    obstacleObj_ = obstacleSaveObj;



                    if (obstacleObj_.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
    }
}
