using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Dungeon {
    public partial class DungeonGenerator : AbstractDungeonGenerator {
        [Header("Parameters")]
        [SerializeField] protected Vector2Int size = new Vector2Int(100, 100);
        [SerializeField] protected int roomQuantity = 25;
        [SerializeField] protected List<RoomData> roomTypes;
        [SerializeField] public List<GameObject> objects;
        [SerializeField] protected int seed;
        //[SerializeField] protected string seed;
        
        private const int HallwayWidth = 3;
        private const double PercentageOfEdges = .08;
        private Dungeon dungeon;
        private List<Edge> graph;
        private int[,] heatmap;

        protected override void runProceduralGeneration() {
            dungeon = new Dungeon();

            int randomSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(seed != 0 ? seed : randomSeed);
            Debug.Log(randomSeed);
            
            generateRooms();
            generateHallways();

            var tiles = dungeon.getFloors();
            Walls.generate(tiles, tilemap);
            generateSpikes(tiles);
            
            var spawnPoint = getSpawnPos();
            Instantiate(players[0], new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
            generateHeatmap(new Vector2Int(spawnPoint.x, spawnPoint.y), tiles);
            setExit();

            foreach (var room in dungeon.rooms) {
                if (!room.exit && Random.value < room.data.treasureRoomChance)
                    populateTreasureRoom(room);
                else
                    populateRoom(room, spawnPoint);
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
                    if (dungeon.exit != Vector2Int.zero || !room.data.playerCanExit || !(cost >= range))
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

        private void populateRoom(Room room, Point spawnPoint) {
            var chestCount = 0;

            int chestAmount = Random.Range(room.data.minChestAmount, room.data.maxChestAmount);

            while (chestCount != chestAmount) {
                var pos = room.floors.ElementAt(Random.Range(0, room.floors.Count));
                if (dungeon.hallways.Contains(pos) || hasAllNeighbors(pos) || !room.floors.Contains(pos + Vector2Int.down))
                    continue;
                Instantiate(room.data.chest, new Vector3(pos.x + .5f, pos.y + .5f, 0), Quaternion.identity);
                chestCount++;
            }
            if (room.center.Equals(spawnPoint))
                return;
            
            int enemyCount = Random.Range(room.data.minEnemyCount, room.data.maxEnemyCount);
            var count = 0;
            
            var enemyList = Random.Range(0, 3) switch {
                0 => room.data.undead,
                1 => room.data.orcs,
                2 => room.data.demons,
                _ => room.data.undead
            };

            while (count != enemyCount) {
                // TODO: Make sure enemies don't have the same positions
                var pos = room.floors.ElementAt(Random.Range(0, room.floors.Count));
                if (!hasAllNeighbors(room, pos))
                    continue;
                
                Instantiate(enemyList[Random.Range(0, enemyList.Count)], new Vector3(pos.x + .5f, pos.y + .5f, 0), Quaternion.identity);
                count++;
            }
        }

        private bool hasAllNeighbors(Vector2Int pos) {
            var floors = dungeon.getFloors();
            return floors.Contains(pos + Vector2Int.up) && floors.Contains(pos + Vector2Int.down) && floors.Contains(pos + Vector2Int.right) && floors.Contains(pos + Vector2Int.left);
        }

        private bool hasAllNeighbors(Room room, Vector2Int pos) {
            return room.floors.Contains(pos + Vector2Int.up) && room.floors.Contains(pos + Vector2Int.down) && room.floors.Contains(pos + Vector2Int.right) && room.floors.Contains(pos + Vector2Int.left);
        }

        private Point getSpawnPos() {
            return dungeon.rooms.Where(room => room.data.playerCanSpawn).Select(room => room.center).First();
        }
    }
}
