using System.Drawing;

namespace TagsCloud.Layout.Layouters;

public class QuadTree
{
    private readonly List<Rectangle> _elements = new();
    private readonly int _bucketCapacity;
    private readonly int _maxDepth;
    private readonly int _level;

    private QuadTree? _upperLeft;
    private QuadTree? _upperRight;
    private QuadTree? _bottomLeft;
    private QuadTree? _bottomRight;

    private Rectangle _bounds;

    public bool IsLeaf => _upperLeft == null;

    public QuadTree(Rectangle bounds, int bucketCapacity = 32, int maxDepth = 5, int level = 0)
    {
        _bounds = bounds;
        _bucketCapacity = bucketCapacity;
        _maxDepth = maxDepth;
        _level = level;
    }

    public void Insert(Rectangle element)
    {
        if (!_bounds.Contains(element))
            ExpandBounds(element);

        if (_elements.Count >= _bucketCapacity)
            Split();

        var containingChild = GetContainingChild(element);

        if (containingChild != null)
            containingChild.Insert(element);
        else
            _elements.Add(element);
    }

    public bool HasIntersection(Rectangle element)
    {
        var nodes = new Queue<QuadTree>();
        nodes.Enqueue(this);

        while (nodes.Count > 0)
        {
            var node = nodes.Dequeue();

            if (!element.IntersectsWith(node._bounds))
                continue;

            foreach (var item in node._elements)
                if (item != element && element.IntersectsWith(item))
                    return true;

            if (node.IsLeaf) continue;

            if (node._upperLeft != null && element.IntersectsWith(node._upperLeft._bounds))
                nodes.Enqueue(node._upperLeft);

            if (node._upperRight != null && element.IntersectsWith(node._upperRight._bounds))
                nodes.Enqueue(node._upperRight);

            if (node._bottomLeft != null && element.IntersectsWith(node._bottomLeft._bounds))
                nodes.Enqueue(node._bottomLeft);

            if (node._bottomRight != null && element.IntersectsWith(node._bottomRight._bounds))
                nodes.Enqueue(node._bottomRight);
        }

        return false;
    }

    private void Clear()
    {
        _elements.Clear();
        _upperLeft = _upperRight = _bottomLeft = _bottomRight = null;
    }

    private void ExpandBounds(Rectangle newElement)
    {
        var minX = Math.Min(_bounds.X, newElement.X);
        var minY = Math.Min(_bounds.Y, newElement.Y);
        var maxX = Math.Max(_bounds.Right, newElement.Right);
        var maxY = Math.Max(_bounds.Bottom, newElement.Bottom);

        var width = maxX - minX;
        var height = maxY - minY;

        var newWidth = width * 2;
        var newHeight = height * 2;

        var centerX = (minX + maxX) / 2;
        var centerY = (minY + maxY) / 2;

        var newBounds = new Rectangle(
            centerX - newWidth / 2,
            centerY - newHeight / 2,
            newWidth,
            newHeight);

        RebuildWithNewBounds(newBounds);
    }

    private void RebuildWithNewBounds(Rectangle newBounds)
    {
        var allElements = GetAllElements();

        Clear();

        _bounds = newBounds;

        foreach (var element in allElements) Insert(element);
    }

    private List<Rectangle> GetAllElements()
    {
        var result = new List<Rectangle>(_elements);

        if (!IsLeaf)
        {
            result.AddRange(_upperLeft.GetAllElements());
            result.AddRange(_upperRight.GetAllElements());
            result.AddRange(_bottomLeft.GetAllElements());
            result.AddRange(_bottomRight.GetAllElements());
        }

        return result;
    }

    private void Split()
    {
        if (!IsLeaf)
            return;

        if (_level + 1 > _maxDepth)
            return;

        var halfWidth = _bounds.Width / 2;
        var halfHeight = _bounds.Height / 2;
        var x = _bounds.X;
        var y = _bounds.Y;

        _upperLeft = new QuadTree(
            new Rectangle(x, y, halfWidth, halfHeight),
            _bucketCapacity, _maxDepth, _level + 1);

        _upperRight = new QuadTree(
            new Rectangle(x + halfWidth, y, halfWidth, halfHeight),
            _bucketCapacity, _maxDepth, _level + 1);

        _bottomLeft = new QuadTree(
            new Rectangle(x, y + halfHeight, halfWidth, halfHeight),
            _bucketCapacity, _maxDepth, _level + 1);

        _bottomRight = new QuadTree(
            new Rectangle(x + halfWidth, y + halfHeight, halfWidth, halfHeight),
            _bucketCapacity, _maxDepth, _level + 1);

        var elements = _elements.ToList();

        foreach (var element in elements)
        {
            var containingChild = GetContainingChild(element);
            if (containingChild != null)
            {
                _elements.Remove(element);
                containingChild.Insert(element);
            }
        }
    }

    private QuadTree? GetContainingChild(Rectangle element)
    {
        if (IsLeaf) return null;

        foreach (var child in new[] { _upperLeft, _upperRight, _bottomLeft, _bottomRight })
            if (child != null && child._bounds.Contains(element))
                return child;

        return null;
    }
}