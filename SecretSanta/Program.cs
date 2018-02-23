using System;

namespace SecretSanta
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SecretSantaDbContext context = new SecretSantaDbContext();
                GiftExchange ge = new GiftExchange(context);
                ge.Initialize("Houk Family Christmas");
                ge.LoadExistingEvent("2017 Christmas Gift Exchange");
                ge.GenerateGiftPairs();
                //ge.EmailGiftPairs();
                //ge.EmailEventLeader();
                Console.WriteLine(ge.PrintGiftPairs());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -- {0}", ex.Message);
            }
        }
    }
}
