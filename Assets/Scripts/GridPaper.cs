using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class GridPaper : MonoBehaviour
{
    [SerializeField]
    private Voxel[] Voxels;
    private int VoxelSize = 10;
    Vector2Int  Resolution = Vector2Int.one;

    private List<Vector3> Positions = new List<Vector3>();
    private Mesh mesh;

    private List<Vector3> vertices;
    private List<int> triangles;
    
    [SerializeField] private Texture2D texture;
    [SerializeField] private bool Build = false;
    
    [SerializeField][ItemCanBeNull] List<GameObject> VoxelPrefab = new List<GameObject>();
    [SerializeField] private Material[] VoxelMaterials;
    
    // Update is called once per frame
    void Update()
    {
        if (Build)
        {
            if (transform.childCount == 0 )
            {
                if (texture != null )
                {
                    Initialize();
                    
                    for (int i = 0, x = 0; x < texture.width; x++)
                    {
                        for (int y = 0; y < texture.height; y++, i++)
                        {
                            Color color = texture.GetPixel(x, y);
                            CreateVoxel(i, x, y, color);
                        }
                    }
                    Triangulate();
                }
            }
        }
        else
        {
            Cleanup();
        }
    }
    public void Initialize ()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "VoxelGrid Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        Positions = new List<Vector3>();
        Resolution.x = texture.width;
        Resolution.y = texture.height;
        Voxels = new Voxel[texture.width * texture.height];
    }
    public void Cleanup ()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "VoxelGrid Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        Resolution.x = 0;
        Resolution.y = 0;
        Voxels = new Voxel[]{};
        Positions = new List<Vector3>();
    }
    
    
    private void TriangulateCellRows ()
    {
        int CellsX = Resolution.x - 1;
        int CellsY = Resolution.y - 1;
        
        for (int i = 0, y = 0; y < CellsY; y++, i++) {
            for (int x = 0; x < CellsX; x++, i++) {
                TriangulateCell(
                    Voxels[i],
                    Voxels[i + 1],
                    Voxels[i + Resolution.y],
                    Voxels[i + Resolution.y+1]);
            }
        }
    }
    
    private void TriangulateCell (Voxel a, Voxel b, Voxel c, Voxel d) 
    {
        int cellType = 0;
        if (a.state) {
            cellType |= 1;
        }
        if (b.state) {
            cellType |= 2;
        }
        if (c.state) {
            cellType |= 4;
        }
        if (d.state) {
            cellType |= 8;
        }
        
        
        switch (cellType) {
            case 0:
                return;
            case 1:
                AddQuad(a.position, a.xEdgePosition, a.yEdgePosition, new Vector2(a.xEdgePosition.x, a.yEdgePosition.y));
                // AddTriangle(a.position, a.xEdgePosition, a.yEdgePosition);
                break;
            case 2:
                AddQuad(b.position, a.xEdgePosition, b.yEdgePosition,new Vector2(b.yEdgePosition.y,a.xEdgePosition.x));
                // AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
                break;
            case 4:
                AddQuad(c.position, c.xEdgePosition, a.yEdgePosition, new Vector2(a.yEdgePosition.y,c.xEdgePosition.x));
                //AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
                break;
            case 8:
                AddQuad(d.position, b.xEdgePosition, c.yEdgePosition, new Vector2( b.xEdgePosition.y,c.yEdgePosition.x));
                //AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
                break;
            case 3:
                AddQuad(a.position, a.yEdgePosition, b.yEdgePosition, b.position);
                break;
            case 5:
                AddQuad(a.position, c.position, c.xEdgePosition, a.xEdgePosition);
                break;
            case 10:
                AddQuad(a.xEdgePosition, c.xEdgePosition, d.position, b.position);
                break;
            case 12:
                AddQuad(a.yEdgePosition, c.position, d.position, b.yEdgePosition);
                break;
            case 15:
                AddQuad(a.position, c.position, d.position, b.position);
                break;
            // case 7:
            //     AddPentagon(a.position, c.position, c.xEdgePosition, b.yEdgePosition, b.position);
            //     break;
            // case 11:
            //     AddPentagon(b.position, a.position, a.yEdgePosition, c.xEdgePosition, d.position);
            //     break;
            // case 13:
            //     AddPentagon(c.position, d.position, b.yEdgePosition, a.xEdgePosition, a.position);
            //     break;
            // case 14:
            //     AddPentagon(d.position, b.position, a.xEdgePosition, a.yEdgePosition, c.position);
            //     break;
            // case 6:
            //      AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
            //      AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
            //      break;
            // case 9:
            //     AddTriangle(a.position, a.yEdgePosition, a.xEdgePosition);
            //     AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
            //     break;
            
        }
        
    }
    
    private void AddTriangle (Vector3 a, Vector3 b, Vector3 c) {
        int vertexIndex = vertices.Count;
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
    
    private void AddQuad (Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
        int vertexIndex = vertices.Count;
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
    
    private void AddPentagon (Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e) {
        int vertexIndex = vertices.Count;
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);
        vertices.Add(e);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex + 4);
    }
    
    private void Triangulate () {
        vertices.Clear();
        triangles.Clear();
        mesh.Clear();

        TriangulateCellRows();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
    private void OnDrawGizmos()
    {
        foreach (var voxel in Voxels)
        {
            Gizmos.color = voxel.state ? Color.black : Color.white;
            Gizmos.DrawCube(new Vector3(voxel.position.x,  voxel.position.y,0),
                Vector3.one * VoxelSize * 0.15f);
        }
    }
    void CreateVoxel(int i, int x, int y, Color color)
    {
        Voxels[i] = new Voxel(x,y,VoxelSize);
        Voxels[i].state = color.Equals(Color.black);
    }
    
    private void SetVoxelColors (int i) {
        
        //VoxelMaterials[i].color = Voxels[i].state ? Color.black : Color.white;
    }
}

[Serializable]
public class Voxel {

    public bool state;

    public Vector2 position, xEdgePosition, yEdgePosition;
    public Voxel (int x, int y, float size) {
        position.x = (x+0.5f)  * size;
        position.y = (y+0.5f)  * size;

        xEdgePosition = position;
        xEdgePosition.x += size * 0.5f;
        yEdgePosition = position;
        yEdgePosition.y += size * 0.5f;
    }
}