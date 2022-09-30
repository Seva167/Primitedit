﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Entities
{
    [Serializable]
    public class SaveData
    {
        public SaveEditorMetadata saveEditorMetadata;

        public int[] version;

        public int seed;

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

            public int[] heightMap;

            public Mat[] materialMap;

            public int[] temperatureMap;

            public int[] rainfallMap;

            public enum Mat
            {
                Meadow,
                Sand,
                Stone,
                Snow,
                ColdMeadow,
                DryMeadow,
                Sulfur,
                Rainforest
            }
        }
    }
}
