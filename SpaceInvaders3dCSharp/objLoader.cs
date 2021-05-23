using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders3dCSharp
{
    class objLoader
    {
        public static void load()
        {
            List<string> lines = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader("C:\\Users\\Yayo\\Desktop\\ship.obj");

            string linex;
            while ((linex = file.ReadLine()) != null)
            {
                lines.Add(linex);
            }

            float[] vertices = new float[lines.Count * 3];
            float[] normals = new float[lines.Count * 3];
            float[] uv = new float[lines.Count * 2];

            int numVertices = 0;
            int numNormals = 0;
            int numUV = 0;
            int numFaces = 0;

            int[] facesVerts = new int[lines.Count * 3];
            int[] facesNormals = new int[lines.Count * 3];
            int[] facesUV = new int[lines.Count * 3];
            int vertexIndex = 0;
            int normalIndex = 0;
            int uvIndex = 0;
            int faceIndex = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (line.StartsWith("v "))
                {
                    string[] tokens = line.Split(' ');
                    vertices[vertexIndex] = (float)Convert.ToDouble(tokens[1]);
                    vertices[vertexIndex + 1] = (float)Convert.ToDouble(tokens[2]);
                    vertices[vertexIndex + 2] = (float)Convert.ToDouble(tokens[3]);
                    vertexIndex += 3;
                    numVertices++;
                    continue;
                }

                if (line.StartsWith("vn "))
                {
                    String[] tokens = line.Split(' ');
                    normals[normalIndex] = (float)Convert.ToDouble(tokens[1]);
                    normals[normalIndex + 1] = (float)Convert.ToDouble(tokens[2]);
                    normals[normalIndex + 2] = (float)Convert.ToDouble(tokens[3]);
                    normalIndex += 3;
                    numNormals++;
                    continue;
                }

                if (line.StartsWith("vt"))
                {
                    String[] tokens = line.Split(' ');
                    uv[uvIndex] = (float)Convert.ToDouble(tokens[1]);
                    uv[uvIndex + 1] = (float)Convert.ToDouble(tokens[2]);
                    uvIndex += 2;
                    numUV++;
                    continue;
                }

                if (line.StartsWith("f "))
                {
                    string[] tokens = line.Split(' '); ;

                    string[] parts = tokens[1].Split('/');
                    facesVerts[faceIndex] = getIndex(parts[0], numVertices);
                    if (parts.Length > 2)
                        facesNormals[faceIndex] = getIndex(parts[2], numNormals);
                    if (parts.Length > 1)
                        facesUV[faceIndex] = getIndex(parts[1], numUV);
                    faceIndex++;

                    parts = tokens[2].Split('/');
                    facesVerts[faceIndex] = getIndex(parts[0], numVertices);
                    if (parts.Length > 2)
                        facesNormals[faceIndex] = getIndex(parts[2], numNormals);
                    if (parts.Length > 1)
                        facesUV[faceIndex] = getIndex(parts[1], numUV);
                    faceIndex++;

                    parts = tokens[3].Split('/');
                    facesVerts[faceIndex] = getIndex(parts[0], numVertices);
                    if (parts.Length > 2)
                        facesNormals[faceIndex] = getIndex(parts[2], numNormals);
                    if (parts.Length > 1)
                        facesUV[faceIndex] = getIndex(parts[1], numUV);
                    faceIndex++;
                    numFaces++;
                    continue;
                }
            }
        }

       static int getIndex(string index, int size) {
		int idx = Convert.ToInt32(index);
		if (idx < 0)
			return size + idx;
		else
			return idx - 1;
	}
    }
}
