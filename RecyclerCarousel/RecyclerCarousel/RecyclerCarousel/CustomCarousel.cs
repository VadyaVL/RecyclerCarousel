using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace RecyclerCarousel
{
    public class CustomCarousel : CarouselView
    {
        public object LastItem { get; private set; }

        public CustomCarousel() : base()
        {

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
        
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (propertyName.Equals("Item"))
            {
                // Save current as last / preview
                this.LastItem = this.Item;
            }

            base.OnPropertyChanging(propertyName);
        }

        protected override DataTemplate GetDataTemplate(object item)
        {
            if (this.Item != null && this.Item.Equals(item))
            {
                // Must be prevented. Blink. Find solution. 
                // return null;
            }


            return base.GetDataTemplate(item);
        }
    }
}
