using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public GameObject NodePrefab;
    public Vector2 Destination;
    public bool IsImpulsed = false;
    public bool IsSwinging = false;
    public LayerMask RopeLayerMask;
    public float ImpulseForce;
    public bool HookMissed;
    public bool _isRopeLoosed {get; private set;} = false;

    [SerializeField]
    private float _speed = 1.0f;
    [SerializeField]
    private float _distanceBetweenNodes = 2f;
    [SerializeField]
    private float _ropeCooldown;

    public bool _hookFinishTravel { get; private set; } = true;

    private GameObject _player;
    private GameObject _lastNode;
    private List<GameObject> _nodesList = new List<GameObject>();
    private int _nbrOfNodes = 2;
    private LineRenderer _lineRenderer;
    private List<Vector2> ropePositions = new List<Vector2>();



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _lineRenderer = GetComponent<LineRenderer>();
        _lastNode = transform.gameObject;
        _nodesList.Add(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, Destination, _speed);

        if((Vector2)transform.position != Destination && !_isRopeLoosed)
        {
            if(Vector2.Distance(_player.transform.position, _lastNode.transform.position) > _distanceBetweenNodes)
            {
                CreateNode();
            }
        }else if (_hookFinishTravel == true)
        {
            _hookFinishTravel = false;
            while (Vector2.Distance(_player.transform.position, _lastNode.transform.position) > _distanceBetweenNodes)
            {
                CreateNode();
            }
            _lastNode.GetComponent<HingeJoint2D>().connectedBody = _player.GetComponent<Rigidbody2D>();

            if (IsImpulsed == true) 
            {
                StartCoroutine(WaitForThrowingPlayer(_ropeCooldown));
            }else if(IsSwinging && !FindObjectOfType<GroundCheckScript>()._playerInAir)
            {
                StartCoroutine(WaitForThrowingPlayer(_ropeCooldown));
            }
            else if(HookMissed == true)
            {
                LooseRope();
                FindObjectOfType<HookLauncher>().CanThrow = true;
            }
            else
            {
                foreach (GameObject go in _nodesList)
                {
                    go.gameObject.GetComponent<Rigidbody2D>().mass = 1f;
                    go.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2f;
                }
                FindObjectOfType<HookLauncher>().LooseRope();

            }

        }

        DrawLine();
    }

    private void ResetRope()
    {
        FindObjectOfType<HookLauncher>().LooseRope();
    }

    void DrawLine()
    {
        _lineRenderer.positionCount = _nbrOfNodes;

        int i;
        for(i = 0; i<_nodesList.Count; i++)
        {
            _lineRenderer.SetPosition(i, _nodesList[i].transform.position);
        }

        if(!_isRopeLoosed)
        _lineRenderer.SetPosition(i,_player.transform.position);
    }

    void CreateNode()
    {
        _nbrOfNodes++;

        Vector2 positionToCreate = _player.transform.position - _lastNode.transform.position;
        positionToCreate.Normalize();
        positionToCreate *= _distanceBetweenNodes;
        positionToCreate += (Vector2)_lastNode.transform.position;

        GameObject newNode = (GameObject)Instantiate(NodePrefab, positionToCreate, Quaternion.identity);

        newNode.transform.SetParent(transform);

        _lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();
        _lastNode = newNode;
        _nodesList.Add(_lastNode);
    }

    public void LooseRope()
    {
        _isRopeLoosed = true;
        FindObjectOfType<HookLauncher>().LooseRope();
        _lastNode.GetComponent<HingeJoint2D>().enabled = false;
        foreach (GameObject go in _nodesList)
        {
            go.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            go.gameObject.GetComponent<Rigidbody2D>().mass = 1f;
        }
        _nbrOfNodes--;
    }

    private void ThrowPlayer()
    {
        ////Ajout d'une force en direction du hook
        var playerToHookDirection = (Destination - (Vector2)_player.transform.position).normalized;
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (IsImpulsed) { 
        _player.GetComponent<Rigidbody2D>().AddForce(playerToHookDirection * ImpulseForce, ForceMode2D.Impulse);
        IsImpulsed = false;
        foreach (GameObject go in _nodesList)
        {
            go.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
        StartCoroutine(DestroyHookCoroutine());
        }
        else
        {
            foreach (GameObject go in _nodesList)
            {
                go.gameObject.GetComponent<Rigidbody2D>().mass = 1f;
                go.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2f;
            }

            var perpendicularDirection = Vector2.zero;
            if (Destination.x > _player.transform.position.x) { 
                perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
            }
            else
            {
                perpendicularDirection = new Vector2(-playerToHookDirection.y, -playerToHookDirection.x);
            }
            _player.GetComponent<Rigidbody2D>().AddForce(perpendicularDirection * ImpulseForce/2, ForceMode2D.Impulse);
        }
    }

    IEnumerator DestroyHookCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        LooseRope();
    }

    IEnumerator WaitForThrowingPlayer(float cd)
    {
        yield return new WaitForSeconds(cd);
        ThrowPlayer();
    }

}
