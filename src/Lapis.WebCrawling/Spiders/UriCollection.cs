/********************************************************************************
 * Module      : Lapis.WebCrawling
 * Class       : UriCollection
 * Description : Represents a collection of URIs.
 * Created     : 2015/8/24
 * Note        :
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.WebCrawling.Spiders
{
    /// <summary>
    /// Represents a collection of URIs.
    /// </summary>
    public class UriCollection : IEnumerable<Uri>
    {
        /// <summary>
        /// Gets the number of URIs in the collection.
        /// </summary>
        /// <value>The number of URIs in the collection.</value>
        public virtual int Count
        {
            get { return _innerCollection.Count(); }
        }

        /// <summary>
        /// Determines whether the collection contains the specified URI.
        /// </summary>
        /// <param name="uri">The URI to find.</param>
        /// <returns>
        ///   <see langword="true"/> if the collection contains <paramref name="uri"/>; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Contains(Uri uri)
        {
            return _innerCollection.Contains(uri);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<Uri> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        internal UriCollection(IEnumerable<Uri> uris)
        {
            _innerCollection = uris;
        }

        private IEnumerable<Uri> _innerCollection;
    }

    internal class UriList : UriCollection
    {
        public override int Count
        {
            get { return _innerList.Count; }
        }

        public override bool Contains(Uri uri)
        {
            return _innerList.Contains(uri);
        }

        public Uri this[int index]
        {
            get { return _innerList[index]; }
            set { _innerList[index] = value; }
        }

        public int IndexOf(Uri uri)
        {
            return _innerList.IndexOf(uri);
        }    

        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        public void Add(Uri uri)
        {
            _innerList.Add(uri);
        }

        public void Clear()
        {
            _innerList.Clear();
        }     

        public bool Remove(Uri uri)
        {
            return _innerList.Remove(uri);
        }

        public UriList(IList<Uri> uris)
            : base(uris)
        {
            _innerList = uris;
        }

        public UriList()
            : this(new List<Uri>())
        { }

        private IList<Uri> _innerList;      
    }

    internal class UriSet : UriCollection
    {
        public override int Count
        {
            get { return _innerSet.Count; }
        }

        public override bool Contains(Uri uri)
        {
            return _innerSet.Contains(uri);
        }

        public void Add(Uri uri)
        {
            _innerSet.Add(uri);
        }

        public void Clear()
        {
            _innerSet.Clear();
        }

        public bool Remove(Uri uri)
        {
            return _innerSet.Remove(uri);
        }

        public UriSet(ISet<Uri> uris)
            : base(uris)
        {
            _innerSet = uris;
        }

        public UriSet()
            : this(new HashSet<Uri>())
        { }

        private ISet<Uri> _innerSet;
    }

    internal class UriStack : UriCollection
    {
        public override int Count
        {
            get { return _innerStack.Count; }
        }

        public override bool Contains(Uri uri)
        {
            return _innerStack.Contains(uri);
        }
          
        public void Push(Uri uri)
        {
            _innerStack.Push(uri);
        }

        public void Clear()
        {
            _innerStack.Clear();
        }

        public Uri Pop()
        {
            return _innerStack.Pop();
        }

        public Uri Peek()
        {
            return _innerStack.Peek();
        }

        public UriStack(Stack<Uri> uris)
            : base(uris)
        {
            _innerStack = uris;
        }

        public UriStack()
            : this(new Stack<Uri>())
        { }

        private Stack<Uri> _innerStack;
    }

    internal class UriQueue : UriCollection
    {
        public override int Count
        {
            get { return _innerQueue.Count; }
        }

        public override bool Contains(Uri uri)
        {
            return _innerQueue.Contains(uri);
        }

        public void Enqueue(Uri uri)
        {
            _innerQueue.Enqueue(uri);
        }

        public void Clear()
        {
            _innerQueue.Clear();
        }

        public Uri Dequeue()
        {
            return _innerQueue.Dequeue();
        }

        public Uri Peek()
        {
            return _innerQueue.Peek();
        }

        public UriQueue(Queue<Uri> uris)
            : base(uris)
        {
            _innerQueue = uris;
        }

        public UriQueue()
            : this(new Queue<Uri>())
        { }

        private Queue<Uri> _innerQueue;
    }
}
