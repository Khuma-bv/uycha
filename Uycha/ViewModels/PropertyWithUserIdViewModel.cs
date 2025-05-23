using Uycha.Models;

public class PropertyWithUserIdViewModel
{
    public List<Property> Properties { get; set; }
    public string CurrentUserId { get; set; }
    public bool IsAdmin { get; set; }
}
