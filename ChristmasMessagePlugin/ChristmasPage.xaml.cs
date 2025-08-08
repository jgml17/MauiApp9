using System.Collections.ObjectModel;

namespace ChristmasMessagePlugin;

public partial class ChristmasPage : ContentPage
{
    private ObservableCollection<ChristmasWish> _wishes = new();
    private int _wishCounter = 1;
    private int _magicCounter = 0;
    private readonly string[] _christmasIcons = { "🎅", "🎄", "🎁", "⭐", "❄️", "🔔", "🕯️", "🦌" };
    private readonly string[] _christmasMessages = 
    {
        "Merry Christmas to all!",
        "Peace on Earth, goodwill to all",
        "May your days be merry and bright",
        "Wishing you joy and happiness",
        "Ho ho ho! Santa is coming!",
        "Let it snow, let it snow!",
        "Christmas magic is in the air",
        "Believe in the magic of Christmas"
    };
    private readonly string[] _santaNames = 
    {
        "Santa Claus", "Mrs. Claus", "Elf Helper", "Rudolph", "Snowman", 
        "Christmas Angel", "North Pole", "Christmas Spirit"
    };

    public ChristmasPage()
    {
        InitializeComponent();
        ChristmasCollection.ItemsSource = _wishes;
        AddInitialWishes();
    }

    private void AddInitialWishes()
    {
        _wishes.Add(new ChristmasWish
        {
            Icon = "🎅",
            Message = "Welcome to Christmas Magic!",
            From = "Santa Claus",
            Time = DateTime.Now.ToString("HH:mm")
        });
        
        _wishes.Add(new ChristmasWish
        {
            Icon = "🎄",
            Message = "The Christmas tree is ready!",
            From = "Christmas Elf",
            Time = DateTime.Now.AddMinutes(-5).ToString("HH:mm")
        });
        
        UpdateCounters();
    }

    private async void OnMakeWish(object sender, EventArgs e)
    {
        var random = new Random();
        var icon = _christmasIcons[random.Next(_christmasIcons.Length)];
        var message = _christmasMessages[random.Next(_christmasMessages.Length)];
        var from = _santaNames[random.Next(_santaNames.Length)];
        
        _wishes.Add(new ChristmasWish
        {
            Icon = icon,
            Message = $"{message} (Wish #{_wishCounter++})",
            From = from,
            Time = DateTime.Now.ToString("HH:mm")
        });
        
        UpdateCounters();
        
        await DisplayAlert("🎁 Wish Made!", 
            $"Your Christmas wish has been sent to the North Pole!\n\n\"{message}\"", 
            "Ho Ho Ho!");
    }

    private async void OnSnowMagic(object sender, EventArgs e)
    {
        _magicCounter++;
        
        var magicalWishes = new[]
        {
            new { Icon = "❄️", Message = "Snowflakes are dancing!", From = "Winter Fairy" },
            new { Icon = "⭐", Message = "A shooting star grants your wish!", From = "Christmas Star" },
            new { Icon = "🔔", Message = "Jingle bells are ringing!", From = "Christmas Bell" },
            new { Icon = "🕯️", Message = "Candles light the way home", From = "Christmas Candle" },
            new { Icon = "🦌", Message = "Reindeer are flying overhead!", From = "Rudolph" }
        };
        
        var random = new Random();
        var magicalWish = magicalWishes[random.Next(magicalWishes.Length)];
        
        _wishes.Add(new ChristmasWish
        {
            Icon = magicalWish.Icon,
            Message = magicalWish.Message,
            From = magicalWish.From,
            Time = DateTime.Now.ToString("HH:mm")
        });
        
        UpdateCounters();
        
        await DisplayAlert("❄️ Snow Magic!", 
            $"Christmas magic has been cast!\n\n{magicalWish.Message}", 
            "Magical!");
    }

    private async void OnStarWish(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var wish = (ChristmasWish)button.BindingContext;
        
        if (!wish.IsStarred)
        {
            wish.IsStarred = true;
            button.Text = "🌟";
            button.BackgroundColor = Color.FromArgb("#FF6347");
            
            await DisplayAlert("⭐ Starred!", 
                $"You starred: \"{wish.Message}\"\n\nThis wish is now extra special!", 
                "Wonderful!");
        }
    }

    private async void OnClearBoard(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("🎄 Clear Christmas Board", 
            "Are you sure you want to clear all Christmas wishes?\n\nSanta might be sad! 🎅", 
            "Yes, Clear", "Keep Wishes");
        
        if (confirm)
        {
            _wishes.Clear();
            _wishCounter = 1;
            _magicCounter = 0;
            UpdateCounters();
            
            await DisplayAlert("🎄 Board Cleared", 
                "All wishes have been sent to Santa's workshop!\n\nReady for new Christmas magic! ✨", 
                "Ho Ho Ho!");
        }
    }

    private void UpdateCounters()
    {
        WishesCount.Text = _wishes.Count.ToString();
        MagicCount.Text = _magicCounter.ToString();
    }
}

public class ChristmasWish
{
    public string Icon { get; set; } = "";
    public string Message { get; set; } = "";
    public string From { get; set; } = "";
    public string Time { get; set; } = "";
    public bool IsStarred { get; set; } = false;
}