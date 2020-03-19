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
        public BinaryTreeNode<T> root = null;
        private List<BinaryTreeNode<T>> returningList = new List<BinaryTreeNode<T>>();

        public void AddMedicine(T medicine, Comparison<T> comparison)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T> { Medicine = medicine, LeftSon = null, RightSon = null, Father = null };
            Insert(root, node, comparison);
        }

        public void Insert(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> newNode, Comparison<T> comparison)
        {
            if (root == null)
            {
                root = newNode;
            }
            else if (comparison.Invoke(currentNode.Medicine, newNode.Medicine) < 0)
            {
                if (currentNode.LeftSon == null)
                {
                    currentNode.LeftSon = newNode;
                    currentNode.LeftSon.Father = currentNode;
                }
                else
                {
                    Insert(currentNode.LeftSon, newNode, comparison);
                }
            }
            else
            {
                if (currentNode.RightSon == null)
                {
                    currentNode.RightSon = newNode;
                    currentNode.RightSon.Father = newNode;
                }
                else
                {
                    Insert(currentNode.RightSon, newNode, comparison);
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
                    currentNode.RightSon = right;
                    currentNode.LeftSon = left;
                }
                else if (currentNode.RightSon != null)
                {
                    currentNode = GetReplacementRight(currentNode.RightSon);
                    currentNode.RightSon = right;
                    currentNode.LeftSon = left;
                }
                else
                {
                    var compareNode = currentNode;
                    if (currentNode.Father.LeftSon == compareNode)
                    {
                        currentNode.Father.LeftSon = null;
                    }
                    else
                    {
                        currentNode.Father.RightSon = null;
                    }
                }
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
                var returningNode = currentNode;
                returningNode.LeftSon = null;
                returningNode.RightSon = null;
                if (currentNode.LeftSon != null)
                {
                    (currentNode.Father).RightSon = currentNode.LeftSon;
                    (currentNode.LeftSon).Father = currentNode.Father;
                }
                else
                {
                    (currentNode.Father).RightSon = null;
                }
                return returningNode;
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
                var returningNode = currentNode;
                returningNode.LeftSon = null;
                returningNode.RightSon = null;
                if (currentNode.RightSon != null)
                {
                    (currentNode.Father).LeftSon = currentNode.RightSon;
                    (currentNode.RightSon).Father = currentNode.Father;
                }
                else
                {
                    (currentNode.Father).LeftSon = null;
                }
                return returningNode;
            }
        }

        public void TakeMed(T value, Comparison<T> comparison)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T> { Medicine = value, Father = null, LeftSon = null, RightSon = null };
            ReduceStock(root, node, comparison);
        }

        private void ReduceStock(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> value, Comparison<T> comparison)
        {
            if (comparison.Invoke(currentNode.Medicine, value.Medicine) < 0)
            {
                ReduceStock(currentNode.LeftSon, value, comparison);
            }
            else if (comparison.Invoke(currentNode.Medicine, value.Medicine) == 0)
            {
                currentNode.Medicine = value.Medicine;
            }
            else
            {
                ReduceStock(currentNode.RightSon, value, comparison);
            }
        }

        public List<BinaryTreeNode<T>> GetList()
        {
            returningList = new List<BinaryTreeNode<T>>();
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

        public void PostOrder(BinaryTreeNode<T> currentNode)
        {
            if (currentNode.LeftSon != null)
            {
                PostOrder(currentNode.LeftSon);
            }
            if (currentNode.RightSon != null)
            {
                PostOrder(currentNode.RightSon);
            }
            returningList.Add(currentNode);
        }

        public void PreOrder(BinaryTreeNode<T> currentNode)
        {
            returningList.Add(currentNode);
            if (currentNode.LeftSon != null)
            {
                PreOrder(currentNode.LeftSon);
            }
            if (currentNode.RightSon != null)
            {
                PreOrder(currentNode.RightSon);
            }
        }
    }
}
