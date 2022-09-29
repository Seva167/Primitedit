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
            Stone,
            Wood,
            Iron,
            Grass,
            Leaf,
            Slime,
            CookedSlime,
            Pyrite,
            RedSlime,
            CookedRedSlime,
            Monument,
            Hematite,
            Wheat,
            WheatStalk,
            DryGrass,
            Bread,
            AncientAlloy,
            AncientPlastic,
            AncientLightweightPlastic,
            AncientEngine,
            Gold,
            Silver,
            Clay,
            Brick,
            Cactus,
            Niter,
            Sulfur,
            Gunpowder,
            GreenSlime,
            Apple,
            Pinecone,
            Rubberwood,
            RubberSeed,
            RawRubber,
            Rubber,
            ConiferWood,
            Ice,
            QuartzSand,
            Glass,
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
}
