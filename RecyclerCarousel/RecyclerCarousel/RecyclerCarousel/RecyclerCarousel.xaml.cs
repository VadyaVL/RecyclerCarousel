/*
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecyclerCarousel
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecyclerCarousel : StackLayout, INotifyPropertyChanged
    {
        #region Bindable Properies

        /// <summary>
        /// Position of current element.
        /// </summary>
        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(int), typeof(RecyclerCarousel), 0, BindingMode.TwoWay);

        /// <summary>
        /// Imagine position in carriage of three elements.
        /// </summary>
        public static readonly BindableProperty ImaginePositionProperty = BindableProperty.Create(nameof(ImaginePosition), typeof(int), typeof(RecyclerCarousel), 0, BindingMode.TwoWay);

        /// <summary>
        /// Template for showing.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RecyclerCarousel), null, BindingMode.TwoWay);

        /// <summary>
        /// List of objects.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RecyclerCarousel), Enumerable.Empty<object>(), BindingMode.TwoWay, null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (bindable is RecyclerCarousel current)
                {
                    if (newvalue is INotifyCollectionChanged observable)
                    {
                        observable.CollectionChanged += (s, a) =>
                        {
                            current.CollectionToImagine(current.Position, current.Count);   // Change first param
                        };
                    }

                    current.OnPropertyChanged(nameof(current.ItemsSource));
                }
            });

        #endregion

        #region Properies

        public int Position
        {
            get => (int)this.GetValue(PositionProperty);
            set => this.SetValue(PositionProperty, value);
        }

        public int ImaginePosition
        {
            get => (int)this.GetValue(ImaginePositionProperty);
            set => this.SetValue(ImaginePositionProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // Count of objects
        public int Count
        {
            get
            {
                var res = 0;

                if (this.ItemsSource is ICollection collection)
                {
                    res = collection.Count;
                }
                else
                {
                    res = this.ItemsSource.Cast<object>().Count();
                }

                return res;
            }
        }

        // Carriage of three elements
        private RangeObservableCollection<object> collection = new RangeObservableCollection<object>();
        public RangeObservableCollection<object> Collection { get => this.collection; }

        public object PreviewItem { get => this.customCarousel.LastItem; }

        public object CurrentItem { get => this.customCarousel.Item;  }

        /// <summary>
        /// Get PreviewItem.
        /// </summary>
        public event Action<object> PositionChanged;

        #endregion

        #region Constructors

        public RecyclerCarousel ()
		{
			InitializeComponent ();
        }

        #endregion

        #region Methods

        // WHAT_DO: CHECK RESET AND ADD NEW ELEMENTS
        // BUG: While we just shuffle, add new item (maybe remove too) and reset  - customCarousel won't update itself.

        /// <summary>
        /// From list of objects to imagine (carriage of three elements)
        /// </summary>
        /// <param name="realPostion">Position in collection.</param>
        /// <param name="realCount">Count in collection.</param>
        private void CollectionToImagine(int realPostion, int realCount, Action action = Action.Init)
        {
            if(realCount <= 0 || realPostion < 0 || realPostion >= realCount)
            {
                return;
            }

            // Find index from Collection
            var firstIndex = realPostion - 1;
            var secondIndex = realPostion;
            var thirdIndex = realPostion + 1;

            if(firstIndex < 0)
            {
                firstIndex = realCount - 1;
            }

            if(thirdIndex >= Count)
            {
                thirdIndex = 0;
            }

            var itemsSourceAsObject = this.ItemsSource.Cast<object>();

            if (action == Action.Init)
            {
                var listToInsert = new List<object>
                {
                    itemsSourceAsObject.ElementAt(firstIndex),
                    itemsSourceAsObject.ElementAt(secondIndex),
                    itemsSourceAsObject.ElementAt(thirdIndex),
                };

                var reinit = this.Collection.Count == 0;

                if (!reinit)
                {
                    var theSame = true;

                    for (int i = 0; i < 3 && theSame; i++)
                    {
                        theSame &= this.Collection[i].Equals(listToInsert[i]);
                    }

                    reinit = !theSame; 
                }

                if (reinit)
                {
                    this.Collection.Reset(listToInsert);
                }

                this.ImaginePosition = 1;  // Position in imagine collection always must be 1
            }
            else if (action == Action.Right)    // ->
            {
                //this.Collection.RemoveFirstInsertEnd(itemsSourceAsObject.ElementAt(thirdIndex));
                // Remove first - bad animation find here
                this.Collection.RemoveAt(0);
                // Add last
                this.Collection.Insert(2, itemsSourceAsObject.ElementAt(thirdIndex));

                //this.Collection.Move(1, 0);
                //this.Collection.RemoveAt(1);
                //this.Collection.Insert(2, itemsSourceAsObject.ElementAt(thirdIndex));
            }
            else                                // <-
            {
                //this.Collection.RemoveLastInsertFirst(itemsSourceAsObject.ElementAt(firstIndex));
                // Remove last
                this.Collection.RemoveAt(2);
                // Add first
                this.Collection.Insert(0, itemsSourceAsObject.ElementAt(firstIndex));
            }
        }
        
        public void Reset()
        {
            this.Position = 0;
            this.ImaginePosition = 0;
            this.CollectionToImagine(this.Position, this.Count);
        }

        #endregion

        #region Events

        private void OnItemSelected(object sender, SelectedPositionChangedEventArgs args)
        {

        }

        private void OnPositionSelected(object sender, SelectedPositionChangedEventArgs args)
        {
#if DEBUG
            // If position changed we could get preview selected item.
            System.Diagnostics.Debug.WriteLine(this.PreviewItem);
#endif
            this.PositionChanged?.Invoke(this.PreviewItem);

            if (args.SelectedPosition is int newPosition)
            {
                if(newPosition == 1)
                {
                    return;
                }

                var count = this.Count;
                var action = Action.Init;

                if (newPosition == 2)    // right
                {
                    this.Position++;
                    action = Action.Right;

                    if (this.Position >= count)
                    {
                        this.Position = 0;
                    }
                }
                else if (newPosition == 0)  // left
                {
                    this.Position--;
                    action = Action.Left;

                    if (this.Position < 0)
                    {
                        this.Position = count - 1;
                    }
                }

                this.CollectionToImagine(this.Position, count, action);
            }
        }

        #endregion

        #region Enums

        private enum Action
        {
            Left,
            Init,
            Right
        }

        #endregion
    }
}