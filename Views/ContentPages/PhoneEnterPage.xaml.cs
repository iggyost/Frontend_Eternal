using System.Text.RegularExpressions;

namespace Frontend_Eternal.Views.ContentPages;

public partial class PhoneEnterPage : ContentPage
{
	public PhoneEnterPage()
	{
		InitializeComponent();
	}

    private async void continueButton_Clicked(object sender, EventArgs e)
    {
        if (phoneEnter.Text == string.Empty || phoneEnter.Text == null)
        {
            await DisplayAlert("Поле для ввода не должно быть пустым", "", "OK");
        }
        else
        {
            var regex = new Regex("^((\\+7|7|8)+([0-9]){10})$");
            if (regex.IsMatch(phoneEnter.Text))
            {
                loadingIndicator.IsRunning = true;
                phoneBorder.IsEnabled = false;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{App.conString}users/phone/{phoneEnter.Text}");
                if (response.IsSuccessStatusCode)
                {
                    App.enteredPhone = phoneEnter.Text;
                    await Navigation.PushAsync(new PasswordEnterPage());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    App.enteredPhone = phoneEnter.Text;
                    await Navigation.PushAsync(new RegistrationPage());
                }
            }
            else
            {
                await DisplayAlert("Номер не соответствует формату", "", "OK");
            }
        }
        phoneBorder.IsEnabled = true;
        loadingIndicator.IsRunning = false;
    }
}