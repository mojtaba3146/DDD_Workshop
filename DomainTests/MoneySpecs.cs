using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
namespace DomainTests;


public class MoneySpecs
{
    static T A<T>(Func<T, T>? customization = null)
    {
        var t = new Fixture().Create<T>();
        if (null != customization)
            t = customization(t);
        return t;
    }

    Money aValidMoney() => A<Money>(with => new Money(Math.Abs(with.Value)));

    // class MoneyDto
    // {
    //     public decimal Amount { get; set; }
    //     public string Currency1 { get; set; }
    //     public string Currency2 { get; set; }
    //     public string Currency3 { get; set; }
    //     public string Currency4 { get; set; }
    //     public string Currency5 { get; set; }
    //     public string Currency6 { get; set; }
    //     public string Currency7 { get; set; }
    //     public string Currency8 { get; set; }
    // }

    void x()
    {
        // var money = A<MoneyDto>.But(with => new MoneyDto
        // {
        //     Amount = Math.Abs(with.Amount)
        // });

    }

    [Theory, AutoData]
    public void Money_cannot_be_negative(decimal amount)
    => new Action(() =>               //Arrange
       new Money(-Math.Abs(amount))   //Act
       ).Should().Throw<Exception>(); //Assert

    [Theory, AutoData]
    public void Supports_subtraction(uint five)
    {
        //Arrange
        var smallerNumber = aValidMoney();
        var biggerNumber = new Money(smallerNumber.Value + five);

        //Act
        (biggerNumber - smallerNumber)

        //Assert
        .Value.Should().Be(five);
    }

    [Fact]
    public void Supports_addition()
    {
        //Arrange
        var firstNumber = aValidMoney();
        var secondNumber = aValidMoney();

        //Act
        (firstNumber + secondNumber)

        //Assert
        .Value.Should().Be(firstNumber.Value + secondNumber.Value);
    }

    [Theory, AutoData]
    public void Supports_lower_operator(uint five)
    {
        //Arrange
        var leftNumber = aValidMoney();
        var rightNumber = new Money(leftNumber.Value + five);

        //Act
        bool result = leftNumber.Value < rightNumber.Value;

        //Assert
        result.Should().Be(true);
    }

    [Theory, AutoData]
    public void Supports_bigger_operator(uint five)
    {
        //Arrange
        var rightNumber = aValidMoney();
        var leftNumber = new Money(rightNumber.Value + five);

        //Act
        bool result = leftNumber.Value > rightNumber.Value;

        //Assert
        result.Should().Be(true);
    }

}