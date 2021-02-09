using System;
using System.Collections.Generic;
using System.Text;

namespace VirtownShared.Global
{
    public enum DirectionEnum : byte
    {
        PlusX = 0,
        MinusY = 1,
        MinusX = 2,
        PlusY = 3
    }

    public enum EntityTypeEnum : byte
    {
        Empty,
        Entity,
        Item,
        Wall,
        Furniture,
        Person
    }
}
