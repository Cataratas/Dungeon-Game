using System.Collections.Generic;
using Dungeon.Data;
using Dungeon.Utils.unity_delaunay_mst.Assets.Scripts.DungeonGen;
using UnityEngine;

namespace Dungeon {
    public partial class DungeonGenerator {
        private void generateRooms() {
            int roomCount = 0, tries = 0;
            while (roomCount != roomQuantity) {
                if (tries > 9999)
                    break;

                var p = roomTypes[Random.Range(0, roomTypes.Count)];
                var roomSize = new Vector2Int(Random.Range(p.minRoomWidth, p.maxRoomWidth), Random.Range(p.minRoomHeight, p.maxRoomHeight));
                var pos = new Vector2Int(Random.Range(startPos.x, startPos.x + size.x), Random.Range(startPos.y, startPos.y + size.y));
                var room = generateRoom(p, pos, roomSize);

                if (room.floors.Count > 0) {
                    dungeon.AddRoom(room);
                    roomCount++;
                } else
                    tries++;
            }
        }

        private Room generateRoom(RoomParameters p, Vector2Int roomPos, Vector2Int roomSize) {
            var room = new Room();
            var floor = new HashSet<Vector2Int>();

            var pos = roomPos + new Vector2Int((-roomSize.x / 2) - p.offset, (roomSize.y / 2) + p.offset);

            for (var i = 0; i < roomSize.x + p.offset * 2; i++) {
                pos += Vector2Int.right;
                floor.Add(pos);
                var vPos = pos;
                for (var j = 0; j < roomSize.y + p.offset * 2; j++) {
                    vPos += Vector2Int.down;
                    floor.Add(vPos);
                }
            }
            if (dungeon.collidesWithRooms(floor))
                return new Room();

            pos = roomPos + new Vector2Int((-roomSize.x / 2), (roomSize.y / 2));
            floor = new HashSet<Vector2Int>();
            for (var i = 0; i < roomSize.x; i++) {
                pos += Vector2Int.right;
                floor.Add(pos);
                var vPos = pos;
                for (var j = 0; j < roomSize.y; j++) {
                    vPos += Vector2Int.down;
                    floor.Add(vPos);
                }
            }
            room.center = new Point(roomPos.x + 1, roomPos.y);
            room.floors = floor;
            room.parameters = p;
            return room;
        }
    }
}
