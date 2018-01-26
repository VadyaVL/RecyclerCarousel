using RecyclerCarousel.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace RecyclerCarousel
{
    // Works with any number of items.
    // Check how it works with a dynamic number by button.
    // Check how it works with a difficult templates.
    public partial class MainPage : ContentPage
    {
        private RangeObservableCollection<WordVm> collection = new RangeObservableCollection<WordVm>();
        public RangeObservableCollection<WordVm> Collection
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

        private void OnClickShuffle(object sender, EventArgs args)
        {
            this.Collection.Reset(this.Collection.OrderBy(_ => Guid.NewGuid()).ToList());
        }

        #endregion
    }
}