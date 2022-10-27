using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteCalculator
{
    public class RouteBuilder
    {
        public List<Route> GetRoutes(List<Pool> pools)
        {
            List<Route> completeRoutes = new();
            List<Route> routes0 = new();
            List<Route> routes1 = new();
            List<Route> routes2 = new();
            // level 0 -- Start
            foreach (var item in pools)
            {
                Route buyRoute = new();
                buyRoute.Steps.Add(new Step
                {
                    Pair = item,
                    Side = ESide.Buy
                });
                routes0.Add(buyRoute);
                Route sellRoute = new();
                sellRoute.Steps.Add(new Step
                {
                    Pair = item,
                    Side = ESide.Sell
                });
                routes0.Add(sellRoute);
                if (buyRoute.IsComplete())
                {
                    completeRoutes.Add(buyRoute);
                }
                if (sellRoute.IsComplete())
                {
                    completeRoutes.Add(sellRoute);
                }
            }
            // level 1 -- Dupletes
            foreach (var item in routes0)
            {
                foreach (var pair in pools)
                {
                    Route route = new()
                    {
                        Steps = new List<Step>()
                    };
                    foreach (var step in item.Steps)
                    {
                        route.Steps.Add(new Step
                        {
                            Pair = step.Pair,
                            Side = step.Side
                        });
                    }
                    bool attempt = route.TryAddPair(pair);
                    if (attempt)
                    {
                        routes1.Add(route);
                        if (route.IsComplete())
                        {
                            completeRoutes.Add(route);
                        }
                    }
                }
            }
            // level 2 -- Triplets, Triangles
            foreach (var item in routes1)
            {
                foreach (var pair in pools)
                {
                    Route route = new()
                    {
                        Steps = new List<Step>()
                    };
                    foreach (var step in item.Steps)
                    {
                        route.Steps.Add(new Step
                        {
                            Pair = step.Pair,
                            Side = step.Side
                        });
                    }
                    bool attempt = route.TryAddPair(pair);
                    if (attempt)
                    {
                        routes2.Add(route);
                        if (route.IsComplete())
                        {
                            completeRoutes.Add(route);
                        }
                    }
                }
            }
            return completeRoutes;
        }
    }
    public class Route
    {
        public List<Step> Steps = new();
        public bool IsComplete()
        {
            if (Steps.Count == 1)
            {
                if (Steps.First().Pair.Token0.EqualTo == Steps.First().Pair.Token1.EqualTo)
                {
                    return true;
                }
            }
            if (Steps.First().Side == ESide.Buy)
            {
                // we have to take Steps.First().Token0 
                if (Steps.Last().Side == ESide.Sell)
                {
                    // check steps last token1 
                    if (Steps.First().Pair.Token1.EqualTo == Steps.Last().Pair.Token1.EqualTo)
                    {
                        // Console.WriteLine("Route is looped!");
                        return true;
                    }
                }
                else
                {
                    if (Steps.First().Pair.Token1.EqualTo == Steps.Last().Pair.Token0.EqualTo)
                    {
                        //  Console.WriteLine("Route is looped!");
                        return true;
                    }
                }

            }
            else
            {
                // we have to take Steps.First().Token1
                if (Steps.Last().Side == ESide.Sell)
                {
                    // check steps last token1 
                    if (Steps.First().Pair.Token0.EqualTo == Steps.Last().Pair.Token1.EqualTo)
                    {
                        //  Console.WriteLine("Route is looped!");
                        return true;
                    }
                }
                else
                {
                    if (Steps.First().Pair.Token0.EqualTo == Steps.Last().Pair.Token0.EqualTo)
                    {
                        //   Console.WriteLine("Route is looped!");
                        return true;
                    }
                }

            }
            return false;
        }
        public bool TryAddPair(Pool pair)
        {
            if (Steps.Select(s => s.Pair).Contains(pair))
            {
                //  Console.WriteLine("Attempt to add the same pair to route");
                return false;
            }
            if (Steps.Last().Side == ESide.Buy)
            {
                if (pair.Token0.EqualTo == Steps.Last().Pair.Token0.EqualTo)
                {
                    Steps.Add(
                        new Step
                        {
                            Pair = pair,
                            Side = ESide.Sell
                        });
                    return true;
                }
                else if (pair.Token1.EqualTo == Steps.Last().Pair.Token0.EqualTo)
                {
                    Steps.Add(new Step
                    {
                        Pair = pair,
                        Side = ESide.Buy
                    });
                    return true;
                }
                else
                {
                    //   Console.WriteLine("Connection is impossible");
                }
            }
            else
            {
                if (pair.Token0.EqualTo == Steps.Last().Pair.Token1.EqualTo)
                {
                    Steps.Add(
                        new Step
                        {
                            Pair = pair,
                            Side = ESide.Sell
                        });
                    return true;
                }
                else if (pair.Token1.EqualTo == Steps.Last().Pair.Token1.EqualTo)
                {
                    Steps.Add(new Step
                    {
                        Pair = pair,
                        Side = ESide.Buy
                    });
                    return true;
                }
                else
                {
                    //  Console.WriteLine("Connection is impossible");
                }
            }
            return false;
        }
    }
    public class Step
    {
        public Pool Pair { get; set; }
        public ESide Side { get; set; }
    }
    public enum ESide
    {
        Buy,
        Sell
    }
}
