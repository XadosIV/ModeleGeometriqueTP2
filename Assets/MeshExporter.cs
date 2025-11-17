using UnityEngine;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

public class MeshExporter : MonoBehaviour
{
    public string fileName = "export.off";

    public bool ExportMeshButton = false;

    private void OnDrawGizmos()
    {
        if (ExportMeshButton)
        {
            ExportMeshButton = false;
            ExportCurrentMesh();
        }
    }

    public void ExportCurrentMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        List<string> lines = new List<string>();

        // Header
        lines.Add("OFF");
        lines.Add($"{vertices.Length} {triangles.Length / 3} 0");

        // Vertices
        foreach (var v in vertices)
        {
            lines.Add(
                v.x.ToString(CultureInfo.InvariantCulture) + " " +
                v.y.ToString(CultureInfo.InvariantCulture) + " " +
                v.z.ToString(CultureInfo.InvariantCulture)
            );
        }

        // Triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            lines.Add($"3 {triangles[i]} {triangles[i+1]} {triangles[i+2]}");
        }

        // Save
        string path = Application.dataPath + "/" + fileName;
        File.WriteAllLines(path, lines);

        Debug.Log("Export : " + path);
    }
}
