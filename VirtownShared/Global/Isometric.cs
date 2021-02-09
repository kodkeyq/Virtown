using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirtownShared.Global
{
    public static class Isometric
    {
        public static Point IsoToCart(Point iso)
        {
            Point cart;
            cart.X = Constants.GridH * (iso.Y - iso.X);
            cart.Y = Constants.GridQ * (iso.Y + iso.X);
            return cart;
        }

        public static void IsoToCart(int isoX, int isoY, out int cartX, out int cartY)
        {
            cartX = Constants.GridH * (isoY - isoX);
            cartY = Constants.GridQ * (isoY + isoX);
        }

        public static Point IsoSmallToCart(Point isoSmall)
        {
            Point cart;
            cart.X = isoSmall.Y - isoSmall.X;
            cart.Y = (isoSmall.Y + isoSmall.X) / 2;
            return cart;
        }

        public static Point IsoToIsoSmall(Point iso)
        {
            Point isoSmall;
            isoSmall.X = iso.X * Constants.GridH;
            isoSmall.Y = iso.Y * Constants.GridH;
            return isoSmall;
        }

        public static Point IsoSmallToIso(Point isoSmall)
        {
            Point iso;
            iso.X = isoSmall.X / Constants.GridH;
            iso.Y = isoSmall.Y / Constants.GridH;
            return iso;
        }

        public static Point IsoSmallToIso(Point isoSmall, out Point isoRemainder)
        {
            Point iso;
            iso.X = isoSmall.X / Constants.GridH;
            iso.Y = isoSmall.Y / Constants.GridH;

            isoRemainder.X = isoSmall.X % Constants.GridH;
            isoRemainder.Y = isoSmall.Y % Constants.GridH;
            return iso;
        }

        public static void IsoToIsoSmall(int isoX, int isoY, out int isoSmallX, out int isoSmallY)
        {
            isoSmallX = isoX * Constants.GridH;
            isoSmallY = isoY * Constants.GridH;
        }

        public static void IsoSmallToCart(int isoSmallX, int isoSmallY, out int cartX, out int cartY)
        {
            cartX = isoSmallY - isoSmallX;
            cartY = (isoSmallY + isoSmallX) / 2;
        }

        public static Point CartToIso(Point cart)
        {
            Point iso;
            iso.X = (cart.Y - cart.X / 2) / Constants.GridH;
            iso.Y = (cart.Y + cart.X / 2) / Constants.GridH;
            return iso;
        }

        public static Point CartToIsoSmall(Point cart)
        {
            Point isoSmall;
            isoSmall.X = cart.Y - cart.X / 2;
            isoSmall.Y = cart.Y + cart.X / 2;
            return isoSmall;
        }
    }
}
