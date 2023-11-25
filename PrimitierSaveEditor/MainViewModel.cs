using HelixToolkit.Wpf.SharpDX;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrimitierSaveEditor
{
    public class MainViewModel
    {
        public EffectsManager EffectsManager { get; }

        public Camera Camera { get; }

        public static Geometry3D Cube { get; private set; }

        public static Geometry3D CameraGizmo { get; private set; }
        public static DiffuseMaterial CameraGizmoMat { get; private set; }

        public static DiffuseMaterial PlayerMat { get; private set; }
        public static DiffuseMaterial RespawnPosMat { get; private set; }

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera()
            {
                FieldOfView = 90,
                CreateLeftHandSystem = true
            };
            MeshBuilder mb = new MeshBuilder();
            mb.AddCube();
            Cube = mb.ToMesh();

            ObjReader rdr = new ObjReader();
            CameraGizmo = rdr.Read("Gizmos\\camera_gizmo.obj")[0].Geometry;
            CameraGizmoMat = new DiffuseMaterial
            {
                DiffuseColor = new SharpDX.Color4(1, 1, 0, 1),
                DiffuseMap = new TextureModel("Gizmos\\camera_gizmo_tex.png"),
                UVTransform = new UVTransform(0, -1)
            };

            PlayerMat = new DiffuseMaterial
            {
                DiffuseColor = new SharpDX.Color4(1, 0, 0, 1),
                DiffuseMap = new TextureModel("Gizmos\\player_tex.png"),
                EnableFlatShading = true
            };

            RespawnPosMat = new DiffuseMaterial
            {
                DiffuseColor = new SharpDX.Color4(0.5f, 1, 0.5f, 1),
                DiffuseMap = new TextureModel("Gizmos\\respawn_pos_tex.png"),
                EnableFlatShading = true
            };
        }
    }
}
