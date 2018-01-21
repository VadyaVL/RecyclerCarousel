# RecyclerCarousel

For Xamarin.Forms.

1. Add: xmlns:rv="clr-namespace:RecyclerCarousel;assembly=RecyclerCarousel"
2. Use:
<rv:RecyclerCarousel x:Name="recyclerCarousel" ItemsSource="{Binding Source={x:Reference root}, Path=Collection}">
    <rv:RecyclerCarousel.ItemTemplate>
        <DataTemplate>
            <StackLayout VerticalOptions="FillAndExpand" Margin="20" BackgroundColor="White">
                <Label Text="{Binding Text}" HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand"/>
            </StackLayout>
        </DataTemplate>
    </rv:RecyclerCarousel.ItemTemplate>
</rv:RecyclerCarousel>

Dependencies: 
* [Xamarin.Forms.CarouselView](https://www.nuget.org/packages/Xamarin.Forms.CarouselView/). 