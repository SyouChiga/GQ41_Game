using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        public class IPlayer : MonoBehaviour
        {
            //操作
            float inputHorizontal;
            float inputVertical;
            //物理
            Rigidbody rb;
            //プレイヤーアニメーション
            Animation.PlayerAnimation playerAnim_; 
            // Start is called before the first frame update
            void Start()
            {
                playerAnim_ = GetComponent<Animation.PlayerAnimation>();
                rb = GetComponent<Rigidbody>();
            }

            // Update is called once per frame
            void Update()
            {
                InputUpdate();
            }
            void FixedUpdate()
            {
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

                // 方向キーの入力値とカメラの向きから、移動方向を決定
                Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

                // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
                transform.position += (moveForward * 3.0f) * 0.005f;


                //キャラクターが動いていないとき
                if (inputHorizontal == 0.0 && inputVertical == 0.0)
                {

                    playerAnim_.Walk = false;
                }
                else
                {
                    playerAnim_.Walk = true;

                }


                // キャラクターの向きを進行方向に
                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                }
            }

            //操作
            void InputUpdate()
            {
                inputHorizontal = Input.GetAxisRaw("Horizontal");
                inputVertical = Input.GetAxisRaw("Vertical");
            }
        }
    }
}
