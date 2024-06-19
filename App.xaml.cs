using Frontend_Eternal.ApplicationData;
using Frontend_Eternal.Views.ContentPages;

namespace Frontend_Eternal;

public partial class App : Application
{
    public static string conString = "http://192.168.0.10:45455/api/";
    public static User enteredUser;
    public static string enteredPhone;
    public static Wallet userWallet;
    public static bool isWalletUpdated = false;
    public static bool isTokensUpdated = false;
    public static bool isFavoriteUpdated = false;
    public static NftView selectedNft;
    public static List<NftView> tokensList = new List<NftView>();
    public static List<FavoritesView> userFavorites = new List<FavoritesView>();
    public App()
	{
		InitializeComponent();

        MainPage = new NavigationPage(new PhoneEnterPage());
        //MainPage = new AppShell();
	}
}
