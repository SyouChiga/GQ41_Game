using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Enemy
        {
            public class EnemyWalkState : BaseEnemyAnimationState
            {
                // Start is called before the first frame update
                void Start()
                {
                    enemyAnimation = (EnemyAnimation)animation;

                    enemyAnimation.Animator.SetBool("is_walk", true);
                }

                // Update is called once per frame
                void Update()
                {
                    if (enemyAnimation.Attack)
                    {
                        AddAnimationAttack();
                    }
                    if (!enemyAnimation.Walk)
                    {

                        AddAnimation();

                    }
                }

                //アニメーションコンポーネントの追加
                new public void AddAnimation()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyWaitState>();
                    enemyAnimation.ChangeAnimationState();
                }
                public void AddAnimationAttack()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyAttackState>();
                    enemyAnimation.ChangeAnimationState();
                }
            }
        }
    }
}
