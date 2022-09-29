using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities
{
    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"{x} {y} {z}";
        }

        public static implicit operator Vector3D(Vector3 vec)
        {
            return new Vector3D(vec.x, vec.y, vec.z);
        }

        public static implicit operator Vector3(Vector3D vec)
        {
            return new Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
        }
    }
}
