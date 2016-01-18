using System;
using NUnit.Framework;
using CalculatorSample;

namespace CalculatorSample.Tests
{
    [TestFixture]
    public class VerifyTests
    {

	[TestCase( 2,  2,  4)]
	[TestCase(-2,  2,  0)]
	[TestCase(-2, -2, -4)]
	public void ThatAddingIsWorkingCorrectly(int a, int b, int expected)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Add(a,b);
	    //Verify
	    Assert.AreEqual(expected, actual, "The Add functionality is not working correctly.");
	}

	[TestCase(int.MaxValue, 2)]
	[TestCase(2,            int.MaxValue)]
	[TestCase(int.MinValue, -2)]
	[TestCase(-2,           int.MinValue)]
	[ExpectedException(typeof(OverflowException))]
	public void ThatAddingIsWorkingCorrectlyForBoundaries(int a, int b)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Add(a, b);
	    //Verify
	    Assert.Fail("The Add functionality is not working correctly.");
	}

	[TestCase( 2,  2,  0)]
	[TestCase(-2,  2, -4)]
	[TestCase(-2, -2, -0)]
	[TestCase(int.MaxValue, 2, int.MaxValue - 2)]
	[TestCase(int.MinValue, -2, int.MinValue + 2)]
	[TestCase(int.MinValue, int.MinValue, 0)]
	public void ThatSubstractingIsWorkingCorrectly(int a, int b, int expected)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Substract(a, b);
	    //Verify
	    Assert.AreEqual(expected, actual, "The Substract functionality is not working correctly.");
	}

	[TestCase(int.MinValue, 2)]
	[TestCase(2, int.MinValue)]
	[TestCase(int.MaxValue, -2)]
	[TestCase(-2, int.MaxValue)]
	[ExpectedException(typeof(OverflowException))]
	public void ThatSubstractingIsWorkingCorrectlyForBoundaries(int a, int b)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Substract(a, b);
	    //Verify
	    Assert.Fail("The Substract functionality is not working correctly.");
	}

	[TestCase( 2, 2,  4)]
	[TestCase(-2, 2, -4)]
	[TestCase(-2, -2, 4)]
	[TestCase(int.MaxValue, -1, int.MinValue + 1)]
	[TestCase(int.MinValue,  1, int.MinValue)]
	public void ThatMultiplyIsWorkingCorrectly(int a, int b, int expected)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Multiply(a, b);
	    //Verify
	    Assert.AreEqual(expected, actual, "The Multiply functionality is not working correctly.");
	}

	[TestCase(int.MaxValue, 2)]
	[TestCase(2, int.MaxValue)]
	[TestCase(int.MinValue, -2)]
	[TestCase(-2, int.MinValue)]
	[TestCase(int.MinValue, -1)]
	[ExpectedException(typeof(OverflowException))]
	public void ThatMultiplyIsWorkingCorrectlyForBoundaries(int a, int b)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    int actual = sut.Multiply(a, b);
	    //Verify
	    Assert.Fail("The Multiply functionality is not working correctly.");
	}
	    
	[TestCase( 2, 2,  1)]
	[TestCase(-2, 2, -1)]
	[TestCase(-2, -2, 1)]
	[TestCase(9, 3, 3)]
	[TestCase(1, 3, 0.33333)]
	[TestCase(int.MaxValue, -1, int.MinValue + 1)]
	[TestCase(int.MinValue, 1, int.MinValue)]
	[TestCase(0, int.MaxValue, 0)]
	public void ThatDivisionIsWorkingCorrectly(int a, int b, double expected)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    double actual = sut.Divide(a, b);
	    //Verify
	    Assert.AreEqual(expected, actual, 0.001, "The Divide functionality is not working correctly.");
	}

	[TestCase(int.MinValue, int.MaxValue, -1d)]
	[TestCase(int.MinValue, int.MinValue, 1d)]
	[TestCase(int.MaxValue, int.MinValue, -1d)]
	[TestCase(int.MaxValue, int.MaxValue, 1d)]
	public void ThatDivisionIsWorkingCorrectlyForBoundaries(int a, int b, double expected)
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    double actual = sut.Divide(a, b);
	    //Verify
	    Assert.AreEqual(expected, actual, 0.001, "The Divide functionality is not working correctly.");
	}

	[Test]
	[ExpectedException(typeof(DivideByZeroException))]
	public void ThatDivisionIsWorkingCorrectlyWhenDivideByZero()
	{
	    //Setup
	    Calculator sut = new Calculator();
	    //Act
	    double actual = sut.Divide(1, 0);
	    //Verify
	    Assert.Fail("The Divide functionality is not working correctly.");
	}

    }
}