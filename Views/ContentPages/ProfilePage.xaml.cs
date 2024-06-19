using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        nameLabel.Text = App.enteredUser.Name;
        tagLabel.Text = App.enteredUser.TagName;
        balanceLabel.Text = App.userWallet.Balance.ToString() + " ETH";
    }
    public async Task GetTokensNFT()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}nftview/get/user/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NftView[]>(content).ToList();
            userCollectionView.ItemsSource = data;
        }
        else
        {
            await DisplayAlert("Ошибка сервера", "", "ОК");
        }
    }
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await GetTokensNFT();
        while (true)
        {
            if (App.isTokensUpdated == true)
            {
                await GetTokensNFT();
                App.isTokensUpdated = false;
            }
            if (App.isWalletUpdated == true)
            {
                balanceLabel.Text = App.userWallet.Balance.ToString() + " ETH";
            }
            await Task.Delay(2000);
        }
    }
}