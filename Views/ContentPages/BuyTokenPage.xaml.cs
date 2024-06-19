using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class BuyTokenPage : ContentPage
{
    public BuyTokenPage()
    {
        InitializeComponent();

        this.BindingContext = App.selectedNft;
        buyButton.IsEnabled = true;
    }
    public async Task GetTokensNFT()
    {
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
    }
    public async Task BuyToken()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}usersnft/update/{App.enteredUser.UserId}/{App.selectedNft.NftId}/{App.selectedNft.CostCurrency}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.userWallet = JsonConvert.DeserializeObject<Wallet>(content);
            buyButton.IsEnabled = false;
            App.isWalletUpdated = true;
            App.isTokensUpdated = true;
            await DisplayAlert("Покупка произведена успешно!", "", "ОК");
            await Navigation.PopModalAsync();
        }
    }
    private void depositButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new DepositPage());
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        balanceLabel.Text = App.userWallet.Balance.ToString() + " ETH";
        while (true)
        {
            if (App.isWalletUpdated == true)
            {
                balanceLabel.Text = App.userWallet.Balance.ToString() + " ETH";
                App.isWalletUpdated = false;
            }
            else
            {

            }
            await Task.Delay(3000);
        }
    }

    private async void buyButton_Clicked(object sender, EventArgs e)
    {
        if (App.userWallet.Balance > App.selectedNft.CostCurrency)
        {
            await BuyToken();
            App.isTokensUpdated = true;
            App.isWalletUpdated = true;
        }
        else
        {
            await DisplayAlert("Недостаточно токенов!", "", "ОК");
        }
    }
}