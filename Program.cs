using Microsoft.Azure.Amqp.Framing;
using System.Configuration;
EstimateTicket EstimateTicket = new EstimateTicket();
Console.WriteLine("Enter a number of a month, from 1 to 12");
int number = Convert.ToInt32(Console.ReadLine());


//EstimateTicket.FeeCalculate(6);
//EstimateTicket.FeeCalculate(7);
//EstimateTicket.FeeCalculate(1);
//EstimateTicket.FeeCalculate(5);
//EstimateTicket.FeeCalculate(4);
//EstimateTicket.FeeCalculate(7);
//EstimateTicket.FeeCalculate(8);
//EstimateTicket.FeeCalculate(12);



public abstract class TrainTicket
{

    string fee = ConfigurationManager.AppSettings["Fee"];

    string path = @"C:\Users\patri\source\repos\TrainTicket\feeFile.txt";

    public MonthStrategy MonthStrategy { get; set; }

    public double FeeCalculate(int month)
    {
        string lineOutputFile = "";

        if (month == 12)
        {
            DecemberStrategy strategy = new DecemberStrategy();
            strategy.MonthNumber(month);

            lineOutputFile = $"{fee} {month} 0 {strategy.MonthNumber(month)}";

        }else if(month == 6 || month == 7)
        {
            MidYearStrategy strategy = new MidYearStrategy();
            strategy.MonthNumber(month);

            lineOutputFile = $"{fee} {month} 25 {strategy.MonthNumber(month)}";

        }else if (month < 1 || month >12)
        {
            Exception exception = new Exception("Try again, usea a correct number between 1 and 12");

        }
        else
        {
            RestYearStrategy strategy = new RestYearStrategy();
            strategy.MonthNumber(month);

            lineOutputFile = $"{fee} {month} 0 {strategy.MonthNumber(month)}";

        }
        try
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(lineOutputFile);
            writer.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return MonthStrategy.MonthNumber(month);
    }

}

public interface MonthStrategy
{

    public double MonthNumber(int month);
}

public class DecemberStrategy : MonthStrategy
{
    double fee = Convert.ToDouble(ConfigurationManager.AppSettings["Fee"]);
    public double MonthNumber(int month)
    {
        double result = 0;
        result = Convert.ToDouble(fee) * 2;
        return result;
    }
}

public class MidYearStrategy : MonthStrategy
{
    double fee = Convert.ToDouble(ConfigurationManager.AppSettings["Fee"]);
    public double MonthNumber(int month)
    {
        double result = 0;
        result = Convert.ToDouble(fee) * 0.75;
        return result;
    }
}

public class RestYearStrategy : MonthStrategy
{
    double fee = Convert.ToDouble(ConfigurationManager.AppSettings["Fee"]);
    public double MonthNumber(int month)
{
    double result = 0;
    result = Convert.ToDouble(fee);
    return result;
}
}

public class EstimateTicket : TrainTicket
{
    public EstimateTicket()
    {
        MonthStrategy = new DecemberStrategy();
        MonthStrategy = new MidYearStrategy();
        MonthStrategy = new RestYearStrategy();
    }
}

