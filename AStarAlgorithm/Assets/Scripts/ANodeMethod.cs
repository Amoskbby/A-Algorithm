using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ANodeMethod
{
    public static void CalHValue(this ANode node, ANode targetNode, int offset = 1, int unitPrice = 1)
    {
        int xDistance = Mathf.Abs(targetNode.x - node.x) / offset;
        int yDistance = Mathf.Abs(targetNode.y - node.y) / offset;

        node.H = (xDistance + yDistance) * unitPrice;
    }

    public static void CalGValue(this ANode node, ANode startNode, int offset = 1, int unitPrice = 1)
    {
        int xDistance = Mathf.Abs(node.x - startNode.x) / offset;
        int yDistance = Mathf.Abs(node.y - startNode.y) / offset;

        node.G = (xDistance + yDistance) * unitPrice;
    }

    public static void CalFValue(this ANode node)
    {
        if (node.G == 0 && node.H == 0)
            return;
        node.F = node.G + node.H;
    }

    public static void UpdateNodeData(this ANode node)
    {

    }

    public static List<ANode> CalClosedNode(this ANode node, Dictionary<string, ANode> nodeDict, List<ANode> closedList)
    {
        List<ANode> returnList = new List<ANode>();

        if (node == null) return null;
        var offset = node.offset;

        //下面是四个方向的~~~~~~判断 可以做成一个方法
        ANode tempNode = null;
        nodeDict.TryGetValue((node.x + offset).ToString() + node.y, out tempNode);
        if (tempNode != null && tempNode.canReached == true && !closedList.Contains(tempNode))
        {
            tempNode.SetParentNode(node);
            returnList.Add(tempNode);
        }

        tempNode = null;
        nodeDict.TryGetValue((node.x - offset).ToString() + node.y, out tempNode);
        if (tempNode != null && tempNode.canReached == true && !closedList.Contains(tempNode))
        {
            tempNode.SetParentNode(node);
            returnList.Add(tempNode);
        }

        tempNode = null;
        nodeDict.TryGetValue(node.x.ToString() + (node.y + offset), out tempNode);
        if (tempNode != null && tempNode.canReached == true && !closedList.Contains(tempNode))
        {
            tempNode.SetParentNode(node);
            returnList.Add(tempNode);
        }

        tempNode = null;
        nodeDict.TryGetValue(node.x.ToString() + (node.y + offset), out tempNode);
        if (tempNode != null && tempNode.canReached == true && !closedList.Contains(tempNode))
        {
            tempNode.SetParentNode(node);
            returnList.Add(tempNode);
        }

        //nodeList = new List<ANode>{new ANode(node.x+offset,node.y),new ANode(node.x-offset,node.y),
        //    new ANode(node.x,node.y+offset),new ANode(node.x,node.y-offset)
        //};

        return returnList;
    }

    public static bool IsMatched(this ANode node, ANode targetNode)
    {
        if (node == null) return false;

        if (node.H != 0) return false;

        if (node.x == targetNode.x && node.y == targetNode.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void UpdateAllData(this ANode node, ANode startNode, ANode endNode)
    {
        node.CalGValue(startNode);
        node.CalHValue(endNode);
        node.CalFValue();
    }

    public static bool CanReached(this ANode node)
    {
        return node.canReached;
    }

    public static void SetParentNode(this ANode node, ANode parent)
    {
        node.parentNode = parent;
    }
}
