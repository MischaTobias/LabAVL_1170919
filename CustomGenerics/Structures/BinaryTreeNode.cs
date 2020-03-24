﻿using System;
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

        public int GetBalanceIndex()
        {
            if (this.LeftSon != null && this.RightSon != null)
            {
                return this.RightSon.GetTreeHeight() - this.LeftSon.GetTreeHeight();
            }
            else if (this.LeftSon == null)
            {
                return this.RightSon.GetTreeHeight();
            }
            else
            {
                return this.LeftSon.GetTreeHeight() * -1;
            }
        }

        public int GetTreeHeight()
        {
            if (this.LeftSon == null && this.RightSon == null)
            {
                return 1;
            }
            else if (this.LeftSon == null || this.RightSon == null)
            {
                if (this.LeftSon == null)
                {
                    return this.RightSon.GetTreeHeight() + 1;
                }
                else
                {
                    return this.LeftSon.GetTreeHeight() + 1;
                }
            }
            else
            {
                if (this.LeftSon.GetTreeHeight() > this.RightSon.GetTreeHeight())
                {
                    return this.LeftSon.GetTreeHeight() + 1;
                }
                else
                {
                    return this.RightSon.GetTreeHeight() + 1;
                }
            }
        }
    }
}
