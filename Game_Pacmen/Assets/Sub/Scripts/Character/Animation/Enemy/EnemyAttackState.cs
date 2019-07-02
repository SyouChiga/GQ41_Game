using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Enemy
        {
            public class EnemyAttackState : BaseEnemyAnimationState
            {
                // Start is called before the first frame update
                void Start()
                {
                    enemyAnimation = (EnemyAnimation)animation;

                    enemyAnimation.Animator.SetBool("is_attack", true);
                 
                    enemyAnimation.Animator.ForceStateNormalizedTime(0.0f);
                }

                // Update is called once per frame
                void Update()
                {
                    
                    AnimatorStateInfo state = enemyAnimation.Animator.GetCurrentAnimatorStateInfo(0);

                    if(state.normalizedTime > 1.0f)
                    {
                        if (enemyAnimation.Attack2 == true)
                        {
                            enemyAnimation.Attack = false;
                            AddAnimation2();
                        }
                        else
                        {
                            enemyAnimation.Attack = false;
                            AddAnimation();
                        }
                    }
                }

                //アニメーションコンポーネントの追加
                new public void AddAnimation()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyWaitState>();
                    enemyAnimation.ChangeAnimationState();
                }

                public void AddAnimation2()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyAttack2State>();
                    enemyAnimation.ChangeAnimationState();
                }
            }
        }
    }
}
