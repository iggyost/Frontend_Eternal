using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class PasswordEnterPage : ContentPage
{
	public PasswordEnterPage()
	{
		InitializeComponent();
	}

    private void passwordEntry_Completed(object sender, EventArgs e)
    {

    }

    private async void continueButton_Clicked(object sender, EventArgs e)
    {
        loadingIndicator.IsRunning = true;

        if (passwordEntry.Text.Length > 3)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}users/enter/{App.enteredPhone}/{passwordEntry.Text}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                App.enteredUser = JsonConvert.DeserializeObject<User>(content);
                Application.Current.MainPage = new AppShell();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                await DisplayAlert("Неверный пароль!", "", "ОК");
            }
        }
        else
        {
            await DisplayAlert("Минимум 3 символа в пароле!", "", "ОК");
        }
        loadingIndicator.IsRunning = false;
    }
}