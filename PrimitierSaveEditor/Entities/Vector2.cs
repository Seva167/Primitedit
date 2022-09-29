using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Entities
{
    public struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{x} {y}";
        }
    }
}
