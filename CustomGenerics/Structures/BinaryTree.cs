using CustomGenerics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures
{
    public class BinaryTree<T> : IDataStructureBase<T> where T : IComparable
    {
        public BinaryTreeNode<T> root = null;
        private List<BinaryTreeNode<T>> returningList = new List<BinaryTreeNode<T>>();

        public void AddMedicine(T medicine, Comparison<T> comparison)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T>() { Medicine = medicine, LeftSon = null, RightSon = null, Father = null };
            Insert(root, node, comparison);
        }

        public void Insert(BinaryTreeNode<T> currentNode, BinaryTreeNode<T> newNode, Comparison<T> comparison)
        {
            if (currentNode == null && currentNode == root)
            {
                currentNode = newNode;
                root = currentNode;
            }
            else if (comparison.Invoke(currentNode.Medicine, newNode.Medicine) < 0)
            {
                if (currentNode.LeftSon == null)
                {
                    currentNode.LeftSon = newNode;
                    newNode.Father = currentNode;
                    Balance(currentNode);
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
                    newNode.Father = currentNode;
                    Balance(currentNode);
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
                if (currentNode.LeftSon != null)
                {
                    currentNode.Medicine = GetReplacementLeft(currentNode.LeftSon).Medicine;
                    Delete(currentNode.LeftSon, GetReplacementLeft(currentNode.LeftSon), comparison);
                }
                else if (currentNode.RightSon != null)
                {
                    currentNode.Medicine = GetReplacementRight(currentNode.RightSon).Medicine;
                    Delete(currentNode.RightSon, GetReplacementRight(currentNode.RightSon), comparison);
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
                    Balance(currentNode.Father);
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
                //var returningNode = currentNode;
                //if (currentNode.LeftSon != null)
                //{
                //    (currentNode.Father).RightSon = currentNode.LeftSon;
                //    (currentNode.LeftSon).Father = currentNode.Father;
                //}
                //else
                //{
                //    (currentNode.Father).RightSon = null;
                //}
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
                //var returningNode = currentNode;
                //if (currentNode.RightSon != null)
                //{
                //    (currentNode.Father).LeftSon = currentNode.RightSon;
                //    (currentNode.RightSon).Father = currentNode.Father;
                //}
                //else
                //{
                //    (currentNode.Father).LeftSon = null;
                //}
                return currentNode;
            }
        }

        public void TakeMed(T value, Comparison<T> comparison)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T> { Medicine = value, Father = null, LeftSon = null, RightSon = null };
            ReduceStock(root, node, comparison);
        }

        private void Balance(BinaryTreeNode<T> node)
        {
            if (node.GetBalanceIndex() == -2)
            {
                if (node.LeftSon.GetBalanceIndex() == 1)
                {
                    LeftRotation(node.LeftSon);
                    RightRotation(node);
                }
                else
                {
                    RightRotation(node);
                }
            }
            else if (node.GetBalanceIndex() == 2)
            {
                if (node.RightSon.GetBalanceIndex() == -1)
                {
                    RightRotation(node.RightSon);
                    LeftRotation(node);
                }
                else
                {
                    LeftRotation(node);
                }
            }
            if (node.Father != null)
            {
                Balance(node.Father);
            }
        }

        private void RightRotation(BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> newLeft = node.LeftSon.RightSon;
            node.LeftSon.RightSon = node;
            node.LeftSon.Father = node.Father;
            if (node.Father != null)
            {
                if (node.Father.RightSon == node)
                {
                    node.Father.RightSon = node.LeftSon;
                }
                else
                {
                    node.Father.LeftSon = node.LeftSon;
                }
            }
            node.Father = node.LeftSon;
            node.LeftSon = newLeft;
            if (newLeft != null)
            {
                newLeft.Father = node;
            }

            if (node.Father.Father == null)
            {
                root = node.Father;
            }
        }

        private void LeftRotation(BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> newRight = node.RightSon.LeftSon;
            node.RightSon.LeftSon = node;
            node.RightSon.Father = node.Father;
            if (node.Father != null)
            {
                if (node.Father.RightSon == node)
                {
                    node.Father.RightSon = node.RightSon;
                }
                else
                {
                    node.Father.LeftSon = node.RightSon;
                }
            }
            node.Father = node.RightSon;
            node.RightSon = newRight;
            if (newRight != null)
            {
                newRight.Father = node;
            }
            if (node.Father.Father == null)
            {
                root = node.Father;
            }
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

        public List<BinaryTreeNode<T>> GetList(int pathing)
        {
            returningList = new List<BinaryTreeNode<T>>();
            switch (pathing)
            {
                case 1:
                    InOrder(root);
                    break;
                case 2:
                    PreOrder(root);
                    break;
                case 3:
                    PostOrder(root);
                    break;
            }
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
