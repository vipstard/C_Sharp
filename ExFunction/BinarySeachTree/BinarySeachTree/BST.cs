namespace BinarySearchTree
{
    // BinarySearchTree Class
    public class BST<T>
    {
        private BinaryTreeNode<T> root = null;
        private Comparer<T> comparer = Comparer<T>.Default;
        
        //1. 루트 노드의 키와 찾고자 하는 값을 비교한다. 찾고자 하는 값이라면 탐색을 종료한다.
        //2. 찾고자 하는 값이 루트 노드의 키보다 작다면 왼쪽 서브 트리로 탐색을 진행한다.
        //3. 찾고자 하는 값이 루트노드의 키보다 크다면 오른쪽 서브트리로 탐색을 진행한다. 
        public void Insert(T val)
        {
            BinaryTreeNode<T> node = root;
            
            if(node ==null)
            {
                root = new BinaryTreeNode<T>(val);
                return;
            }

            while (node!=null)
            {
                // x 가 크면 -1, 같으면 0, x가 작으면 1
                int result = comparer.Compare(node.Data, val);
                if (result == 0)
                {
                    return;
                }
                else if (result > 0)
                {
                    if (node.Left == null)
                    {
                        node.Left = new BinaryTreeNode<T>(val);
                        return;
                    }

                    node = node.Left;
                }

                else
                {
                    if (node.Right == null)
                    {
                        node.Right = new BinaryTreeNode<T>(val);
                        return;
                    }

                    node = node.Right;
                }
            }
        }


        public void PreOrderTraversal()
        {
            PreOrderRecursive(root);
        }

        private void PreOrderRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return;
            Console.WriteLine(node.Data);
            PreOrderRecursive(node.Left);
            PreOrderRecursive(node.Right);
        }

    }

    internal class BinaryTreeNode<T>
    {
        public T Data { get; set; }
        public BinaryTreeNode<T>  Left { get; set; }
        public BinaryTreeNode<T>  Right { get; set; }

        public BinaryTreeNode(T data)
        {
            this.Data = data;
        }
    }
}
