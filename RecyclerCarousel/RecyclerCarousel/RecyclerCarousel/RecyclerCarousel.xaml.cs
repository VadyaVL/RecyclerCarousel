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

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(int), typeof(RecyclerCarousel), 0, BindingMode.TwoWay);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RecyclerCarousel), null, BindingMode.TwoWay);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RecyclerCarousel), Enumerable.Empty<object>(), BindingMode.TwoWay, null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (bindable is RecyclerCarousel current)
                {
                    if (newvalue is INotifyCollectionChanged observable)
                    {
                        observable.CollectionChanged += (s, a) =>
                        {
                            current.OnPropertyChanged(nameof(current.ItemsSource));
                            current.RealToImagine(current.realPosition, current.Count);   // Change first param
                        };
                    }

                    current.OnPropertyChanged(nameof(current.ItemsSource));
                }
            });

        #endregion

        #region Properies
        
        private int realPosition = 0;
        public int RealPosition
        {
            get => this.realPosition;
            set
            {
                this.realPosition = value;
                this.OnPropertyChanged(nameof(this.RealPosition));
            }
        }

        public int Position
        {
            get => (int)this.GetValue(PositionProperty);
            set => this.SetValue(PositionProperty, value);
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

        // Use 1, 2 or 3 element(-s)
        // Use range OC
        private RangeObservableCollection<object> collection = new RangeObservableCollection<object>();
        public RangeObservableCollection<object> Collection
        {
            get => this.collection;
        }

        #endregion

        #region Constructors

        public RecyclerCarousel ()
		{
			InitializeComponent ();
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// From real list to imagine (with 3 elements)
        /// </summary>
        /// <param name="realPostion">Position in real collection.</param>
        /// <param name="realCount">Count in real collection.</param>
        private void RealToImagine(int realPostion, int realCount, Action action = Action.Init)
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
                // Clear imagine collection
                // Insert elements to imagine collection
                this.Collection.Reset(new List<object>
                {
                    itemsSourceAsObject.ElementAt(firstIndex),
                    itemsSourceAsObject.ElementAt(secondIndex),
                    itemsSourceAsObject.ElementAt(thirdIndex),
                });
                this.Position = 1;  // Position in imagine collection always must be 1
            }
            else if (action == Action.Right)    // ->
            {
                // Remove first
                this.Collection.RemoveAt(0);
                // Add last
                this.Collection.Add(itemsSourceAsObject.ElementAt(thirdIndex));
            }
            else                                // <-
            {
                // Remove last
                this.Collection.RemoveAt(2);
                // Add first
                this.Collection.Insert(0, itemsSourceAsObject.ElementAt(firstIndex));
            }
        }
        
        #endregion

        #region Events

        private void OnPositionSelected(object sender, SelectedPositionChangedEventArgs args)
        {
            if(args.SelectedPosition is int newPosition)
            {
                if(newPosition == 1)
                {
                    return;
                }

                var count = this.Count;
                var action = Action.Init;

                if (newPosition == 2)    // right
                {
                    this.RealPosition++;
                    action = Action.Right;

                    if (this.RealPosition >= count)
                    {
                        this.RealPosition = 0;
                    }
                }
                else if (newPosition == 0)  // left
                {
                    this.RealPosition--;
                    action = Action.Left;

                    if (this.RealPosition < 0)
                    {
                        this.RealPosition = count - 1;
                    }
                }

                this.RealToImagine(this.RealPosition, count, action);
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