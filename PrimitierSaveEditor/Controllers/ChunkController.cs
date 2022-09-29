using PrimitierSaveEditor.Entities;
using PrimitierSaveEditor.Entities.Primitier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PrimitierSaveEditor.Controllers
{
    public static class ChunkController
    {
        public static PrimitierChunk[] CreateChunks()
        {
            PrimitierChunk[] chunks = new PrimitierChunk[SaveController.Save.chunks.Count];

            for (int k = 0; k < SaveController.Save.chunks.Count; k++)
            {
                PrimitierChunk chunk = new PrimitierChunk(SaveController.Save.chunks[k]);

                for (int g = 0; g < SaveController.Save.chunks[k].groups.Count; g++)
                {
                    var saveGroup = SaveController.Save.chunks[k].groups[g];

                    PrimitierGroup group = new PrimitierGroup(saveGroup, chunk);

                    chunk.Children.Add(group);

                    for (int c = 0; c < saveGroup.cubes.Count; c++)
                    {
                        CubeData saveCube = saveGroup.cubes[c];

                        PrimitierCube cube = new PrimitierCube(saveCube, group);

                        group.Children.Add(cube);
                    }
                }

                chunks[k] = chunk;
            }

            return chunks;
        }
    }
}
