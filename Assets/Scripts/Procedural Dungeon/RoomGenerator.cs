using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomWidthMin;
    private int roomLengthMin;
    public RoomGenerator(int maxIterations, int roomWidthMin, int roomLengthMin)
    {
        this.maxIterations = maxIterations;
        this.roomWidthMin = roomWidthMin;
        this.roomLengthMin = roomLengthMin;
    }

    public List<RoomNode> GenerateRoomInGivenSpaces(List<Node> roomSpaces, float roomBtmCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();

        foreach (var space in roomSpaces)
        {
            Vector2Int newBtmLeftPt = StructureHelper.GenerateBtmLefCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBtmCornerModifier, roomOffset
                ); 
            Vector2Int newTopRightPt = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset
                );
            space.BottomLeftAreaCorner = newBtmLeftPt;
            space.TopRightAreaCorner = newTopRightPt;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPt.x, newBtmLeftPt.y);
            space.TopLeftAreaCorner = new Vector2Int(newBtmLeftPt.x, newTopRightPt.y);
            listToReturn.Add((RoomNode)space);
        }
        return listToReturn;
    }
}