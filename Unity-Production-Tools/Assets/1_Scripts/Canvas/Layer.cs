using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class Layer
{
    // Data
    [SerializeField] public int Index { get; private set; }
    [SerializeField] public Texture2D Texture { get; private set; }
    [SerializeField] public string Name { get; private set; }
    public bool Visable { get; private set; }

    public GameObject GameObject;
    private List<ColorPos> updatedPixels = new List<ColorPos>();
    private Vector2Int canvasSize;

    //--------------------------------------------------

    public Layer(string name, int index, Vector2Int canvasSize, Color background){

        this.Name = name;
        this.Index = index;
        this.canvasSize = canvasSize;

        Texture = new Texture2D(canvasSize.x, canvasSize.y)
        {
            filterMode = FilterMode.Point,
        };

        GenerateQuad();

        for (int y = 0; y < canvasSize.y; y++){
            for (int x = 0; x < canvasSize.x; x++){
                SetPixel(new Vector2Int(x, y), background, false);
            }   
        }
        UpdateTexture();
        EventManager.Invoke(Events.OnNewLayer, this);
    }

    // load Layer From Save
    public Layer(string name, int index, Vector2Int canvasSize, Texture2D texture){

        this.Name = name;
        this.Index = index;
        this.canvasSize = canvasSize;

        Texture = texture;
        Texture.filterMode = FilterMode.Point;
        
        GenerateQuad();
        EventManager.Invoke(Events.OnNewLayer, this);
    }

    public void Delete(){
        GameObject.Destroy(this.GameObject);
    }

    public Color GetPixel(Vector2Int pos){
        if (!IsInLayerBounds(pos)) { return default; }

        return Texture.GetPixel(pos.x, canvasSize.y - pos.y - 1);
    }

    public void SetPixel(Vector2Int pos, Color color, bool updateTexure){
        if (!IsInLayerBounds(pos)) { return; }

        updatedPixels.Add(new ColorPos(new Vector2Int(pos.x, canvasSize.y - pos.y - 1), color));
        if (updateTexure) UpdateTexture();
    }

    public bool IsInLayerBounds(Vector2Int pos){
        if (pos.x < 0 || pos.x >= canvasSize.x) { return false; }
        if (pos.y < 0 || pos.y >= canvasSize.y) { return false; }
        return true;
    }

    public void Clear(){
        GameObject.Destroy(this.GameObject);
    }

    public void SetActive(bool value){
        GameObject.SetActive(value);
    }

    public void SetIndex(int newIndex){
        Index = newIndex;
    }

    public void SetName(string newName){
        Name = newName;
    }

    public void Visability(bool value){
        Visable = value;

        if (value){
            GameObject.SetActive(true);
        }
        else{
            GameObject.SetActive(false);
        }
    }

    //---------------------------------------------

    private void UpdateTexture(){

        if (updatedPixels.Count == 0) return;

        foreach (ColorPos current in updatedPixels){
            Texture.SetPixel(current.pos.x, current.pos.y, current.color);
        }

        updatedPixels.Clear();
        Texture.Apply();
    }

    private void GenerateQuad(){

        // GameObject
        GameObject = new GameObject("LayerGameObject");
        GameObject.transform.SetParent(GameManager.Instance.gameObject.transform);
        GameObject.transform.position = new Vector3(0, 0, -Index);
        GameObject.isStatic = true;

        // Mesh
        Mesh mesh = new Mesh();
        MeshFilter meshFilter = GameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = GameObject.AddComponent<MeshRenderer>();
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
        vertices[1] = new Vector3(canvasSize.x, 0, 0);
        vertices[2] = new Vector3(0, canvasSize.y, 0);
        vertices[3] = new Vector3(canvasSize.x, canvasSize.y, 0);

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
