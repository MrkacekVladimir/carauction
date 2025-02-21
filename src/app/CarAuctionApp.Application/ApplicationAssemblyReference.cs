using System.Reflection;

namespace CarAuctionApp.Application;    

public class ApplicationAssemblyReference
{
    public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
}
