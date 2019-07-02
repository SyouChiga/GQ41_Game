using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundStore : MonoBehaviour
    {
        [SerializeField]
        SoundData.DATA [] soundStoreS_;
        public SoundData.DATA[] SoundStoreS
        {
            get
            {
                return soundStoreS_;
            }
            set
            {
                soundStoreS_ = value;
            }
        }
        [SerializeField]
        string[] soundName_;
        public string[] SoundName
        {
            get
            {
                return soundName_;
            }
            set
            {
                soundName_ = value;
            }
        }

    }
}
