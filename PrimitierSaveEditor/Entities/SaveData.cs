using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Entities
{
    [Serializable]
    public class SaveData
    {
        public SaveEditorMetadata saveEditorMetadata;

        public int[] version;

        public bool isCreativeMode;

        public int seed;

        public float terrainHorizontalScale = 1f;

        public float terrainVerticalScale = 1f;

        public float time;

        public Vector3 playerPos;

        public float playerAngle;

        public float playerMaxLife;

        public float playerLife;

        public Vector3 respawnPos;

        public float respawnAngle;

        public Vector3 cameraPos;

        public Quaternion cameraRot;

        public Vector3[] holsterPositions;

        public List<ChunkData> chunks;

        public List<TerrainData> terrains;

        [Serializable]
        public class ChunkData
        {
            public int x;

            public int z;

            public List<GroupData> groups;

            [Serializable]
            public class GroupData
            {
                public Vector3 pos;

                public Quaternion rot;

                public List<CubeData> cubes;

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("Position: ");
                    sb.AppendLine(pos.ToString());
                    sb.Append("Rotation: ");
                    sb.AppendLine(rot.ToString());
                    sb.Append("Cubes count: ");
                    sb.AppendLine(cubes.Count.ToString());

                    return sb.ToString();
                }
            }
        }

        [Serializable]
        public class TerrainData
        {
            public int x;

            public int z;

            public bool generated;

            public bool materialGenerated;

            public bool skyGenerated;

            public List<SkyTerrainData> skyTerrains;

            public int[] heightMap;

            public Mat[] materialMap;

            public int[] temperatureMap;

            public int[] rainfallMap;

            public enum Mat
            {
                Meadow = 0,
                Sand = 1,
                Stone = 2,
                Snow = 3,
                ColdMeadow = 4,
                DryMeadow = 5,
                Sulfur = 6,
                Rainforest = 7,
                Fort = 8
            }
        }

        [Serializable]
        public class SkyTerrainData
        {
            public Type type;

            public Vector3 pos;

            public Vector3 scale;

            public List<Vector2Int> ungeneratedChunks;

            public enum Type
            {
                Island,
                Moon
            }
        }
    }
}
