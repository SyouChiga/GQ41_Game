using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        public class BaseAnimation : MonoBehaviour
        {
            [SerializeField]
            protected Animator anim; //アニメーション
            public Animator Animator
            {
                get
                {
                    return anim;
                }
            }
            [SerializeField]
            protected BaseAnimationState animationState_; //アニメーションステート
            public BaseAnimationState AnimationStateValue
            {
                set
                {
                    animationState_ = value;
                }
                get
                {
                    return animationState_;
                }
            }
            // Start is called before the first frame update
            void Start()
            {
                animationState_ = GetComponent<BaseAnimationState>();
                animationState_.Animation = this;
                anim = GetComponent<Animator>();
            }

            // Update is called once per frame
            void Update()
            {
                AnimationState();
            }

            //アニメーションステート
            protected void AnimationState()
            {
                
            }

            public void ChangeAnimationState()
            {
               
              
                animationState_.Animation = this;


            }
        }
    }
}
