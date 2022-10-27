namespace RouteCalculator
{
    internal class Program
    {
        static void Main()
        {
            List<Token> tokens = new();
            Token usdt = new()
            {
                EqualTo = "USDT",
                Decimals = 6,
                Name = "USDT"
            };
            Token scrt = new()
            {
                EqualTo = "SCRT",
                Decimals = 6,
                Name = "SCRT"
            };
            Token usdc = new()
            {
                EqualTo = "USDT",
                Decimals = 6,
                Name = "USDC"
            };
            tokens.Add(usdt);
            tokens.Add(scrt);
            tokens.Add(usdc);

            List<Pool> pools = new();
            Pool usdtUsdc = new()
            {
                Token0 = usdt,
                Token1 = usdc,
                Amount0 = 47673050,
                Amount1 = 52440530
            };
            Pool scrtUsdt = new()
            {
                Token0 = scrt,
                Token1 = usdt,
                Amount0 = 50000000,
                Amount1 = 52440530
            };
            Pool scrtUsdc = new()
            {
                Token0 = scrt,
                Token1 = usdc,
                Amount0 = 50000000,
                Amount1 = 47673050
            };
            pools.Add(usdtUsdc);
            pools.Add(scrtUsdt);
            pools.Add(scrtUsdc);

            RouteBuilder routeBuilder = new();
            List<Route> routes = routeBuilder.GetRoutes(pools);

            Console.WriteLine($"Routes.Count = {routes.Count}");
            foreach (var route in routes)
            {
                foreach (var step in route.Steps)
                {
                    Console.Write($"{step.Side} {step.Pair.Token0.Name}{step.Pair.Token1.Name} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Hello, World!");
        }
    }
}