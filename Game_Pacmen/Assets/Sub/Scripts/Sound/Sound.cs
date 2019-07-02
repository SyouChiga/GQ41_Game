using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Manager
    {
        public class Sound : MonoBehaviour
        {
            [SerializeField]
            private static Dictionary<string, SoundData.DATA> soundStore_;

            private void Awake()
            {
                SoundStore store = GetComponent<SoundStore>();
                int cnt = 0;
                soundStore_ = new Dictionary<string, SoundData.DATA>();
                for (int obj = 0; obj < store.SoundStoreS.Length; obj++)
                {
                    store.SoundStoreS[obj].Audio = new AudioSource();
                    store.SoundStoreS[obj].Audio = gameObject.AddComponent<AudioSource>();

                    store.SoundStoreS[obj].Audio.clip = store.SoundStoreS[obj].Sound;
                    soundStore_.Add(store.SoundName[cnt], store.SoundStoreS[obj]);
                    cnt++;
                }
            }
            void Start()
            {




                //BGM
           

            }

            public static void Play(string playName)
            {
                soundStore_[playName].Audio.volume = soundStore_[playName].Volum;
                soundStore_[playName].Audio.Play();

            }
            public static void PlayOnShot(string playName)
            {
                soundStore_[playName].Audio.PlayOneShot(soundStore_[playName].Audio.clip);
            }

            public static void Stop(string name)
            {
                soundStore_[name].Audio.Stop();
            }
        }
    }
}
