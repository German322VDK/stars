using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace stars
{
    /// <summary>
    /// Структура звезда
    /// </summary>
    struct Star
    {
        public int N { get; set; }
        private const double pi = Math.PI;

        /// <summary>
        /// Имя звезды
        /// </summary>
        public string Name { get; set;}

        /// <summary>
        /// Номер в таблице
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// Прямое восхождение: часы, минуты, секунды, радианы
        /// </summary>
        public int Ra_h { get; set; }
        public int Ra_m { get; set; }
        public double Ra_s { get; set; }
        public double Ra { get; set; }

        /// <summary>
        /// Склонение: градусы, минуты, секунды, радианы
        /// </summary>
        public int Dec_g { get; set; }
        public int Dec_m { get; set; }
        public double Dec_s { get; set; }
        public double Dec { get; set; }

        /// <summary>
        /// Растояние в световых годах
        /// </summary>
        public int R_LI { get; set; }

        /// <summary>
        /// Декартовы координаты x y z
        /// </summary>
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        /// <summary>
        /// Топологические вероятности
        /// </summary>
        public double[] P { get; set; }
        public double Pq { get; set; }
        public double Pmax { get; set; }
        public double Pmin { get; set; }
        public double dP { get; set; }

        /// <summary>
        /// Энтропии
        /// </summary>
        public double[] H { get; set; }
        public double Hq { get; set; }
        public double Hmax { get; set; }
        public double Hmin { get; set; }
        public double dH { get; set; }

        /// <summary>
        /// Расстояния
        /// </summary>
        public double[] R { get; set; }
        public double Rq { get; set; }
        public double Rmax { get; set; }
        public double Rmin { get; set; }
        public double dR { get; set; }
        /// <summary>
        /// вычисления данных звезды
        /// </summary>
        public void Compute()
        {
            Compute_Rad();

            Compute_xyz();

            R = new double[N];
            P = new double[N];
            H = new double[N];
        }
        
        /// <summary>
        /// Преобразования в радианы
        /// </summary>
        public void Compute_Rad()
        {
            Ra = (Ra_h + Ra_m / 60 + Ra_s / 360) * 2 * pi / 24;
            Dec = (Dec_g + Dec_m / N + Dec_s / (N * N)) * pi / 180;
        }

        /// <summary>
        /// Вычисления декартовых координат
        /// </summary>
        public void Compute_xyz()
        {
            x = R_LI * Math.Sin(Dec) * Math.Cos(Ra);
            y = R_LI * Math.Sin(Dec) * Math.Sin(Ra);
            z = R_LI * Math.Cos(Dec);
        }

        /// <summary>
        /// Информация о звезде
        /// </summary>
        /// <returns>Информация о звезде</returns>
        public string GetInfo() => $"Num = {Num}, Name = {Name}: {nameof(dR)} = {dR}, " +
            $"{nameof(dP)} = {dP}, {nameof(dH)} = {dH}, Hsum = {Hq}";

    }

    class Program
    {
        public static int N;

        /// <summary>
        /// Массив звёзд
        /// </summary>
        public static Star[] Stars;
        /// <summary>
        /// Получение данных о звёздах из файла
        /// </summary>
        /// <returns>массив звёзд</returns>
        public static async Task <Star[]> Get_Data()
        {
            Star[] stars;

            string Name;
            int Rah, Ram, Decg, Decm, Rli, num;
            double Ras, Decs;

            using (StreamReader sr = new StreamReader("stars.txt"))
            {
                string s, sN = await sr.ReadLineAsync();

                if( !Int32.TryParse(sN, out N))
                {
                    Console.WriteLine("Таблица имеет не правильный формат(N)(");
                    throw new AggregateException("Таблица имеет не правильный формат(N)(");
                }

                stars = new Star[N];

                for (int i = 0; i < N; i++)
                {
                    s = await sr.ReadLineAsync();

                    #region Parse Name

                    string sj = "";
                    int j = 0;

                    while (s[j] != '\t')
                    {
                        sj += s[j];
                        j++;
                    }

                    Name = sj;

                    #endregion

                    #region Parse RA

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Int32.TryParse(sj, out Rah))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(rah)(");
                        throw new AggregateException("Таблица имеет не правильный формат(rah)(");
                    }

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Int32.TryParse(sj, out Ram))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(ram)(");
                        throw new AggregateException("Таблица имеет не правильный формат(ram)(");
                    }

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Double.TryParse(sj, out Ras))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(ras)(");
                        throw new AggregateException("Таблица имеет не правильный формат(ras)(");
                    }

                    #endregion

                    #region Parse Dec

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Int32.TryParse(sj, out Decg))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(rah)(");
                        throw new AggregateException("Таблица имеет не правильный формат(rah)(");
                    }

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Int32.TryParse(sj, out Decm))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(ram)(");
                        throw new AggregateException("Таблица имеет не правильный формат(ram)(");
                    }

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Double.TryParse(sj, out Decs))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(ras)(");
                        throw new AggregateException("Таблица имеет не правильный формат(ras)(");
                    }

                    #endregion

                    #region Parse R and Num

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    {
                        sj += s[j];
                        j++;
                    }

                    if (!Int32.TryParse(sj, out Rli))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(r)(");
                        throw new AggregateException("Таблица имеет не правильный формат(r)(");
                    }

                    sj = "";

                    while (s[j] == '\t' || s[j] == ' ')
                        j++;

                    while (s[j] != '\t' && s[j] != ' ')
                    { 
                        sj += s[j];
                        if (j == s.Length - 1)
                            break;
                        j++;
                    }

                    if (!Int32.TryParse(sj, out num))
                    {
                        Console.WriteLine("Таблица имеет не правильный формат(num)(");
                        throw new AggregateException("Таблица имеет не правильный формат(num)(");
                    }

                    #endregion

                    #region Write to Struct

                    stars[i].N = N;
                    stars[i].Name = Name;
                    stars[i].Num = num;

                    stars[i].Ra_h = Rah;
                    stars[i].Ra_m = Ram;
                    stars[i].Ra_s = Ras;

                    stars[i].Dec_g = Decg;
                    stars[i].Dec_m = Decm;
                    stars[i].Dec_s = Decs;

                    stars[i].R_LI = Rli;

                    #endregion

                }
            }

            return stars;
        }

        /// <summary>
        /// Вычисление расстояний
        /// </summary>
        public static void Compute_R()
        {
            for (int i = 0; i < N; i++)
            {
                Stars[i].Rq = 0;
                Stars[i].Rmax = 0;
                Stars[i].Rmin = 0;

                for (int j = 0; j < N; j++)
                {
                    Stars[i].R[j] = Math.Sqrt((Stars[i].x - Stars[j].x) * (Stars[i].x - Stars[j].x)
                        + (Stars[i].y - Stars[j].y) * (Stars[i].y - Stars[j].y)
                        + (Stars[i].z - Stars[j].z) * (Stars[i].z - Stars[j].z));

                    Stars[i].Rq += Stars[i].R[j];

                    if (Stars[i].R[j] != 0)
                    {
                        if (Stars[i].Rmax == 0)
                            Stars[i].Rmax = Stars[i].Rmin = Stars[i].R[j];

                        if (Stars[i].R[j] > Stars[i].Rmax) 
                            Stars[i].Rmax = Stars[i].R[j];

                        if (Stars[i].R[j] < Stars[i].Rmin)
                            Stars[i].Rmin = Stars[i].R[j];
                    }
                }

                Stars[i].dR = (Stars[i].Rmax - Stars[i].Rmin) / (Stars[i].Rq / N*N);
            }
        }

        /// <summary>
        /// Вычисление топологических вероятностей
        /// </summary>
        public static void Compute_P()
        {
            for (int i = 0; i < N; i++)
            {
                Stars[i].Pq = 0;
                Stars[i].Pmax = 0;
                Stars[i].Pmin = 0;

                for (int j = 0; j < N; j++)
                {
                    Stars[i].P[j] = Stars[i].R[j] / Stars[i].Rq;

                    Stars[i].Pq += Stars[i].P[j];

                    if (Stars[i].P[j] != 0)
                    {
                        if (Stars[i].Pmax == 0)
                            Stars[i].Pmax = Stars[i].Pmin = Stars[i].P[j];

                        if (Stars[i].P[j] > Stars[i].Pmax)
                            Stars[i].Pmax = Stars[i].P[j];

                        if (Stars[i].P[j] < Stars[i].Pmin)
                            Stars[i].Pmin = Stars[i].P[j];
                    }

                    Stars[i].dP = (Stars[i].Pmax - Stars[i].Pmin) / (Stars[i].Pq / N * N);
                }
            }
        }

        /// <summary>
        /// Вычисление энтропий
        /// </summary>
        public static void Compute_H()
        {
            for (int i = 0; i < N; i++)
            {
                Stars[i].Hq = 0;
                Stars[i].Hmax = 0;
                Stars[i].Hmin = 0;

                for (int j = 0; j < N; j++)
                {
                    Stars[i].H[j] = Stars[i].P[j] * (1 - Stars[i].P[j]);

                    Stars[i].Hq += Stars[i].H[j];

                    if (Stars[i].H[j] != 0)
                    {
                        if (Stars[i].Hmax == 0)
                            Stars[i].Hmax = Stars[i].Hmin = Stars[i].H[j];

                        if (Stars[i].H[j] > Stars[i].Hmax)
                            Stars[i].Hmax = Stars[i].H[j];

                        if (Stars[i].H[j] < Stars[i].Hmin)
                            Stars[i].Hmin = Stars[i].H[j];
                    }

                    Stars[i].dH = (Stars[i].Hmax - Stars[i].Hmin) / (Stars[i].Hq / N * N);
                }
            }
        }

        /// <summary>
        /// запись в файлы
        /// </summary>
        public static void WriteFile()
        {
            using (StreamWriter file = new StreamWriter("R.txt"))
            {
                foreach (var el in Stars)
                {
                    for (int i = 0; i < N; i++)
                    {
                        double elr = Math.Round(el.R[i], 2);
                        file.Write(elr + "   ");
                    }
                    file.WriteLine();
                }
            }

            using (StreamWriter file = new StreamWriter("P.txt"))
            {
                foreach (var el in Stars)
                {
                    for (int i = 0; i < N; i++)
                    {
                        double elr = Math.Round(el.P[i], 2);
                        file.Write(elr + "   ");
                    }
                    file.WriteLine();
                }
            }

            using (StreamWriter file = new StreamWriter("H.txt"))
            {
                foreach (var el in Stars)
                {
                    for (int i = 0; i < N; i++)
                    {
                        double elr = Math.Round(el.H[i], 2);
                        file.Write(elr + "   ");
                    }
                    file.WriteLine();
                }
            }
        }

        /// <summary>
        /// вывод инфорамации на экран
        /// </summary>
        public static void WriteConsole()
        {
            foreach (var el in Stars)
            {
                Console.WriteLine(el.GetInfo());
            }
        }

        static async Task Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Stars = await Get_Data();

            for (int i = 0; i < N; i++)
            {
                Stars[i].Compute();
            }

            Compute_R();
            Compute_P();
            Compute_H();

            WriteFile();
            WriteConsole();

            Console.WriteLine("Работа выполнена!");
            Console.ReadKey();
        }
    }
}
