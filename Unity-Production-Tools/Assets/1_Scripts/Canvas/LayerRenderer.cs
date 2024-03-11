using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerRenderer
{
    private MeshRenderer[,] meshRenderers;
    private Vector2Int meshChunksSize;
    private Vector2Int meshChunkSize;
    private Material renderMaterial;

    // References
    private Layer layer;

    //-------------------------------------------------

    public LayerRenderer(Layer layer){
        this.layer = layer;
        meshChunkSize = ToolSettings.Instance.MeshChunkSize;
        renderMaterial = ToolSettings.Instance.RenderMaterial;

        #if UNITY_EDITOR
            if (meshChunkSize.x == 0 || meshChunkSize.y == 0) { Debug.LogError("DesiredMeshChunkSize in ToolSettings is 0"); }
        #endif

        meshChunksSize = new Vector2Int(layer.GetSize().x / meshChunkSize.x + 1, layer.GetSize().y / meshChunkSize.y + 1);
        meshRenderers = new MeshRenderer[meshChunksSize.x, meshChunksSize.y];

        GenerateCanvas();
    }

    public void UpdateCanvas(List<Vector2Int> positions){

    }

    //--------------------------------------------------

    private void GenerateCanvas(){
        for (int y = 0; y < meshChunksSize.y; y++){
            for (int x = 0; x < meshChunksSize.x; x++){
                GenerateMesh(meshChunkSize, new Vector2Int(x * meshChunkSize.x, y * meshChunkSize.y));
            }
        }
    }

    private void GenerateMesh(Vector2Int size, Vector2Int pos){
        
        // GameObject
        GameObject gameObject = new GameObject();
        gameObject.transform.SetParent(GameManager.Instance.gameObject.transform);
        gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
        gameObject.transform.name = "MeshChunk (" + pos.x / meshChunkSize.x + ", " + pos.y / meshChunkSize.y + ")";
        gameObject.isStatic = true;

        // Mesh
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.material = renderMaterial;
        Mesh mesh = new Mesh();

        // Create data arrays
        Vector3[] vertices = new Vector3[4 * size.x * size.y]; // Points in the grid (4 because a quad has 4 vertices)
        Color[] vertexColors = new Color[4 * size.x * size.y];
        int[] triangles = new int[6 * size.x * size.y]; // Defines the index of the vertices (6 because a quad had 6 sides (2 triangles))

        // Set Data
        int index = 0;
        int verticeIndex;
        int triangleIndex;
        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){

                // Vertices
                // This generates 4 vertices clockwise because a quad had 4 vertices
                // Vertices start rendering from the bottom left
                verticeIndex = index * 4; 
                vertices[verticeIndex + 0] = new Vector3(x    , -y    , 0);
                vertices[verticeIndex + 1] = new Vector3(x    , -y + 1, 0);
                vertices[verticeIndex + 2] = new Vector3(x + 1, -y + 1, 0);
                vertices[verticeIndex + 3] = new Vector3(x + 1, -y    , 0);

                // Vertex Colors
                Cell currentCell = layer.GetCell(new Vector2Int(pos.x + x, pos.y + y));
                if (currentCell != null){
                    vertexColors[verticeIndex + 0] = new Color(currentCell.Red / 255.0f, currentCell.Green / 255.0f, currentCell.Blue / 255.0f, currentCell.Alpha);
                    vertexColors[verticeIndex + 1] = new Color(currentCell.Red / 255.0f, currentCell.Green / 255.0f, currentCell.Blue / 255.0f, currentCell.Alpha);
                    vertexColors[verticeIndex + 2] = new Color(currentCell.Red / 255.0f, currentCell.Green / 255.0f, currentCell.Blue / 255.0f, currentCell.Alpha);
                    vertexColors[verticeIndex + 3] = new Color(currentCell.Red / 255.0f, currentCell.Green / 255.0f, currentCell.Blue / 255.0f, currentCell.Alpha);
                }
                else{
                    vertexColors[verticeIndex + 0] = Color.clear;
                    vertexColors[verticeIndex + 1] = Color.clear;
                    vertexColors[verticeIndex + 2] = Color.clear;
                    vertexColors[verticeIndex + 3] = Color.clear;
                }

                // Triangles
                // This assigns the order the vertices are connected
                triangleIndex = index * 6;
                // Triangle 1
                triangles[triangleIndex + 0] = verticeIndex + 0;
                triangles[triangleIndex + 1] = verticeIndex + 1;
                triangles[triangleIndex + 2] = verticeIndex + 2;
                // Triangle 2
                triangles[triangleIndex + 3] = verticeIndex + 0;
                triangles[triangleIndex + 4] = verticeIndex + 2;
                triangles[triangleIndex + 5] = verticeIndex + 3;


                index++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = vertexColors;

        meshFilter.gameObject.transform.position = new Vector3(pos.x, -pos.y - 1, 0);
        meshFilter.mesh = mesh;
    }
}
