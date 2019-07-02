using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
#if UNITY_EDITOR
    [ExecuteInEditMode()]
    public class LinkDebug : MonoBehaviour
    {
        double waitTime = 0;
        void OnEnable()
        {
            waitTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += EditorUpdate;
        }

        void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
        }

        // 更新処理
    
        void EditorUpdate()
        {

           // １／６０秒に１回更新
           if ((EditorApplication.timeSinceStartup - waitTime) >= 0.01666f)
           {
               // 君だけの更新処理を書こう！
               Updatefunc();

               SceneView.RepaintAll(); // シーンビュー更新
               waitTime = EditorApplication.timeSinceStartup;
           }

        }

        void Updatefunc()
        {
            Link link = GetComponent<Link>();
            Material mat = GetComponent<MeshRenderer>().material;

            if(link.Obstacle == true)
            {
                mat.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                if (link.UseObstacle == true)
                {
                    mat.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                }
            }
            else
            {
                mat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        
    }
#endif
}
