using System.Collections.Generic;
using UnityEngine;
public class DungeonGenerator
{
    private int dungeonWidth, dungeonLength;
    private List<RoomNode> allSpaceNodes = new List<RoomNode>();
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }
    public List<Node> CalculateDungeons(int maxIterations, int roomWidthMin, int roomLengthMin, float roomBtmCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomWidthMin, roomLengthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomInGivenSpaces(roomSpaces, roomBtmCornerModifier, roomTopCornerModifier, roomOffset);


        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridors(allSpaceNodes, corridorWidth);
        
        
        return new List<Node>(roomList);
    }
}