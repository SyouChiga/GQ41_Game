using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyAttackState : BaseEnemyState
            {
                // Start is called before the first frame update
                void Start()
                {
                    GetComponent<BaseEnemy>().Animation.Attack2 = true;
                }

                // Update is called once per frame
                void Update()
                {
                    Attack();
                    if(GetComponent<BaseEnemy>().Animation.Attack2 == true)
                    {
                        Object.Destroy(GetComponent<BaseEnemy>().State);
                        GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyAttack2State>();
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
