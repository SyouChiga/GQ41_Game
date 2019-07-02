using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Enemy
        {
            public class BaseEnemyAnimationState : BaseAnimationState
            {
                protected EnemyAnimation enemyAnimation; //アニメーション
                public EnemyAnimation EnemyAnimation
                {
                    set
                    {
                        enemyAnimation = value;
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
}