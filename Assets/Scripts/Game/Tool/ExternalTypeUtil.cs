using cfg;
using UnityEngine;

namespace Game.Tool
{
    /// <summary>
    /// Luban 类型转化工具
    /// </summary>
    public static class ExternalTypeUtil
    {
        public static Vector3 NewVector3(vector3 ve)
        {
            return new Vector3(ve.X, ve.Y, ve.X);
        }
    
        public static Vector2 NewVector2(vector2 ve)
        {
            return new Vector2(ve.X, ve.Y);
        }
    }
}