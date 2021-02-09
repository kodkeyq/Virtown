using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VirtownShared.Atlas
{
    public static partial class Atlas
    {
        private static Point GetIsoDirectionSize(int direction, Point isoSize)
        {
            if (direction == 1 || direction == 3)
            {
                isoSize = new Point(isoSize.Y, isoSize.X);
            }
            return isoSize;
        }

        private static bool Flip(int direction)
        {
            return direction == 2 || direction == 3;
        }

        private static Point GetTransformedIso(int direction, Point iso)
        {
            if (direction == 0 || direction == 1)
            {
                return iso;
            }
            else
            {
                return new Point(iso.Y, iso.X);
            }
        }

        private static Point GetNullPoint(int direction, int animation, Point nullPoint, Point isoSize, int isoSizeZ)
        {
            int ySh = ((isoSize.X + isoSize.Y) * Constants.GridQ + isoSizeZ * Constants.GridH) * animation;
            int xSh = 0;
            if (direction == 1 || direction == 2)
            {
                xSh = isoSize.Y * Constants.Grid;
            }
            nullPoint.X += xSh;
            nullPoint.Y += ySh;
            return nullPoint;
        }

        private static bool MaskIn(int i, int j)
        {
            int x0 = -2 * (j - Constants.GridQ + 1);
            int x1 = -2 * (j - Constants.Grid - Constants.GridQ ) - 1;
            int x2 = 2 * (j + Constants.GridQ + 1) - 1; 
            int x3 = 2 * (j - Constants.GridHQ);

            return i > 0 && i < 31 && i > x0 && i < x2 && i > x3 && i < x1;
        }
    }
}
