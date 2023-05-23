using MainProject.Core;
using MainProject.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace MainProject;

public class Program
{
    public static void SeedivitRaghaca(App app)
    {
        app.DB.Add(new Gift() { GiftName = "sachuqari1" });
        app.DB.Add(new Gift() { GiftName = "sachuqari2" });
        app.DB.Add(new Gift() { GiftName = "sachuqari3" });
        app.DB.Add(new Gift() { GiftName = "sachuqari4" });
        app.DB.Add(new Gift() { GiftName = "sachuqari5" });
        app.DB.Add(new Gift() { GiftName = "sachuqari6" });
        app.DB.Add(new Gift() { GiftName = "sachuqari7" });
        app.DB.Add(new Gift() { GiftName = "sachuqari8" });
        app.DB.Add(new Gift() { GiftName = "sachuqari9" });
        app.DB.Add(new Gift() { GiftName = "sachuqari10" });
    }
    public static void Main()
    {
        App Application = new App(@"C:\Users\skavlashvili\Desktop\EuropeProject\MainProject\Tables\");
        Application.Register("test5", "TestParoli");
        Application.Login("test5", "TestParoli");
    }
}