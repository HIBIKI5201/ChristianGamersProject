using System;

[Flags]
public enum LayersEnum : int
{
    None = 1 << 0,
    Default = 1 << 1,
    TransparentFX = 1 << 2,
    Player = 1 << 3,
    Water = 1 << 4,
    UI = 1 << 5,
    Ground = 1 << 6,
}
