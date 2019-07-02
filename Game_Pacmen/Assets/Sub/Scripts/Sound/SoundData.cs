using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class SoundData : MonoBehaviour
    {
        [System.Serializable]
        public struct DATA
        {
            [SerializeField]
            private AudioClip sound_;
            public AudioClip Sound
            {
                set
                {
                    sound_ = value;
                }
                get
                {
                    return sound_;
                }
            }
            [SerializeField]
            private bool loop_;
            public bool Loop
            {
                set
                {
                    loop_ = value;
                }
                get
                {
                    return loop_;
                }
            }
            [SerializeField]
            private AudioSource audio_;
            public AudioSource Audio
            {
                get
                {
                    return audio_;
                }
                set
                {
                    audio_ = value;
                }
            }
            [SerializeField]
            private float volum_;
            public float Volum
            {
                get
                {
                    return volum_;
                }
                set
                {
                    volum_ = value;
                }
            }
            

        }

    }
}
