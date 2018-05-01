using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownScroller.Nodes
{

    public class NodeManager : MonoBehaviour
    {
        public List<Node> nodes;

        public float levelWidth;
        public float height;

        public float nodeHeight = .2f;
        private int nodeCount;

        Vector3 left, right;

        bool nodesDivided = false;

        [ContextMenu("Set Gizmos Position")]
        public void SetGizmosPos()
        {
            left = new Vector3(-levelWidth / 2, height);
            right = new Vector3(levelWidth / 2, height);
            nodesDivided = false;
        }

        [ContextMenu("Generate Nodes")]
        public void GenerateNodes()
        {
            nodes = new List<Node>();
            foreach (Node node in GetComponentsInChildren<Node>())
            {
                nodes.Add(node);
            }
            nodeCount = nodes.Count;
            
            ArangeNodes();
        }

        [ContextMenu("Arrange Nodes")]
        public void ArangeNodes()
        {
            if (nodes != null)
            {
                if (nodeCount <= 2)
                {
                    Debug.LogError("nodeCount to small. Something went horribly wrong.");
                    return;
                }
                float space = levelWidth / (nodeCount + 1);
                float startPos = -levelWidth / 2 + space;
                for (int i = 0; i < nodeCount; i++)
                {
                    nodes[i].transform.position = new Vector3(startPos, height);
                    startPos += space;
                }
                nodesDivided = true;
            }
            else nodesDivided = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(left, right);

            if (!nodesDivided)
                return;

            Gizmos.color = Color.red;
            for (int i = 0; i < nodeCount; i++)
            {
                Vector3 pos = nodes[i].transform.position;
                Gizmos.DrawLine(pos + Vector3.up * nodeHeight, pos - Vector3.up * nodeHeight);
            }
        }
    }

}