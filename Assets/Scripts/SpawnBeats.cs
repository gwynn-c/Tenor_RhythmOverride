using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class SpawnBeats : MonoBehaviour
{
    private Conductor conductor;
    

    public GameObject beatImage;
    [SerializeField] private GameObject parentCanvas;
    void Start()
    {
        conductor = Conductor.Instance;
    }

   
     void SpawnBeatImage()
    { 
        GameObject currentBeat = Instantiate(beatImage, parentCanvas.transform);
        //Interpolate it and move it towards the center
        //if player presses the given Input(MouseBtn1) +- .5 seconds
        //Return success and continue whatever
        //else return fail and continue whatever
        Destroy(currentBeat, conductor.secondsPerBeat);
    }
     
}
