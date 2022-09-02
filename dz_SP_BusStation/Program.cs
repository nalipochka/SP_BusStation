

 
using dz_SP_BusStation;
using System.Net.Http.Headers;

class Program
{
    static List<Bus> buses = new List<Bus>();
    static Station station = new Station { CountPeople = 0 };
    static void Main()
    {
        Semaphore semaphoreBuses = new Semaphore(1, 5, "my_sem_bus");
        Semaphore semaphoreStation = new Semaphore(1, 5, "my_sem_station");
        for (int i = 0; i < 5; i++)
        {
            buses.Add(new Bus { BusNumber = i, BusSize = 32, PeapleCount = 0 });
        }
        station.AddPeaple();
        Console.WriteLine($"На остановке {station.CountPeople}");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int arg = j;
                Thread t1 = new Thread(() => CircleBus(buses[arg], station, semaphoreBuses));
                Thread t2 = new Thread(() => AddPeopleInStation(station, semaphoreStation));
                t1.Start();
                t2.Start();
            }
        }
    }
    static private void AddPeopleInStation(Station st, object sem)
    {
        Semaphore? s = sem as Semaphore;
        if (s!.WaitOne())
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    int count = st.AddPeaple();
                    Console.WriteLine($"На остановку пришло {count}");
                    Thread.Sleep(300);
                }
                    
            }
            finally
            {
                //Console.WriteLine($"На остановке {st.CountPeople}");
                s.Release();
            }
           
        }
    }
    static private void CircleBus(Bus bs, Station st, object? sem)
    {
        Semaphore? semaphore = sem as Semaphore;
        bool stop = false;
        while (!stop)
        {
            if (semaphore!.WaitOne())
            {
                try
                {
                    if (st.CountPeople <= bs.BusSize)
                    {
                        bs.PeapleCount = st.CountPeople;
                        st.CountPeople = 0;
                    }
                    else
                    {
                        int tmp = st.CountPeople - bs.BusSize;
                        bs.PeapleCount = st.CountPeople - tmp;
                        st.CountPeople = tmp;
                    }

                }
                finally
                {
                    stop= true;
                    Console.WriteLine($"В автобус№{bs.BusNumber} село {bs.PeapleCount}");
                    // Thread.Sleep(200);
                    Console.WriteLine($"На остановке {st.CountPeople}");
                    //Thread.Sleep(200);
                    Console.WriteLine($"Автобус №{bs.BusNumber} уехал");
                    Thread.Sleep(600);
                    semaphore.Release();
                }
            }
            else
            {
                Console.WriteLine($"{st.CountPeople} ожидают на остановке автобуса");
            }
        }
    }
}
