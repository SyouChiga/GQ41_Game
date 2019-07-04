﻿using UnityEngine;

using UnityEditor;

using System.Collections;

using System.IO;

using System.Collections.Generic;

using System.Linq;

namespace Game
{
    namespace Tool
    {
#if UNITY_EDITOR
        //================================================
        //ToolMapCreate:マップ生成ツール(テクスチャ)
        //================================================
        public class ToolMapCreate : EditorWindow
        {
            public const int fieldX_ = 10;
            public const int  fieldY_ = 10;

            //ファイルパス名
            string filePath = "Assets/Sub/Texture/Dungeon";
            public string FilePath
            {
                get
                {
                    return filePath;
                }
            }

            string selectImagePath = null;
            public string SelectImagePath
            {
                get
                {
                    return selectImagePath;
                }
            }
            //子ウィンドウ
            ToolMapCreateUI chiled_;
            [MenuItem("Tools/MapCreate %l")]
            static void Init()
            {
                EditorWindow.GetWindow<ToolMapCreate>(true, "MapCreate");
            }


            //現在選択しているType
            private Field.FIELD_TYPE type_;
            public Field.FIELD_TYPE Type
            {
                get
                {
                    return type_;
                }
            }

            //画像
            void DrawTexture()
            {
                string[] names = Directory.GetFiles(filePath, "*.png");

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();

                //カウント数
                int cntY = 0;
                int typeCnt = 0;
                foreach(var texturePath in names)
                {
                    Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));

                    if(cntY % 4 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                       EditorGUILayout.BeginHorizontal();
                    }
                    if(GUILayout.Button(tex, GUILayout.MaxWidth(50.0f), GUILayout.MaxHeight(50.0f), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    {
                        selectImagePath = texturePath;
                        type_ = (Field.FIELD_TYPE)GetType(typeCnt);
                    }
                    typeCnt++;
                    cntY++;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

            }
            int GetType(int type)
            {
                int returnType = 0;

                switch (type)
                {
                    case 0:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CONVEX_DOWN;
                        break;
                    case 1:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CONVEX_LEFT;
                        break;
                    case 2:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CONVEX_RIGHT;
                        break;
                    case 3:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CONVEX_UP;
                        break;
                    case 4:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORNER_LEFTDOWN;
                        break;
                    case 5:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORNER_LEFTUP;
                        break;
                    case 6:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORNER_RIGHTDOWN;
                        break;
                    case 7:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORNER_RIGHTUP;
                        break;
                    case 8:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORRIDOR_LEFTRIGHT;
                        break;
                    case 9:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CORRIDOR_UPDOWN;
                        break;
                    case 10:
                        returnType = (int)Field.FIELD_TYPE.FIELD_CROSSROAD;
                        break;
                    case 11:
                        returnType = (int)Field.FIELD_TYPE.FIELD_ENDOFROAD_DOWN ;
                        break;
                    case 12:
                        returnType = (int)Field.FIELD_TYPE.FIELD_ENDOFROAD_LEFT;
                        break;
                    case 13:
                        returnType = (int)Field.FIELD_TYPE.FIELD_ENDOFROAD_RIGHT;
                        break;
                    case 14:
                        returnType = (int)Field.FIELD_TYPE.FIELD_ENDOFROAD_UP;
                        break;
                }


                return returnType;
            }

            void DrawSelectTexture()
            {
                if(selectImagePath != null)
                {
                    Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(selectImagePath, typeof(Texture2D));

                    EditorGUILayout.BeginVertical();

                    GUILayout.FlexibleSpace();

                    GUILayout.Label("select : " + selectImagePath);

                    GUILayout.Box(tex);

                    EditorGUILayout.EndVertical();

                }
            }

            void DrawButton()
            {
                EditorGUILayout.BeginVertical();

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("open map editor"))
                {

                    if (chiled_ == null)
                    {

                        chiled_ = ToolMapCreateUI.WindowCreate(this);

                    }
                    else
                    {

                        chiled_.Focus();

                    }

                }

                EditorGUILayout.EndVertical();
            }

            private void DrawAll()
            {
                DrawTexture();
                DrawButton();
                DrawSelectTexture();
            }

            private void OnGUI()
            {
                DrawAll();

            }



    }
        public class ToolMapCreateUI : EditorWindow
        {

            struct FIELD
            {
                public Field.FIELD_TYPE type_;
                public string filePath_;
                public string objectpath_;
                public GameObject obj_;
                public Link link_;
            }

            struct INDEX
            {
                public int indexX;
                public int indexY;
            }
            //親ウィンドウ
            static public ToolMapCreate parent_;
            //フィールド配列
            private FIELD[,] field_;
            //描画モデル
            private PreviewRenderUtility renderer;
            //カメラ座標
            private Vector3 cameraPos_ = Vector3.zero;
            //フィールドサイズ
            private int fieldMaxX_;
            private int fieldMaxY_;
            public static ToolMapCreateUI WindowCreate(ToolMapCreate mapCreate)
            {
                ToolMapCreateUI window = (ToolMapCreateUI)EditorWindow.GetWindow(typeof(ToolMapCreateUI), false);
                window.Show();
                window.fieldMaxX_ = ToolMapCreate.fieldX_;
                window.fieldMaxY_ = ToolMapCreate.fieldY_;
                window.field_ = new FIELD[window.fieldMaxX_, window.fieldMaxY_];
                
                parent_ = mapCreate;
                window.minSize = new Vector2(300.0f, 300.0f);
                window.InitField();
                window.FieldInit();
                window.fieldMaxX_ = ToolMapCreate.fieldX_;
                window.fieldMaxY_ = ToolMapCreate.fieldY_;
                return window;

           }

            //フィールドの読み込み
            private void FieldInit()
            {
                GameObject parentDungenObj = GameObject.Find("FieldObject/Dungeon");
                //子供がいるかどうか
                if (parentDungenObj.transform.childCount == 0) return;

                Field[] chiledDungenObj = parentDungenObj.GetComponentsInChildren<Field>();



                foreach(var chiled in chiledDungenObj)
                {
                    field_[chiled.toolPosX, chiled.toolPosY].type_ = chiled.type;
                    field_[chiled.toolPosX, chiled.toolPosY].filePath_ = chiled.imgPath;
                    field_[chiled.toolPosX, chiled.toolPosY].obj_ = chiled.gameObject;
                    field_[chiled.toolPosX, chiled.toolPosY].objectpath_ = "FieldObject/Dungeon/" + chiled.name;
                }
            }

            //フィールドのパス
            private string fieldPath_ = "Assets/Sub/Prefab/Field";
            //フィールド_Convexパス
            private string fieldConvexPath_ = "/Field_Convex";
            //フィールド_Cornerパス
            private string fieldConerPath_ = "/Field_Corner";
            //フィールド_Corridorパス
            private string fieldCorridorPath_ = "/Field_Corridor";
            //フィールド_CrossRoadパス
            private string fieldCrossRoadPath_ = "/Field_CrossRoad";
            //フィールド_EndOfRoadパス
            private string fieldEndOfRoaddPath_ = "/Field_EndOfRoad";
            //フィールドlist
            private List<GameObject> fieldObj_ = new List<GameObject>();

            //Postのパス
            private string postPath_ = "Assets/Sub/Prefab/Field/Post/post.prefab";

            //フィールド初期化
            private void InitField()
            {
                for(int cntX = 0; cntX < fieldMaxX_; cntX++)
                {
                    for(int cntY = 0; cntY < fieldMaxY_; cntY++)
                    {
                        field_[cntX, cntY].type_ = Field.FIELD_TYPE.FIELD_NONE;
                    }
                }

                FieldLoad(fieldConerPath_);
                FieldLoad(fieldConvexPath_);
                FieldLoad(fieldCorridorPath_);               
                FieldLoad(fieldEndOfRoaddPath_);
                FieldLoad(fieldCrossRoadPath_);
            }

            //フィールド読み込み
            private void FieldLoad(string filePath)
            {
                //フィールドConvexの読み込み
                string[] filePaths = Directory.GetFiles(fieldPath_ + filePath, "*.prefab");
                foreach(var name in filePaths)
                {
                    GameObject obj = AssetDatabase.LoadAssetAtPath(name, typeof(GameObject)) as GameObject;
                    if(obj != null)
                    {
                        fieldObj_.Add(obj);
                    }
                }
            }

            static float time = 0.0f;
            //グリッド描画
            private void DrawGird()
            {
                for (int nCnt = 0; nCnt < fieldMaxX_; nCnt++)
                {
                    //縦
                    Handles.DrawLine(

                        new Vector2((float)nCnt * 30, 0.0f),

                        new Vector2((float)nCnt * 30, 300.0f));
                    if (nCnt == fieldMaxX_ - 1)
                    {
                        nCnt++;
                        //縦
                        Handles.DrawLine(

                            new Vector2((float)nCnt * 30, 0.0f),

                            new Vector2((float)nCnt * 30, 300.0f));

                    }
                }
                for (int nCnt = 0; nCnt < fieldMaxY_; nCnt++)
                {

                    //横
                    Handles.DrawLine(

                        new Vector2(0.0f, (float)nCnt * 30.0f),

                        new Vector2(300.0f, (float)nCnt * 30.0f));

                    if (nCnt == fieldMaxY_ - 1)
                    {
                        nCnt++;
                        //縦
                        Handles.DrawLine(

                            new Vector2((float)nCnt * 30, 0.0f),

                            new Vector2((float)nCnt * 30, 300.0f));

                        //横
                        Handles.DrawLine(

                            new Vector2(0.0f, (float)nCnt * 30.0f),

                            new Vector2(300.0f, (float)nCnt * 30.0f));
                    }
                }
            }
            //テクスチャ描画
           private void DrawTexture()
            {

                for (int cntX = 0; cntX < fieldMaxX_; cntX++)
                {
                    for(int cntY = 0; cntY < fieldMaxY_; cntY++)
                    {
                        if (field_[cntX, cntY].type_ == Field.FIELD_TYPE.FIELD_NONE) continue;
                        Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(field_[cntX,cntY].filePath_, typeof(Texture2D));

                        GUI.DrawTexture(new Rect(new Vector2((float)cntX * 30.0f, (float)cntY * 30.0f), new Vector2(30.0f, 30.0f)), tex);

                        
                    }
                }
               

            }
            //モデル削除
            private void DeleteModel(int posX,int posY)
            {
                Vector3 pos = new Vector3((float)posX * 8.0f, 0.0f, (10.0f - (float)posY * 8.0f));
                GameObject parentDungenObj = GameObject.Find("FieldObject/Dungeon");
  

                if (field_[posX, posY].type_ == Field.FIELD_TYPE.FIELD_NONE) return;

                GameObject.DestroyImmediate(field_[posX, posY].obj_);

                field_[posX, posY].type_ = Field.FIELD_TYPE.FIELD_NONE;

            }
            //モデル描画
           private void DrawModel(int posX,int posY)
            {

               Vector3 pos = new Vector3((float)posX * 8.0f, 0.0f, (10.0f - (float)posY * 8.0f));
                GameObject parentDungenObj = GameObject.Find("FieldObject/Dungeon");
                GameObject obj = null;
                //すでに指定されていたら
                if (field_[posX,posY].type_ != Field.FIELD_TYPE.FIELD_NONE)
                {
                    
                    int type = (int)field_[posX, posY].type_;

                    GameObject.DestroyImmediate(field_[posX, posY].obj_);
                    GameObject searchObj = null;
                    foreach (var objs in fieldObj_)
                    {
                        if (parent_.Type == objs.GetComponent<Field>().type)
                        {
                            searchObj = objs.gameObject;
                            break;
                        }
                    }
                    obj = Instantiate(searchObj);
                    obj.transform.localPosition = pos;
                    field_[posX, posY].objectpath_ = "FieldObject/Dungeon/" + obj.name;
                    obj.transform.parent = parentDungenObj.transform;

                }
                else
                {
                    GameObject searchObj = null;
                    foreach(var objs in fieldObj_)
                    {
                        if(parent_.Type == objs.GetComponent<Field>().type)
                        {
                            searchObj = objs.gameObject;
                            break;
                        }
                    }
                    obj = Instantiate(searchObj);
                    obj.transform.localPosition = pos;
                    obj.transform.parent = parentDungenObj.transform;

                    field_[posX, posY].objectpath_ = "FieldObject/Dungeon/" + obj.name;
                }
                field_[posX, posY].type_ = parent_.Type;
                field_[posX, posY].filePath_ = parent_.SelectImagePath;
                field_[posX, posY].obj_ = obj;
                field_[posX, posY].obj_.GetComponent<Field>().toolPosX = posX;
                field_[posX, posY].obj_.GetComponent<Field>().toolPosY = posY;
                field_[posX, posY].obj_.GetComponent<Field>().imgPath = parent_.SelectImagePath;

            }
            

            //マウスアップデート
           private void MouseUpdate()
            {
                Event e = Event.current;
                Vector2 mousePos = e.mousePosition;

                
                if(e.type == EventType.MouseDown)
                {
                    //上限を超えたら
                    if (mousePos.x >= fieldMaxX_ * 30.0f ||mousePos.y >= fieldMaxY_ * 30.0f) return;
                    int x = FieldPosX(mousePos.x);
                    int y = FieldPosY(mousePos.y);

                    DrawModel(x, y);
                }
                else if(e.type == EventType.ContextClick)
                {
                    //上限を超えたら
                    if (mousePos.x >= fieldMaxX_ * 30.0f || mousePos.y >= fieldMaxY_ * 30.0f) return;
                    int x = FieldPosX(mousePos.x);
                    int y = FieldPosY(mousePos.y);

                    DeleteModel(x, y);
                }
            }
            private void KeyUpdate()
            {
                Event e = Event.current;
                if(e.type == EventType.KeyDown)
                {
                    //左右
                    if(e.keyCode == KeyCode.RightArrow)
                    {
                        cameraPos_.x-= 0.5f;
                    }
                    else if (e.keyCode == KeyCode.LeftArrow)
                    {
                        cameraPos_.x += 0.5f;
                    }
                    //上下
                    if (e.keyCode == KeyCode.UpArrow)
                    {
                        cameraPos_.z += 0.5f;
                    }
                    else if (e.keyCode == KeyCode.DownArrow)
                    {
                        cameraPos_.z -= 0.5f;
                    }
                }
            }

            //フィールドのpositionX返し
            private int FieldPosX(float posX)
            {
                int x = 0;

                x = Mathf.RoundToInt(Mathf.Abs(((((Mathf.Abs(15.0f - posX) + 30.0f) * fieldMaxX_) / 30.0f) / fieldMaxX_))) - 1;
                return x;
            }
            //フィールドのpositionY返し
            private int FieldPosY(float posY)
            {
                int y = 0;

                y = Mathf.RoundToInt(Mathf.Abs(((((Mathf.Abs(15.0f + posY) + 30.0f) * fieldMaxY_) / 30.0f) / fieldMaxY_))) - 2;
                return y;
            }

            private void CreateDijkstr()
            {
                GameObject parentDungenObj = GameObject.Find("Post");

                GameObject obj = AssetDatabase.LoadAssetAtPath(postPath_, typeof(GameObject)) as GameObject;


                int cntID = 1;
                for (int cntX = 0; cntX < fieldMaxX_; cntX++)
                {
                    for (int cntY = 0; cntY < fieldMaxY_; cntY++)
                    {
                        if (field_[cntX, cntY].type_ == Field.FIELD_TYPE.FIELD_NONE) continue;

                        GameObject localObject = field_[cntX, cntY].obj_;

                        GameObject post = Instantiate(obj, localObject.transform.position, Quaternion.identity);

                        Link link = post.GetComponent<Link>();

                        field_[cntX, cntY].link_ = link;

                        link.LinkID = cntID;
                        cntID++;

                        post.transform.SetParent(parentDungenObj.transform);
                    }
                }

                //link設定
                for (int cntX = 0; cntX < fieldMaxX_; cntX++)
                {
                    for (int cntY = 0; cntY < fieldMaxY_; cntY++)
                    {
                        if (field_[cntX, cntY].type_ == Field.FIELD_TYPE.FIELD_NONE) continue;
                        DijkstraFix(field_[cntX, cntY].type_, cntX, cntY);

                    }
                }
            }

            private void DijkstraLink(INDEX index,int indexX,int indexY)
            {
                if (field_[indexX, indexY].link_.LinkObject.Length == 0)
                {
                    GameObject[] objs = new GameObject[1];

                    objs[0] = field_[index.indexX, index.indexY].link_.gameObject;

                    field_[indexX, indexY].link_.LinkObject = objs;
                }
                else
                {
                    GameObject[] objs = new GameObject[field_[indexX, indexY].link_.LinkObject.Length + 1];

                    if (index.indexX == -1 || index.indexY == -1) return;

                    int cnt = 0;
                    foreach (var links in field_[indexX, indexY].link_.LinkObject)
                    {
                        objs[cnt] = links;
                        cnt++;
                    }

                    objs[cnt] = field_[index.indexX, index.indexY].link_.gameObject;
                    field_[indexX, indexY].link_.LinkObject = objs;
                }

            }

            private void DijkstraFix(Field.FIELD_TYPE type,int indexX,int indexY)
            {
                INDEX index;
                index.indexX = 0;
                index.indexY = 0;
                switch (type)
                {
                    case Field.FIELD_TYPE.FIELD_CONVEX_DOWN:
                        //右
                        if(indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CONVEX_LEFT:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CONVEX_RIGHT:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CONVEX_UP:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORNER_LEFTDOWN:
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORNER_LEFTUP:
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORNER_RIGHTDOWN:
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORNER_RIGHTUP:
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORRIDOR_LEFTRIGHT:
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CORRIDOR_UPDOWN:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_CROSSROAD:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_ENDOFROAD_DOWN:
                        //下
                        if (indexY + 1 < fieldMaxY_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, 1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_ENDOFROAD_LEFT:
                        //左
                        if (indexX - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, -1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_ENDOFROAD_RIGHT:
                        //右
                        if (indexX + 1 < fieldMaxX_)
                        {
                            index = GetNextDijkStra(indexX, indexY, 1, 0);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                    case Field.FIELD_TYPE.FIELD_ENDOFROAD_UP:
                        //上
                        if (indexY - 1 >= 0)
                        {
                            index = GetNextDijkStra(indexX, indexY, 0, -1);
                            DijkstraLink(index, indexX, indexY);
                        }
                        break;
                }

                
            }

            private INDEX GetNextDijkStra(int indexX,int indexY,int addIndexX,int addIndexY)
            {
                INDEX returnIndex;

                int signX = System.Math.Sign(addIndexX);
                int signY = System.Math.Sign(addIndexY);

                if (signX == 1)
                {
                    if (indexX + addIndexX >= fieldMaxX_) returnIndex.indexX = -1;
                    else returnIndex.indexX = indexX + addIndexX;
                }
                else
                {
                    if (indexX + addIndexX < 0) returnIndex.indexX = -1;
                    else returnIndex.indexX = indexX + addIndexX;
                }

                if (signY == 1)
                {
                    if (indexY + addIndexY >= fieldMaxY_) returnIndex.indexY = -1;
                    else returnIndex.indexY = indexY + addIndexY;
                }
                else
                {
                    if (indexY + addIndexY < 0) returnIndex.indexY = -1;
                    else returnIndex.indexY = indexY + addIndexY;
                }

                return returnIndex;
            }

            private void OnGUI()
            {
                Repaint();
                KeyUpdate();
                MouseUpdate();
                DrawGird();
                DrawTexture();

                EditorGUILayout.BeginVertical();

                GUILayout.FlexibleSpace();
                //ダイクストラを設置
                if (GUILayout.Button("Create editor"))
                {
                    CreateDijkstr();
                }

                EditorGUILayout.EndVertical();
            }
            
        }
#endif
    }
}
