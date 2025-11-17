using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class MeshWriter : MonoBehaviour
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();

    private TextFileReader tfr;
    public int file;
    private int lastFileDraw;

    public bool calculateNormals = false;

    [SerializeField] private Vector3 gravityCenter;

    void Start()
    {
        tfr = GetComponent<TextFileReader>();
    }

    private void OnDrawGizmos()
    {
        if (lastFileDraw == file) return;
        lastFileDraw = file;


        vertices.Clear();
        triangles.Clear();

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();

        tfr.Read(file);

        string[] content = tfr.contenu;
        int nbVertices = int.Parse(content[1].Split(" ")[0]);
        int nbTriangles = int.Parse(content[1].Split(" ")[1]);

        gravityCenter = Vector3.zero;

        for (int i = 2; i < content.Length; i++)
        {
            string[] line = content[i].Split(" ");
            if (i - 2 > nbVertices - 1)
            {
                // nb triangles
                triangles.Add(int.Parse(line[1]));
                triangles.Add(int.Parse(line[2]));
                triangles.Add(int.Parse(line[3]));

                if (calculateNormals)
                {
                    Debug.Log(GetNormal(vertices[int.Parse(line[1])], vertices[int.Parse(line[2])], vertices[int.Parse(line[3])]));
                }
            }
            else
            {
                // nb vertices
                Vector3 pos = new Vector3(
                    float.Parse(line[0], CultureInfo.InvariantCulture),
                    float.Parse(line[1], CultureInfo.InvariantCulture),
                    float.Parse(line[2], CultureInfo.InvariantCulture)
                   );
                vertices.Add(pos);
                gravityCenter += pos;
            }
        }

        gravityCenter /= nbVertices;
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i] - gravityCenter;
        }

        float maximum = 0f;
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 v = vertices[i];
            float maxComponent = Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
            maximum = Mathf.Max(maximum, maxComponent);
        }


        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] /= maximum;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    Vector3 GetNormal(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 AC = C - A;
        AC = AC.normalized;
        Vector3 AB = B - A;
        AB = AB.normalized;

        return Vector3.Cross(AB, AC);
    }
}