namespace RecyclerCarousel.ViewModels
{
    public class WordVm
    {
        public string Text { get; set; }

        public WordVm()
        {
            this.Text = string.Empty;
        }

        // For Debug WriteLine
        public override string ToString()
        {
            return this.Text;
        }
    }
}