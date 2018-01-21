using System.Collections;
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

        #endregion

        public RecyclerCarousel ()
		{
			InitializeComponent ();
		}
	}
}