# RecyclerCarousel

For Xamarin.Forms.

1. Add:
```xml
xmlns:rv="clr-namespace:RecyclerCarousel;assembly=RecyclerCarousel"
```xml
2. Use:
```xml
<rv:RecyclerCarousel x:Name="recyclerCarousel" ItemsSource="{Binding Source={x:Reference root}, Path=Collection}">
    <rv:RecyclerCarousel.ItemTemplate>
        <DataTemplate>
            <StackLayout VerticalOptions="FillAndExpand" Margin="20" BackgroundColor="White">
                <Label Text="{Binding Text}" HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand"/>
            </StackLayout>
        </DataTemplate>
    </rv:RecyclerCarousel.ItemTemplate>
</rv:RecyclerCarousel>
```xml

Dependencies: 
* [Xamarin.Forms.CarouselView](https://www.nuget.org/packages/Xamarin.Forms.CarouselView/). 