using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Entities
{
    [Serializable]
    public class CubeData
    {
        public Vector3 pos;

        public Quaternion rot;

        public Vector3 scale;

        public float lifeRatio;

        public Anchor anchor;

        public Substance substance;

        public CubeName name;

        public List<int> connections;

        public float temperature;

        public bool isBurning;

        public float burnedRatio;

        public SectionState sectionState;

        public UVOffset uvOffset;

        public List<string> behaviors;

        public List<string> states;

        public enum Anchor
        {
            Free,
            Temporary,
            Permanent
        }

        public enum Substance
        {
            Stone = 0,
            Wood = 1,
            Iron = 2,
            Grass = 3,
            Leaf = 4,
            Slime = 5,
            CookedSlime = 6,
            Pyrite = 7,
            RedSlime = 8,
            CookedRedSlime = 9,
            Monument = 10,
            Hematite = 11,
            Wheat = 12,
            WheatStalk = 13,
            DryGrass = 14,
            Bread = 15,
            AncientAlloy = 16,
            AncientPlastic = 17,
            AncientDrone = 18,
            AncientEngine = 19,
            Gold = 20,
            Silver = 21,
            Clay = 22,
            Brick = 23,
            Cactus = 24,
            Niter = 25,
            Sulfur = 26,
            Gunpowder = 27,
            GreenSlime = 28,
            Apple = 29,
            Pinecone = 30,
            Rubberwood = 31,
            RubberSeed = 32,
            RawRubber = 33,
            BouncyRubber = 34,
            ConiferWood = 35,
            Ice = 36,
            QuartzSand = 37,
            Glass = 38,
            Helium = 39,
            TungstenOre = 40,
            Tungsten = 41,
            SolarCell = 42,
            LED = 43,
            ElectricMotor = 44,
            Battery = 45,
            AncientLightweightPlastic = 46,
            YellowSlime = 47,
            CookedYellowSlime = 48,
            RepairFiller = 49,
            AncientSuicideDrone = 50,
            BossCore = 51,
            MixedAcid = 52,
            Nitrocellulose = 53,
            RocketEngine = 54,
            MoonRock = 55,
            MoonMonument = 56,
            Transistor = 57,
            TriggerSwitch = 58,
            Quartz = 59,
            Rubber = 60,
            OreDeposit = 61
        }

        public enum SectionState
        {
            Right = 1,
            Left = 2,
            Top = 4,
            Bottom = 8,
            Front = 0x10,
            Back = 0x20
        }

        public struct UVOffset
        {
            public Vector2 right;

            public Vector2 left;

            public Vector2 top;

            public Vector2 bottom;

            public Vector2 front;

            public Vector2 back;

            public override string ToString()
            {
                return $"RT:{right}\nLT:{left}\nTP:{top}\nBM:{bottom}\nFT:{front}\nBK:{back}";
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Position: ");
            sb.AppendLine(pos.ToString());
            sb.Append("Rotation: ");
            sb.AppendLine(rot.ToString());
            sb.Append("Scale: ");
            sb.AppendLine(scale.ToString());
            sb.Append("Life Ratio: ");
            sb.AppendLine(lifeRatio.ToString());
            sb.Append("Anchor: ");
            sb.AppendLine(anchor.ToString());
            sb.Append("Substance: ");
            sb.AppendLine(substance.ToString());
            sb.Append("Connection count: ");
            sb.AppendLine(connections.Count.ToString());
            sb.Append("Temperature: ");
            sb.AppendLine(temperature.ToString());
            sb.Append("Is Burning: ");
            sb.AppendLine(isBurning.ToString());
            sb.Append("Burned ratio: ");
            sb.AppendLine(burnedRatio.ToString());
            sb.Append("Section state: ");
            sb.AppendLine(sectionState.ToString());
            sb.Append("UV Offset: ");
            sb.AppendLine("  ");
            sb.AppendLine(uvOffset.ToString());
            sb.Append("Behaviour count: ");
            sb.AppendLine(behaviors.Count.ToString());
            sb.Append("State count: ");
            sb.AppendLine(states.Count.ToString());

            return sb.ToString();
        }
    }

    public enum CubeName
    {
        None,
        RespawnPoint,
        BeamTurret,
        HomingBeamTurret,
        DroneSpawner,
        SuicideDroneSpawner,
        BearingOuter,
        BearingAxis,
        EngineBody,
        EngineAxis,
        ElectricMotorBody,
        ElectricMotorAxis,
        SlimeAlive,
        RedSlimeAlive,
        GreenSlimeAlive,
        YellowSlimeAlive,
        TransistorNormal,
        TransistorInverted
    }
}
