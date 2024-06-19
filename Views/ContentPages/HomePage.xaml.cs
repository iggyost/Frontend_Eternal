using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class HomePage : ContentPage
{
    public class StaticCategories
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    };
    public static int selectedCategory;
    public static List<StaticCategories> categories = new List<StaticCategories>()
    {
        new StaticCategories { Name = "Новые", CategoryId = 1 },
        new StaticCategories { Name = "Все", CategoryId = 2 },
        new StaticCategories { Name = "Популярные", CategoryId = 3 },
    };
    public async Task GetTokensNFT()
    {
        loadingIndicator.IsRunning = true;
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}nftview/get");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.tokensList = JsonConvert.DeserializeObject<NftView[]>(content).ToList();
        }
        else
        {
            await DisplayAlert("Ошибка сервера", "", "ОК");
        }
        loadingIndicator.IsRunning = false;
    }
    public async Task GetUserWallet()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}wallet/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.userWallet = JsonConvert.DeserializeObject<Wallet[]>(content).FirstOrDefault();
        }
        else
        {
            await DisplayAlert("Не удалось загрузить кошелек", "", "ОК");
        }
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

    public HomePage()
    {
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await GetTokensNFT();
        await GetUserFavorites();
        categoriesCollectionView.ItemsSource = categories;
        await GetUserWallet();
        while (true)
        {
            if (App.isFavoriteUpdated == true)
            {
                await GetUserFavorites();
                App.isFavoriteUpdated = false;
            }
            if (App.isTokensUpdated == true)
            {
                await GetTokensNFT();
                tokensCarouselView.ItemsSource = App.tokensList;
                App.isTokensUpdated = false;
            }
            await Task.Delay(3000);
        }
    }

    private void categoryButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        if (radioButton.IsChecked == true)
        {
            var categoryId = int.Parse(radioButton.AutomationId.ToString());
            switch (categoryId)
            {
                case 1:
                    tokensCarouselView.ItemsSource = App.tokensList.Where(x => x.CreationDate.Month == DateTime.Now.Month).ToList();
                    selectedCategory = 1;
                    break;
                case 2:
                    tokensCarouselView.ItemsSource = App.tokensList;
                    selectedCategory = 2;
                    break;
                case 3:
                    selectedCategory = 3;
                    var recordCount = 4;
                    var entityList = App.tokensList.OrderBy(t => Guid.NewGuid()).Take(recordCount).DistinctBy(x => x.Name);
                    tokensCarouselView.ItemsSource = entityList;
                    break;
            }
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

    private async void nftGesture_Tapped(object sender, TappedEventArgs e)
    {
        Grid grid = sender as Grid;
        var nftId = int.Parse(grid.AutomationId.ToString());
        App.selectedNft = App.tokensList.Where(x => x.NftId == nftId).FirstOrDefault();
        await Navigation.PushModalAsync(new TokenPage(App.selectedNft));
    }
    private async void buyButton_Clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        var nftId = int.Parse(imageButton.AutomationId.ToString());
        App.selectedNft = App.tokensList.Where(x => x.NftId == nftId).FirstOrDefault();
        await Navigation.PushModalAsync(new BuyTokenPage());
    }

    private void buyButton_Loaded(object sender, EventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        var nftId = int.Parse(imageButton.AutomationId.ToString());
        var isUserHave = App.tokensList.Where(x => x.UserId == App.enteredUser.UserId && x.NftId == nftId).FirstOrDefault();
        if (isUserHave != null)
        {
            imageButton.IsVisible = false;
        }
        else
        {
            imageButton.IsVisible = true;
        }
    }

    private void searchEntry_Completed(object sender, EventArgs e)
    {
        if (searchEntry.Text.Length > 0)
        {
            tokensCarouselView.ItemsSource = App.tokensList.Where(x => x.Name.Contains(searchEntry.Text)).ToList();
        }
        else
        {
            switch (selectedCategory)
            {
                case 1:
                    tokensCarouselView.ItemsSource = App.tokensList.Where(x => x.CreationDate.Month == DateTime.Now.Month).ToList();
                    selectedCategory = 1;
                    break;
                case 2:
                    tokensCarouselView.ItemsSource = App.tokensList;
                    selectedCategory = 2;
                    break;
                case 3:
                    selectedCategory = 3;
                    var recordCount = 4;
                    var entityList = App.tokensList.OrderBy(t => Guid.NewGuid()).Take(recordCount).DistinctBy(x => x.Name);
                    tokensCarouselView.ItemsSource = entityList;
                    break;
            }
        }
    }
}