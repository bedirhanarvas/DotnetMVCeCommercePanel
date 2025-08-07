namespace eCommercePanel.DAL.Entities;

public class Role
{
    public int Id { get; set; }

    public string RoleType { get; set; } = "Customer";

    public ICollection<User> Users { get; set; } = new List<User>();
}
