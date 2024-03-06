// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class CellGridRenderer
// {
//     private MeshFilter[,] meshFilters;
//     private Dictionary<CellTypes, Vector2> atlasUV00 = new Dictionary<CellTypes, Vector2>();
//     private Dictionary<CellTypes, Vector2> atlasUV11 = new Dictionary<CellTypes, Vector2>();

//     // References
//     private GridManager gridManager;
//     private GameManager gameManager;

//     //-----------------------------------------------

//     public CellGridRenderer(GridManager gridManager, GameManager gameManager){
//         this.gridManager = gridManager;
//         this.gameManager = gameManager;

//         InitAtlas();
//         InitMeshGrid();
//     }

//     // The [,] in the parameter is in what chunk the list is
//     public void Update(List<Vector2Int>[,] changedCellsList){

//         for (int y = 0; y < gridManager.ChunkGridSize.y; y++){
//             for (int x = 0; x < gridManager.ChunkGridSize.x; x++){
//                 List<Vector2Int> changedCells = changedCellsList[x, y];

//                 if (changedCells.Count != 0){
//                     UpdateChunkMesh(changedCells, meshFilters[x, y]);
//                     changedCells.Clear();
//                 }
//             }
//         }
//     }

//     //---------------------------------------------------------------

//     private void InitMeshGrid(){
//         meshFilters = new MeshFilter[gridManager.ChunkGridSize.x, gridManager.ChunkGridSize.y];

//         // Instance MeshFilters
//         for (int y = 0; y < gridManager.ChunkGridSize.y; y++){
//             for (int x = 0; x < gridManager.ChunkGridSize.x; x++){
//                 GameObject newMesh = new GameObject("MeshGrid(" + x + ", " + y + ")");
//                 newMesh.transform.SetParent(gameManager.transform);
//                 MeshRenderer meshRenderer = newMesh.AddComponent<MeshRenderer>();
//                 meshRenderer.material = GameSettings.Instance.Atlas;
//                 meshFilters[x, y] = newMesh.AddComponent<MeshFilter>();
//             }
//         }

//         // Generate inital Meshes
//         for (int y = 0; y < gridManager.ChunkGridSize.y; y++){
//             for (int x = 0; x < gridManager.ChunkGridSize.x; x++){
//                 GenerateChunkMesh(meshFilters[x, y], gridManager.ChunkSize, gridManager.GetChunkOrgin(new Vector2Int(x, y)));
//             }
//         }
//     }

//     private void InitAtlas(){
//         int cellTypeAmount = Enum.GetNames(typeof(CellTypes)).Length;
//         int index = 0;
//         for (int y = 0; y < GameSettings.Instance.AtlasSize.y; y += GameSettings.Instance.TextureSize.y){
//             for (int x = 0; x < GameSettings.Instance.AtlasSize.x; x += GameSettings.Instance.TextureSize.x){
//                 if (index >= cellTypeAmount) { break; }
                
//                 Vector2 uv00 = AtlasPosToUV00(new Vector2Int(x, y));
//                 Vector2 uv11 = AtlasPosToUV11(new Vector2Int(x, y));

//                 uv00 += GameSettings.Instance.UVFloatErrorMargin;
//                 uv11 -= GameSettings.Instance.UVFloatErrorMargin;

//                 atlasUV00.Add((CellTypes)index, uv00);
//                 atlasUV11.Add((CellTypes)index, uv11);

//                 index++; 
//             }
//         }
//     }

//     private void UpdateChunkMesh(List<Vector2Int> changedCellsInChunk, MeshFilter meshFilter){

//         Vector2[] uv = meshFilter.mesh.uv;

//         foreach (Vector2Int currentCell in changedCellsInChunk){
 
//             Vector2Int cellChunkPos = gridManager.CellPosToChunkPos(currentCell);

//             int index = cellChunkPos.x + (cellChunkPos.y * gridManager.ChunkSize.y); 
//             int verticeIndex = 4 * index; 

//             // UV
//             // UV's start rendering from the bottom left (UV00 is bottom left and UV11 is top right)
//             CellTypes currentCellType = gridManager.GetCell(currentCell).CellType;
//             atlasUV00.TryGetValue(currentCellType, out Vector2 uv00);
//             atlasUV11.TryGetValue(currentCellType, out Vector2 uv11);

//             uv[verticeIndex + 0] = new Vector2(uv00.x, uv00.y);
//             uv[verticeIndex + 1] = new Vector2(uv11.x, uv00.y);
//             uv[verticeIndex + 2] = new Vector2(uv00.x, uv11.y);
//             uv[verticeIndex + 3] = new Vector2(uv11.x, uv11.y);
//         }

//         meshFilter.mesh.uv = uv;
//     }

//     private void GenerateChunkMesh(MeshFilter meshFilter, Vector2Int size, Vector2Int pos){
        
//         Mesh mesh = new Mesh();

//         // Create data arrays
//         Vector3[] vertices = new Vector3[4 * size.x * size.y]; // Points in the grid (4 because a quad has 4 vertices)
//         Vector2[] uv = new Vector2[4 * size.x * size.y]; // Part of texture that matches with vertice (uv lenght is the same is vertices lenght)
//         int[] triangles = new int[6 * size.x * size.y]; // Defines the index of the vertices (6 because a quad had 6 sides (2 triangles))

//         int index = 0;
//         int verticeIndex = 0;
//         int triangleIndex = 0;
//         for (int y = 0; y < size.y; y++){
//             for (int x = 0; x < size.x; x++){

//                 // Vertices
//                 // This generates 4 vertices clockwise because a quad had 4 vertices
//                 // Vertices start rendering from the bottom left
//                 verticeIndex = index * 4; // 
//                 vertices[verticeIndex + 0] = new Vector3(x    , -y    , 0);
//                 vertices[verticeIndex + 1] = new Vector3(x    , -y + 1, 0);
//                 vertices[verticeIndex + 2] = new Vector3(x + 1, -y + 1, 0);
//                 vertices[verticeIndex + 3] = new Vector3(x + 1, -y    , 0);

//                 // UV
//                 // UV's start rendering from the bottom left (UV00 is bottom left and UV11 is top right)
//                 CellTypes currentCellType = gridManager.GetCell(new Vector2Int(pos.x + x, pos.y + y)).CellType;
//                 atlasUV00.TryGetValue(currentCellType, out Vector2 uv00);
//                 atlasUV11.TryGetValue(currentCellType, out Vector2 uv11);

//                 uv[verticeIndex + 0] = new Vector2(uv00.x, uv00.y);
//                 uv[verticeIndex + 1] = new Vector2(uv11.x, uv00.y);
//                 uv[verticeIndex + 2] = new Vector2(uv00.x, uv11.y);
//                 uv[verticeIndex + 3] = new Vector2(uv11.x, uv11.y);

//                 // Triangles
//                 // This assigns the order the vertices are connected
//                 triangleIndex = index * 6;
//                 // Triangle 1
//                 triangles[triangleIndex + 0] = verticeIndex + 0;
//                 triangles[triangleIndex + 1] = verticeIndex + 1;
//                 triangles[triangleIndex + 2] = verticeIndex + 2;
//                 // Triangle 2
//                 triangles[triangleIndex + 3] = verticeIndex + 0;
//                 triangles[triangleIndex + 4] = verticeIndex + 2;
//                 triangles[triangleIndex + 5] = verticeIndex + 3;

//                 index++;
//             }
//         }

//         mesh.vertices = vertices;
//         mesh.uv = uv;
//         mesh.triangles = triangles;

//         meshFilter.gameObject.transform.position = new Vector3(pos.x, -pos.y - 1, 0);
//         meshFilter.mesh = mesh;
//     }

//     private Vector2 AtlasPosToUV00(Vector2Int pos){
//         float xUV = Map(pos.x, 0, GameSettings.Instance.AtlasSize.x, 0, 1);
//         float yUV = Map(pos.y + GameSettings.Instance.TextureSize.y, 0, GameSettings.Instance.AtlasSize.y, 1, 0);
//         return new Vector2(xUV, yUV);
//     }

//     private Vector2 AtlasPosToUV11(Vector2Int pos){
//         float xUV = Map(pos.x + GameSettings.Instance.TextureSize.x, 0, GameSettings.Instance.AtlasSize.x, 0, 1);
//         float yUV = Map(pos.y, 0, GameSettings.Instance.AtlasSize.y, 1, 0);
//         return new Vector2(xUV, yUV);
//     }
    
//     private float Map(float value, float inMin, float inMax, float outMin, float outMax){
//         return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
//     }
// }
