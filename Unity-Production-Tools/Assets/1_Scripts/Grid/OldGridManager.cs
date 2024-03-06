// using System.Collections;
// using System.Collections.Generic;
// using Unity.Collections;
// using UnityEngine;

// public class GridManager : IUpdateable
// {
//     private Cell[][][][] grid; // First [] is chunkIndex.x, the second [] is chunkIndex.y, the third [] is pos.x and the fourth [] is pos.y
//     public Vector2Int GridSize { get; private set; } // Size of grid in cells
//     public Vector2Int ChunkGridSize { get; private set; } // Size of grid in chunks
//     public Vector2Int ChunkSize { get; private set; } // Size of 1 chunk

//     private CellGridRenderer gridRenderer;
//     private List<Vector2Int>[,] changedCells;

//     // References
//     private GameManager gameManager;
//     private InputManager inputManager;

//     //-------------------------------------------------

//     public GridManager(GameManager gameManager, InputManager inputManager){

//         this.gameManager = gameManager;
//         this.inputManager = inputManager;
//         inputManager.OnLeftMouse += OnLeftMouse;

//         SetGridSizes();
//         InitChangedCells();
//         InitGrid();

//         mapGenerator = new MapGenerator(this);
//         mapGenerator.Generate();

//         gridRenderer = new CellGridRenderer(this, gameManager);
//     }

//     public void OnUpdate(){

//         // StressTest
//         for (int y = 0; y < ChunkGridSize.y; y++){
//             for (int x = 0; x< ChunkGridSize.x; x++){
//                 changedCells[x, y].Add(Vector2Int.zero);
//             }
//         }

//         gridRenderer.Update(changedCells);
//     }

//     public Cell GetCell(Vector2Int pos){
//         if (!IsInGridBounds(pos)) { Debug.LogWarning("Tried to get cell (" + pos.x + ", " + pos.y + ") but its outside the gridbounds"); return null; }

//         Vector2Int chunkIndex = CellPosToChunkIndex(pos);
//         Vector2Int cellChunkPos = CellPosToChunkPos(pos);
//         return grid[chunkIndex.x][chunkIndex.y][cellChunkPos.x][cellChunkPos.y];
//     }

//     public void SetCellType(Vector2Int pos, CellTypes cellType, bool updateChunk){
//         Cell currentCell = GetCell(pos);

//         if (currentCell != null){
//             currentCell.SetType(cellType);

//             if (updateChunk){
//                 Vector2Int chunkIndex = CellPosToChunkIndex(pos);
//                 changedCells[chunkIndex.x, chunkIndex.y].Add(pos);
//             }
//         }
//     }

//     public bool IsInGridBounds(Vector2Int pos){
//         if (pos.x < 0 || pos.x >= GameSettings.Instance.GridSize.x) { return false; }
//         if (pos.y < 0 || pos.y >= GameSettings.Instance.GridSize.y) { return false; }
//         return true;
//     }

//     public Vector2Int GetSize(){
//         return GridSize;
//     }



//     //--------------------------------------------------------

//     private void InitGrid(){

//         // Allocate 4D array of Cells
//         grid = new Cell[ChunkGridSize.x][][][];
//         for (int i = 0; i < ChunkGridSize.x; i++){
//             grid[i] = new Cell[ChunkGridSize.y][][];
//             for (int j = 0; j < ChunkGridSize.y; j++){
//                 grid[i][j] = new Cell[ChunkSize.x][];
//                 for (int k = 0; k < ChunkSize.x; k++){
//                     grid[i][j][k] = new Cell[ChunkSize.y];
//                     for (int l = 0; l < ChunkSize.y; l++){
//                         grid[i][j][k][l] = new Cell();
//                     }
//                 }
//             }
//         }
//     }

//     private void InitChangedCells(){

//         changedCells = new List<Vector2Int>[ChunkGridSize.x, ChunkGridSize.y];

//         for (int y = 0; y < ChunkGridSize.y; y++){
//             for (int x = 0; x< ChunkGridSize.x; x++){
//                 changedCells[x, y] = new List<Vector2Int>();
//             }
//         }
//     }

//     private void SetGridSizes(){
//         GridSize = GameSettings.Instance.GridSize;
//         Vector2Int desiredChunkSize = GameSettings.Instance.ChunkSize;
//         ChunkSize = CalcChunkSize(GridSize, desiredChunkSize);
//         ChunkGridSize = new Vector2Int(GridSize.x / ChunkSize.x, GridSize.y / ChunkSize.y);
//     }

//     private void OnLeftMouse(){
//         Vector3 mouseScreenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         SetCellType(new Vector2Int((int)mouseScreenToWorldPoint.x, -(int)mouseScreenToWorldPoint.y), CellTypes.vacuum, true);
//     }


// }