namespace Lab_SolarSystem.Data;

public class Node<T>
{
    public T Item { get; set; }

    public Node<T> ParentNode { get; set; }

    public List<Node<T>> Childrens { get; set; } = new();

	public Node(T item) => Item = item;

	public Node(T item, Node<T> parentNode) : this(item)
    {
        ParentNode = parentNode;
        ParentNode.Childrens.Add(this);
    }
}