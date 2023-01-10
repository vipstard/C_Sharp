namespace Tree
{
    public class Program
    {
        static void Main(string[] args)
        {
            BinaryTree<int> tree = new BinaryTree<int>();
            tree.Root = new BinaryTreeNode<int>(1);
            tree.Root.Left = new BinaryTreeNode<int>(2);
            tree.Root.Right = new BinaryTreeNode<int>(3);
            tree.Root.Left.Left = new BinaryTreeNode<int>(4);

            tree.PreOrderTraversal(tree.Root);

        }
    }
}