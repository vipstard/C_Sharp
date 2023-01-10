namespace LinkedList
{
    public class Program
    {
        //링크드 리스트는 특정 노드에서 노드를 삽입, 삭제하기 편리 하지만 ( O(1) ),
        //특정 노드를 검색하기 위해서는 O(n)의 시간이 소요된다.

        static void Main(string[] args)
        {
            LinkedList<String> list = new LinkedList<String>();

            list.AddLast("Apple");
            list.AddLast("Banana");
            list.AddLast("Lemon");

            LinkedListNode<String> node = list.Find("Banana");
            LinkedListNode<String> newNode = new LinkedListNode<string>("Grape");

            list.AddAfter(node, newNode); //Banana 뒤에 Grape를 추가한다.

            // Enumerator 리스트 출력
            foreach (var l in list)
            {
                Console.WriteLine(l);
            }

            Console.WriteLine();
            // 람다식 출력
            list.ToList<String>().ForEach(l=> Console.WriteLine(l));



        }
    }
}