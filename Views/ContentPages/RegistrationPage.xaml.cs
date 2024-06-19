using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();

        phoneEnter.Text = App.enteredPhone;
        phoneEnter.IsEnabled = false;

    }

    private async void continueButton_Clicked(object sender, EventArgs e)
    {
        loadingIndicator.IsRunning = true;
        if (userNameEntry.Text.Length >= 1)
        {
            if (tagNameEntry.Text.Length >= 1)
            {
                if (passwordEntry.Text.Length >= 3)
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync($"{App.conString}users/reg/{App.enteredPhone}/{userNameEntry.Text}/{tagNameEntry.Text}/{passwordEntry.Text}");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        App.enteredUser = JsonConvert.DeserializeObject<User>(content);
                        Application.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    await DisplayAlert("Пароль должен содержать не менее 3 символов", "", "ОК");
                }

            }
            else
            {
                await DisplayAlert("Тег пользователя не может быть пустым", "", "ОК");
            }
        }
        else
        {
            await DisplayAlert("Имя пользователя не может быть пустым", "", "ОК");
        }
        loadingIndicator.IsRunning = false;
    }
}