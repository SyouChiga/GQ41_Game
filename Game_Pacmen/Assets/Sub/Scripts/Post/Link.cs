using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    public class Link : MonoBehaviour
    {
        //接続するオブジェクト
        [SerializeField]
        private GameObject[] linkObj_;
        public GameObject[] LinkObject
        {
            get
            {
                return linkObj_;
            }
            set
            {
                linkObj_ = value;
            }

        }
        //接続しているオブジェクトの距離
        [SerializeField]
        private List<float> linkLength_;
        public List<float> LinkLength
        {
            get
            {
                return linkLength_;
            }
        }
        //ID
        [SerializeField]
        private int linkID_;
        public int LinkID
        {
            set
            {
                linkID_ = value;
            }
            get
            {
                return linkID_;
            }
        }
        //ID
        [SerializeField]
        private int linkIDNext_;
        public int linkIDNext
        {
            get
            {
                return linkIDNext_;
            }
            set
            {
                linkIDNext_ = value;
            }
        }
        //障害物
        [SerializeField]
        private bool obstacle_ = false;
        public bool Obstacle
        {
            set
            {
                obstacle_ = value;
            }
            get
            {
                return obstacle_;
            }
        }
        [SerializeField]
        private int obstacleID_;
        public int ObstacleID
        {
            set
            {
                obstacleID_ = value;
            }
            get
            {
                return obstacleID_;
            }
        }
        //障害物発生!
        [SerializeField]
        private bool useObstacle_ = false;
        public bool UseObstacle
        {
            get
            {
                return useObstacle_;
            }
            set
            {
                useObstacle_ = value;
            }
        }



        //ウィンドウのサイズ
        private Rect windowSize_ = new Rect(new Vector2(0.0f, 0.0f), new Vector2(100.0f, 50.0f));
        public Rect WindoqSIze
        {
            get
            {
                return windowSize_;
            }
        }
        private Vector2 move_ = new Vector2(0.0f, 0.0f);

        // Start is called before the first frame update
        void Awake()
        {
            InitLink();
            MateriakChange();
        }

        // Update is called once per frame
        void Update()
        {
            MateriakChange();
        }

        void MateriakChange()
        {
            Material mat = GetComponent<MeshRenderer>().material;

            if (Obstacle)
            {
                mat.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            }
            if (useObstacle_)
            {
                mat.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                mat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        //リンクの初期化
        public void InitLink()
        {
            //接続しているオブジェクトの距離をリストに入れる
            linkLength_ = new List<float>(linkLength_.Count);
            foreach (var obj in linkObj_)
            {
                float length = (obj.transform.position - transform.position).magnitude;
                linkLength_.Add(length);
            }
        }
        //デバッグビュー
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            //接続しているオブジェクトと線を結ぶ
            foreach (var obj in linkObj_)
            {
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }
#if UNITY_EDITOR
        public void ToolUpdate(Vector2 move)
        {
            move_ += move;
            if (windowSize_ != null) windowSize_.position = new Vector2(transform.position.x * 20.0f + move_.x, transform.position.z * 20.0f + move_.y);

        }
        //toolするときの描画処理
        public void ToolDraw()
        {
            windowSize_ = GUI.Window(linkID_, windowSize_, ToolWindowDraw, "LINK" + linkID_);
            DrawLine();
        }

        private void ToolWindowDraw(int id)
        {
            GUI.DragWindow();
            GUI.Label(new Rect(30, 20, 100, 100), "ID" + id, EditorStyles.label);


        }

        void DrawLine()
        {
            foreach (var obj in linkObj_)
            {
                Vector3 start = new Vector3(windowSize_.position.x + 100.0f / 2.0f, windowSize_.position.y + 50.0f / 2.0f, 0.0f);
                Vector3 end = new Vector3(obj.GetComponent<Link>().windowSize_.position.x + 100.0f / 2.0f, obj.GetComponent<Link>().windowSize_.position.y + 50.0f / 2.0f, 0.0f);


                Handles.DrawLine(start, end);
            }
        }
#endif

    }

   
}
