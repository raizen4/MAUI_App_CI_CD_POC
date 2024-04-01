using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using MauiMicroMvvm;
using Plugin.Firebase.Auth;
using INavigation = MauiMicroMvvm.INavigation;

namespace MauiAppCiCd.ViewModels;

public class MainPageViewModel : MauiMicroViewModel
{
    private readonly IFirebaseAuth _auth;
    public ICommand LoginCommand { get; set; }
    public ICommand OpenPopupCommand { get; set; }


    public string Email
    {
        get=> Get<string>();
        set => Set(value);
    }
    
    public string Password
    {
        get=> Get<string>();
        set => Set(value);
    }

    public MainPageViewModel(IPopupService popupService, 
        INavigation navigation,
        IFirebaseAuth auth, 
        ViewModelContext context) : base(context)
    {
        _auth = auth;
        LoginCommand = new Command(async () =>
        {
            var result = await _auth.CreateUserAsync(Email, Password);
            if (result.Uid != null)
            {
                // Navigate to the next page
            }
            else
            {
                // Show error message
            }
        });
        OpenPopupCommand = new Command(async () =>
        {
            await popupService.ShowPopupAsync<PopupTestPageViewModel>();
        });
    }
}