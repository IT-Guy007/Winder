using DataModel;

RegisterCheck registerCheck = new RegisterCheck();
bool resultaat;

Console.WriteLine("Voer Email in: ");
string email = Console.ReadLine();
resultaat = registerCheck.CheckEmail(email);
Console.WriteLine("Je Email is " + resultaat);


Console.WriteLine("Voer wachtwoord in:");
string wachtwoord = Console.ReadLine();
resultaat = registerCheck.CheckPassword(wachtwoord);
Console.WriteLine("Je wachtwoord is " + resultaat);
