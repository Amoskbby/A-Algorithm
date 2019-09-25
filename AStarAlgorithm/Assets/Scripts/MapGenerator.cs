using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public int maxSize;
    public int border;
    public GameObject squareA;
    public GameObject squareB;

    public int coordinateStartX;
    public int coordinateStartY;

    public int coordinateEndX;
    public int coordinateEndY;

    public int xStart;
    public int yStart;

    public int xEnd;
    public int yEnd;

    List<ANode> nodeList = new List<ANode>();
    public Dictionary<string, ANode> nodeDict = new Dictionary<string, ANode>();

    private void Start()
    {
        //for (int i = 0; i < maxSize; i += border)
        //{
        //    for (int j = 0; j < maxSize; j += border)
        //    {
        //        var go = Instantiate(sourcePrefab, gameObject.transform, false);
        //        go.transform.localPosition = new Vector3(i, j, 0);
        //    }
        //}
        GenerateAllNode(coordinateEndX, coordinateEndY, 50);

        InitNodeGameObjectData();

        CalNodeData(nodeDict);
    }

    void InitNodeGameObjectData()
    {
        foreach (var node in nodeList)
        {
            GameObject go = null;
            if (node.canReached)
            {
                go = Instantiate(squareA, gameObject.transform, false);
            }
            else
            {
                go = Instantiate(squareB, gameObject.transform, false);
            }
            go.transform.localPosition = new Vector3(node.x, node.y, 0);
            go.name = "square" + "(" + node.x + "," + node.y + ")";
            //go.GetComponent<ANode>()?.SetXY(node.x, node.y);
        }
    }

    public void GenerateAllNode(int xLength, int yLength, int offset = 1)
    {
        nodeList.Clear();

        for (int x = coordinateStartX; x < xLength; x += offset)
        {
            for (int y = coordinateStartY; y < yLength; y += offset)
            {
                bool can = Random.Range(0, 10) < 2 ? false : true;
                ANode node = new ANode(x, y, can, offset);
                nodeList.Add(node);
                nodeDict.Add(x.ToString() + y, node);
            }
        }
    }

    public void CalNodeData(Dictionary<string, ANode> nodeDict)
    {
        ANode startNode = null;
        nodeDict.TryGetValue(xStart.ToString() + yStart, out startNode);
        if (startNode == null || !startNode.CanReached())
        {
            Debug.Log("起点不可达！");
        }


        ANode endNode = null;
        nodeDict.TryGetValue(xEnd.ToString() + yEnd, out endNode);
        if (endNode == null || !endNode.CanReached())
        {
            Debug.Log("终点不可达！");
        }

        List<ANode> openList = new List<ANode>();
        List<ANode> closedList = new List<ANode>();
        HashSet<string> hasOpenSet = new HashSet<string>();
        HashSet<string> closedSet = new HashSet<string>();

        bool reached = false;

        if (startNode.IsMatched(endNode))
        {
            Debug.Log("已经处于改点!!");
            return;
        }

        closedList.Add(startNode);
        closedSet.Add(startNode.x.ToString() + startNode.y);

        openList.AddRange(startNode.CalClosedNode(nodeDict,closedList));
        if (openList == null || openList.Count <= 0) return;

        foreach (var node in openList)
        {
            node.UpdateAllData(startNode, endNode);
            hasOpenSet.Add(node.x.ToString() + node.y);
        }

        ANode nowNode = startNode;
        ANode lastNode = null;

        List<ANode> tempList = new List<ANode>();
        StreamWriter sw = new StreamWriter("F:/path.txt", false, System.Text.Encoding.Default);

        while (true)
        {

            int minFValue = 0;
            nowNode = null;
            foreach (var node in openList)
            {
                //Debug.Log("H value: " + node.H);
                if (minFValue == 0 || node.F < minFValue)
                {
                    minFValue = node.F;
                    nowNode = node;
                }
                else if (node.F == minFValue)
                {
                    if (node.H <= nowNode.H)
                    {
                        nowNode = node;
                    }
                }
            }


            if (nowNode.IsMatched(endNode))
            {
                lastNode = nowNode;
                sw.WriteLine("我终于到达啦！！" + " x: " + nowNode.x + " y: " + nowNode.y);
                Debug.Log("我终于到达啦！！" + " x: " + nowNode.x + " y: " + nowNode.y);
                break;
            }

            if (!closedSet.Contains(nowNode.x.ToString() + nowNode.y))
            {
                closedList.Add(nowNode);
                closedSet.Add(nowNode.x.ToString() + nowNode.y);
            }

            sw.WriteLine("x: " + nowNode.x + " y: " + nowNode.y);
            Debug.Log("x: " + nowNode.x + " y: " + nowNode.y);


            var nowClosedList = nowNode.CalClosedNode(nodeDict,closedList);

            foreach (var nowClosedNode in nowClosedList)
            {
                string nodeName = nowClosedNode.x.ToString() + nowClosedNode.y;
                if (!hasOpenSet.Contains(nodeName) && !closedSet.Contains(nodeName))
                {
                    nowClosedNode.UpdateAllData(startNode, endNode);
                    hasOpenSet.Add(nodeName);
                    openList.Add(nowClosedNode);
                }
            }

            openList.Remove(nowNode);

            if (openList.Count == 0)
            {
                Debug.Log("我真的找不到路了！！");
                reached = true;
            }
        }

        sw.Close();

        List<ANode> trackNodeList = new List<ANode>();
        ANode tempNode = null;

        while (lastNode != null)
        {
            trackNodeList.Add(lastNode);
            tempNode = lastNode.parentNode;
            lastNode = tempNode;
        }

        for (int i = 0; i < trackNodeList.Count; i++)
        {
            SetGreenColor("square" + "(" + trackNodeList[i].x + "," + trackNodeList[i].y + ")");
        }
    }

    public void SetGreenColor(string name)
    {
        var go = GameObject.Find(name);
        var image = go.transform.Find("edge").GetComponent<Image>();
        Color color = new Color(0, 255, 0);
        image.color = color;
    }
}
