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
            setExit();

            foreach (var room in dungeon.rooms) {
                if (!room.exit && Random.value < room.parameters.treasureRoomChance)
                    populateTreasureRoom(room);
                else
                    populateRoom(room);
                tilemap.paintRoomObjects(room.chests);
            }
            
            tilemap.paintFloorTiles(tiles);
            tilemap.paintHeatmap(heatmap);
        }

        // TODO: Make sure that exit room has only one hallway (ie. Edge)
        private void setExit() {
            var range = .99f;
            while (true) {
                var heatmapCost = getMinMaxHeatmap();
                foreach (var room in dungeon.rooms) {
                    float cost = Mathf.Clamp01((float) heatmap[room.center.x, room.center.y] / heatmapCost.Item2);
                    if (dungeon.exit != Vector2Int.zero || !room.parameters.playerCanExit || !(cost >= range))
                        continue;
                    dungeon.exit = new Vector2Int(room.center.x, room.center.y);
                    room.exit = true;
                    Debug.Log(dungeon.exit);
                    return;
                }
                range -= .01f;
            }
        }

        private static void populateTreasureRoom(Room room) {
            
        }

        private void populateRoom(Room room) {
            var chestCount = 0;

            int chestAmount = Random.Range(room.parameters.minChestAmount, room.parameters.maxChestAmount);

            while (chestCount != chestAmount) {
                var pos = room.floors.ElementAt(Random.Range(0, room.floors.Count));
                if (dungeon.hallways.Contains(pos) || hasAllNeighbors(room, pos) || !room.floors.Contains(pos + Vector2Int.down) || hasChestAt(room, pos + Vector2Int.up) || hasChestAt(room, pos + Vector2Int.down))
                    continue;
                room.chests.Add(new Chest(pos));
                chestCount++;
            }
        }

        private static bool hasChestAt(Room room, Vector2Int pos) {
            return room.chests.Any(chest => chest.pos == pos);
        }
        
        private static bool hasAllNeighbors(Room room, Vector2Int pos) {
            return room.floors.Contains(pos + Vector2Int.up) && room.floors.Contains(pos + Vector2Int.down) && room.floors.Contains(pos + Vector2Int.right) && room.floors.Contains(pos + Vector2Int.left);
        }

        private Point getSpawnPos() {
            return dungeon.rooms.Where(room => room.parameters.playerCanSpawn).Select(room => room.center).FirstOrDefault();
        }
    }
}
