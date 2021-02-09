using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VirtownShared.Atlas;
using VirtownShared.Entities;
using VirtownShared.Global;
using VirtownShared.Worlds;

namespace Virtown.Render
{
    public class Renderer
    {
        private SpriteBatch _spriteBatch;
        private World _world;
        private const int _isoSizeZ = Constants.EntityMaxIsoSizeZ;

        public Renderer(SpriteBatch spriteBatch, World world)
        {
            _spriteBatch = spriteBatch;
            _world = world;
        }


        public void RenderFrame(Point camera)
        {
            for (int i = 0; i < _world.IsoSize.X; i++)
            {
                for (int j = 0; j < _world.IsoSize.Y; j++)
                {
                    
                }
            }
        }
    }
}
