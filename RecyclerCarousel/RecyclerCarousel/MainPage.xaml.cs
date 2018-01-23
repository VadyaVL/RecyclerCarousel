using RecyclerCarousel.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace RecyclerCarousel
{
    // Works with any number of items.
    // Check how it works with a dynamic number by button.
    // Check how it works with a difficult templates.
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
            this.Collection.Add(new WordVm { Text = "Word 6" });
            this.Collection.Add(new WordVm { Text = "Word 7" });
            this.Collection.Add(new WordVm { Text = "Word 8" });
            this.Collection.Add(new WordVm { Text = "Word 9" });
        }

        #region Events

        private void OnClickReset(object sender, EventArgs args)
        {
            this.recyclerCarousel.Reset();
        }

        private void OnClickAdd(object sender, EventArgs args)
        {
            this.Collection.Add(new WordVm { Text = "Word " + (this.Collection.Count + 1) });
        }

        #endregion
    }
}