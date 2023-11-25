using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace PrimitierSaveEditor.Entities
{
    [Serializable]
    public class SaveEditorMetadata
    {
        public Version version;

        public Point3D cameraPos;

        public Vector3D cameraDir;
    }
}
