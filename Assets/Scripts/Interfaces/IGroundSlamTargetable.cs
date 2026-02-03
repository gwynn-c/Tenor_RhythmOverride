using UnityEngine;

public interface IGroundSlamTargetable
{
    public Transform GetTransform();
    public void DirectSlam();
    public void WithinSlamRadius(float distanceFromEpicenter);
}