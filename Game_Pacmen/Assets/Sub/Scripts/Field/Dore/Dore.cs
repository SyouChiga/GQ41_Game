using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class Dore : MonoBehaviour
    {
        [SerializeField]
        private GameObject linkObj;
        [SerializeField]
        private bool useObstacle;
        [SerializeField]
        private Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            useObstacle = false;
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            DoreAnimation();
        }

        void DoreAnimation()
        {
            Link link = linkObj.GetComponent<Link>();

            anim.SetBool("Opne", useObstacle);

            link.UseObstacle = useObstacle;
        }
    }
}
