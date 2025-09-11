using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxSize) //최초 셋팅
    {
        items = new T[maxSize]; //최대크기만큼 배열 생성
    }

    public void Add(T item) // 매개변수로 들어간 값을 추가
    {
        item.HeapIndex = currentItemCount; //heapIndex(우선순위)에 현재 개수 집어넣고(가장 낮은 우선순위)
        items[currentItemCount++] = item; //개수 1증가 후 해당 위치에 값 넣고
        SortUp(item); //위로 정렬
    }

    public T RemoveFirst() // 우선순위가 가장 높은 값을 빼냄
    {
        T first = items[0]; // 가장 우선순위가 높은 값을 먼저 first에 넣어둠(얕은 복사라서 주소가 복사됨)
                            // 즉 이 시점에서 first와 items[0]는 같은 값을 가리킴
        currentItemCount--; // 값 하나 뺄 예정이니 전체 개수 -1

        items[0] = items[currentItemCount]; // item의 0번째 위치에 가장 우선순위가 낮은 값 할당
                                            // 이 시점에서 item[0]와 item[currentItemCount]는 같은 값을 가리키고, first는 기존의 item[0]가
                                            // 가리키던 값을 그대로 가리키게 된다.
        items[0].HeapIndex = 0; // 가장 우선순위가 낮은 값의 우선순위를 0으로(가장 높은 우선순위)로 바꾼다.

        SortDown(items[0]); // 올바른 위치를 찾아가도록 아래로 정렬,
                            // 가장 우선순위가 높은 애를 첫번째 위치로 만들 수 있도록
        return first; // 기존의 first를 반환한다.
    }

    public void UpdateItem(T item) 
    {
        SortUp(item); //f코스트가 바뀌었을 때, 그거에 맞춰서 우선순위 재정렬
    }
    public bool Contains(T item) //매개변수로 들어온 값이 해당 우선순위 위치에 있는지 확인 
    {
        return items[item.HeapIndex].Equals(item); 
    }
    public int Count { get { return currentItemCount; } }
    

    void SortDown(T item) //매개변수로 들어온 값을 아래로 정렬
                          //(우선순위가 가장 높은 값을 빼고 난 후에 가장 우선순위가 낮은애를 위로 올려서 전체 정렬하는 용도)
    {
        while (true)
        {
            int left = item.HeapIndex * 2 + 1;
            int right = left + 1;
            //heap 인덱스가 0으로 시작해서, 루트노드 기준 0, 왼쪽은 1, 오른쪽은 2 / 1기준 왼쪽은 3, 오른쪽은 4
            //매개변수로 들어온 값의 왼쪽과 오른쪽 자식노드의 인덱스 구하기
            int swap = 0;
            //임시 인트값

            if (left < currentItemCount) //left가 전체개수보다 작다면(개수 전체 다 돌리기용)
            {
                swap = left;// swap쪽에 일단 할당
                if (right < currentItemCount &&
                    items[right].CompareTo(items[left]) > 0) // 오른쪽에 전체 개수보다 적고, 오른쪽의 f코스트가 왼쪽보다 작다면
                                                             // (GridManager쪽의 CompareTo참고) 즉 우선순위가 더 높다면
                    swap = right; //swap에 오른쪽 할당

                if (items[swap].CompareTo(item) > 0) //swap에 할당된 값의 f코스트가 매개변수로 들어온 item보다 작다면(우선순위가 더 높다면)
                    Swap(item, items[swap]); // 걔네 둘의 heap인덱스(우선순위)를 바꿈
                else return; // 만약 아니라면, 매개변수로 들어온 값이 올바른 자리에 들어갔다고 판단하고 종료
            }
            else return; // left가 전체개수보다 크다면(자식노드가 없다면) 종료
        }
    }

    void SortUp(T item)//매개변수로 들어온 값을 위로 정렬(새로 값을 추가했을 때)
    {
        int parent = (item.HeapIndex - 1) / 2; // 자식 1, 2인 기준 부모노드는 0일것이고,
                                               // 자식 3 4인 기준 부모노드는 1일것이고(sort down참고)
        while (true)
        {
            var parentItem = items[parent];
            if (item.CompareTo(parentItem) > 0) // 매개변수로 들어온 item의 f코스트가 부모노드보다 작다면(우선순위가 더 높다면)
                Swap(item, parentItem); // 둘의 heap인덱스(우선순위)를 바꿈
            else break; // 아니라면, 매개변수로 들어온 값이 올바른 자리에 들어갔다고 판단하고 종료
            parent = (item.HeapIndex - 1) / 2;// 인덱스가 바뀌었다면, 다시 그것의 부모노드를 구하고 재시작
        }
    }

    void Swap(T a, T b) // 두 매개변수의 우선순위를 바꾸는 내용
    {
        items[a.HeapIndex] = b;
        items[b.HeapIndex] = a;
        int temp = a.HeapIndex;
        a.HeapIndex = b.HeapIndex;
        b.HeapIndex = temp;
    }
}

public interface IHeapItem<T> : IComparable<T> // Node를 위한 인터페이스 셋팅(HeapIndex를 쓰기위해)
{
    int HeapIndex { get; set; }
}