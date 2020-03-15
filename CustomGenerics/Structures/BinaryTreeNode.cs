using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class BinaryTreeNode<T>
    {
        public BinaryTreeNode<T> LeftSon { get; set; }
        public BinaryTreeNode<T> RightSon { get; set; }
        public BinaryTreeNode<T> Father { get; set; }
        public T Medicine { get; set; }
    }
}
