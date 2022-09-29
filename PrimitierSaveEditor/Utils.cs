using SharpDX;
using System;
using System.Collections.Generic;
using System.Text;
using PrimitierSaveEditor.Entities;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Diagnostics;

namespace PrimitierSaveEditor
{
    public static class Utils
    {
        private static Dictionary<string, HelixToolkit.Wpf.SharpDX.Material> SubstanceMaterials { get; } = new Dictionary<string, HelixToolkit.Wpf.SharpDX.Material>();
        private static Dictionary<string, Color4> BiomeColors { get; } = new Dictionary<string, Color4>();

        static Utils()
        {
            string colorsJson = File.ReadAllText("Colors\\substanceColors.json");
            Dictionary<string, Color4> substanceColors = JsonConvert.DeserializeObject<Dictionary<string, Color4>>(colorsJson);
            foreach (var item in substanceColors)
                SubstanceMaterials.Add(item.Key, new HelixToolkit.Wpf.SharpDX.DiffuseMaterial { DiffuseColor = item.Value, EnableFlatShading = true });
            SubstanceMaterials.Add("Invalid", new HelixToolkit.Wpf.SharpDX.DiffuseMaterial { DiffuseColor = new Color4(1, 0, 1, 1), EnableUnLit = true, EnableFlatShading = true });

            string biomesJson = File.ReadAllText("Colors\\biomeColors.json");
            BiomeColors = JsonConvert.DeserializeObject<Dictionary<string, Color4>>(biomesJson);
        }

        public static Color4 GetColorForBiome(SaveData.TerrainData.Mat biome)
        {
            return BiomeColors.TryGetValue(biome.ToString(), out Color4 res) ? res : new Color4(1, 0, 1, 1);
        }

        public static HelixToolkit.Wpf.SharpDX.Material GetMaterialForSubstance(CubeData.Substance substance)
        {
            return SubstanceMaterials.TryGetValue(substance.ToString(), out HelixToolkit.Wpf.SharpDX.Material res) ? res : SubstanceMaterials["Invalid"];
        }

        public static object ConvString(string str, Type type) => str;

        public static object ConvFloat(string str, Type type)
        {
            return float.Parse(str);
        }

        public static object ConvInt(string str, Type type)
        {
            return int.Parse(str);
        }

        public static object ConvBool(string str, Type type)
        {
            return bool.Parse(str);
        }

        public static object ConvEnum(string str, Type type)
        {
            return Enum.Parse(type, str);
        }

        public static object ConvVector3(string str, Type type)
        {
            string[] numberStrings = str.Split(' ');

            float[] numbers = (from num in numberStrings
                               select float.Parse(num)).ToArray();

            return new Entities.Vector3(numbers[0], numbers[1], numbers[2]);
        }

        public static object ConvQuaternion(string str, Type type)
        {
            return EulerToQuaternion((Entities.Vector3)ConvVector3(str, type));
        }

        public static object ConvUvOffset(string str, Type type)
        {
            string[][] offsetStrs = (from line in str.Split('\n')
                                     select line[3..].Split(' ')).ToArray();

            Entities.Vector2[] offsets = (from ostr in offsetStrs
                                 select new Entities.Vector2(float.Parse(ostr[0]), float.Parse(ostr[1]))).ToArray();

            return new CubeData.UVOffset
            {
                right = offsets[0],
                left = offsets[1],
                top = offsets[2],
                bottom = offsets[3],
                front = offsets[4],
                back = offsets[5]
            };
        }

        public static Vector3D CubePosToWorldPos(Vector3D pos, Vector3D groupPos, Vector3D chunkPos)
        {
            return pos + groupPos + chunkPos;
        }

        public static Vector3D WorldPosToCubePos(Vector3D pos, Vector3D groupPos, Vector3D chunkPos)
        {
            return pos - groupPos - chunkPos;
        }

        private const float rad2Deg = (float)(180 / Math.PI);
        private const float deg2Rad = (float)(Math.PI / 180);

        public static Entities.Quaternion EulerToQuaternion(Entities.Vector3 v)
        {
            v.x *= deg2Rad;
            v.y *= deg2Rad;
            v.z *= deg2Rad;

            float cy = (float)Math.Cos(v.z * 0.5);
            float sy = (float)Math.Sin(v.z * 0.5);
            float cp = (float)Math.Cos(v.y * 0.5);
            float sp = (float)Math.Sin(v.y * 0.5);
            float cr = (float)Math.Cos(v.x * 0.5);
            float sr = (float)Math.Sin(v.x * 0.5);

            return new Entities.Quaternion
            {
                w = cr * cp * cy + sr * sp * sy,
                x = sr * cp * cy - cr * sp * sy,
                y = cr * sp * cy + sr * cp * sy,
                z = cr * cp * sy - sr * sp * cy
            };

        }

        public static Entities.Vector3 QuaternionToEuler(Entities.Quaternion q)
        {
            Entities.Vector3 angles;

            double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            angles.x = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            double sinp = 2 * (q.w * q.y - q.z * q.x);
            if (Math.Abs(sinp) >= 1)
            {
                angles.y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.y = (float)Math.Asin(sinp);
            }

            double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            angles.z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            angles.x *= rad2Deg;
            angles.y *= rad2Deg;
            angles.z *= rad2Deg;

            return angles;
        }

        public static void OpenLink(string url)
        {
            Process link = new Process();
            link.StartInfo.FileName = url;
            link.StartInfo.UseShellExecute = true;
            link.Start();
        }
    }
}
