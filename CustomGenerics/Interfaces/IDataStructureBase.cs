using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Interfaces
{
    public interface IDataStructureBase <T>
    {
        void Insert(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> newNode, Comparison<T> comparison);
        void Delete(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> value, Comparison<T> comparison);
    }
}
