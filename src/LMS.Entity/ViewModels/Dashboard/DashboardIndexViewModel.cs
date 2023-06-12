namespace LMS.Entity.ViewModels.Dashboard;

public class DashboardIndexViewModel
{
    public (int NonDeleted, int Deleted) Authors { get; set; }
    public (int NonDeleted, int Deleted) Books { get; set; }
    public (int NonDeleted, int Deleted) Categories { get; set; }
    public (int NonDeleted, int Deleted) Publishers { get; set; }
}