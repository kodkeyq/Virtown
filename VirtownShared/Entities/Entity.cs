using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VirtownShared.Global;
using System.IO;
using VirtownShared.Atlas;

namespace VirtownShared.Entities
{
    public class Entity
    {
        private EntityData _data;
        private readonly Point[] _isoDirectionSize = new Point[4];


        public DirectionEnum Direction { get; private set; }
        public Point IsoDirectionSize { get { return _isoDirectionSize[(byte)Direction]; } }
        public Point IsoLocation { get; private set; }
        public int IsoSizeZ { get { return _data.IsoSizeZ; } }

        public Entity(EntityData data, Point isoLocation, DirectionEnum direction)
        {
            _data = data;
            _isoDirectionSize[(byte)DirectionEnum.PlusX] = data.IsoSize;
            _isoDirectionSize[(byte)DirectionEnum.MinusY] = new Point(data.IsoSize.Y, data.IsoSize.X);
            _isoDirectionSize[(byte)DirectionEnum.MinusX] = data.IsoSize;
            _isoDirectionSize[(byte)DirectionEnum.PlusY] = new Point(data.IsoSize.Y, data.IsoSize.X);
            IsoLocation = isoLocation;
            Direction = direction;
        }
        public virtual void SetLocation(Point isoLocation) { IsoLocation = isoLocation; }
        public virtual Point GetSprite(Point isoCall, int isoCallZ)
        {
            if (isoCall.X >= 0 && isoCall.Y >= 0 && isoCall.X < IsoDirectionSize.X && isoCall.Y < IsoDirectionSize.Y)
            {
                return _data.Map[(byte)Direction, 0, isoCall.X, isoCall.Y, isoCallZ];
            }
            else
            {
                return new Point(0, 0);
            }
        }

        public virtual void Pack()
        {

        }
    }
}
