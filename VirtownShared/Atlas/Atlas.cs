using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VirtownShared.Global;

namespace VirtownShared.Atlas
{
    public static partial class Atlas
    {
        private static RenderTarget2D _atlas;
        private static GraphicsDevice _graphicsDevice;
        private static SpriteBatch _spriteBatch;
        private static Texture2D _mask;
        private static AlphaTestEffect _maskEffect;
        private static DepthStencilState _maskState;
        private static DepthStencilState _spriteState;
        private static bool _begin = false;
        private static Point _spriteIndex;
        public static void Begin(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            _atlas = new RenderTarget2D(graphicsDevice,
                Constants.SpriteIndexMax * Constants.Grid, Constants.Grid * Constants.SpriteIndexMax,
                false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _graphicsDevice.SetRenderTarget(_atlas);
            _graphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Transparent, 0, 0);
            _spriteIndex = new Point(0, 0);
            NewMask();
            graphicsDevice.Clear(ClearOptions.Target, Color.Transparent, 0, 0);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, _spriteState, null, _maskEffect);
            _begin = true;
        }

        
        private static void NewMask()
        {
            _mask = new Texture2D(_graphicsDevice, Constants.Grid, Constants.Grid);
            Color[] maskData = new Color[Constants.Grid * Constants.Grid];

            for (int i = 0; i < Constants.Grid; i++)
            {
                for (int j = 0; j < Constants.Grid; j++)
                {
                    maskData[i + j * Constants.Grid] = MaskIn(i, j) ? Color.White : Color.Transparent;
                }
            }
            _mask.SetData(maskData);
            _maskEffect = new AlphaTestEffect(_graphicsDevice);
            _maskEffect.Projection = Matrix.CreateOrthographicOffCenter(0, _atlas.Width, _atlas.Height, 0, 0, 1);

            _maskState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };
            _spriteState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, _maskState, null, _maskEffect);
            for (int i = 0; i < Constants.SpriteIndexMax; i++)
            {
                for (int j = 0; j <Constants.SpriteIndexMax; j++)
                {
                    _spriteBatch.Draw(_mask, new Vector2(i * Constants.Grid, j * Constants.Grid), Color.White);
                }
            }
            _spriteBatch.End();
        }

        public static void End()
        {
            if (!_begin) { throw new Exception("Where is begin method?"); }
            else
            {
                _spriteBatch.End();
                _graphicsDevice.SetRenderTarget(null);
                _atlas = null;
                _graphicsDevice = null;
                _spriteBatch = null;
                _spriteState = null;
                _maskState = null;
                _mask = null;
                _maskEffect = null;
                _begin = false;
            }
        }

        public static RenderTarget2D GetAtlas()
        {
            if (!_begin) { throw new Exception("Where is begin method?"); }
            else
            {
                return _atlas;
            }
        }

        public static Point[,,,,] Map(Texture2D texture, Point nullPoint, Point isoSize, int isoSizeZ,int maxDirectionIndex, int maxAnimationIndex)
        {
            Point[,,,,] map = new Point[
            4,
            Constants.EntityMaxAnimationIndex,
            Constants.EntityMaxIsoSizeX,
            Constants.EntityMaxIsoSizeY,
            Constants.EntityMaxIsoSizeZ];

            if (!_begin) { throw new Exception("Where is begin method?"); }
            else
            {
                for (int i = 0; i < maxDirectionIndex; i++)
                {
                    Point isoDirectionSize = GetIsoDirectionSize(i, isoSize);
                    
                    for (int j = 0; j < maxAnimationIndex; j++)
                    {
                        Point newNullPoint = GetNullPoint(i, j, nullPoint, isoSize, isoSizeZ);
                        for (int k = 0; k < isoDirectionSize.X; k++)
                        {
                            for (int l = 0; l < isoDirectionSize.Y; l++)
                            {
                                for (int m = 0; m < isoSizeZ; m++)
                                {
                                    if (k == isoDirectionSize.X - 1 || l == isoDirectionSize.Y - 1 || m == isoSizeZ - 1)
                                    {
                                        map[i, j, k, l, m] = _spriteIndex;

                                        Point iso = GetTransformedIso(i, new Point(k, l));
                                        int x, y;
                                        Isometric.IsoToCart(iso.X, iso.Y, out x, out y);

                                        Rectangle srcRect = new Rectangle(x - Constants.GridH + newNullPoint.X, y - Constants.GridH + newNullPoint.Y - Constants.GridH * m,
                                            Constants.Grid, Constants.Grid);

                                        Vector2 position = new Vector2(_spriteIndex.X * Constants.Grid, _spriteIndex.Y * Constants.Grid);
                                        if (Flip(i))
                                        {
                                            _spriteBatch.Draw(texture, position, srcRect, 
                                                Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0);
                                        }
                                        else
                                        {
                                            _spriteBatch.Draw(texture, position, srcRect, Color.White);
                                        }



                                        _spriteIndex.X++;
                                        if (_spriteIndex.X >= Constants.SpriteIndexMax)
                                        {
                                            _spriteIndex.X = 0;
                                            _spriteIndex.Y++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return map;
        }
    }
}
