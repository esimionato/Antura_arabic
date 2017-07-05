﻿using System.Collections;
using System.Collections.Generic;
using Antura.Database;

namespace Antura.Core
{
    public class MainMiniGame
    {
        public string id;
        public List<MiniGameInfo> variations;

        public string GetIconResourcePath()
        {
            return variations[0].data.GetIconResourcePath();
        }

        public MiniGameCode GetFirstVariationMiniGameCode()
        {
            return variations[0].data.Code;
        }
    }
}