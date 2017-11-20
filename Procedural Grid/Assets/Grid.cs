using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Tutorial project.
/// Create a grid of points.
/// Use a coroutine to analyze their placement.
/// Define a surface with triangles.
/// Automatically generate normals.
/// Add texture coordinates and tangents.
/// found at: http://catlikecoding.com/unity/tutorials/procedural-grid/
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    private Mesh mesh;
    /// <summary>
    /// A list of Vectors.
    /// </summary>
    private Vector3[] vertices;
    /// <summary>
    /// the horizontal size
    /// </summary>
    public int xSize;
    /// <summary>
    /// the vertical size
    /// </summary>
    public int ySize;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// 
    /// Awake is used to initialize any variables or game state before the game starts. Awake is 
    /// called only once during the lifetime of the script instance. Awake is called after all 
    /// objects are initialized.
    /// </summary>
    private void Awake()
    {
        Generate();
    }
    /// <summary>
    /// Generates the list of vertices and the mesh associated with them.
    /// </summary>
    private void Generate()
    {

		// create a mesh
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";

		// want to make a grid x cells wide by y cells tall.
		// each cell goes from 0 to 1 wide, 0 to 1 tall, so 
		// sizes go from 0 to x,y size + 1
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                //uv[i] = new Vector2(((float) x / xSize) * .0625f, ((float) y / ySize) * .0625f);
                //Debug.Log(vertices[i]+"::"+uv[i]);
            }
		}
		mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        // triangles need to be clockwise to be considered forward facing.
        // 1st triangle goes from 0 -> xSize + 1
        // xSize + 1 -> 1
        // 1 -> 0
        // 2nd triangle goes from 1 -> xSize + 1
        // xSize + 1 -> xSize + 2
        // xSize + 2 -> 1
        // since triangles share vertices, the declarations are shared
        // need 6 vertices to cover one quad.
        int[] triangles = new int[xSize * ySize * 6];
		// loop fills all quads in the mesh
		for (int i = 0; i < xSize * ySize; i++) {
			int val = i + (i / xSize);
			triangles[i * 6] = val;
			triangles[(i * 6) + 1] = triangles[(i * 6) + 4] = xSize + 1 + val;
			triangles[(i * 6) + 2] = triangles[(i * 6) + 3] = val + 1;
			triangles [(i * 6) + 5] = xSize + 2 + val;
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		/*
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        // want to make a grid x cells wide by y cells tall.
        // each cell goes from 0 to 1 wide, 0 to 1 tall, so 
        // sizes go from 0 to x,y size + 1
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        // grid is vertical, going from 0->x, 0->y, but staying at depth z=0
        // view from forward:
        // ^
        // |
        // | y
        // |________
        // 0    x
        //
        // view from above:
        // ^
        // |
        // | z
        // |________
        // 0    x
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                int index = y * (xSize + 1) + x;
                vertices[index] = new Vector3(x, y);
                uv[index] = new Vector2(x / xSize, y / ySize);
                mesh.vertices = vertices;
                mesh.uv = uv;
                int[] orig = new int[0];
                if (mesh.triangles != null
                    && mesh.triangles.Length > 0)
                {
                    orig = new int[mesh.triangles.Length];
                    mesh.triangles.CopyTo(orig, 0);
                }
                int[] tris = new int[6];
                if (x < xSize
                    && y < ySize)
                {
                    Debug.Log(String.Format("triangle={0}, {1}, {2}", y * (xSize + 1) + x, ((y + 1) * (xSize + 1) + x), y * (xSize + 1) + x + 1));
                    tris[0] = y * (xSize + 1) + x;
                    tris[1] = (y + 1) * (xSize + 1) + x;
                    tris[2] = tris[0] + 1;
                    tris[3] = tris[1];
                    tris[4] = tris[1] + 1;
                    tris[5] = tris[2];
                }
                int[] temp = new int[orig.Length + tris.Length];
                orig.CopyTo(temp, 0);
                tris.CopyTo(temp, orig.Length);
                mesh.triangles = temp;
                mesh.RecalculateNormals();
                orig = null;
                temp = null;
                tris = null;
                yield return wait;
            }
        }
        /*
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                GetComponent<MeshFilter>().mesh = mesh = new Mesh();
                mesh.name = "Procedural Grid";
                vertices[i] = new Vector3(x, y);
                mesh.vertices = vertices;
                // a mesh is made up of vertices. the vertices determine the mesh's shape
                // our grid is made up of (width + 1) * (height + 1) vertices,
                // so a 3x2 grid has 12 vertices.
                // the vertices are numbered 0,0 to x+1,y+1.
                //int[] triangles = new int[3] { 0, xSize + 1, 1 };
                int[] temp = new int[] { x, xSize };
                if (mesh.triangles.Length > 0)
                {
                    int[] triangles = new int[mesh.triangles.Length + 3];
                } else
                {
                    int[] triangles = new int[mesh.triangles.Length + 3];
                }
                mesh.triangles = triangles;
                yield return wait;
            }
        }
        */
    }
    /// <summary>
    /// Implemented to draw gizmos that are also pickable and always drawn.
    /// Runs every frame.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (vertices != null)
        {
            // draw a black sphere at every vertices location
            Gizmos.color = Color.black;
            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
