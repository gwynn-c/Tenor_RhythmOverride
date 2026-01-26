using UnityEngine;

public class CorridorNode : Node
{

    private Node structure1;
    private Node structure2;
    private int corridorWidth;
    public CorridorNode(Node structure1, Node structure2, int corridorWidth) : base(null)
    {
        this.structure1 = structure1;
        this.structure2 = structure2;
        this.corridorWidth = corridorWidth;

        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        var relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.structure1, this.structure2);

                break;
        }
    }

    private void ProcessRoomInRelationUpOrDown(Node p0, Node p1)
    {
        throw new System.NotImplementedException();
    }

    private void ProcessRoomInRelationRightOrLeft(Node p0, Node p1)
    {
        throw new System.NotImplementedException();
    }

    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        Vector2 middlePointStructure1Temp = ((Vector2)structure1.TopRightAreaCorner + structure1.BottomLeftAreaCorner/2);
        Vector2 middlePointStructure2Temp = ((Vector2)structure2.TopRightAreaCorner + structure2.BottomLeftAreaCorner/2);

        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if (angle < 45 && angle >= 0 || angle > -45 && angle < 0)
        {
            return RelativePosition.Right;
        }
        else if (angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
    {
     
            return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y,
                middlePointStructure2Temp.x - middlePointStructure1Temp.x)*Mathf.Rad2Deg;
    }
}