using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector3Int startPos;

    private bool moving = false;
    private List<GameObject> path;
    private int pathIndex;
    [SerializeField]
    private float moveDuration;
    private float timeElapsed = 0;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerTile = MapManager.Instance.getTileAtCoord(startPos);

        playerTile.GetComponent<TileOverlay>().setUnit(gameObject);
        transform.position = playerTile.transform.position;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timeElapsed = timeElapsed + Time.deltaTime;

            transform.position = Vector3.Lerp(initialPos, path[pathIndex].transform.position, timeElapsed / moveDuration);

            if (timeElapsed >= moveDuration)
            {
                timeElapsed = 0;
                initialPos = transform.position;
                ++pathIndex;
            }

            if (pathIndex == path.Count)
            {
                path.First().GetComponent<TileOverlay>().setUnit(null);
                path.Last().GetComponent<TileOverlay>().setUnit(gameObject);

                moving = false;
            }
        }
    }

    public void moveAlongPath(List<GameObject> path)
    {
        this.path = path;
        pathIndex = 1;

        moving = true;
    }

    public bool isMoving()
    {
        return moving;
    }
}
