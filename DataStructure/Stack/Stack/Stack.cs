namespace Stack
{
    // 2 C#  Stack 구현 
    // 한 쪽 끝에서만 자료를 넣고 뺄 수 있는 LIFO(Last In First Out) 형식의 자료 구조
    public class Stack
    {
        private int[] arr;
        private int top;


        static void Main(string[] args)
        {
            // 1 2 3 4 5 순으로 입력되지만 
            // 5 4 3 2 1 순으로 출력이 된다.
            Stack stack = new Stack(10);
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);
            stack.Push(5);

            for (int i = 0; i < 5; i++)
            {
                Console.Write(stack.Pop() + " ");
            }
        }

        public Stack(int capacity)
        {
            this.arr = new int[capacity];
            this.top = -1;
        }

        public void Push(int data)
        {
            if (IsFull()) throw new ApplicationException("Stack Is Full");
            this.arr[++top] = data;
        }

        public bool IsFull()
        {
            return top == this.arr.Length - 1;
        }

        public int Pop()
        {
            int returnData = -1;

            if (IsEmpty()) throw new ApplicationException("Stack Is Empty");
            else
            {
                returnData = this.arr[top];
                this.arr[top--] = 0;
            }

            return returnData;
        }

        public bool IsEmpty()
        {
            return top == -1;
        }

    }
}