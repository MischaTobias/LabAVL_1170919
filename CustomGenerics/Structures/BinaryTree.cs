using CustomGenerics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class BinaryTree<T> : IDataStructureBase<T>
    {
        public BinaryTreeNode<T> root;
        private List<BinaryTreeNode<T>> returningList = new List<BinaryTreeNode<T>>();

        public void AddMedicine(T medicine, Comparison<T> comparison)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T> { Medicine = medicine, LeftSon = null, RightSon = null, Father = null };
            Insert(root, node, comparison);
        }

        public void Insert(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> newNode, Comparison<T> comparison)
        {
            if (currentNode == null)
            {
                currentNode = newNode;
            }
            else if (currentNode.LeftSon == null && currentNode.RightSon == null)
            {
                newNode.Father = currentNode;
                if (comparison.Invoke(currentNode.Medicine, newNode.Medicine) < 0)
                {
                    currentNode.LeftSon = newNode;
                }
                else
                {
                    currentNode.RightSon = newNode;
                }
            }
        }

        public void Delete(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> value, Comparison<T> comparison)
        {
            if (comparison.Invoke(currentNode.Medicine, value.Medicine) == 0)
            {
                var left = currentNode.LeftSon;
                var right = currentNode.RightSon;
                if (currentNode.LeftSon != null)
                {
                    currentNode = GetReplacementLeft(currentNode.LeftSon);
                }
                else if (currentNode.RightSon != null)
                {
                    currentNode = GetReplacementRight(currentNode.RightSon);
                }
                else
                {
                    currentNode = null;
                }
                currentNode.RightSon = right;
                currentNode.LeftSon = left;
            }
            else if (comparison.Invoke(currentNode.Medicine, value.Medicine) < 0)
            {
                Delete(currentNode.LeftSon, value, comparison);
            }
            else
            {
                Delete(currentNode.RightSon, value, comparison);
            }
        }

        private BinaryTreeNode<T> GetReplacementLeft(BinaryTreeNode<T> currentNode)
        {
            if (currentNode.RightSon != null)
            {
                return GetReplacementLeft(currentNode.RightSon);
            }
            else
            {
                if (currentNode.LeftSon != null)
                {
                    (currentNode.Father).RightSon = currentNode.LeftSon;
                    (currentNode.LeftSon).Father = currentNode.Father;
                }
                else
                {
                    (currentNode.Father).RightSon = null;
                }
                return currentNode;
            }
        }

        private BinaryTreeNode<T> GetReplacementRight(BinaryTreeNode<T> currentNode)
        {
            if (currentNode.LeftSon != null)
            {
                return GetReplacementRight(currentNode.LeftSon);
            }
            else
            {
                if (currentNode.RightSon != null)
                {
                    (currentNode.Father).LeftSon = currentNode.RightSon;
                    (currentNode.RightSon).Father = currentNode.Father;
                }
                else
                {
                    (currentNode.Father).LeftSon = null;
                }
                return currentNode;
            }
        }

        public BinaryTreeNode<T> Search(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> medicine, Comparison<T> comparison)
        {
            if (comparison.Invoke(medicine.Medicine, currentNode.Medicine) < 0)
            {
                if (currentNode.LeftSon != null)
                {
                    return Search(currentNode.LeftSon, medicine, comparison);
                }
            }
            else if (comparison.Invoke(medicine.Medicine, currentNode.Medicine) == 0)
            {
                return currentNode;
            }
            else
            {
                if (currentNode.RightSon != null)
                {
                    return Search(currentNode.RightSon, medicine, comparison);
                }
            }
            return null;
        }

        public List<BinaryTreeNode<T>> GetList()
        {
            InOrder(root);
            return returningList;
        }

        public void InOrder(BinaryTreeNode<T> currentNode)
        {
            if (currentNode.LeftSon != null)
            {
                InOrder(currentNode.LeftSon);
            }
            returningList.Add(currentNode);
            if (currentNode.RightSon != null)
            {
                InOrder(currentNode.RightSon);
            }
        }
    }
}
