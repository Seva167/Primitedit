using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Entities
{
    public struct Quaternion
    {
        public float x, y, z, w;

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion Identity { get => new Quaternion(0, 0, 0, 1); }

        public override string ToString()
        {
            Vector3 angles = Utils.QuaternionToEuler(this);
            return $"{angles.x} {angles.y} {angles.z}";
        }

        public static implicit operator System.Windows.Media.Media3D.Quaternion(Quaternion quat)
        {
            return new System.Windows.Media.Media3D.Quaternion(quat.x, quat.y, quat.z, quat.w);
        }
    }
}
