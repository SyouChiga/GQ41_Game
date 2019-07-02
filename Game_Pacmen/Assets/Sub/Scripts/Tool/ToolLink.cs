using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    namespace Tool
    {
        public class ToolLink : BaseTool
        {
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }

#if UNITY_EDITOR
            [MenuItem("Tools/LINK %g")]
            //リンクの再接続
            public static void ReturnLink()
            {
                //Scene上にあるリンクを拾う
                
                Link[] links = GameObject.FindObjectsOfType<Link>();
                
                foreach (var link in links)
                {
                    //接続先のオブジェクト
                    GameObject[] linkObj = link.LinkObject;

                    foreach (var obj in linkObj)
                    {
                        Debug.Log(obj);
                        //接続先のオブジェクトのリンク
                        Link nextLink = obj.GetComponent<Link>();
                        
                        bool safe = false; //接続先の自分が存在しているかどうか
                        
                        GameObject[] Object = nextLink.LinkObject;
                        foreach (var nextLinkObj in nextLink.LinkObject)
                        {
                            if (nextLinkObj.name == link.gameObject.name)
                            {
                                safe = true;
                                break;
                            }
                            else
                            {
                                safe = false;
                            }

                        }
                        if (!safe)
                        {

                            GameObject[] saveObject = new GameObject[nextLink.LinkObject.Length + 1];
                            int cnt = 0; //カウント
                            for (cnt = 0; cnt < nextLink.LinkObject.Length; cnt++)
                            {
                                saveObject[cnt] = nextLink.LinkObject[cnt];
                            }
                            //接続先に自分自身を入れる
                            saveObject[cnt] = link.gameObject;
                            nextLink.LinkObject = saveObject;
                        }

                        Debug.Log(safe);
                        //接続先に自分が存在していなかったら

                    }
                    link.InitLink();
                }
            }
#endif
        }
    }
}
