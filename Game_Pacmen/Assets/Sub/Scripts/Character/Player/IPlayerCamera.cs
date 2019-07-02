using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        public class IPlayerCamera : MonoBehaviour
        {
            //プレイヤーのポジション
            Transform playerPos;
            // Start is called before the first frame update
            void Start()
            {
                playerPos = GameObject.Find("unitychan").transform;
            }

            // Update is called once per frame
            void Update()
            {
                float mouseInputX = Input.GetAxis("Vertical2");

                float mouseInputY = Input.GetAxis("Horizontal2");

                if (mouseInputX > 0 || mouseInputX < 0 || mouseInputY > 0 || mouseInputY < 0)
                {

                    // targetの位置のY軸を中心、回転（公転）する
                    transform.RotateAround(playerPos.position, Vector3.up, mouseInputX * Time.deltaTime * 200f);
                    // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
                    transform.RotateAround(playerPos.position, transform.right, mouseInputY * Time.deltaTime * 200f);


                }

                Vector3 pos = playerPos.forward;
                Vector3 playerPosition = playerPos.position;
                playerPosition.y = 2.0f;
               
                transform.position = playerPosition - new Vector3(pos.x * 3.0f,0.0f, pos.z * 3.0f);


            }
        }
    }
}
