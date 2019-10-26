using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS GETTERS AND SETTERS TO BE USED BETWEEN COMPONENTS
 * 
 * Jason calls getSwarmCoordinates() to get the cordinates of each origami location in the swarm, returns List<Vector3>
 * 
 * 
 * */

public class Swarm_mesh : MonoBehaviour
{
    // public List<Vector3> MeshStructVertices = new List<Vector3>();
    public List<Vector3> MeshStructVertices;
    public List<List<Vector3>> MeshTriangleExtraction;
    public Material planetMaterial;
    public bool swarmCalculated;
    // Start is called before the first frame update
    void Start()
    {
        swarmCalculated = false;
        planetMaterial = new Material(Shader.Find("VR/SpatialMapping/Wireframe"));

        MeshTriangleExtraction = new List<List<Vector3>>();

        CreatePlanet();

        foreach(int triangle in planetMesh.triangles)
        {
            print(triangle);
        }

        // Small swarm 42 triangle origamis
        //



        foreach(Vector3 tri in MeshStructVertices)
        {
            print("Vectors " + tri.x + " " + tri.y + " " + tri.z);
        }
        print("Vector list obtained");
        swarmCalculated = true;


        int CountTriangles = MeshTriangleExtraction.Count;
        print("Number of triangle obtained" + CountTriangles);
    }
    public bool isSwarmCalculated()
    {
        return swarmCalculated;
    }


    public List<List<Vector3>> getSwarmCoordinates()
    {
        return MeshTriangleExtraction;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public float planetSize = 1f;
    GameObject planet;
    Mesh planetMesh;
    Vector3[] planetVertices;
    int[] planetTriangles;
    MeshRenderer planetMeshRenderer;
    MeshFilter planetMeshFilter;
    MeshCollider planetMeshCollider;


    public void CreatePlanet()
    {
        CreatePlanetGameObject();
        //do whatever else you need to do with the sphere mesh
        RecalculateMesh();
    }

    void CreatePlanetGameObject()
    {
        planet = new GameObject();
        planetMeshFilter = planet.AddComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.AddComponent<MeshRenderer>();
        //need to set the material up top
        planetMeshRenderer.material = planetMaterial;
        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
        Create(planet);
    }

    void RecalculateMesh()
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();
    }


    private struct TriangleIndices
    {
        public int v1;
        public int v2;
        public int v3;

        public TriangleIndices(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;

        }
    }

    // return index of point in the middle of p1 and p2
    private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        long smallerIndex = firstIsSmaller ? p1 : p2;
        long greaterIndex = firstIsSmaller ? p2 : p1;
        long key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if(cache.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it
        Vector3 point1 = vertices[p1];
        Vector3 point2 = vertices[p2];
        Vector3 middle = new Vector3
        (
            (point1.x + point2.x) / 2f,
            (point1.y + point2.y) / 2f,
            (point1.z + point2.z) / 2f
        );

        // add vertex makes sure point is on unit sphere
        int i = vertices.Count;
        vertices.Add(middle.normalized * radius);

        // store it, return index
        cache.Add(key, i);

        return i;
    }

    public void Create(GameObject gameObject)
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        mesh.Clear();
        Vector3[] vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;
        List<Vector3> vertList = new List<Vector3>();
        Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
        //int index = 0;

        int recursionLevel = 0;
        float radius = 5f;

        // create 12 vertices of a icosahedron
        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        Vector3 Vec0 = new Vector3(-1f, t, 0f);
        Vector3 Vec1 = new Vector3(1f, t, 0f);
        Vector3 Vec2 = new Vector3(-1f, -t, 0f);
        Vector3 Vec3 = new Vector3(1f, -t, 0f);
        Vector3 Vec4 = new Vector3(0f, -1f, t);
        Vector3 Vec5 = new Vector3(0f, 1f, t);
        Vector3 Vec6 = new Vector3(0f, -1f, -t);
        Vector3 Vec7 = new Vector3(0f, 1f, -t);
        Vector3 Vec8 = new Vector3(t, 0f, -1f);
        Vector3 Vec9 = new Vector3(t, 0f, 1f);
        Vector3 Vec10 = new Vector3(-t, 0f, -1f);
        Vector3 Vec11 = new Vector3(-t, 0f, 1f);


        vertList.Add(new Vector3(-1f, t, 0f).normalized * radius);      //0
        vertList.Add(new Vector3(1f, t, 0f).normalized * radius);       //1
        vertList.Add(new Vector3(-1f, -t, 0f).normalized * radius);     //2
        vertList.Add(new Vector3(1f, -t, 0f).normalized * radius);      //3
        vertList.Add(new Vector3(0f, -1f, t).normalized * radius);      //4
        vertList.Add(new Vector3(0f, 1f, t).normalized * radius);       //5
        vertList.Add(new Vector3(0f, -1f, -t).normalized * radius);     //6
        vertList.Add(new Vector3(0f, 1f, -t).normalized * radius);      //7
        vertList.Add(new Vector3(t, 0f, -1f).normalized * radius);      //8
        vertList.Add(new Vector3(t, 0f, 1f).normalized * radius);       //9
        vertList.Add(new Vector3(-t, 0f, -1f).normalized * radius);     //10
        vertList.Add(new Vector3(-t, 0f, 1f).normalized * radius);      //11


        // create 20 triangles of the icosahedron
        List<TriangleIndices> faces = new List<TriangleIndices>();

        // 5 faces around point 0
        faces.Add(new TriangleIndices(0, 11, 5));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec0,
            Vec11,
            Vec5
        });

        faces.Add(new TriangleIndices(0, 5, 1));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec0,
            Vec5,
            Vec1
        });

        faces.Add(new TriangleIndices(0, 1, 7));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec0,
            Vec1,
            Vec7
        });

        faces.Add(new TriangleIndices(0, 7, 10));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec0,
            Vec7,
            Vec10
        });

        faces.Add(new TriangleIndices(0, 10, 11));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec0,
            Vec10,
            Vec11
        });

        // 5 adjacent faces 
        faces.Add(new TriangleIndices(1, 5, 9));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec1,
            Vec5,
            Vec9
        });

        faces.Add(new TriangleIndices(5, 11, 4));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec5,
            Vec11,
            Vec4
        });

        faces.Add(new TriangleIndices(11, 10, 2));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec11,
            Vec10,
            Vec2
        });

        faces.Add(new TriangleIndices(10, 7, 6));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec10,
            Vec7,
            Vec6
        });

        faces.Add(new TriangleIndices(7, 1, 8));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec7,
            Vec1,
            Vec8
        });

        // 5 faces around point 3
        faces.Add(new TriangleIndices(3, 9, 4));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec3,
            Vec9,
            Vec4
        });

        faces.Add(new TriangleIndices(3, 4, 2));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec3,
            Vec4,
            Vec2
        });

        faces.Add(new TriangleIndices(3, 2, 6));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec3,
            Vec2,
            Vec6
        });

        faces.Add(new TriangleIndices(3, 6, 8));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec3,
            Vec6,
            Vec8
        });

        faces.Add(new TriangleIndices(3, 8, 9));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec3,
            Vec8,
            Vec9
        });

        // 5 adjacent faces 
        faces.Add(new TriangleIndices(4, 9, 5));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec4,
            Vec9,
            Vec5
        });

        faces.Add(new TriangleIndices(2, 4, 11));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec2,
            Vec4,
            Vec11
        });

        faces.Add(new TriangleIndices(6, 2, 10));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec6,
            Vec2,
            Vec10
        });

        faces.Add(new TriangleIndices(8, 6, 7));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec8,
            Vec6,
            Vec7
        });

        faces.Add(new TriangleIndices(9, 8, 1));
        MeshTriangleExtraction.Add(new List<Vector3>()
        {
            Vec9,
            Vec8,
            Vec1
        });


        // refine triangles
        for(int i = 0; i < recursionLevel; i++)
        {
            List<TriangleIndices> faces2 = new List<TriangleIndices>();

            List<List<Vector3>> MeshTriangleExtraction2 = new List<List<Vector3>>();

            foreach(var tri in faces)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, radius);
                int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, radius);
                int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, radius);

                faces2.Add(new TriangleIndices(tri.v1, a, c));
                MeshTriangleExtraction2.Add(new List<Vector3>()
                 {
                   vertList[tri.v1],
                   vertList[a],
                   vertList[c]
                });

                faces2.Add(new TriangleIndices(tri.v2, b, a));
                MeshTriangleExtraction2.Add(new List<Vector3>()
                 {
                   vertList[tri.v2],
                   vertList[b],
                   vertList[a]
                });

                faces2.Add(new TriangleIndices(tri.v3, c, b));
                MeshTriangleExtraction2.Add(new List<Vector3>()
                 {
                   vertList[tri.v3],
                   vertList[c],
                   vertList[b]
                });

                faces2.Add(new TriangleIndices(a, b, c));
                MeshTriangleExtraction2.Add(new List<Vector3>()
                 {
                   vertList[a],
                   vertList[b],
                   vertList[c]
                });
            }
            faces = faces2;
            MeshTriangleExtraction = MeshTriangleExtraction2;
        }

        mesh.vertices = vertList.ToArray();

        List<int> triList = new List<int>();
        for(int i = 0; i < faces.Count; i++)
        {
            triList.Add(faces[i].v1);
            triList.Add(faces[i].v2);
            triList.Add(faces[i].v3);
        }
        mesh.triangles = triList.ToArray();
        mesh.uv = new Vector2[vertices.Length];

        Vector3[] normales = new Vector3[vertList.Count];
        for(int i = 0; i < normales.Length; i++)
            normales[i] = vertList[i].normalized;


        mesh.normals = normales;

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        //mesh.Optimize();


        /*  foreach (var tri in faces)
          {
              print("TRIANGLE " + tri.v1 + " " + tri.v2 + " " + tri.v3);
          }
          */


        MeshStructVertices = new List<Vector3>(vertList);
        //MeshStructVertices = vertList;


    }
}

