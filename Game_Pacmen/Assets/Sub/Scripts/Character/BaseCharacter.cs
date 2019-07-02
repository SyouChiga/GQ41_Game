using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    namespace Character
    {
        public class BaseCharacter : MonoBehaviour
        {
            private Rect windowSize_ = new Rect(new Vector2(0.0f, 0.0f), new Vector2(100.0f, 50.0f));
            protected Vector2 toolMove_ = new Vector2(0.0f, 0.0f);
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }
#if UNITY_EDITOR
            public void ToolDraw()
            {
                windowSize_ = GUI.Window(0, windowSize_, ToolWindowDraw, gameObject.name);
            }

            public void ToolUpdate(Vector2 move)
            {
                toolMove_ += move;
                windowSize_.position = new Vector2(transform.position.x * 20.0f + toolMove_.x, transform.position.z * 20.0f + toolMove_.y);
            }

            void ToolWindowDraw(int id)
            {
                GUI.DragWindow();
                GUI.Label(new Rect(30, 20, 100, 100), gameObject.name, EditorStyles.label);
            }
#endif
        }
    }
}