using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        public class BaseAnimationState : MonoBehaviour
        {
            [SerializeField]
            protected BaseAnimation animation; //アニメーション
            public BaseAnimation Animation
            {
                set
                {
                    animation = value;
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

            //アニメーションステートのセット
            public void SetAnimation(BaseAnimationState animation)
            {
                animation = this;
            }
            //削除処理
            protected void Release() { }

            //アニメーションコンポーネントの追加
            public void AddAnimation() { }
        }
    }
}
