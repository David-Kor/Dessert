using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldNavigation : MonoBehaviour {
    private Field field;
    private GroundNode[,] allNode;

    public void InitFieldNavigation(Field _field)
    {
        field = _field;
        allNode = new GroundNode[field.sizeOfFieldWidth, field.sizeOfFieldHeight];
    }

    public void AddNode(GroundNode _groundNode, int i, int j) { allNode[i, j] = _groundNode; }

    public List<GroundNode> FindPath(GroundNode _start, GroundNode _dest)   //경로가 없으면 null반환
    {
        List<GroundNode> openList = new List<GroundNode>();
        List<GroundNode> closeList = new List<GroundNode>();
        List<GroundNode> path = new List<GroundNode>();
        GroundNode current = null;

        //시작노드 정보 초기화
        _start.SetParent(_start);
        _start.CalcGScore();
        _start.CalcDist(_dest);

        //openList 초기화
        openList.Add(_start);

        while (openList.Count > 0)
        {   //openList가 전부 비워질 때까지

            current = LowestFScoreInList(openList);     //openList에서 가장 적은 F비용 노드 선택

            if (current == _dest)
            {   //선택된 노드가 목적지인 경우
                CreatePath(_start, _dest, path);
                return path;
            }

            openList.Remove(current);   //선택한 노드를 openList에서 제거
            closeList.Add(current);     //closeList에 추가

            List<GroundNode> neighbors = FindNeighbors(current); //인접노드 탐색

            for (int i = 0; i < neighbors.Count; i++)
            {

                if (closeList.IndexOf(neighbors[i]) >= 0) { continue; } //인접노드가 closeList에 있다면 무시
                if (openList.IndexOf(neighbors[i]) < 0)
                {   //openList에 없다면 추가하고 부모를 선택 노드로 설정
                    openList.Add(neighbors[i]);
                    neighbors[i].SetParent(current);
                    //G스코어와 목적지까지의 거리를 계산하여 노드내에 정보 저장
                    neighbors[i].SetGScore(neighbors[i].CalcGScore());
                    neighbors[i].SetHScore(neighbors[i].CalcDist(_dest));
                }

                //시작노드에서 Neighbor까지의 gScore가 current를 거쳐서 Neighbor까지 가는 gScore비용보다 싸면 무시
                if (current.GetGScore() + current.CalcDistNeighbor(neighbors[i]) >= neighbors[i].GetGScore()) { continue; }

                //current를 거쳐서 가는 것이 더 좋다면 Neighbor의 부모를 current로 변경하고 비용을 다시 계산함
                neighbors[i].SetParent(current);
                neighbors[i].SetGScore(current.GetGScore() + current.CalcDistNeighbor(neighbors[i]));

            }

        }   //while문 종료

        //openList가 전부 비었으므로 경로가 존재하지 않음
        return null;
    }

    private GroundNode LowestFScoreInList(List<GroundNode> _openList)     //openList에서 가장 작은 F값을 가진 노드 탐색
    {
        int lowest = 0;

        for (int i = 1; i < _openList.Count; i++)
        {
            if (_openList[lowest].GetFScore() > _openList[i].GetFScore())
            {
                lowest = i;
            }
        }
        return _openList[lowest];
    }

    private void CreatePath(GroundNode _start, GroundNode _dest, List<GroundNode> _path)   //목적지로부터 부모를 추적하며 경로생성
    {

        GroundNode _parent = _dest.GetParent();
        _path.Add(_dest);

        while (_parent != _start)
        {
            _path.Add(_parent);
            _parent = _parent.GetParent();
        }

    }

    private List<GroundNode> FindNeighbors(GroundNode _ground)
    {
        int x, y;
        List<GroundNode> _neighbors = new List<GroundNode>();

        x = _ground.GetIndex()[0];
        y = _ground.GetIndex()[1];

        if (x > 0)
        {   //field 가장 왼쪽일 때
            if (allNode[x-1, y].IsPassable())
            {
                _neighbors.Add(allNode[x - 1, y]);
            }
        }
        if (x < field.sizeOfFieldWidth - 1)
        {   //field 가장 오른쪽일 때
            if (allNode[x + 1, y].IsPassable())
            {
                _neighbors.Add(allNode[x + 1, y]);
            }
        }
        if (y > 0)
        {   //field 가장 아래쪽일 때
            if (allNode[x, y - 1].IsPassable())
            {
                _neighbors.Add(allNode[x, y - 1]);
            }
        }
        if (y < field.sizeOfFieldHeight - 1)
        {   //field 가장 위쪽일 때
            if (allNode[x, y + 1].IsPassable())
            {
                _neighbors.Add(allNode[x, y + 1]);
            }
        }

        //대각선 움직임을 포함할 때만 사용
        /*if (x > 0)
        {

            if (y > 0)
            {
                if (allNode[x - 1, y].IsPassable() && allNode[x, y - 1].IsPassable() && allNode[x-1,y-1].IsPassable())
                {   //대각선 좌측하단 노드
                    _neighbors.Add(allNode[x - 1, y - 1]);
                }
            }
            if (y < field.sizeOfFieldHeight - 1)
            {
                if (allNode[x - 1, y].IsPassable() && allNode[x, y + 1].IsPassable() && allNode[x-1, y+1].IsPassable())
                {   //대각선 좌측상단 노드
                    _neighbors.Add(allNode[x - 1, y + 1]);
                }
            }

        }
        if (x < field.sizeOfFieldWidth - 1)
        {

            if (y > 0)
            {
                if (allNode[x + 1, y].IsPassable() && allNode[x, y - 1].IsPassable() && allNode[x + 1, y - 1].IsPassable())
                {   //대각선 우측하단 노드
                    _neighbors.Add(allNode[x + 1, y - 1]);
                }
            }
            if (y < field.sizeOfFieldHeight - 1)
            {
                if (allNode[x + 1, y].IsPassable() && allNode[x, y + 1].IsPassable() && allNode[x + 1, y + 1].IsPassable())
                {   //대각선 우측상단 노드
                    _neighbors.Add(allNode[x + 1, y + 1]);
                }
            }

        }*/

        return _neighbors;
    }
}
