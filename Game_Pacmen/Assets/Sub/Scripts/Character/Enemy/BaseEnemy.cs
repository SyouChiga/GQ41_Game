using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class BaseEnemy : BaseCharacter
            {
                protected GameObject[] postObj_;
                public GameObject[] PostObject
                {
                    get
                    {
                        return postObj_;
                    }
                }
                protected Animation.EnemyAnimation enemyAnimation_;
                public Animation.EnemyAnimation Animation
                {
                    get
                    {
                        return enemyAnimation_;
                    }
                }
                protected BaseEnemyState state_;
                public BaseEnemyState State
                {
                    get
                    {
                        return state_;
                    }
                    set
                    {
                        state_ = value;
                    }
                }
                //近いところにあるpost
                [SerializeField]
                protected int indexPost_;
                public int IndexPost
                {
                    get
                    {
                        return indexPost_;
                    }
                }
                // Start is called before the first frame update
                void Start()
                {
                    Init();
                }

                // Update is called once per frame
                void Update()
                {
                    PostSearch();
                }

                //Init
                protected void Init()
                {
                    state_ = gameObject.AddComponent<EnemyWaitState>();
                    enemyAnimation_ = GetComponent<Animation.EnemyAnimation>();
                    GameObject[] post = GameObject.FindGameObjectsWithTag("post_tag");
                    postObj_ = post;
                    PostSearch();
                }

                protected void PostSearch()
                {
                    Link[] link = GameObject.Find("FieldObject/Post").GetComponent<PostManager>().Link;

                    float length = 0.0f;
                    float newlegth = 0.0f;
                    bool safe = false;

                    foreach(var linkObj in link)
                    {
                        newlegth = (linkObj.transform.position - transform.position).magnitude;
                        if(newlegth <= length || safe == false)
                        {
                            safe = true;
                            length = newlegth;
                            indexPost_ = linkObj.LinkID - 1;
                        }
                    }
                }
            }
        }
    }
}
