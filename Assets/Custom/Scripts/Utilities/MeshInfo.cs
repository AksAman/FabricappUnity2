using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
public class MeshInfo : MonoBehaviour
{
    Mesh mesh;
    public List<int> subMeshVerticesCount;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        if (mesh == null)
        {
            Debug.Log("Mesh property is null!");
            return;
        }

        subMeshVerticesCount = new List<int>();
        GetsubMeshVerticesCount(mesh);
    }

    void GetsubMeshVerticesCount(Mesh mesh)
    {
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            subMeshVerticesCount.Add(mesh.GetTriangles(i).Length);
        }
    }

    public int GetMaterialIndexFromTriangleIndex(int triangleIndex)
    {
        int listIndex;
        int correspondingVertexIndex = triangleIndex * 3;
        int verticesTillNow = 0;
        for (listIndex = 0; listIndex < subMeshVerticesCount.Count; listIndex++)
        {
            verticesTillNow += subMeshVerticesCount[listIndex];
            if (correspondingVertexIndex < verticesTillNow)
                return listIndex;

        }
        return -1;

        
    }
}
