using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHelpers
{
    public static Mesh CreateSpriteQuadMesh() 
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        
        vertices[0] = new Vector3(-.5f, -.5f);
        vertices[1] = new Vector3(-.5f, +.5f);
        vertices[2] = new Vector3(+.5f, +.5f);
        vertices[3] = new Vector3(+.5f, -.5f);
        
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }
}
