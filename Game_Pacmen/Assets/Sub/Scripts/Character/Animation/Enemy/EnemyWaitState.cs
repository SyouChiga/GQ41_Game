using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Enemy
        {
            public class EnemyWaitState : BaseEnemyAnimationState
            {
                // Start is called before the first frame update
                void Start()
                {
                    enemyAnimation = (EnemyAnimation)animation;
                    enemyAnimation.Animator.SetBool("is_attack", false);
                    enemyAnimation.Animator.SetBool("is_attack2", false);
                    enemyAnimation.Animator.SetBool("is_walk", false);
                    enemyAnimation.Walk = false;
                }

                // Update is called once per frame
                void Update()
                {

                    AnimatorStateInfo state = enemyAnimation.Animator.GetCurrentAnimatorStateInfo(0);

                    if (enemyAnimation.Attack)
                    {
                        AddAnimationAttack();
                    }
                    if (enemyAnimation.Walk)
                    {

                        AddAnimation();

                    }
                }

                //アニメーションコンポーネントの追加
                new public void AddAnimation()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyWalkState>();
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