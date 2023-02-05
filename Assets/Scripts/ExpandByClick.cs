using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandByClick : MonoBehaviour
{
     Mesh mesh;
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;

    List<Vector3> targets;

    Camera cam;

    [field: SerializeField, Range(0, 10f)]public float Increment {get; private set;} = 1f;
    [field: SerializeField, Range(0, 10f)]public float Width {get; private set;} = 1f;
    [field: SerializeField, Range(1f, 100f)]public float Buffer {get; private set;} = 10f;

    // public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mesh = GetComponent<MeshFilter>().mesh;

        targets = new List<Vector3>();
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
        
        float dis = Buffer +1;
        for (int v = 0; v < vertices.Length; v++)
        {
            updatedVertices.Add(vertices[v]);            
            if (Vector3.Distance(target, transform.TransformPoint(updatedVertices[v])) < dis)
            {
                c = v;
                dis = Vector3.Distance(target, transform.TransformPoint(updatedVertices[c]));
            }            
        }

        if (dis > Buffer) {return;}
        if (targets.Contains(target)) {return;}
        
        targets.Add(target);

        float dis2 = Buffer + 1;
        for (int v=0; v < vertices.Length; v++)
        {
            if (Vector3.Distance(target, transform.TransformPoint(updatedVertices[v])) < dis2)
            {
                if (v != c)
                {
                    c2 = v;
                    dis2 = Vector3.Distance(target, transform.TransformPoint(updatedVertices[c2]));
                }
            }
        }

        Debug.Log("Target Position" + target);        

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
        Vector3 s2 = transform.InverseTransformPoint(target) - updatedVertices[c2];
        Vector3 crossProduct = Vector3.Cross(s1, s2).normalized;

        if (crossProduct.z > 0f)
        {
            t0 = c;
            t1 = c2;
        }

        

        Vector3 mid = MidpointBetween(transform.TransformPoint(updatedVertices[t0]), transform.TransformPoint(updatedVertices[t1]));
        Vector3 dir = DirectionBetween(mid, target);
        Vector3 t2V = target + GetPerpendicularVector(mid - target) * -Width/2f;
        Vector3 t3V = target + GetPerpendicularVector(mid - target) * Width/2f;

        Debug.DrawLine(target, mid, Color.yellow, 10f);
        Debug.DrawLine(target, t2V, Color.green, 10f);
        Debug.DrawLine(target, t3V, Color.red, 10f);

        if (updatedVertices.Contains(t2V)) return;
        //mid = transform.InverseTransformPoint(mid);
        t2V = transform.InverseTransformPoint(t2V);
        t3V = transform.InverseTransformPoint(t3V);
        //target = transform.InverseTransformPoint(target);

        if (updatedVertices.Contains(t3V)) return;
                
        updatedVertices.Add(t2V);
        updatedVertices.Add(t3V);

        updatedUVs.Add(new Vector2(t2V.x, t2V.y));
        updatedUVs.Add(new Vector2(t3V.x, t3V.y));

        updatedTriangles.Add(t0);
        updatedTriangles.Add(t1);
        updatedTriangles.Add(updatedVertices.IndexOf(t3V));

        updatedTriangles.Add(t1);
        updatedTriangles.Add(updatedVertices.IndexOf(t2V));
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
