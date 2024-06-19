using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class DepositPage : ContentPage
{
    public DepositPage()
    {
        InitializeComponent();
        payBtn.IsEnabled = false;
    }

    private async void payBtn_Clicked(object sender, EventArgs e)
    {
        if (cvvCodeEntry.Text == null && cardNumEntry.Text == null)
        {
            await DisplayAlert("Ошибка!", "Недопустимы пустые поля для ввода!", "Закрыть");
        }
        else
        {
            if (cardNumEntry.Text.Length == 16)
            {
                if (cvvCodeEntry.Text.Length == 3)
                {
                    await DisplayAlert("Оплата прошла успешно", string.Empty, "Закрыть");

                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync($"{App.conString}wallet/deposit/{App.enteredUser.UserId}/{countTokensEntry.Text}");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        App.userWallet = JsonConvert.DeserializeObject<Wallet[]>(content).FirstOrDefault();
                        App.isWalletUpdated = true;
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Ошибка оплаты", "Проверьте правильность введенных данных", "Закрыть");
                    }

                }
                else
                {
                    await DisplayAlert("Ошибка", "CVV-код должен состоять из 3 цифр", "Закрыть");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Номер карты должен состоять из 16 цифры", "Закрыть");
            }
        }
    }

    private void countTokensEntry_Completed(object sender, EventArgs e)
    {
        try
        {
            if (countTokensEntry.Text.Length != 0)
            {
                payBtn.IsEnabled = true;
                payBtn.Text = $"К оплате: {decimal.Parse(countTokensEntry.Text) * 286123,00} ₽";
            }
            else if (countTokensEntry.Text.Length == 0)
            {
                payBtn.IsEnabled = false;
            }
        }
        catch (Exception)
        {

        }
    }
}