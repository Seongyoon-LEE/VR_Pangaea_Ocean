using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;


public class GridManager : MonoBehaviour
{
    public Vector3 gridWorldSize = new Vector3(50, 50, 50);
    public float nodeRadius = 1f;
    public LayerMask terrainMask;

    Node[,,] grid;
    public bool IsGridSet
    {
        get; private set;
    } = false;
    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2; //노드 하나의 사이즈, 반지름의 두배
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);

    }
    private void OnEnable()
    {
        CreateGrid();
        this.IsGridSet = true;
    }
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ]; // 3D 배열로 그리드 생성
        Vector3 worldBottomLeft = transform.position
            - Vector3.right * gridWorldSize.x / 2
            - Vector3.up * gridWorldSize.y / 2
            - Vector3.forward * gridWorldSize.z / 2; // 중심점 기준으로 그리드의 맨 왼쪽, 맨 아래, 맨 앞쪽 위치 가져오기
                                                     // = 실제 사이즈의 절반만큼 다 빼기

        /*for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft
                        + Vector3.right * (x * nodeDiameter + nodeRadius)
                        + Vector3.up * (y * nodeDiameter + nodeRadius)
                        + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    //각 노드의 실제 위치(중심점) 구하기

                    bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, terrainMask); // 장애물에 해당하는 콜라이더가 없는것을 체크,
                                                                                               // 없어야 이동 가능
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z); // 값 추가
                }*/
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                //x랑 z축만 돌리면서, 위에서 아래로 레이 쏴서 닿은곳 아래로는 전부 장애물 판정
                //terrain은 아래에서 위로 쐈을때 충돌판정이 없다
                Vector3 worldPoint = worldBottomLeft
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (z * nodeDiameter + nodeRadius)
                    + Vector3.up * gridWorldSize.y;// 맨 위층 그리드들의 윗면 중심점


                RaycastHit hit;
                int y = 0;
                int nonObstacleCnt = gridSizeY;
                if (Physics.Raycast(worldPoint, Vector3.down * gridWorldSize.y, out hit, gridWorldSize.y, this.terrainMask))
                    nonObstacleCnt = (int)Vector3.Distance(worldPoint, hit.point) / (int)nodeDiameter;
                //Debug.DrawRay(worldPoint, Vector3.down * gridWorldSize.y, Color.red, 1000f);
                for (; y < nonObstacleCnt; y++)
                {
                    grid[x, gridSizeY - (y + 1), z] = new Node(true, worldPoint + Vector3.down * (y * nodeDiameter + nodeRadius), x, gridSizeY - (y + 1), z);
                }
                for (; y < gridSizeY; y++)
                {
                    grid[x, gridSizeY - (y + 1), z] = new Node(false, worldPoint + Vector3.down * (y * nodeDiameter + nodeRadius), x, gridSizeY - (y + 1), z);
                }
            }
        }
    }

    public List<Node> GetNeighbors(Node node) // 노드의 주변 노드들 가져오는것(최대 26개 가능)
    {
        List<Node> neighbors = new List<Node>();

        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dy == 0 && dz == 0) continue;

                    int nx = node.gridX + dx;
                    int ny = node.gridY + dy;
                    int nz = node.gridZ + dz;

                    if (nx >= 0 && nx < gridSizeX &&
                        ny >= 0 && ny < gridSizeY &&
                        nz >= 0 && nz < gridSizeZ)
                    {
                        neighbors.Add(grid[nx, ny, nz]);
                    }
                }
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = Mathf.Clamp01((worldPos.x - this.transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPos.y - this.transform.position.y + gridWorldSize.y / 2) / gridWorldSize.y);
        float percentZ = Mathf.Clamp01((worldPos.z - this.transform.position.z + gridWorldSize.z / 2) / gridWorldSize.z);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, y, z];
    }
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        foreach (var n in grid)
        {
            n.gCost = 0;
            n.hCost = 0;
            n.parent = null;
            n.HeapIndex = 0; // 노드들에 들어있던 heap인덱스(우선순위) 초기화
        }
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);
        Heap<Node> openSet = new Heap<Node>(gridSizeX * gridSizeY * gridSizeZ);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet.RemoveFirst();
            closedSet.Add(current);

            if (current == targetNode) //현재 위치가 목적지라면
                return RetracePath(startNode, targetNode); // 경로 셋팅 시작

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) // 이동 불가능하거나, 이미 클로즈셋에 있다면
                    continue;

                int newCost = current.gCost + GetDistance(current, neighbor); // 이동 코스트(실제 이동 거리 계산)
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor)) // 그게 이전보다 비용이 적거나, 아직 오픈셋에 없다면
                {
                    neighbor.gCost = newCost; // 이동 코스트에 넣고
                    neighbor.hCost = GetDistance(neighbor, targetNode); // 휴리스틱(예상 거리) 값 할당하고
                    neighbor.parent = current; // 현재 노드를 부모로 설정

                    if (!openSet.Contains(neighbor)) // 오픈셋에 없다면 추가
                        openSet.Add(neighbor);
                    else// 이미 있다면 업데이트(SortUp해서 위쪽에 있는 애들(부모 노드)와 비교해서 올바른 위치로 이동하도록)
                        openSet.UpdateItem(neighbor);
                }
            }
        }

        return null; // 경로 없음
    }

    List<Node> RetracePath(Node start, Node end) //길 찾기 끝난 이후에 경로 셋팅
    {
        List<Node> path = new List<Node>(); //경로 리스트 셋팅하고
        Node current = end; // 목적지부터 시작해서

        while (current != start) // 시작점이 아닌 동안
        {
            path.Add(current); //경로 리스트에 추가하고
            current = current.parent; // 현재 위치를 부모로 변경
        }
        path.Reverse(); //목적지부터 시작해서 길을 셋팅했으니 뒤집는다
        return path; // 반환
    }

    int GetDistance(Node a, Node b) //일정한 크기의 그리드로 했으니, 그냥 두 그리드 사이의 거리를 구한다.
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        int dz = Mathf.Abs(a.gridZ - b.gridZ);
        return dx + dy + dz;
    }
    public bool drawGizmo = false;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        if (grid == null || !this.drawGizmo) return;
        foreach (Node n in grid)
        {
            Gizmos.color = n.walkable ? Color.white : Color.red;
            if (!n.walkable)
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter * .8f));
        }
    }

    // 내부 Node 클래스
    [System.Serializable]
    public class Node : IHeapItem<Node>
    {
        public bool walkable; // 이동 가능 여부
        public Vector3 worldPosition; // 실제 좌표
        public int gridX, gridY, gridZ; // 그리드 상의 좌표
        public int gCost, hCost; // g코스트 : 이동코스트(실제 거리), h코스트 : 휴리스틱코스트(기대값)
        public Node parent; // 경로 셋팅할때 들어갈 부모(여기로 오기 전에 있을 노드)
        int heapIndex; // 힙에서의 인덱스(우선순위)

        public Node(bool walkable, Vector3 pos, int x, int y, int z)
        {
            this.walkable = walkable;
            this.worldPosition = pos;
            gridX = x; gridY = y; gridZ = z;
        }

        public int fCost { get { return gCost + hCost; } } // f코스트 : 코스트 두개 더한거

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }
        public int CompareTo(Node other)
        {
            int cmp = fCost.CompareTo(other.fCost); //g코스트 : 이동코스트(실제 거리), h코스트 : 휴리스틱코스트(기대값) f코스트 : 그 두개 더한거,
                                                    //보통 f코스트가 낮은게 비용이 더 저렴하다고 판단해서 우선순위를 높임
            if (cmp == 0) cmp = hCost.CompareTo(other.hCost);// f코스트가 같으면 h코스트로 비교
            return -cmp; // 더 낮은게 우선순위를 높게 해야하니 - 붙여서 반환
        }
    }
}