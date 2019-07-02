using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Game
{
    namespace EditorSound
    {
#if UNITY_EDITOR
        public class SoundStore : EditorWindow
        {
            [SerializeField]
            private SoundData.DATA[] soundData_;
            [SerializeField]
            private string[] name_;
            [SerializeField]
            private bool fixSound_ = false;
            [SerializeField]
            private int intValue_;
            [SerializeField]
            private AudioClip[] soundS_;
            [SerializeField]
            private Vector2 leftScrollPos_ = Vector2.zero;
            [SerializeField]
            private GameObject objSound;
            [MenuItem("Editor/Sound")]
            public static void  Init()
            {
                GetWindow<SoundStore>("Sound");
            }

            private void OnGUI()
            {
               if(!fixSound_)
                {
                    StartGUI();
                }
               else
                {
                    FixGUI();
                }
            }

            private void StartGUI()
            {
                this.intValue_ = EditorGUILayout.IntField("intValue", this.intValue_);

                if (GUILayout.Button("Create"))
                {
                    soundData_ = new SoundData.DATA[intValue_];
                    name_ = new string[intValue_];
                    soundS_ = new AudioClip[intValue_];
                    fixSound_ = true;

                    int cnt = 0;
                    foreach (var data in soundData_)
                    {
                        soundData_[cnt] = new SoundData.DATA();
                        soundData_[cnt].Sound = soundS_[cnt];
                        cnt++;
                    }
              
                }
            }

            private void FixGUI()
            {
                int cnt = 0;
                leftScrollPos_ = EditorGUILayout.BeginScrollView(leftScrollPos_, GUI.skin.box);
                for(int length = 0; length < intValue_; length++)
                {
                    soundData_[length].Volum = EditorGUILayout.FloatField("VOLUM", soundData_[length].Volum);
                    soundData_[length].Sound = EditorGUILayout.ObjectField(soundData_[length].Sound, typeof(AudioClip), true) as AudioClip;
                    bool loop = soundData_[length].Loop;
                    loop = EditorGUILayout.Toggle("LOOP",loop);
                    soundData_[length].Loop = loop;
                    name_[cnt] = EditorGUILayout.TextField("name", name_[cnt]);
                    cnt++;
                   
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginVertical();
                objSound = EditorGUILayout.ObjectField(objSound, typeof(GameObject), true) as GameObject;
                EditorGUILayout.EndVertical();
                if (GUILayout.Button("Create"))
                {
                    if(objSound)
                    {
                        Game.SoundStore store = objSound.GetComponent<Game.SoundStore>();
                        store.SoundStoreS = new SoundData.DATA[intValue_];
                        store.SoundName = new string[intValue_];
                        int cntObj = 0;
                        foreach (var data in soundData_)
                        {
                            store.SoundStoreS[cntObj] = new SoundData.DATA();
                            store.SoundStoreS[cntObj] = data;
                            store.SoundName[cntObj] = name_[cntObj];
                            cntObj++;
                        }
                        
                        Undo.RegisterCreatedObjectUndo(objSound, "Create New GameObject");

                        objSound.GetComponent<Game.SoundStore>().SoundStoreS = store.SoundStoreS;
                    }
                    
                }

            }
        }
#endif
    }
}
