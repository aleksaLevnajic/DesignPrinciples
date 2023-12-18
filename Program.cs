IMembershipDiscountCalculator memb = new MembershipDiscountCalculator();
IAccountTypeDiscountCalculator accType = new AccountTypeDiscountCaluculator();

var disc = new Discount(memb, accType);

Console.WriteLine(disc.CalculateDiscount(100, AccountType.NotRegisterd, 1));
Console.WriteLine(disc.CalculateDiscount(100, AccountType.BasicCustomet, 2));
Console.WriteLine(disc.CalculateDiscount(100, AccountType.ValuableCustomer, 3));
Console.WriteLine(disc.CalculateDiscount(100, AccountType.MostValuableCustomer, 7));

public class Class1
{
    public decimal Calculate(decimal amount, int type, int years)
    {
        decimal result = 0;
        decimal disc = (years > 5) ? (decimal)5 / 100 : (decimal)years / 100;
        if (type == 1)
        {
            result = amount;
        }
        else if (type == 2)
        {
            //tip account-a korisnika           |    duzina clanstva   
            result = (amount - (0.1m * amount)) - disc * (amount - (0.1m * amount));
        }
        else if (type == 3)
        {
            //result = (0.7m * amount) - disc * (0.7m * amount);
            result = (amount - (0.3m * amount)) - disc * (amount - (0.3m * amount));
        }
        else if (type == 4)
        {
            result = (amount - (0.5m * amount)) - disc * (amount - (0.5m * amount));
        }
        return result;
    }
}

/* WHAT THE GIVEN CODE DOES AND HOW:
Method for caluclating discount
Discount is caulcaleted on users status and of how many years of membership he has
User type is presented in numbers 1-4
 */



public class Discount
{
    private readonly IMembershipDiscountCalculator _membershipDiscountCalculator;
    private readonly IAccountTypeDiscountCalculator _accTypeDiscountCalculator;

    public Discount(IMembershipDiscountCalculator membershipDiscountCalculator, IAccountTypeDiscountCalculator accTypeDiscountCalculator)
    {
        _membershipDiscountCalculator = membershipDiscountCalculator;
        _accTypeDiscountCalculator = accTypeDiscountCalculator;
    }

    public decimal CalculateDiscount(decimal price, AccountType accountType, int yearsOfMembership)
    {
        decimal result = _accTypeDiscountCalculator.CalucateDiscountAccountType(accountType, price);
        result = result - _membershipDiscountCalculator.CalculateDiscountMembership(price, yearsOfMembership);

        return result;
    }
}
public enum AccountType
{
    NotRegisterd = 1,
    BasicCustomet = 2,
    ValuableCustomer = 3,
    MostValuableCustomer = 4
}

public static class Constants
{
    public const int MAXIMUM_DISCOUNT_FOR_LOYALTY = 5;
    public const decimal DISCOUNT_FOR_SIMPLE_CUSTOMERS = 0.1m;
    public const decimal DISCOUNT_FOR_VALUABLE_CUSTOMERS = 0.3m;
    public const decimal DISCOUNT_FOR_MOST_VALUABLE_CUSTOMERS = 0.5m;
}

public interface IMembershipDiscountCalculator
{
    decimal CalculateDiscountMembership(decimal price, int yearsOfMembership);
}

public class MembershipDiscountCalculator : IMembershipDiscountCalculator
{
    public decimal CalculateDiscountMembership(decimal price, int yearsOfMembership)
    {
        decimal discountPercentage = 0;
        if (yearsOfMembership > Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY)
        {
            discountPercentage = (decimal)Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY / 100;
        }
        else
        {
            discountPercentage = (decimal)yearsOfMembership / 100;
        }

        var result = discountPercentage * (price - (discountPercentage * price));

        return result;
    }
}

public interface IAccountTypeDiscountCalculator
{
    decimal CalucateDiscountAccountType(AccountType accountType, decimal price);
}

public class AccountTypeDiscountCaluculator : IAccountTypeDiscountCalculator
{
    public decimal CalucateDiscountAccountType(AccountType accountType, decimal price)
    {
        decimal result = 0;

        switch(accountType)
        {
            case AccountType.NotRegisterd:
                result = price;
                break;
            case AccountType.BasicCustomet:
                result = price - (Constants.DISCOUNT_FOR_SIMPLE_CUSTOMERS * price);
                break;
            case AccountType.ValuableCustomer:
                result = price - (Constants.DISCOUNT_FOR_VALUABLE_CUSTOMERS * price);
                break;
            case AccountType.MostValuableCustomer:
                result = price - (Constants.DISCOUNT_FOR_MOST_VALUABLE_CUSTOMERS * price);
                break;
            default:
                throw new NotImplementedException();
        }

        return result;
    }
}
