using System.Text;
using System.Xml.Linq;


    string currencyCode = GetUserInput("Enter the currency code (e.g., USD, EUR): ");

    try
    {
        DateTime today = DateTime.Now;
    string todayDate = today.ToString("dd/MM/yyyy");
        string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={todayDate}";
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();
            string xmlString = Encoding.UTF8.GetString(contentBytes);
            XElement root = XElement.Parse(xmlString);

            bool foundCurrency = false;
            foreach (XElement currency in root.Elements("Valute"))
            {
                string code = currency.Element("CharCode")?.Value;
                if (code == currencyCode)
                {
                    string value = currency.Element("Value")?.Value;
                    Console.WriteLine($"Exchange Rate: {value}");
                    foundCurrency = true;
                    break;
                }
            }

            if (!foundCurrency)
            {
                Console.WriteLine($"Currency with code \"{currencyCode}\" not found");
            }
        }
        else
        {
            Console.WriteLine("Failed to fetch currency quotes");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }


static string GetUserInput(string message)
{
    Console.Write(message);
    return Console.ReadLine()?.Trim().ToUpper();
}
