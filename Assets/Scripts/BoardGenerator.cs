using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class BoardGenerator : MonoBehaviour {
    public static BoardGenerator instance;

    private GameObject cornerHolder, edgeHolder, tileHolder;
    private Grid grid;
    public GameObject cornerSetter, roadSetter;

    private Dictionary<Vector3, GameObject> vertexPort = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, GameObject> roadPorts = new Dictionary<Vector3, GameObject>();

    private int tileCount = 0;
    [System.Serializable]
    public struct Tile {
        public int minInstances;
        public GameObject hexTile;
        public int maxInstances;
    }

    [System.Serializable]
    public struct RingConfiguration {
        public int ring;
        public List<Tile> tiles;
    }

    [SerializeField]
    public List<RingConfiguration> ringConfigurations;

    private List<Vector2Int> allTilesDown;
    [SerializeField]
    private HexagonalMapConfig mapConfig;
    [SerializeField]
    private NumberMapConfig numberConfig;
    [SerializeField]
    private ResourceDistributor resourceDistributor;

    void Start() {
        instance = this;

        GameObject tilesFolder = new GameObject("Tiles");
        tileHolder = new GameObject("Tiles Holder");
        cornerHolder = new GameObject("Corner Holder");
        edgeHolder = new GameObject("Edge Holder");

        tileHolder.transform.SetParent(tilesFolder.transform);
        cornerHolder.transform.SetParent(tilesFolder.transform);
        edgeHolder.transform.SetParent(tilesFolder.transform);

        grid = GetComponent<Grid>();
        CreateRings();
    }

    public void DisableRoadPlacement() {
        edgeHolder.SetActive(false);
    }
    public void DisableSettlePlacement() {
        cornerHolder.SetActive(false);
    }
    public void EnableRoadPlacement() {
        edgeHolder.SetActive(true);

    }
    public void EnableSettlePlacement() {
        cornerHolder.SetActive(true);
    }

    private void CreateRings() {

        allTilesDown = new List<Vector2Int>() { new Vector2Int(0, 0) };

        // Tile in ring starting with center tile
        List<Vector2Int> currentTilesInRing = new List<Vector2Int>() { new Vector2Int(0, 0) };

        for (int ringCount = 0; ringCount < ringConfigurations.Count; ringCount++) {
            List<Vector2Int> nextTilesInRing = new List<Vector2Int>();

            for (int tileInRing = 0; tileInRing < currentTilesInRing.Count; tileInRing++) {
                nextTilesInRing.AddRange(GetSurroundingVertices(currentTilesInRing[tileInRing]));
            }

            currentTilesInRing = nextTilesInRing.Except(allTilesDown).ToList();

            AddNewRingTiles(currentTilesInRing, ringCount);

            allTilesDown.AddRange(currentTilesInRing);
        }
    }

    Vector2Int[] GetSurroundingVertices(Vector2Int center) {
        // Define the six neighbors' relative positions in a hexagon grid
        Vector3Int[] neighborOffsets;

        if (center.y % 2 == 0) {
            neighborOffsets = new Vector3Int[]
            {
            new Vector3Int(-1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, 0)
            };
        } else {
            neighborOffsets = new Vector3Int[]
            {
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(0, 1, 0)
            };
        }

        Vector2Int[] vertices = new Vector2Int[6];

        for (int i = 0; i < 6; i++) {
            Vector3Int offset = neighborOffsets[i];
            vertices[i] = new Vector2Int(center.x + offset.x, center.y + offset.y);
        }

        return vertices;
    }
    private void GetTilesSixVertices(GameObject tile) {
        float cornerHeight = (grid.cellSize.x / 2) / Mathf.Tan(1.047f);

        float halfWidth = grid.cellSize.x / 2;
        float halfHeight = grid.cellSize.y / 2;

        Vector3 center = tile.transform.position;

        // Calculate positions for all six vertices
        Vector3[] vertexOffsets = new Vector3[] {
        new Vector3(halfHeight, 0, 0),
        new Vector3(cornerHeight, 0, halfWidth),
        new Vector3(-cornerHeight, 0, halfWidth),
        new Vector3(-halfHeight , 0, 0),
        new Vector3(-cornerHeight, 0, -halfWidth),
        new Vector3(cornerHeight, 0, -halfWidth)
    };

        // Instantiate objects at each vertex
        foreach (Vector3 offset in vertexOffsets) {

            Vector3 roundedVector = new Vector3(
                Mathf.Round((center.x + offset.x) * 10) / 10f,
                Mathf.Round((center.y + offset.y) * 10) / 10f,
                Mathf.Round((center.z + offset.z) * 10) / 10f
            );

            // Check if the rounded vector already exists in the dictionary
            bool found = false;
            foreach (Vector3 existingVector in vertexPort.Keys) {
                if (Vector3.Distance(roundedVector, existingVector) <= 0.25f) {
                    AssignCornerToTile(tile, vertexPort[existingVector]);
                    found = true;
                    break;
                }
            }

            // If the rounded vector is not found, instantiate a new corner object
            if (!found) {
                GameObject tileVertex = Instantiate(cornerSetter, center + offset, Quaternion.identity, cornerHolder.transform);
                vertexPort[roundedVector] = tileVertex;

                AssignCornerToTile(tile, tileVertex);
            }
        }
    }


    private void GetTilesSixSides(GameObject tile) {
        
        TileGeometryTracker tileCornerTracker = tile.GetComponent<TileGeometryTracker>();

        if (tileCornerTracker != null) {
            List<GameObject> corners = tileCornerTracker.tileCorner;
            int numCorners = corners.Count;

            // Instantiate road setters between each pair of corners
            for (int i = 0; i < numCorners; i++) {
                GameObject cornerA = corners[i];
                GameObject cornerB = corners[(i + 1) % numCorners]; // Wrap around to the first corner for the last corner

                // Calculate position for road setter (in the middle between corners)
                Vector3 roadSetterPos = (cornerA.transform.position + cornerB.transform.position) / 2f;

                // Round the roadSetterPos vector to a smaller precision
                roadSetterPos = new Vector3(
                    Mathf.Round(roadSetterPos.x * 100f) / 100f,
                    Mathf.Round(roadSetterPos.y * 100f) / 100f,
                    Mathf.Round(roadSetterPos.z * 100f) / 100f
                );


                if (roadPorts.ContainsKey(roadSetterPos)) {
                    AssignRoadToTile(tile, roadPorts[roadSetterPos]);
                } else {
                    // Instantiate road setter
                    GameObject road = Instantiate(roadSetter, roadSetterPos, Quaternion.identity, edgeHolder.transform);

                    // Calculate direction from road setter to one of the corners (e.g., cornerA)
                    Vector3 directionToCorner = cornerA.transform.position - roadSetterPos;
                    Quaternion rotation = Quaternion.LookRotation(directionToCorner, Vector3.up);

                    // Apply rotation to road setter
                    road.transform.rotation = rotation;

                    roadPorts[roadSetterPos] = road;

                    AssignRoadToTile(tile, road);
                    AssignEdgeToCorners(road, cornerA, cornerB);
                }
            }
        } else {
            Debug.LogWarning("Problem during map generation: no tile corner tracker found on tile");
        }
        
    }


    private void AssignEdgeToCorners(GameObject road, GameObject cornerA, GameObject cornerB) {
        TileGeometryTracker.cornersEdges[new System.Tuple<GameObject, GameObject>(cornerA, cornerB)] = road;
        TileGeometryTracker.cornersEdges[new System.Tuple<GameObject, GameObject>(cornerB, cornerA)] = road;
    }

    private void AssignRoadToTile(GameObject tile, GameObject road) {
        TileGeometryTracker tileCornerTracker = tile.GetComponent<TileGeometryTracker>();

        if (tileCornerTracker != null) {
            tileCornerTracker.tileEdges.Add(road);
        } else {
            Debug.LogWarning("Problem during map generation: no tile corner tracker found on tile");
        }
    }


    private void AssignCornerToTile(GameObject tile, GameObject corner) {
        TileGeometryTracker tileCornerTracker = tile.GetComponent<TileGeometryTracker>();

        if (tileCornerTracker != null) {
            tileCornerTracker.tileCorner.Add(corner);
        } else {
            Debug.LogWarning("Problem during map generation: no tile corner tracker found on tile");
        }
    }

    private void AddNewRingTiles(List<Vector2Int> newTilePositions, int ring) {
        foreach (Vector2Int newTile in newTilePositions) {
            GameObject createdTile = Instantiate(mapConfig.hexFields[tileCount], grid.CellToWorld(new Vector3Int(newTile.x, newTile.y, 0)), Quaternion.Euler(0, 30, 0), tileHolder.transform);
            GameObject createdNumberChip = numberConfig.CreateChip(mapConfig.numberFields[tileCount]);
            createdNumberChip.transform.SetParent(createdTile.transform);
            createdNumberChip.transform.localPosition = Vector3.zero;

            resourceDistributor.numberTile.Add(mapConfig.numberFields[tileCount], createdTile);

            GetTilesSixVertices(createdTile);
            GetTilesSixSides(createdTile);
            tileCount++;
        }
    }


}
