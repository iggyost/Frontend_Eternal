using Frontend_Eternal.ApplicationData;

namespace Frontend_Eternal.Views.ContentPages;

public partial class TokenPage : ContentPage
{
	public TokenPage()
	{
		InitializeComponent();
	}
    public static NftView currentNft = new NftView();
    public TokenPage(NftView nft)
    {
        InitializeComponent();

        currentNft = nft;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        List<NftView> nftView = new List<NftView>();
        nftView.Add(App.selectedNft);
        tokenCollectionView.ItemsSource = nftView;
        if (App.selectedNft.UserId == App.enteredUser.UserId)
        {

        }
    }

    private void buyTokenButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new BuyTokenPage());
    }

    private void buyTokenButton_Loaded(object sender, EventArgs e)
    {
        ImageButton imageButton = sender as ImageButton;
        if (App.enteredUser.UserId == App.selectedNft.UserId)
        {
            imageButton.IsVisible = false;
        }
    }
}