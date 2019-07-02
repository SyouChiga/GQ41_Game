using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyAttack2State : BaseEnemyState
            {
                void Start()
                {
                   
                }

                // Update is called once per frame
                void Update()
                {
                    Attack();
                    if (GetComponent<BaseEnemy>().Animation.Attack2 == false)
                    {
                        Object.Destroy(GetComponent<BaseEnemy>().State);
                        if(GetComponent<BaseEnemy>().Animation.Walk) GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyWalkState>();
                        else GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyWaitState>();
                    }
                }

                //攻撃
                void Attack()
                {

                }
            }
        }
    }
}
