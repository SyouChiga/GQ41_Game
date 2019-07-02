using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Player
        {
            public class BasePlayerAnimationState : BaseAnimationState
            {
                protected PlayerAnimation playerAnimation; //アニメーション
                public PlayerAnimation PlayerAnimation
                {
                    set
                    {
                        playerAnimation = value;
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
