using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ANode
{
    [Description("坐标X")]
    public int x;
    [Description("坐标Y")]
    public int y;

    public int offset;
    public bool canReached;
    public ANode parentNode = null;

    [Description("从该点到终点预估的代价")]
    public int H { get; set; }
    [Description("起点到该点消耗的代价")]
    public int G { get; set; }
    [Description("F=H+G,走到终点消耗的代价值")]
    public int F { get; set; }

    public ANode(int x, int y, bool canReached = true, int offset = 1)
    {
        this.x = x;
        this.y = y;
        this.canReached = canReached;
        this.offset = offset;
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
