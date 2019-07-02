using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Enemy
        {
            public class EnemyAttack2State : BaseEnemyAnimationState
            {
                // Start is called before the first frame update
                void Start()
                {
                    enemyAnimation = (EnemyAnimation)animation;
                    enemyAnimation.Attack = false;
                    enemyAnimation.Animator.SetBool("is_attack2", true);
                    enemyAnimation.Animator.SetBool("is_attack", false);
                    enemyAnimation.Animator.ForceStateNormalizedTime(0.0f);
                }

                // Update is called once per frame
                void Update()
                {

                    AnimatorStateInfo state = enemyAnimation.Animator.GetCurrentAnimatorStateInfo(0);
                    if (state.normalizedTime > 1.0f)
                    {
                        enemyAnimation.Attack2 = false;
                        AddAnimation();
                    }
                }

                //アニメーションコンポーネントの追加
                new public void AddAnimation()
                {
                    Object.Destroy(enemyAnimation.AnimationStateValue);
                    if(enemyAnimation.Walk) enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyWalkState>();
                    else enemyAnimation.AnimationStateValue = gameObject.AddComponent<EnemyWaitState>();
                    enemyAnimation.ChangeAnimationState();
                }
            }
        }
    }
}
