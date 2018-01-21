using RecyclerCarousel.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace RecyclerCarousel
{
	public partial class MainPage : ContentPage
    {
        private ObservableCollection<WordVm> collection = new ObservableCollection<WordVm>();
        public ObservableCollection<WordVm> Collection
        {
            get => this.collection;
        }

        public MainPage()
		{
			InitializeComponent();

            this.Collection.Add(new WordVm { Text = "Word 1" });
            this.Collection.Add(new WordVm { Text = "Word 2" });
            this.Collection.Add(new WordVm { Text = "Word 3" });
            this.Collection.Add(new WordVm { Text = "Word 4" });
            this.Collection.Add(new WordVm { Text = "Word 5" });
        }
    }
}