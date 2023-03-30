using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;
using Dungeon.Data;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Dungeon {
    public partial class DungeonGenerator : AbstractDungeonGenerator {
        [Header("Parameters")]
        [SerializeField] protected Vector2Int size = new Vector2Int(100, 100);
        [SerializeField] protected int roomQuantity = 25;
        [SerializeField] protected List<RoomParameters> roomTypes;
        [SerializeField] protected int seed;
        //[SerializeField] protected string seed;
        
        private const int HallwayWidth = 3;
        private const double PercentageOfEdges = .08;
        private Dungeon dungeon;
        private List<Edge> graph;
        private int[,] heatmap;

        protected override void runProceduralGeneration() {
            dungeon = new Dungeon();
            heatmap = new int[size.x + 20, size.y + 20];
            graph = new List<Edge>();
            
            int randomSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(seed != 0 ? seed : randomSeed);
            Debug.Log(randomSeed);
            
            generateRooms();
            generateHallways();

            var tiles = dungeon.getFloors();
            Walls.generate(tiles, tilemap);
            generateSpikes(tiles);
            
            var spawnPos = getSpawnPos();
            generateHeatmap(new Vector2Int(spawnPos.x, spawnPos.y), tiles);
            
            tilemap.paintFloorTiles(tiles);
            tilemap.paintHeatmap(heatmap);
        }

        private Point getSpawnPos() {
            return dungeon.rooms.Where(room => room.parameters.playerCanSpawn).Select(room => room.center).FirstOrDefault();
        }
    }
}
