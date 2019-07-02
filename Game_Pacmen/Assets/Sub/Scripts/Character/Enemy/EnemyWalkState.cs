using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWalkState : BaseEnemyState
            {
                Transform goal_;
                public Transform GoalPosition
                {
                    set
                    {
                        goal_ = value;
                    }
                }

                // Start is called before the first frame update
                void Start()
                {

                }

                // Update is called once per frame
                void Update()
                {
                    //目的地についたら
                    if(Walk())
                    {
                        Object.Destroy(GetComponent<BaseEnemy>().State);
                        GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyWaitState>();
                        GetComponent<BaseEnemy>().Animation.Walk = false;
                    }
                }

                //目的地まで向かう
                bool Walk()
                {
                    Vector3 goalVectorXZ = new Vector3(goal_.transform.position.x, 0.0f,goal_.transform.position.z);
                    Vector3 enemyVectorXZ = new Vector3(transform.position.x, 0.0f, transform.position.z);

                    Vector3 goalVector = (goalVectorXZ - enemyVectorXZ);
                    transform.position += goalVector.normalized * 0.05f;

                  
                    //if (transform.position.y != goal_.position.y)
                    //{
                    //    goal_.position = new Vector3(goal_.position.x, transform.position.y, goal_.position.z);
                    //}
                    Quaternion targetRotation = Quaternion.LookRotation(goal_.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.05f);
                    if (goalVector.magnitude < 1.0f)
                    {
                        return true;
                    }


                    return false;
                }
            }
        }
    }
}