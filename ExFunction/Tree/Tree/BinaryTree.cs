using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; set; }

        // 트리 데이타 출력 예
        public void PreOrderTraversal(BinaryTreeNode<T> node)
        {
            if (node == null) return;

            Console.WriteLine(node.Data);
            PreOrderTraversal(node.Left);
            PreOrderTraversal(node.Right);
        }
    }
}
