using UnityEngine;

public class DungeonGeneratorScript : MonoBehaviour
{

    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    [Range(0f, .3f)] public float roomBtmCornerModifier;
    [Range(0.7f, 1f)]
    public float roomTopCornerModifier;
    [Range(0f, 2f)]
    public int roomOffset;
    

    public Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeons(maxIterations, roomWidthMin, roomLengthMin, roomBtmCornerModifier, roomTopCornerModifier, roomOffset, corridorWidth);


        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 btmLeftCorner, Vector2 topRightCorner)
    {
        Vector3 V_bottomLeftCorner = new Vector3(btmLeftCorner.x,0, btmLeftCorner.y);
        Vector3 V_bottomRightCorner = new Vector3(topRightCorner.x,0, btmLeftCorner.y);
        Vector3 V_topLeftCorner = new Vector3(btmLeftCorner.x,0, topRightCorner.y);
        Vector3 V_topRightCorner = new Vector3(topRightCorner.x,0, topRightCorner.y);


        Vector3[] vertices = new Vector3[]
        {
            V_topLeftCorner,
            V_topRightCorner,
            V_bottomLeftCorner,
            V_bottomRightCorner
        };
        
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        GameObject dungeonFloor = new GameObject("Mesh " + V_bottomRightCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}