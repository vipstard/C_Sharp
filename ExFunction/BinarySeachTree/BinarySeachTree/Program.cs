using BinarySearchTree;

namespace BinarySeachTree
{
    public class Program
    {
        static void Main(string[] args)
        {
            BST<int> bst = new BST<int>();

            bst.Insert(4);
            bst.Insert(2);
            bst.Insert(6);
            bst.Insert(1);
            bst.Insert(3);
            bst.Insert(7);
            bst.Insert(10);

            bst.PreOrderTraversal();
            Console.WriteLine();

            // .NET이 이진검색트리를 클래스를 public으로 제공하지 않지만,
            // 내부적으로 BST를 사용해 SortedDictionary를 구현하고 있다.

            SortedDictionary<int, string> tmap = new SortedDictionary<int, string>();
            tmap.Add(1001, "Tom");
            tmap.Add(1023, "John");
            tmap.Add(1010, "Irina");

            // Key 1010의 데이타 읽기
            string name1010 = tmap[1010];

            // Iterator 사용
            foreach (KeyValuePair<int, string> kv in tmap)
            {
                Console.WriteLine("{0}:{1}", kv.Key, kv.Value);
            }
        }
    }
}