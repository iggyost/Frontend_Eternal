using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class FavoritesPage : ContentPage
{
	public FavoritesPage()
	{
		InitializeComponent();
	}
    public async Task GetUserFavorites()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favorites/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.userFavorites = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
        }
        else
        {
            await DisplayAlert("Не удалось загрузить 'Понравившиеся'", "", "ОК");
        }
    }
    public async Task SaveFavorite(int nftId)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favorites/save/{App.enteredUser.UserId}/{nftId}");
        if (response.IsSuccessStatusCode)
        {
            App.isFavoriteUpdated = true;
        }
    }
    public async Task RemoveFavorite(int nftId)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}favorites/remove/{App.enteredUser.UserId}/{nftId}");
        if (response.IsSuccessStatusCode)
        {
            App.isFavoriteUpdated = true;
        }
    }
    private void favoriteButton_Loaded(object sender, EventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        var nftId = int.Parse(imageButton.AutomationId.ToString());
        var isFavorite = App.userFavorites.Where(x => x.NftId == nftId).FirstOrDefault();
        if (isFavorite != null)
        {
            imageButton.Source = "heart_icon_check.png";
        }
        else
        {
            imageButton.Source = "heart_icon_uncheck.png";
        }
    }

    private async void favoriteButton_Clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        var nftId = int.Parse(imageButton.AutomationId.ToString());
        imageButton.IsEnabled = false;
        var isFavorite = App.userFavorites.Where(x => x.NftId == nftId).FirstOrDefault();
        if (isFavorite != null)
        {
            await RemoveFavorite(nftId);
            imageButton.Source = "heart_icon_uncheck.png";
        }
        else
        {
            await SaveFavorite(nftId);
            imageButton.Source = "heart_icon_check.png";
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        favoritesCollectionView.ItemsSource = App.userFavorites.ToList();
        while (true)
        {
            if (App.isFavoriteUpdated == true)
            {
                await GetUserFavorites();
                favoritesCollectionView.ItemsSource = App.userFavorites.ToList();
                App.isFavoriteUpdated = false;
            }
            await Task.Delay(2000);
        }
    }
}