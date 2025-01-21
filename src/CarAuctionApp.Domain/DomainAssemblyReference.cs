using System.Reflection;

namespace CarAuctionApp.Domain;

public static class DomainAssemblyReference
{
    public static Assembly Assembly => typeof(DomainAssemblyReference).Assembly;
}
