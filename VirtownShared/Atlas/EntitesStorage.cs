using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Global;
using VirtownShared.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace VirtownShared.Atlas
{
    public static class EntitiesStorage
    {
        public static RenderTarget2D Atlas2D;

        public static Point[,,,,] EmptyMap = new Point[
            4,
            Constants.EntityMaxAnimationIndex,
            Constants.EntityMaxIsoSizeX,
            Constants.EntityMaxIsoSizeY,
            Constants.EntityMaxIsoSizeZ];


        private static EntityData EmptyEntityData = new EntityData("empty", new Point(0, 0), 0, EmptyMap, EntityTypeEnum.Empty);
        private static Dictionary<string, EntityData> _storage = new Dictionary<string, EntityData>(); 
        private static bool _begin = false;
        private static ContentManager _contentManager;
        public static void Begin(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _begin = true;
            Atlas.Begin(graphicsDevice, spriteBatch);
        }

        public static void End()
        {
            if (!_begin) { throw new Exception("Where is begin method?"); }
            else
            {
                Atlas2D = Atlas.GetAtlas();
                Atlas.End();
                _begin = false;
                _contentManager = null;
            }
        }
        public static void NewEntityData(string name, int isoSizeX, int isoSizeY, int isoSizeZ, EntityTypeEnum type,
            string textureName, int nullX, int nullY, int maxDirectionIndex = 4, int maxAnimationIndex = 1)
        {
            if (!_begin) { throw new Exception("Where is begin method?"); }
            else
            {
                Texture2D texture = _contentManager.Load<Texture2D>(textureName);
                Point[,,,,] map = Atlas.Map(texture, new Point(nullX, nullY), new Point(isoSizeX, isoSizeY), isoSizeZ, maxDirectionIndex, maxAnimationIndex);

                EntityData entityData = new EntityData(name, new Point(isoSizeX, isoSizeY), isoSizeZ, map, type);
                _storage.TryAdd(name, entityData);
            }
        }

        public static EntityData GetEntityData(string name)
        {
            EntityData entityData;
            if (_storage.TryGetValue(name, out entityData))
            {
                return entityData;
            }
            else
            {
                return EmptyEntityData;
            }
        }
    }
}
