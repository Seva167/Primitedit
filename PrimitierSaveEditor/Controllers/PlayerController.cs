using PrimitierSaveEditor.Entities.Primitier;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierSaveEditor.Controllers
{
    public static class PlayerController
    {
        public static PrimitierPlayer Player { get; private set; }
        public static PrimitierRespawnPos RespawnPos { get; private set; }
        public static PrimitierCamera Camera { get; private set; }

        public static PrimitierPlayer CreatePlayer()
        {
            Player = new PrimitierPlayer();
            return Player;
        }

        public static PrimitierRespawnPos CreateRespawnPos()
        {
            RespawnPos = new PrimitierRespawnPos();
            return RespawnPos;
        }

        public static PrimitierCamera CreateCamera()
        {
            Camera = new PrimitierCamera();
            return Camera;
        }
    }
}
