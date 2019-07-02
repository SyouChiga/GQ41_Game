using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{ 
 public class Field : MonoBehaviour
 {
     public enum FIELD_TYPE
     {
        FIELD_NONE = -1,
        FIELD_CONVEX_UP,
        FIELD_CONVEX_DOWN,
        FIELD_CONVEX_RIGHT,
        FIELD_CONVEX_LEFT,
        FIELD_CORNER_LEFTUP,
        FIELD_CORNER_LEFTDOWN,
        FIELD_CORNER_RIGHTUP,
        FIELD_CORNER_RIGHTDOWN,
        FIELD_CORRIDOR_UPDOWN,
        FIELD_CORRIDOR_LEFTRIGHT,
        FIELD_ENDOFROAD_UP,
        FIELD_ENDOFROAD_DOWN,
        FIELD_ENDOFROAD_RIGHT,
        FIELD_ENDOFROAD_LEFT,
        FIELD_CROSSROAD,
        FIELD_MAX
     }
     public FIELD_TYPE type;
     // Start is called before the first frame update
     void Start()
     {
 
     }
 
     // Update is called once per frame
     void Update()
     {
 
     }
 }
}
