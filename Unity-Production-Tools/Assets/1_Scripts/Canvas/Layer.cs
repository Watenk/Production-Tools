using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class Layer : IUpdateable
{
    // Data
    [SerializeField] public Texture2D Texture { get; private set; }
    [SerializeField] private string name;
    [SerializeField] private int index;

    private List<ColorPixel> updatedPixels = new List<ColorPixel>();
    private GameObject gameObject;
    private Canvas canvas;

    //--------------------------------------------------

    public Layer(string name, int index, Canvas canvas){

        this.name = name;
        this.index = index;

        Texture = new Texture2D(canvas.Size.x, canvas.Size.y)
        {
            filterMode = FilterMode.Point,
        };
    }

    public Layer(Texture2D texture, int index){

        Texture = texture;
        this.index = index;
        
        Texture.filterMode = FilterMode.Point;
    }

    public void Init(Canvas canvas){
        this.canvas = canvas;
        GenerateQuad();
    }

    public void OnUpdate(){
        UpdateTexture();
    }

    public Color GetPixel(Vector2Int pos){
        if (!IsInLayerBounds(pos)) { return default; }

        return Texture.GetPixel(pos.x, pos.y);
    }

    public void SetPixel(Vector2Int pos, Color color){
        if (!IsInLayerBounds(pos)) { return; }

        updatedPixels.Add(new ColorPixel(new Vector2Int(pos.x, canvas.Size.y - pos.y - 1), color));
    }

    public bool IsInLayerBounds(Vector2Int pos){
        if (pos.x < 0 || pos.x >= canvas.Size.x) { return false; }
        if (pos.y < 0 || pos.y >= canvas.Size.y) { return false; }
        return true;
    }

    public void Clear(){
        GameObject.Destroy(this.gameObject);
    }

    public void SetActive(bool value){
        gameObject.SetActive(value);
    }

    //---------------------------------------------

    private void UpdateTexture(){

        if (updatedPixels.Count == 0) { return; }

        //TODO: improve performance
        foreach (ColorPixel current in updatedPixels){
            Texture.SetPixel(current.pos.x, current.pos.y, current.color);
        }

        updatedPixels.Clear();
        Texture.Apply();
    }

    private void GenerateQuad(){

        // GameObject
        gameObject = new GameObject("Layer");
        gameObject.transform.SetParent(GameManager.Instance.gameObject.transform);
        gameObject.transform.position = Vector3.zero;
        gameObject.isStatic = true;

        // Mesh
        Mesh mesh = new Mesh();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Material material = new Material(Resources.Load<Shader>("UnlitTransparent"));
        material.mainTexture = Texture;
        meshRenderer.material = material;

        // Create data arrays
        Vector3[] vertices = new Vector3[4]; // Points in the grid (4 because a quad has 4 vertices)
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6]; // Defines the index of the vertices (6 because a quad had 6 sides (2 triangles))

        // Vertices from the bottom left, couterclockwise
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(canvas.Size.x, 0, 0);
        vertices[2] = new Vector3(0, canvas.Size.y, 0);
        vertices[3] = new Vector3(canvas.Size.x, canvas.Size.y, 0);

        // Triangle 1
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        // Triangle 2
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        // UV
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
