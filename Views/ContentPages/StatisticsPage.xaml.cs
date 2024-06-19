using Frontend_Eternal.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_Eternal.Views.ContentPages;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
	}
	public async Task GetStatistics()
	{
		HttpClient client = new HttpClient();
		HttpResponseMessage response = await client.GetAsync($"{App.conString}usersview/get");
		if (response.IsSuccessStatusCode)
		{
			string content = await response.Content.ReadAsStringAsync();
			usersCollectionView.ItemsSource = JsonConvert.DeserializeObject<UsersView[]>(content).ToList();
		}
	}
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
		GetStatistics();
    }
}