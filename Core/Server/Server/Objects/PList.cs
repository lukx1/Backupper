using Server.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Server.Objects
{
    /// <summary>
    /// Pomocna trida pro permise
    /// </summary>
    public class PList : IList<Permission>
    {
        private List<Permission> list = new List<Permission>();

        public int Count =>list.Count;

        public bool IsReadOnly => true;

        Permission IList<Permission>.this[int index] { get => list[index]; set => list[index] =value; }

        public void ForEach(Action<Permission> action)
        {
            list.ForEach(action);
        }

        public bool Contains(Permission permission)
        {
            if (list.Contains(Permission.SKIP))
                return true;
            return list.Contains(permission);
        }

        private IEnumerator<Permission> GetSkipEnumerator()
        {
            foreach (Permission e in Enum.GetValues(typeof(Permission)))
            {
                yield return e;
            }
        }

        public IEnumerator<Permission> GetEnumerator()
        {
            if (list.Contains(Permission.SKIP))
                return GetSkipEnumerator();
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (list.Contains(Permission.SKIP))
                return GetSkipEnumerator();
            return list.GetEnumerator();
        }

        public int IndexOf(Permission item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Permission item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Add(Permission item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public void CopyTo(Permission[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Permission item)
        {
            return list.Remove(item);
        }
    }
}