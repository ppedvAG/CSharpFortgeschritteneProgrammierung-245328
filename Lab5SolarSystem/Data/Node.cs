using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab5SolarSystem.Data
{
    [Serializable]
    public class Node<T>
    {
        private T _item;

        //[field:NonSerialized]
        private Node<T> _parentNode = default!;

        private List<Node<T>> _children = new List<Node<T>>();

        public Node()
        {
        }

        public Node(T item)
        {
            _item = item;
        }

        public Node(T item, Node<T> parentNode)
        {
            _item = item;
            _parentNode = parentNode;
        }

        public void SetParentNode(Node<T> parentNode)
        {
            ParentNode = parentNode;
        }

        public void SetParentNodeInChilds()
        {
            foreach (Node<T> child in _children)
                child.SetParentNode(this);
        }

        public T Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public List<Node<T>> Childrens { get => _children; set => _children = value; }

        [JsonIgnore]
        [XmlIgnore]
        [field: NonSerialized]
        public Node<T> ParentNode { get => _parentNode; set => _parentNode = value; }

        public void AddChild(T child)
        {
            Childrens.Add(new Node<T>(child, this));
        }

        public void AddChild(Node<T> child)
        {
            child.ParentNode = this;
            Childrens.Add(child);
        }

        public void RemoveChild(T child)
        {
            var node = Childrens.FirstOrDefault(e => e.Item.Equals(child));
            if (node != null)
                Childrens.Remove(node);
        }
    }
}
