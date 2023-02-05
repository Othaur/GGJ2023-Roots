using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandByClick : MonoBehaviour
{
     Mesh mesh;
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;

    Camera cam;

    //[field: SerializeField, Range(0, 10f)]public float Increment {get; private set;} = 1f;
    [field: SerializeField, Range(0, 10f)]public float Width {get; private set;} = 1f;
    [field: SerializeField, Range(1f, 100f)]public float Buffer {get; private set;} = 10f;

    // public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mesh = GetComponent<MeshFilter>().mesh;

        //ExtendMesh(Target.position);
    }
    
    void InitializeMesh()
    {


    }


    public void ExtendMesh(Vector3 target)
    {
        vertices = mesh.vertices;
        uvs = mesh.uv;
        triangles = mesh.triangles;
    

        List<Vector3> updatedVertices = new List<Vector3>();
        List<Vector2> updatedUVs = new List<Vector2>();
        List<int> updatedTriangles = new List<int>();


        int c = 0;
        int c2 = 0;
        //float dis = Vector2.Distance(target, vertices[0]);
        float dis = Buffer +1;
        for (int v = 0; v < vertices.Length; v++)
        {
            updatedVertices.Add(vertices[v]);
            //Check distance to target
            //dis = Vector2.Distance(target, vertices[v]);
            if (Vector3.Distance(target, vertices[v]) < dis)
            {
                c = v;
                dis = Vector3.Distance(target, vertices[c]);
            }            
        }

        if (dis > Buffer) return;
        float dis2 = Buffer + 1;
        for (int v=0; v < vertices.Length; v++)
        {
            if (Vector3.Distance(target, vertices[v]) < dis2)
            {
                if (v != c)
                {
                    c2 = v;
                    dis2 = Vector3.Distance(target, vertices[c2]);
                }
            }
        }

        Debug.Log("Target Position" + target);
        // Debug.Log("Closest position: " + vertices[c]);
        // Debug.Log("Closest index: " + c);
        // Debug.Log("2nd Closest position: " + vertices[c2]);
        // Debug.Log("2nd Closest index: " + c2);
        // Debug.Log("Vertex 0: " + vertices[0]);

        for (int u = 0; u < uvs.Length; u++)
        {
            updatedUVs.Add(uvs[u]);
        }

        for (int t = 0; t < triangles.Length; t++)
        {
            updatedTriangles.Add(triangles[t]);
        }

        

        int t0 = c2;
        int t1 = c;        
        
        Vector3 s1 = updatedVertices[c] - updatedVertices[c2];
        Vector3 s2 = target - updatedVertices[c2];
        Vector3 crossProduct = Vector3.Cross(s1, s2).normalized;

        if (crossProduct.z > 0f)
        {
            t0 = c;
            t1 = c2;
        }

        //Debug.Log("Cross Product: " + crossProduct);

        Vector3 mid = MidpointBetween(updatedVertices[t0], updatedVertices[t1]);
        Vector3 dir = DirectionBetween(mid, target);
        Vector3 t2V = target + GetPerpendicularVector(mid - target) * -Width/2f;
        Vector3 t3V = target + GetPerpendicularVector(mid - target) * Width/2f;

        Debug.DrawLine(target, mid, Color.yellow, 5f);
        Debug.DrawLine(target, t2V, Color.green, 5f);
        Debug.DrawLine(target, t3V, Color.red, 5f);
                
        updatedVertices.Add(t2V);
        updatedVertices.Add(t3V);

        updatedUVs.Add(new Vector2(t2V.x, t2V.y));
        updatedUVs.Add(new Vector2(t3V.x, t3V.y));

        updatedTriangles.Add(t0);
        updatedTriangles.Add(t1);
        updatedTriangles.Add(updatedVertices.IndexOf(target));
        updatedTriangles.Add(updatedVertices.IndexOf(t3V));

        mesh.Clear();

        mesh.vertices = updatedVertices.ToArray();
        mesh.uv = updatedUVs.ToArray();
        mesh.triangles = updatedTriangles.ToArray();


        mesh.RecalculateBounds();
    }

    Vector3 MidpointBetween(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + (b.x - a.x)/2f, a.y + (b.y - a.y)/2f, 0);
    }

    Vector3 DirectionBetween(Vector3 origin, Vector3 destination)
    {
        return (destination - origin).normalized;
    }

    Vector3 GetPerpendicularVector(Vector3 v)
    {
        //return CodeMonkey.Utils.UtilsClass.ApplyRotationToVector(v, -90f);
        //return Vector2.Perpendicular(v);
        return new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2f) + Mathf.Pow(v.y, 2f));
    }
}
