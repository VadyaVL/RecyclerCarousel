using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace RecyclerCarousel
{
    // https://peteohanlon.wordpress.com/2008/10/22/bulk-loading-in-observablecollection/
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public bool SuppressNotification { get; set; }

        public Func<T, T, bool> TheSameChecker;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }
        
        public void AddRange(ICollection<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this.SuppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }

            this.SuppressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<T> list)
        {
            this.AddRange(list.ToList());
        }

        public void Reset(ICollection<T> range)
        {
            this.Items.Clear();

            AddRange(range);
        }


        // Check exceptions. Write safety code.

        //public void RemoveFirstInsertEnd(T newObject)
        //{
        //    this.SuppressNotification = true;

        //    this.RemoveAt(0);
        //    this.Add(newObject);

        //    this.SuppressNotification = false;

        //    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //}

        //public void RemoveLastInsertFirst(T newObject)
        //{
        //    this.SuppressNotification = true;

        //    this.RemoveAt(this.Count - 1);
        //    this.Insert(0, newObject);

        //    this.SuppressNotification = false;

        //    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //}

        public void Refresh()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
