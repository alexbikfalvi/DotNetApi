/* 
 * Copyright (C) Ury Jamshy / ujamshy@yahoo.com
 *
 * THIS WORK IS PROVIDED "AS IS", "WHERE IS" AND "AS AVAILABLE", WITHOUT ANY EXPRESS OR
 * IMPLIED WARRANTIES OR CONDITIONS OR GUARANTEES. YOU, THE USER, ASSUME ALL RISK IN ITS
 * USE, INCLUDING COPYRIGHT INFRINGEMENT, PATENT INFRINGEMENT, SUITABILITY, ETC. AUTHOR
 * EXPRESSLY DISCLAIMS ALL EXPRESS, IMPLIED OR STATUTORY WARRANTIES OR CONDITIONS, INCLUDING
 * WITHOUT LIMITATION, WARRANTIES OR CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY
 * OR FITNESS FOR A PARTICULAR PURPOSE, OR ANY WARRANTY OF TITLE OR NON-INFRINGEMENT, OR
 * THAT THE WORK (OR ANY PORTION THEREOF) IS CORRECT, USEFUL, BUG-FREE OR FREE OF VIRUSES.
 * 
 */

using System;
using System.Collections.Generic;

namespace DotNetApi.Collections.Generic
{
	/// <summary>
	/// An internal class for a red-black tree.
	/// </summary>
	/// <typeparam name="T">The tree type.</typeparam>
    internal class RedBlackTree<T>
    {
        #region Public Methods

        public RedBlackTree()
        {
            m_comparer = Comparer<T>.Default;
        }

        public RedBlackTree(IComparer<T> comparer)
        {
            m_comparer = comparer;
        }

        public void Clear()
        {
            m_root = null;
            Count = 0;
        }

        public void Add(T item)
        {
            TreeNode node = new TreeNode(item);
            if (m_root == null)
                m_root = node;
            else
            {
                TreeNode parent = null;
                TreeNode current = m_root;
                while (current != null)
                {
                    parent = current;
                    //int comparison = Compare(node, current);
                    int comparison = m_comparer.Compare(node.Item, current.Item);
                    if (comparison < 0)
                        current = current.LeftChild;
                    else if (comparison > 0)
                        current = current.RightChild;
                    else // comparison == 0, already exists
                        ThrowInternalException();;
                }
                // here - current == null;
                if (Compare(node, parent) < 0)
                    parent.LeftChild = node;
                else
                    parent.RightChild = node;
                node.Parent = parent;
                FixAfterAdd(node);
            }
            ++Count;
        }

        public void Remove(T item)
        {
            TreeNode nodeToRemove = FindNode(item, /*exact=*/true);
            if (nodeToRemove == null)
                throw new KeyNotFoundException();

            if (nodeToRemove.LeftChild == null)
                RemoveNode(nodeToRemove, nodeToRemove.RightChild);
            else if (nodeToRemove.RightChild == null)
                RemoveNode(nodeToRemove, nodeToRemove.LeftChild);
            else // Has both left and right children
            {
                // Find rightmost child of left subtree and replace with current node
                TreeNode node = FindRightmostNode(nodeToRemove.LeftChild);
                nodeToRemove.Item  = node.Item;
                RemoveNode(node, node.LeftChild);
            }
            --Count;
        }

        public TreeNode FindGreaterEqual(T item)
        {
            return FindNode(item, /*exact=*/false);
        }

        public TreeNode Find(T item)
        {
            return FindNode(item, /*exact=*/true);
        }

        public TreeNode First()
        {
            if (m_root == null)
                return null;

            return FindLeftmostNode(m_root);
        }

        public TreeNode Next(TreeNode node)
        {
            if (node == null)
                ThrowInternalException();

            if (node.RightChild != null)
                return FindLeftmostNode(node.RightChild);

            // Climb up until current node is a left node
            TreeNode parent = node.Parent;
            while (parent != null && parent.RightChild == node)
            {
                node = parent;
                parent = parent.Parent;
            }
            return parent;
        }

        public bool IsValid()
        {
            CountVisitor counter = new CountVisitor();
            TravelTree(counter);
            if (counter.Count != Count)
                return false;

            if (m_root != null && m_root.Parent != null)
                return false;

            ValidatorVisitor validator = new ValidatorVisitor();
            if (!TravelTree(validator))
                return false;

            return true;
        }

        // Travel the tree using prefix strategy - parent then left subtree and finaly right subtree
        public bool TravelTree(ITreeVisitor visitor)
        {
            return TravelTree(m_root, visitor);
        }

        #endregion

        #region Public Interfaces

        public interface ITreeVisitor
        {
            bool Visit(TreeNode node);
        }

        #endregion

        #region Public Properties

        public int Count;
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region Internal Classes

        private class CountVisitor : ITreeVisitor
        {
            public int Count
            {
                get;
                set;
            }

            public bool Visit(TreeNode node)
            {
                ++Count;
                return true;
            }
        }

        private class ValidatorVisitor : ITreeVisitor
        {
            public bool Visit(TreeNode node)
            {
                if (node.LeftChild != null && node.LeftChild.Parent != node)
                    return false;
                if (node.RightChild != null && node.RightChild.Parent != node)
                    return false;
                if (node.IsRed && node.Parent != null && node.Parent.IsRed)
                    return false;

                // Check that the height is the same for all leaves
                if (node.LeftChild == null || node.RightChild == null) // A leaf
                {
                    int height = 0;
                    for( ; node != null; node = node.Parent)
                        if (node.IsBlack)
                            ++height;
                    if (m_blackHeight == -1)
                        m_blackHeight = height;
                    else
                    {
                        if (m_blackHeight != height)
                            return false;
                    }
                }

                return true;
            }
            private int m_blackHeight = -1;
        }

        internal enum NodeColor
        {
            Black, Red
        }

        internal class TreeNode 
        {
            public TreeNode(T item)
            {
                Item  = item;
                Color = NodeColor.Red;
            }

            public TreeNode Parent;
            //{
            //    get;
            //    set;
            //}

            public TreeNode GrandParent
            {
                get
                {
                    return Parent != null ? Parent.Parent : null;
                }
            }


            public TreeNode LeftChild;
            //{
            //    get;
            //    set;
            //}

            public TreeNode RightChild;
            //{
            //    get;
            //    set;
            //}

            public TreeNode Uncle
            {
                get
                {
                    TreeNode grandParent = GrandParent;
                    if (grandParent == null)
                        return null;
                    return Parent.IsLeftChild ? grandParent.RightChild : grandParent.LeftChild;
                }
            }

            public NodeColor Color;
            //{
            //    get;
            //    set;
            //}

            public bool IsBlack
            {
                get
                {
                    return Color == NodeColor.Black;
                }
            }

            public bool IsRed
            {
                get
                {
                    return Color == NodeColor.Red;
                }
            }

            public bool IsLeftChild
            {
                get
                {
                    if (Parent == null)
                        return false;
                    return Parent.LeftChild == this;
                }
            }

            public bool IsRightChild
            {
                get
                {
                    if (Parent == null)
                        return false;
                    return Parent.RightChild == this;
                }
            }

            public T Item;
            //{
            //    get;
            //    set;
            //}

        }

        #endregion

        #region Private Methods

        static private void ThrowInternalException()
        {
            throw new ApplicationException();
        }

        private int Compare(TreeNode node1, TreeNode node2)
        {
            return m_comparer.Compare(node1.Item, node2.Item);
        }

        private int Compare(T item, TreeNode node)
        {
            return m_comparer.Compare(item, node.Item);
        }

        static private TreeNode FindRightmostNode(TreeNode node)
        {
            while (node.RightChild != null)
                node = node.RightChild;
            return node;
        }

        static private TreeNode FindLeftmostNode(TreeNode node)
        {
            while (node.LeftChild != null)
                node = node.LeftChild;
            return node;
        }

        private TreeNode FindNode(T item, bool exact)
        {
            TreeNode current = m_root;
            TreeNode parent = null;
            while (current != null)
            {
                parent = current;
                //int comparison = Compare(item, current);
                int comparison = m_comparer.Compare(item, current.Item);
                if (comparison == 0)
                    return current;
                else if (comparison < 0)
                    current = current.LeftChild;
                else
                    current = current.RightChild;
            }
            // exact match not found
            if (exact || parent == null)
                return null;

            if (Compare(item, parent) < 0)
                return parent;
            else
                return Next(parent);
        }

        private void LeftRotate(TreeNode x)
        {
            TreeNode y = x.RightChild;
            if (y == null)
                ThrowInternalException();;

            if (x == m_root)
                m_root = y;

            x.RightChild = y.LeftChild;
            if (x.RightChild != null)
                x.RightChild.Parent = x;

            y.LeftChild = x;

            // Link parents
            if (x.Parent != null)
            {
                if (x.IsLeftChild)
                    x.Parent.LeftChild = y;
                else
                    x.Parent.RightChild = y;
            }

            y.Parent = x.Parent;
            x.Parent = y;
        }

        private void RightRotate(TreeNode y)
        {
            TreeNode x = y.LeftChild;
            if (x == null)
                ThrowInternalException();;

            if (y == m_root)
                m_root = x;

            y.LeftChild = x.RightChild;
            if (y.LeftChild != null)
                y.LeftChild.Parent = y;

            x.RightChild = y;

            if (y.Parent != null)
            {
                if (y.IsLeftChild)
                    y.Parent.LeftChild = x;
                else
                    y.Parent.RightChild = x;
            }

            x.Parent = y.Parent;
            y.Parent = x;
        }

        private void LeftRotate()
        {
            if (m_root != null && m_root.RightChild != null)
                LeftRotate(m_root);
        }

        private void RightRotate()
        {
            if (m_root != null && m_root.LeftChild != null)
                RightRotate(m_root);
        }

        static private TreeNode FindSibling(TreeNode node, TreeNode parent)
        {
            if (parent == null)
                 return null;

            return node == parent.LeftChild ? parent.RightChild : parent.LeftChild;
        }

        static private bool NodeIsBlack(TreeNode node)
        {
            return node == null || node.Color == NodeColor.Black;
        }

        private void FixAfterRemove(TreeNode node, TreeNode parent)
        {
            //TreeVisualizer<T> visualizer = new TreeVisualizer<T>();
            for (; ; )
            {
                if (parent == null) // Reached root - ignore extra black
                {
                    if (node != m_root)
                        ThrowInternalException();
                    return;
                }
                //visualizer.DisplayTree("FixAfterRemove Before", this);

                TreeNode sibling = FindSibling(node, parent);
                if (sibling.IsRed)
                {
                    // Since both children of siblings are black - rotate will result in black sibling 
                    if (sibling.IsRightChild)
                        LeftRotate(parent);
                    else
                        RightRotate(parent);
                    //visualizer.DisplayTree("FixAfterRemove sibling.IsRed", this);
                    sibling.Color = NodeColor.Black;
                    parent.Color = NodeColor.Red;
                    sibling = FindSibling(node, parent);
                }
                // At this point sibling color is black
                if (NodeIsBlack(sibling.RightChild) && NodeIsBlack(sibling.LeftChild))
                {
                    sibling.Color = NodeColor.Red;
                    if (parent.IsRed)
                    {
                        parent.Color = NodeColor.Black;
                        //visualizer.DisplayTree("FixAfterRemove sibling and children Black (was red)", this);
                        return;
                    }
                    else
                    {
                        node = parent;
                        parent = node == null ? null : node.Parent;
                        //visualizer.DisplayTree("FixAfterRemove sibling and children Black (was black)", this);
                        continue;
                    }
                }
                // sibling is black and at least one of its children is red
                // Make sure node and red sibling child are opposite in direction
                if (sibling.IsRightChild && NodeIsBlack(sibling.RightChild) ||
                    sibling.IsLeftChild && NodeIsBlack(sibling.LeftChild))
                {
                    if (sibling.IsRightChild)
                        RightRotate(sibling);
                    else
                        LeftRotate(sibling);
                    sibling.Color = NodeColor.Red;
                    sibling.Parent.Color = NodeColor.Black; // Change sibling parent (former child) color
                    //visualizer.DisplayTree("FixAfterRemove 1 child red (not opposite)", this);
                    sibling = FindSibling(node, parent);
                }
                // Here sibling is black and its opposite child is red
                if (sibling.IsRightChild)
                {
                    LeftRotate(parent);
                    sibling.RightChild.Color = NodeColor.Black;
                }
                else
                {
                    RightRotate(parent);
                    sibling.LeftChild.Color = NodeColor.Black;
                }
                sibling.Color = parent.Color;
                parent.Color = NodeColor.Black;
                //visualizer.DisplayTree("FixAfterRemove 1 child red (opposite)", this);
                return;
            }
        }

        private void RemoveNode(TreeNode nodeToRemove, TreeNode childSubstitute)
        {
            TreeNode parent = nodeToRemove.Parent;
            if (parent == null)
            {
                if (nodeToRemove != m_root)
                    ThrowInternalException();
                m_root = childSubstitute;
            }
            else
            {
                if (nodeToRemove.IsLeftChild)
                    parent.LeftChild = childSubstitute;
                else
                    parent.RightChild = childSubstitute;
            }

            if (childSubstitute != null)
                childSubstitute.Parent = parent;

            // If color was red - tree is still legal
            if (nodeToRemove.IsBlack)
            {
                if (childSubstitute != null && childSubstitute.IsRed)
                    childSubstitute.Color = NodeColor.Black;
                else // We have an extra black - tree should be fixed
                    FixAfterRemove(childSubstitute, parent);
            }
        }

        static private void DetachNode(TreeNode node)
        {
            if (node.IsLeftChild)
                node.Parent.LeftChild = null;
            else if (node.IsRightChild)
                node.Parent.RightChild = null;
            else
                ThrowInternalException();
        }

        private void FixAfterAdd(TreeNode node)
        {
            //TreeVisualizer<T> visualizer = new TreeVisualizer<T>();
            for (; ; )
            {
                if (node.Parent == null || node.Parent.Color != NodeColor.Red)
                    return;

                //visualizer.DisplayTree("FixAfterAdd Before", this);
                if (node.Parent == m_root)
                {
                    node.Parent.Color = NodeColor.Black;
                    //visualizer.DisplayTree("FixAfterAdd - Changed ROOT color", this);
                    return;
                }

                TreeNode uncle = node.Uncle;
                TreeNode grandParent = node.GrandParent;
                if (uncle != null && uncle.IsRed)
                {
                    uncle.Color = node.Parent.Color = NodeColor.Black;
                    grandParent.Color = NodeColor.Red;
                    node = grandParent;
                    //visualizer.DisplayTree("Uncle RED, Switched P and U Colors", this);
                    continue; // Now granfParent is the problematic - continue with it
                }

                // uncle.Color is Black
                if (node.IsRightChild && node.Parent.IsLeftChild)
                {
                    node = node.Parent;
                    LeftRotate(node);
                    //visualizer.DisplayTree("Uncle BLACK RR, Left Rotated Parent", this);
                }
                else if (node.IsLeftChild && node.Parent.IsRightChild)
                {
                    node = node.Parent;
                    RightRotate(node);
                    //visualizer.DisplayTree("Uncle BLACK LL, Right Rotated Parent", this);
                }

                // Node and uncle are opposite - one is a left child and the other is a right child
                if (node.IsRightChild && node.Parent.IsLeftChild || node.IsLeftChild && node.Parent.IsRightChild)
                    ThrowInternalException();; // Shouldn't happen
                if (node.IsLeftChild)
                    RightRotate(grandParent);
                else
                    LeftRotate(grandParent);

                //visualizer.DisplayTree("Uncle BLACK Opposite, Rotated", this);

                grandParent.Color = NodeColor.Red;
                node.Parent.Color = NodeColor.Black;
                //visualizer.DisplayTree("Uncle BLACK Opposite, switched P GP colors", this);
                return;
            }
        }

        private bool TravelTree(TreeNode node, ITreeVisitor Visitor)
        {
            if (node == null)
                return true;

            if (!Visitor.Visit(node))
                return false;

            return TravelTree(node.LeftChild, Visitor) && TravelTree(node.RightChild, Visitor);
        }

        #endregion

        #region Private Members

        private TreeNode     m_root;
        private IComparer<T> m_comparer;

        #endregion
    }
}
