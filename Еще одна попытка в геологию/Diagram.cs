using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Еще_одна_попытка_в_геологию
{
    class Diagram<T>
    {
        //Конструктор для создания новой диаграммы.
        public Diagram(string NewName)
        {
            Name = NewName;//Задаем имя для нашей диаграммы
            C_Corner = 0;
            C_Length = 0;
        }

        //Уникальное имя диаграммы, по которому мы будем отличать её от остальных.
        public string Name { get; set; }


        //С помощью этой переменной мы будем хранить число версий карт, написанных для неё.
        public int Count { get; set; }

        //Угол и длинна центральной трещины, если в момент начала расчетов они будут ненулевыми
        //с их помощью мы выделим эту трещину на диаграмме.
        double C_Corner { get; set; }
        double C_Length { get; set; }

        int C_X1 { get; set; }
        int C_X2 { get; set; }
        int C_Y1 { get; set; }
        int C_Y2 { get; set; }

        int C_Sector { get; set; }
        //Ссылки на предыдущий и следующий элементы в списке диаграмм.
        public Diagram<T> Previous { get; set; }
        public Diagram<T> Next { get; set; }

        //Первый элемент списка.
        Point1<T> head=null;

        //Последний элемент списка.
        Point1<T> tail=null;

        //Добавление координат очередной нанесенной на карту точки.
        public void Add(int x, int y)
        {
            Point1<T> node = new Point1<T>(x, y);
            if (head == null)
            {
                head = node;
                head.Previous = null;
            }
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            Count++;
        }

        //Получаем из сохраненных данных информацию о центральной трещине (либо же, если данная трещина не была создана в рамках данной диаграммы) набор нулей
        public void AddSavedMain(string Data)
        {
            string[] s = Data.Split(' ');
            C_Corner = Convert.ToDouble(s[0]);
            C_Length = Convert.ToDouble(s[1]);
            C_Sector = Convert.ToInt32(s[2]);
            C_X1 = Convert.ToInt32(s[3]);
            C_X2 = Convert.ToInt32(s[4]);
            C_Y1 = Convert.ToInt32(s[5]);
            C_Y2 = Convert.ToInt32(s[6]);
        }

        //Получаем из сохраненных данных прочую информацию по диаграмме (координаты точек, углы трещин и т.д.)
        public void AddSaved(string Data)
        {
            string[] s = Data.Split(' ');
            Add(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]));
            tail.Corner = Convert.ToInt32(s[2]);
            tail.Koef = Convert.ToInt32(s[3]);
            tail.Length = Convert.ToInt32(s[4]);
        }

        //Мы воссоединяем прямую и (если требуется) меняем координаты ее крайних точек.
        //Координаты меняются только при st = 1.
        //При st = 2 мы лишь воссоединяем прямую.
        //Первый набор координат - оригинальные координаты прямой (нужны для поиска оной).
        //Второй набор координат - то на что мы должны заменить оригинальные координаты.
        public void ReunionAndChange(int x1, int y1, int x2, int y2, int x1_2, int y1_2, int x2_2, int y2_2, int st)
        {
            int i = 0;
            Point1<T> current = head.Next;
            while (i == 0)
            {
                if (current.Previous.X == x1 && current.Previous.Y == y1 && current.Next.X == x2 && current.Next.Y == y2)
                {
                    i++;
                    Point1<T> current1 = current.Previous;
                    Point1<T> current2 = current.Next;
                    current1.Next = current2;
                    current2.Previous = current1;
                    current = null;
                    current = current1;
                    if (st == 1)
                    {
                        current.X = x1_2;
                        current.Y = y1_2;
                        current.Next.X = x2_2;
                        current.Next.Y = y2_2;
                    }
                }
                else
                    current = current.Next;
            }
        }

        //Функция которая, получив на входе карту, наносит на неё все, принадлежащие данной диаграмме точки и прямые, после чего возвращает полученное обратно.
        public Image Draw(Image OldMap, ref int i, int k, int st,string Name1)
        {                                                     //st - переменная, через которую мы понимаем, сколь много нам надо нарисовать.
                                                              //Если st = 1 рисуем все точки и линии данной диаграммы.
                                                              //Если st = 2 рисуем только одну точку или линию.
            Image NewMap = OldMap;                            //i - переменная покоторой мы отслеживаем количество нанесенных на карту точек и трещин
            Graphics graph;                                   //(т.к. каждая такая точка или трещина - новая версия карты за количеством которых надо следить)
            Color a;                                          //k - Коэффициент, на который нам в случае масштабирования придется умножать длинну отрезка
            if (st == 1&&Name!=Name1 || st == 2 && Name != Name1)                           //Name1 - имя текущей диаграммы, если мы на ней, все надо рисовать зеленым
                a = Color.Blue;
            else
                a = Color.Green;
            Pen p = new Pen(a, 5);
            Bitmap A;
            Bitmap B;
            Point1<T> current;
            if (st == 1 || st == 3 || st == 4)
            {
                Brush b1 = Brushes.Blue;
                Brush b2 = Brushes.Blue;
                if (st == 1)
                {
                    b1 = Brushes.Blue;
                    b2 = Brushes.Red;
                }
                if (st == 3)
                    b1 = b2 = Brushes.Green;
                if(Name==Name1)
                     b1 = b2 = Brushes.Green;
                current = head;
                int stop = 0;
                while (current != null)
                {
                    stop = 0;
                    A = (Bitmap)NewMap;
                    B = new Bitmap(NewMap.Width, NewMap.Height);
                    graph = Graphics.FromImage(B);
                    //Добавить красную точку к началу прямой
                    if (current.Previous != null && current != null)
                    {
                        if (current.Previous.X != (-1) && current.X != (-1))//Если вводим уже не первую красную точку, Закрашиваем предыдущую и ставим новую
                        {
                            graph.FillRectangle(b1, current.Previous.X - 5, current.Previous.Y - 5, 10, 10);
                            graph.FillRectangle(b2, current.X - 5, current.Y - 5, 10, 10);
                            graph.DrawLine(p, current.Previous.X, current.Previous.Y, current.X, current.Y);
                            if (k != 0 && k != 1)//Запоминаем 
                                current.Koef = k;
                            graph.Dispose();
                        }
                        else
                        {

                            if (st != 3)
                            {
                                Brush b5;
                                if(Name==Name1)
                                    b5 = Brushes.Green;
                                else
                                    b5 = Brushes.Blue;
                                if (current.Previous.X != (-1))
                                {
                                    if (current.Previous.Previous != null)
                                    {
                                        if (current.X == -1 && current.Previous.Previous.X == -1)
                                            stop = 1;
                                    }
                                    if (current.Previous.Previous == null && current.X == -1)
                                        stop = 1;
                                    if (stop != 1)
                                        graph.FillRectangle(b5, current.Previous.X - 5, current.Previous.Y - 5, 10, 10);
                                }
                                else
                                {
                                    if (current.Next != null)
                                    {
                                        if (current.Next.X == -1)
                                            stop = 1;
                                    }
                                    if (stop != 1)
                                        graph.FillRectangle(Brushes.Red, current.X - 5, current.Y - 5, 10, 10);
                                }
                            }
                            else
                            {
                                if (current.X == -1)
                                {
                                    if (current.Previous.Previous != null)
                                    {
                                        if (current.X == -1 && current.Previous.Previous.X == -1)
                                            stop = 1;
                                    }
                                    if (stop != 1)
                                        graph.FillRectangle(b1, current.Previous.X - 5, current.Previous.Y - 5, 10, 10);
                                }
                            }

                        }
                    }

                    if (current.Previous == null && current != null || i == -1)//Если мы ввели первую свою точку отмечаем ее
                    {
                        if(current.Next!=null)
                        {
                            if (current.Next.X == -1)
                                stop++;
                        }
                        if (stop != 1)
                            graph.FillRectangle(Brushes.Red, current.X - 5, current.Y - 5, 10, 10);
                    }
                    if (stop == 0)
                    {
                        Graphics g = Graphics.FromImage(A);
                        g.DrawImage(B, 0, 0, NewMap.Width, NewMap.Height);
                        g.Dispose();
                        NewMap = A;
                        if (st == 1 && i != -5 || st == 2 && i != -5)
                        {
                            NewMap.Save("save/" + "Map" + Convert.ToString(i));
                            i++;
                        }
                    }
                    current = current.Next;
                }
            }
            if (st == 2)
            {
                A = (Bitmap)NewMap;
                B = new Bitmap(NewMap.Width, NewMap.Height);
                graph = Graphics.FromImage(B);
                Brush b3;
                if (Name == Name1)
                    b3 = Brushes.Green;
                else
                    b3 = Brushes.Blue;
                if (tail.Previous != null && tail != null)
                {
                    if (tail.Previous.X != (-1) && tail.X != (-1))//Если вводим уже не первую красную точку, Закрашиваем предыдущую и ставим новую
                    {
                        graph.FillRectangle(b3, tail.Previous.X - 5, tail.Previous.Y - 5, 10, 10);
                        graph.FillRectangle(Brushes.Red, tail.X - 5, tail.Y - 5, 10, 10);
                        graph.DrawLine(p, tail.Previous.X, tail.Previous.Y, tail.X, tail.Y);
                        if (k != 0 && k != 1)//Запоминаем 
                            tail.Koef = k;
                        graph.Dispose();
                    }
                    else
                    {
                        if (tail.Previous.X != (-1))
                            graph.FillRectangle(b3, tail.Previous.X - 5, tail.Previous.Y - 5, 10, 10);
                        else
                            graph.FillRectangle(Brushes.Red, tail.X - 5, tail.Y - 5, 10, 10);
                    }

                }
                if (tail.Previous == null && tail != null || i == -1)//Если мы ввели первую свою точку отмечаем ее
                {
                    graph.FillRectangle(Brushes.Red, tail.X - 5, tail.Y - 5, 10, 10);
                }
                Graphics g = Graphics.FromImage(A);
                g.DrawImage(B, 0, 0, NewMap.Width, NewMap.Height);
                g.Dispose();
                NewMap = A;
                NewMap.Save("save/" + "Map" + Convert.ToString(i));
                i++;
            }
            return NewMap;
        }

        //Рисуем центральную трещину диаграммы, если оная была нанесена
        //Также мы можем, с помощью этой функции, работать с прямой, которую
        //надо сместить.
        public Image DrawC(Image SavedMap,ref int i,int CX1,int CY1,int CX2,int CY2)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            if(CX1==-1)
            {
                x1 = C_X1;
                x2 = C_X2;
                y1 = C_Y1;
                y2 = C_Y2;
            }
            else
            {
                x1 = CX1;
                x2 = CX2;
                y1 = CY1;
                y2 = CY2;
            }
            Color a = Color.LightGreen;
            Pen p = new Pen(a, 5);
            Bitmap A;
            Bitmap B;
            Graphics graph;
            Brush b1 = Brushes.LightGreen;
            Brush b2 = Brushes.LightGreen;
            A = (Bitmap)SavedMap;
            B = new Bitmap(SavedMap.Width, SavedMap.Height);
            graph = Graphics.FromImage(B);
            graph.FillRectangle(b1, x1 - 5, y1 - 5, 10, 10);
            graph.FillRectangle(b2, x2 - 5, y2 - 5, 10, 10);
            graph.DrawLine(p, x1, y1, x2, y2);
            graph.Dispose();
            Graphics g = Graphics.FromImage(A);
            g.DrawImage(B, 0, 0, SavedMap.Width, SavedMap.Height);
            g.Dispose();
            SavedMap = A;
            if (CX1 == -1)
            {
                SavedMap.Save("save/" + "Map" + Convert.ToString(i));
                i++;
            }
            return SavedMap;
        }

        //Проверка на наличие хоть каких то координат в данной диаграмме.
        public bool IsEmpty()
        {
            if (head == null)
                return false;
            else
                return true;
        }

        // удаление координат нанесенной на карту точки.
        public void Remove(ref int a)
        {
            Point1<T> current = tail;
            if (head != null)
            {
                if (tail.Previous != null)
                {
                    if (tail.X != -1)
                        a = 1;
                    if (tail.Previous.X == -1)
                        a = 2;
                    if (current != null)
                    {
                        tail = current.Previous;
                        tail.Next = null;
                        if (tail.X == -1&&tail.Previous!=null)
                        {
                            while (tail.Previous.X == -1)
                            {
                                tail = current.Previous;
                                tail.Next = null;
                                current = current.Previous;
                            }
                        }
                        Count--;
                    }
                }
                else
                {
                    head = tail = null;
                    Count--;
                    a = 1;
                }
            }
            else
                a = -1;

        }

        //Проводим расчеты над отдельно взятой диограммой.
        public void Calculate(double grad, int zal, int par, double S, string s, int ImageHeight, int ImageWidth, int on)
        {
            Bitmap NewDiagram = new Bitmap(1500, 1200);
            double[] Cracks = new double[72];//Таблица количества трещин для нашей будущей розы-диаграммы
            double[] Lengths = new double[72];//Таблица сумм длинн трещин для нашей будущей розы-диаграммы

            double[] x1 = new double[72];//Координаты для будущих линий нашей розы
            double[] x2 = new double[72];
            double[] y1 = new double[72];
            double[] y2 = new double[72];

            double[] G = new double[36];//Координаты ненулевых секторов, окруженных нулевыми секторами
            double[] L = new double[36];

            double[] TX1 = new double[72];//Набор координат точек, которые не лежат в центре окружности, будет использоваться при заливке
            double[] TY1 = new double[72];
            double[] TX2 = new double[72];
            double[] TY2 = new double[72];
            int spears = 0;
            int t1 = 0;
            int t2 = 50;
            t1++;
            string NameN = " ";
            int kol = 0;//С помощью этой переменной мы будем получать общее количество трещин, которое было использовано для создания данной диаграммы.
            if (par == 1 || par == 2 && S != 0 || par == 3 && S != 0)
            {
                double max = 0;
                int p2 = 0;//Подсчитывает количество ненулевых точек
                int nx = 0;
                int ny = 0;
                for (int j = 0; j < 72; j++) //Сначала мы обнуляем наши таблицы для данных
                {
                    if (j < 36)
                    {
                        G[j] = 0;
                        L[j] = 0;
                    }
                    TX1[j] = 0;
                    TY1[j] = 0;
                    TX2[j] = 0;
                    TY2[j] = 0;
                    x1[j] = 0;
                    x2[j] = 0;
                    y1[j] = 0;
                    y2[j] = 0;
                    Cracks[j] = 0;
                    Lengths[j] = 0;
                }
                SolidBrush Brush = new SolidBrush(Color.Red);//Кисть, на случай, если мы захотим залить область
                SolidBrush Brush2 = new SolidBrush(Color.White);//Кисть, чтобы удалить разрывы если таковые будут в наличии
                Pen p3 = new Pen(Color.Green, 3);//Карандаш для закрашивания центральной прямой, если выберем заливку (и если данная трещина вообще будет отмечена в данной диаграмме).
                Walk(ref Cracks, ref x1, ref x2, ref y1, ref y2, ref grad, ref max, ref TX1, ref TY1, ref TX2, ref TY2, ref p2, ref on, ref Lengths, ref par, ref S, ref G, ref L, ref spears, ref kol);//Собираем данные для диаграммы
                var font = new Font("TimesNewRoman", 25, FontStyle.Bold, GraphicsUnit.Pixel);
                string KolvK = "Общее число трещин в данной диаграмме равно: ";
                Bitmap bmp = new Bitmap(1500, 1200);//Рисуем эту самую диаграмму
                Graphics graph = Graphics.FromImage(bmp);
                PointF[] b = new PointF[3];
                KolvK += Convert.ToString(kol);
                graph.FillRectangle(Brush2, 0, 0, 1500, 1200);
                if (C_X1 != 0)
                {
                    graph.DrawLine(p3, C_X1, C_Y1, C_X2, C_Y2);
                }
                if (par == 1)
                    NameN = Convert.ToString(max);
                if (par == 2 || par == 3)
                {
                    S *= 50;
                    if (s == " sm" && S > 100)
                    {
                        s = " m";
                        S /= 100;
                    }
                    if (s == " m" && S > 1000)
                    {
                        s = " km";
                        S /= 1000;
                    }
                    S = Math.Round(S, 2);
                    NameN = Convert.ToString(S) + s;
                }
                int F = -1;
                if (zal == 1)//Мы выбрали заливку
                {

                    for (int i = 0; i < spears; i++)
                    {
                        Pen p = new Pen(Color.Red, 3);
                        Pen p1 = new Pen(Color.White, 3);
                        if (par == 1)//Если мы работаем с количеством трещин
                        {
                            if (G[i] != 0)
                            {
                                graph.DrawPie(p, 50 + ((10 - (int)L[i]) * 50), 50 + ((10 - (int)L[i]) * 50), 1000 - ((10 - (int)L[i]) * 100), 1000 - ((10 - (int)L[i]) * 100), 0, (int)G[i]);
                                graph.DrawEllipse(p1, 50 + ((10 - (int)L[i]) * 50), 50 + ((10 - (int)L[i]) * 50), 1000 - ((10 - (int)L[i]) * 100), 1000 - ((10 - (int)L[i]) * 100));
                                graph.DrawLine(p1, 550, 550, 550 + 50 * (int)L[i], 550);
                            }
                            else
                                F = i;
                        }
                        else//Работаем с суммой длинн трещин
                        {
                            if (G[i] != 0)
                            {
                                graph.DrawPie(p, 550 - (int)L[i], 550 - (int)L[i], 2 * (int)L[i], 2 * (int)L[i], 0, (int)G[i]);
                                graph.DrawEllipse(p1, 550 - (int)L[i], 550 - (int)L[i], 2 * (int)L[i], 2 * (int)L[i]);
                                graph.DrawLine(p1, 550, 550, 550 + (int)L[i], 550);
                            }
                            else
                                F = i;
                        }
                    }
                    if (par != 1)
                        t2 = 1;
                    if (F != -1)
                        graph.DrawLine(new Pen(Color.Red, 3), 550, 550, 550 + t2 * (int)L[F], 550);
                    b[0] = new Point(550, 550);
                    for (int i = 0; i < p2; i++)
                    {
                        b[1] = new Point((int)TX1[i], (int)TY1[i]);
                        b[2] = new Point((int)TX2[i], (int)TY2[i]);
                        graph.FillPolygon(Brush, b);//Закрашиваем область между ненулевыми точками
                    }
                    graph.DrawString(NameN, font, Brushes.Black, new Point(1150, 520));
                }
                if (zal == 2 && spears > 0)
                {
                    for (int i = 0; i < spears; i++)
                    {
                        if (G[i] != 0)
                        {
                            graph.DrawPie(new Pen(Color.Red, 3), 50 + ((10 - (int)L[i]) * 50), 50 + ((10 - (int)L[i]) * 50), 1000 - ((10 - (int)L[i]) * 100), 1000 - ((10 - (int)L[i]) * 100), 0, (int)G[i]);
                            graph.DrawEllipse(new Pen(Color.White, 3), 50 + ((10 - (int)L[i]) * 50), 50 + ((10 - (int)L[i]) * 50), 1000 - ((10 - (int)L[i]) * 100), 1000 - ((10 - (int)L[i]) * 100));
                            graph.DrawLine(new Pen(Color.White, 3), 550, 550, 550 + 50 * (int)L[i], 550);
                        }
                        else
                            F = i;
                    }
                    if (F != -1)
                        graph.DrawLine(new Pen(Color.White, 2), 550, 550, 550 + 50 * (int)L[F], 550);
                }
                for (double i = 0; i <= 360; i += grad)
                    graph.DrawPie(Pens.Black, 50, 50, 1000, 1000, 0, (float)i);
                for (int i = 0; i < 500; i += 50)
                    graph.DrawEllipse(Pens.Black, i + 50, i + 50, 1000 - (i * 2), 1000 - (i * 2));
                graph.DrawString("N", font, Brushes.Black, new Point(530, 20));//Стрелки компаса
                graph.DrawString("S", font, Brushes.Black, new Point(530, 1050));
                graph.DrawString("W", font, Brushes.Black, new Point(0, 520));
                graph.DrawString("E", font, Brushes.Black, new Point(1050, 520));
                graph.DrawLine(Pens.Black, 1150, 500, 1200, 500);//Показываем масштаб деления
                graph.DrawLine(Pens.Black, 1150, 490, 1150, 510);
                graph.DrawLine(Pens.Black, 1200, 490, 1200, 510);
                graph.DrawString(KolvK, font, Brushes.Black, new Point(0, 1150));
                int t = 0;
                if (zal == 2)
                {
                    for (int i = 0; i < (360 / grad); i++)//Рисуем диаграмму
                    {
                        if (x1[i] == x2[i] && y1[i] == y2[i] && x1[i] == y1[i] && x1[i] == 550)
                            t++;
                        if (t == 0)
                        {
                            if (i + 1 < (360 / grad))
                            {
                                graph.DrawLine(new Pen(Color.Red, 2), (int)x1[i], (int)(y1[i]), (int)x2[i], (int)y2[i]);//Рисуем нашу розу
                                nx = (int)x2[i];
                                ny = (int)y2[i];
                            }
                            else
                                graph.DrawLine(new Pen(Color.Red, 2), nx, ny, (int)(x1[0]), (int)(y1[0]));//Замыкаем нашу розу
                        }
                        t = 0;
                    }
                    graph.DrawString(NameN, font, Brushes.Black, new Point(1150, 520));
                }
                if (C_X1 != 0)
                {
                    graph.DrawLine(p3, C_X1, C_Y1, C_X2, C_Y2);
                }
                Bitmap b2 = new Bitmap(bmp, new Size(bmp.Width - 600, bmp.Height - 540));
                Form3 f3 = new Form3(b2);
                f3.Text = Name;
                b2.Save("save/" + "Picture " + Convert.ToString(Name));
                graph.Dispose();
                f3.Show();
            } 
        }

        //Ищем длинну прямой, положение крйних точек которой мы получаем в параметрах функции.
        double Length(int x1, int x2, int y1, int y2)
        {
            double cat1;
            if (x2 > x1) { cat1 = x2 - x1; }
            else { cat1 = x1 - x2; }
            double cat2;
            if (y2 > y1) { cat2 = y2 - y1; }
            else { cat2 = y1 - y2; }
            double a = Math.Sqrt(Math.Pow(cat1, 2) + Math.Pow(cat2, 2));
            return Math.Round(a, 2);
        }

        public void SaveData(string path)
        {
            string Name1 = Name + ".txt";
            string s;
            s = Convert.ToString(C_Corner) + " ";
            s += Convert.ToString(C_Length) + " ";
            s += Convert.ToString(C_Sector) + " ";
            s += Convert.ToString(C_X1) + " ";
            s += Convert.ToString(C_X2) + " ";
            s += Convert.ToString(C_Y1) + " ";
            s += Convert.ToString(C_Y2);
            s += "\n";
            Point1<T> current = head;
            int i = 0;
            byte[] array;
            using (FileStream fstream = new FileStream($"{path}\\" + Name1, FileMode.OpenOrCreate))
            {
                while (current != null)
                {
                    if (i == 0)
                    {
                        i++;
                        array = Encoding.Default.GetBytes(s);
                        fstream.Write(array, 0, array.Length);
                    }
                    s = Convert.ToString(current.X) + " ";
                    s += Convert.ToString(current.Y) + " ";
                    s += Convert.ToString(current.Corner) + " ";
                    s += Convert.ToString(current.Koef) + " ";
                    s += Convert.ToString(current.Length);
                    if (current.Next != null)
                        s += "\n";
                    array = Encoding.Default.GetBytes(s);
                    fstream.Write(array, 0, array.Length);
                    current = current.Next;
                }
            }
        }
        //Данная функция используется в процессе рисования диаграммы.
        //Суть ее в том, что мы отсчитываем от центра окружности заданное количество секторов вверх (или вниз)
        //и уже в выбранном месте ставим очередную точку из числа тех, по которым будет рисоваться диаграмма.
        int Intend(double a, int b)
        {
            int i = 550;
            int p = 0;
            while (p < a)
            {
                i -= (50 * b);
                p++;
            }
            return i;
        }

        //Вычисляем сумму длинн прямых, лежащих в данном секторе.
        double CheckLength(double a, double b)
        {
            int k = 0;//Переменная, сигнализирующая о нестандартных ситуациях
            if (a < 0)
            {
                k = 1;
                a *= (-1);
                a = 360 - a;
            }
            if (b > 360)
            {
                b -= 360;
                k = 2;
            }
            double c = 0;
            Point1<T> current = head;
            while (current != null)//Совершаем обход по списку прямых
            {
                if (current.X != -1 && current.Next.X != -1)//Если две соседние точки лежат в одной трещине а её угол лежит в заданном секторе
                {
                    if (k == 0)
                    {
                        if (current.Corner > a && current.Corner < b || current.Corner == a && current.Corner < b || current.Corner > a && current.Corner == b)
                            c += current.Length;
                    }
                    else
                    {
                        if (current.Corner > a && current.Corner > b || current.Corner < a && current.Corner < b || current.Corner == a && current.Corner < b || current.Corner < a && current.Corner == b || current.Corner > a && current.Corner == b || current.Corner == a && current.Corner > b)
                            c += current.Length;
                    }
                }
                current = current.Next;
            }
            return Math.Round(c, 2);
        }

        //Находим координаты для очередной точки нашей диаграмы.
        int CheckMoving(double Corner, double h, ref int x, int y, int q, int st)
        {
            int r = 50;
            if (st != 1)
                r = 1;
            Corner -= (q - 1) * 90;
            if (Corner < 0&&Corner>-90)
                Corner *= -1;
            if (Corner < 0 && Corner < -90 && Corner > -180)
                Corner += 180;
            if (Corner < 0 && Corner < -90 && Corner < -180)
                Corner += 270;
            if (q == 4 || q == 2)
                Corner = 90 - Corner;
            double t = Math.Sin(Corner * (3.14 / 180));
            int n = (int)(h * r * Math.Sin(Corner * (3.14 / 180)));
            int m = (int)(h * r * (1 - Math.Cos(Corner * (3.14 / 180))));
            if (q == 1)
            {
                x += n;
                y += m;
            }
            if (q == 2)
            {
                if (Corner == 180)
                    m = 0;
                x += n;
                y -= m;
            }
            if (q == 3)
            {
                if (Corner == -90)
                    n *= -1;
                x -= n;
                y -= m;
            }
            if (q == 4)
            {
                x -= n;
                y += m;
            }
            return y;
        }

        //Отзеркаливаем нашу розу относительно центральной точки.
        void Mirror(ref double[] C, int a)
        {                               //a - количество секторов в одной четверти окружности
            for (int i = 0; i <= a; i++)
            {
                if (C[i] > 0)
                    C[(i + 2 * a)] = C[i];
                if (C[(4 * a - (i + 1))] > 0)
                    C[(2 * a - (i + 1))] = C[(4 * a - (i + 1))];
            }
        }

        //Ищем одиноко стоящие ненулевые сектора, чтоб провести в них прямые из центра окружности.
        void SearchSpears(ref double[] C, int a, ref double[] G, ref double[] L, ref int s, double Corner, int st)
        {

            if (C[0] > 0 && C[1] == 0 && C[a - 1] == 0)//Ищем копье в нулевом секторе
            {
                G[s] = 270;
                if (st == 1)
                    L[s] = C[0];
                else
                    L[s] = 550 - C[0];
                s++;
            }
            if (C[a - 1] > 0 && C[0] == 0 && C[a - 2] == 0)//Ищем копье в последнем секторе
            {
                G[s] = 270 - Corner;
                if (st == 1)
                    L[s] = C[a - 1];
                else
                    L[s] = 550 - C[a - 1];
                s++;
            }
            for (int i = 1; i < a - 1; i++)//Ищем копья в прочих секторах
            {
                if (C[i] > 0 && C[i + 1] == 0 && C[i - 1] == 0)//Если такое копье найдено действуем
                {
                    if ((Corner * i) < 90)
                        G[s] = 270 + (Corner * i);
                    else
                        G[s] = (Corner * i) - 90;
                    if (st == 1)
                        L[s] = C[i];
                    else
                        L[s] = 550 - C[i];
                    s++;
                }
            }
        }

        //Задаем координаты для будущих прямых нашей розы.
        void SearchCoordinates(ref double[] x1, ref double[] x2, ref double[] y1, ref double[] y2, ref double[] C, int r, ref double[] X1, ref double[] Y1, ref double[] X2, ref double[] Y2, ref int F, int st)
        {
            //Cracks - длинна продвижения по каждому сектору. r - кол-во секторов
            int pr = 0;
            int i = 0;//Номер сектора, в котором мы работаем
            int j = 0;
            int nx1 = 0;//Координаты прямых розы
            int nx2 = 0;
            int ny1 = 0;
            int ny2 = 0;
            int x = 0;
            int y = 0;
            int r3 = 0;
            int sector = C_Sector;
            int corner = (int)C_Corner;
            double p = 10;
            int g = 360 / r;//Угол, на которых мы будем сдвигаться при переходе с сектора на сектор
            while (i < r - 1)//Пока мы не совершим обход по всем секторам
            {
                if (i == 19)
                     i = 19;
                if ((g * i) != 0 && (g * i) != 90 && (g * i) != 180 && (g * i) != 270)
                {
                    x = nx2;
                    y = ny2;
                }
                if (corner != 0 && i == sector)
                {
                    if (st != 1)
                        p = C_Length;
                    if ((g * (i + 1) > 0) && g * (i + 1) <= 90)//Мы в первой четверти
                    {
                        int cy;
                        int cx = 550;
                        if (st == 1)
                            cy = 50;
                        else
                            cy = 550 - (int)C_Length;
                        C_Y2 = CheckMoving(g * i, p, ref (cx), (cy), 1, st);
                        C_X2 = cx;
                        cx = 550;
                        if (st == 1)
                            cy = 1050;
                        else
                            cy = 550 + (int)C_Length;
                        C_Y1 = CheckMoving((g * i)+180, p, ref (cx), (cy), 3, st);
                        C_X1 = cx;
                    }
                    if ((g * (i + 1) > 90) && (g * (i + 1) <= 180))
                    {
                        int cy;
                        int cx = 550;
                        if (st == 1)
                            cy = 1050;
                        else
                            cy = 550 + (int)C_Length;
                        C_Y2 = CheckMoving(g * i, p, ref (cx), (cy), 2, st);
                        C_X2 = cx;
                        cx = 550;
                        if (st == 1)
                            cy = 50;
                        else
                            cy = 550 - (int)C_Length;
                        C_Y1 = CheckMoving((g * i) + 180, p, ref (cx), (cy), 4, st);
                        C_X1 = cx;
                    }
                    if ((g * (i + 1) > 180) && (g * (i + 1) <= 270))
                    {
                        int cy;
                        int cx = 550;
                        if (st == 1)
                            cy = 1050;
                        else
                            cy = 550 + (int)C_Length;
                        C_Y2 = CheckMoving(g * i, p, ref (cx), (cy), 3, st);
                        C_X2 = cx;
                        cx = 550;
                        if (st == 1)
                            cy = 50;
                        else
                            cy = 550 - (int)C_Length;
                        C_Y1 = CheckMoving((g * i) - 180, p, ref (cx), (cy), 1, st);
                        C_X1 = cx;
                    }
                    if ((g * (i + 1) > 270) && (g * (i + 1) <= 360))
                    {
                        int cy;
                        int cx = 550;
                        if (st == 1)
                            cy = 50;
                        else
                            cy = 550 - (int)C_Length;
                        C_Y2 = CheckMoving(g * i, p, ref (cx), (cy), 4, st);
                        C_X2 = cx;
                        cx = 550;
                        if (st == 1)
                            cy = 1050;
                        else
                            cy = 550 + (int)C_Length;
                        C_Y1 = CheckMoving((g * i) - 180, p, ref (cx), (cy), 2, st);
                        C_X1 = cx;
                    }
                }
                if (C[i] == 0 && C[i + 1] != 0 || C[i] != 0 && C[i + 1] != 0)//Если ни на старой, ни на новой прямой движения нет, держимся центра окружности
                {
                    if ((g * (i + 1) > 0) && g * (i + 1) <= 90)//Мы в первой четверти
                    {
                        nx1 = 550;
                        if (st == 1)
                        {
                            if ((g * i) == 0)
                            {
                                x = 550;
                                y = Intend(C[i], 1);
                            }
                            ny1 = Intend(C[i + 1], 1);
                        }
                        if (st == 2)
                        {
                            if ((g * i) == 0)
                            {
                                x = 550;
                                y = 550 - (int)C[i];
                            }
                            ny1 = 550 - (int)C[i + 1];
                        }
                        ny2 = CheckMoving(g * (i + 1), C[i + 1], ref (nx1), ny1, 1, st);
                        nx2 = nx1;
                    }
                    if ((g * (i + 1) > 90) && (g * (i + 1) <= 180))//Мы во второй четверти
                    {
                        nx1 = 550;
                        if (st == 1)
                        {
                            if ((g * i) == 90)
                            {
                                y = 550;
                                x = Intend(C[i], -1);
                            }
                            ny1 = Intend(C[i + 1], -1);
                        }
                        if (st == 2)
                        {
                            if ((g * i) == 90)
                            {
                                y = 550;
                                x = 550 + (int)C[i];
                            }
                            ny1 = 550 + (int)C[i + 1];
                        }
                        ny2 = CheckMoving((180 - (g * (i + 1))), C[i + 1], ref (nx1), ny1, 2, st);
                        nx2 = nx1;
                    }
                    if ((g * (i + 1) > 180) && (g * (i + 1) <= 270))//Мы в третьей четверти
                    {
                        nx1 = 550;
                        if (st == 1)
                        {
                            if ((g * i) == 180)
                            {
                                x = nx1;
                                y = Intend(C[i], -1);
                            }
                            ny1 = Intend(C[i + 1], -1);
                        }
                        if (st == 2)
                        {
                            if ((g * i) == 180)
                            {
                                x = 550;
                                y = 550 + (int)C[i];
                            }
                            ny1 = 550 + (int)C[i + 1];
                        }
                        ny2 = CheckMoving(((g * (i + 1)) - 180), C[i + 1], ref (nx1), ny1, 3, st);
                        nx2 = nx1;
                    }
                    if ((g * (i + 1) > 270) && (g * (i + 1) <= 360))//Мы в четвертой четверти
                    {
                        nx1 = 550;
                        if (st == 1)
                        {
                            if ((g * i) == 270)
                            {
                                x = Intend(C[i], 1);
                                y = 550;
                            }
                            ny1 = Intend(C[i + 1], 1);
                        }
                        if (st == 2)
                        {
                            if ((g * i) == 270)
                            {
                                x = 550 - (int)C[i];
                                y = 550;
                            }
                            ny1 = 550 - (int)C[i + 1];
                        }
                        ny2 = CheckMoving((g * (i + 1) - 270), C[i + 1], ref (nx1), ny1, 4, st);
                        nx2 = nx1;
                    }
                }
                else
                {
                    nx2 = ny2 = 550;
                    if (i == 0 && C[i] != 0)
                    {
                        x = 550;
                        if (st == 1)
                            y = Intend(C[i], 1);
                        else
                            y = 550 - (int)C[i];
                    }
                    if (C[i] == 0 && C[r - 1] == 0)
                        x = y = 550;
                }
                x1[i] = x;
                y1[i] = y;
                x2[i] = nx2;
                y2[i] = ny2;
                if (C[i] != 0 && C[i + 1] != 0)
                {
                    X1[j] = x;
                    Y1[j] = y;
                    X2[j] = nx2;
                    Y2[j] = ny2;
                    r3 = i + 1;
                    j++;
                }
                i++;
            }
            if (C[0] != 0 && C[r - 1] != 0 && C[r - 2] == 0&&pr==0)
            {
                pr++;
                if (st == 1)
                    y = Intend(C[r - 1], 1);
                else
                    y = 550 - (int)C[r - 1];
                nx1 = 550;
                ny1 = Intend(C[r - 1], 1);
                ny2 = CheckMoving((g * (r-1) - 270), C[r-1], ref (nx1), ny1, 4, st);
                nx2 = nx1;
                X1[j] = nx2;
                Y1[j] = ny2;
                if (st == 1)
                    Y2[j] = Intend(C[0], 1);
                else
                    Y2[j] = 550 - (int)C[0];
                X2[j] = 550;
                j++;
            }            
            if (C[0] != 0 && r3 == r - 1)
            {
                X1[j] = X2[j - 1];
                Y1[j] = Y2[j - 1];
                X2[j] = 550;
                if (st == 1)
                    Y2[j] = Intend(C[0], 1);
                else
                    Y2[j] = 550 - (int)C[0];
                j++;
            }
            F = j;
        }

        //Вычисляем количество прямых, лежащих в данном секторе.
        int CheckCrack(double a, double b)
        {
            int k = 0;//Переменная, сигнализирующая о нестандартных ситуациях
            if (a < 0)
            {
                k = 1;
                a *= (-1);
                a = 360 - a;
            }
            if (b > 360)
            {
                b -= 360;
                k = 2;
            }
            int c = 0;
            int c1 = 0;
            Point1<T> current = head;
            while (current != null && c1 == 0)//Совершаем обход по списку прямых
            {
                if (current.X != -1 && current.Next.X != -1)//Если две соседние точки лежат в одной трещине а её угол лежит в заданном секторе
                {
                    if (k == 0)
                    {
                        if (current.Corner > a && current.Corner < b || current.Corner == a && current.Corner < b || current.Corner > a && current.Corner == b)
                            c++;
                    }
                    else
                    {
                        if (current.Corner > a && current.Corner > b || current.Corner < a && current.Corner < b || current.Corner == a && current.Corner < b || current.Corner < a && current.Corner == b || current.Corner > a && current.Corner == b || current.Corner == a && current.Corner > b)
                            c++;
                    }
                }
                current = current.Next;
            }
            return c;
        }

        //Ищем максимальное количество трещин в том или ином секторе, чтоб определить масштаб диаграммы (количество трещин в одном секторе).
        double SearchMax(ref double[] a, int b, ref int l)
        {                                              // l - страховка от ситуаций, когда у нас слишком мало трещин 
            double MAX = a[0];
            int r = 0;
            for (int j = 1; j < b; j++)
            {
                if (MAX < a[j])
                {
                    MAX = a[j];
                    r = j;
                }
            }
            int u = b;
            b = 10;//Здесь мы должны определить масштаб деления
            int i = 1;
            if (MAX > 5)
            {
                l = 1;
                while (i != 0)
                {
                    if (MAX <= b)
                        i = 0;
                    if ((MAX > b) && (MAX <= (b * 2)))
                        i = 2;
                    if ((MAX > (b * 2)) && (MAX <= (b * 5)))
                        i = 5;
                    if (MAX > (b * 5))
                        i = 10;
                    if (i != 0)
                        b *= i;
                }
            }
            else//Если трещин на карте очень мало, надо предусмотреть особый масштаб
            {
                l = -1;
                if (MAX > 2 && MAX <= 5)
                    b *= 2;
                if (MAX <= 2)
                    b *= 5;
            }
            b = (b / 10);//Длинна еденичной клетки
            int st;
            if (a[r] * b <= 10)
                st = 1;
            else
                st = 2;
            int c;
            if (st == 1)
            {
                for (int j = 0; j < u; j++)//Определяем на какую длинну мы должны уходить по каждому сектору
                {
                    i = 0;
                    c = b;
                    while (i == 0)
                    {
                        if (a[j] * b >= (c + b))
                            c += b;
                        else
                            i++;
                    }
                    if (a[j] != 0)
                        a[j] = c;
                }
            }
            if (st == 2)
            {
                if (C_Length != 0)
                    C_Length /= b;
                for (int q = 0; q < u; q++)//Если число трещин в том или ином секторе исчисляется десятками или даже сотнями, 
                {                         //необходимо проредить массив, и уменьшить значение каждого его элемента на заданное число порядков
                    int d;
                    if (a[q] != 0)
                    {
                        d = (int)(a[q] / b);
                        if ((a[q] - (d * b)) > (b / 2))
                            d++;
                        a[q] = d;
                    }
                }
            }

            return b;
        }

        //Ищем максимальную сумму длинн в том или ином секторе, чтобы определить масштаб диаграммы.
        void SearchMax2(ref double[] L, int b, ref double l, ref int a)
        {
            double MAX = L[0];
            for (int j = 1; j < b; j++)
            {
                if (MAX < L[j])
                    MAX = L[j];
            }
            int i = 1;
            while ((MAX / i) > 550)//Определяем пиксельный масштаб нашей карты (Дабы наша диаграма влезла в установленные рамки)
                i++;
            for (int j = 0; j < b; j++)//Приводим все сектора окружности к данному масштабу
            {
                L[j] = L[j] / i;
            }
            if (C_Length != 0)
                C_Length /= i;
            l *= i;
            a = i;
        }

        //Исходя из положения прямой определяем её угол.
        double Corner(int x1, int x2, int y1, int y2, double gip)
        {
            double dop = 0;
            double cat1 = 0;
            if (x2 > x1 && y2 < y1)//Прямая идет вправо вверх
            {
                cat1 = y1 - y2;
            }
            if (x2 > x1 && y2 > y1)//Прямая идет вправо вниз
            {
                dop = 270;
                cat1 = x2 - x1;
            }
            if (x2 < x1 && y2 > y1)//Прямая идет влево вниз
            {
                cat1 = y2 - y1;
            }
            if (x2 < x1 && y2 < y1)//Прямая идет влево вверх
            {
                dop = 270;
                cat1 = x1 - x2;
            }
            double a = cat1 / gip;
            a = Math.Round(Math.Acos(a) * 180 / 3.14, 2);
            return dop + a;
        }

        //Здесь мы собственно находим и сохраняем координаты точек, по которым будет рисоваться данная диаграмма.
        public void Walk(ref double[] a, ref double[] x1, ref double[] x2, ref double[] y1, ref double[] y2, ref double q, ref double m, ref double[] X1, ref double[] Y1, ref double[] X2, ref double[] Y2, ref int z2, ref int on, ref double[] R, ref int st, ref double S, ref double[] G, ref double[] L, ref int spears, ref int kolv)//Здесь мы проходим по трещщинам и проводим расчеты
        {
            double bt = q;
            Get_Sector(q);
            if (tail != null)
            {
                if (tail.X != (-1))//Страховка, на случай если мы забудем замкнуть последнюю трещину
                    Add(-1, -1);
                if (tail.X == -1 && tail.Previous.X == -1)//Если пользователь в конце несколько раз нажал пробел, это надо исправить, дабы уберечь программу от бага
                {
                    Point1<T> current3 = tail.Previous;
                    while (tail.Previous.X != -1)
                    {
                        current3.Next = tail = null;
                        tail = current3;
                    }
                }
            }
            Point1<T> current = head;//Стоим в начале всего и изучаем каждую трещину, пока не дойдем до конца
            if (on == 0)//Если мы только что загрузили карту в систему, нам потребуется расчитать все длинны и углы. 
            {          //В противном случае данные расчеты не повторяются, так как все нужные данные у нас уже в наличии
                on++;
                while (current != null)
                {
                    if (current.Next != null)
                    {
                        if (current.X != (-1) && current.Next.X != (-1))
                        {
                            current.Length = Length(current.X, current.Next.X, current.Y, current.Next.Y);
                            current.Corner = Corner(current.X, current.Next.X, current.Y, current.Next.Y, current.Length);
                        }
                    }
                    current = current.Next;
                }
            }
            int r2 = 0;
            if (q == 22.5)
            {
                r2 = 16;
            }
            else
            {
                r2 = 360 / (int)q;
            }
            if (st == 1)
            {
                for (int j = 0; j < (360 / q); j++)//Считаем число точек, лежащих в том или ином секторе, чтобы собрать данные для рисования диаграммы
                {
                    a[j] = CheckCrack(j * bt - (bt / 2), j * bt + (bt / 2));
                    kolv += (int)a[j];
                }
                int g = 0;
                m = SearchMax(ref a, r2, ref g);
                Mirror(ref a, r2 / 4);
                if (g == -1)
                    m = 1 / m;
                SearchCoordinates(ref x1, ref x2, ref y1, ref y2, ref a, r2, ref X1, ref Y1, ref X2, ref Y2, ref z2, 1);
                SearchSpears(ref a, r2, ref G, ref L, ref spears, q, 1);
            }
            if (st == 2 || st == 3)
            {
                int k = 0;
                for (int j = 0; j < (360 / q); j++)//Суммируем длины трещин, лежащих в том или ином секторе
                {
                    R[j] = CheckLength(j * bt - (bt / 2), j * bt + (bt / 2));
                    a[j] = CheckCrack(j * bt - (bt / 2), j * bt + (bt / 2));
                    kolv += (int)a[j];
                    if (st == 3)
                    {
                        if (a[j] != 0)
                            R[j] /= a[j];
                    }
                }
                SearchMax2(ref R, r2, ref S, ref k);
                Mirror(ref R, r2);
                SearchCoordinates(ref x1, ref x2, ref y1, ref y2, ref R, r2, ref X1, ref Y1, ref X2, ref Y2, ref z2, 2);
                SearchSpears(ref R, r2, ref G, ref L, ref spears, q, 2);
            }
        }

        //Очищаем программу от следов работы над старой картой, чтоб можно было бы сразу начать работу над новой.
        public void Clear()
        {
            Point1<T> current = tail;
            while (current != head)
            {
                current = tail.Previous;
                if (current != null)
                    current.Next = null;
                tail = null;
                tail = current;
            }
            head = null;
        }

        //Через эти две функции мы узнаем координаты последней нанесенной на карту точки.
        public int GetLastX()
        {
            return tail.X;
        }
        public int GetLastY()
        {
            return tail.Y;
        }
        public double GetC_Length()
        {
            return C_Length;
        }

        //Вычисляем угол и длинну центральной трещины
        public void GetC(int x1, int x2, int y1, int y2)
        {
            C_Length = Length(x1, x2, y1, y2);
            C_Corner = Corner(x1, x2, y1, y2, C_Length);
        }

        //Удаляем прямую, расположенную между указанными точками
        public void RemoveMidle(int x1,int y1,int x2,int y2)
        {
            int i = 0;
            Point1<T> current = head;
            while(i==0)
            {
                if(current.Previous!=null)
                {
                    //Итак, мы нашли нужную прямую, что с ней делать?
                    if (current.Previous.X == x1 && current.Previous.Y == y1 && current.X == x2 && current.Y == y2)
                    {
                        i++;
                        //Переменная, с помощью которой мы разъеденим прямую то.е. удалим ее из карты
                        Point1<T> current1 = new Point1<T>(-1, -1);
                        current1.Previous = current.Previous;
                        current1.Next = current;
                        current.Previous.Next = current1;
                        current.Previous = current1;
                    }
                }
                if (i == 0)
                    current = current.Next;
            }
        }

        //Оказавшись в нужной диаграмме, ищем нужную линию по координатам точек и работаем над ней.
        //1. Выделяем линию вместе с ее точками зеленым.
        //2. Заправшиваем возможность удаления выделенной прямой
        public Image WorkOnTheLine(int x1,int y1,int x2,int y2, ref int Px1, ref int Py1, ref int Px2, ref int Py2, ref int Nx1, ref int Ny1, ref int Nx2, ref int Ny2, Image Map,ref int st)
        {
            int i = 0;
            Point1<T> current = head;
            while (i == 0)
            {
                if (current.Previous != null)
                {
                    if (x1 == current.Previous.X && x2 == current.X && y1 == current.Previous.Y && y2 == current.Y)
                        i++;

                    else
                        current = current.Next;
                }
                else
                    current = current.Next;
            }

             
            if (st != -1)
            {
                //Если мы здесь, значит нужная прямая найдена и мы должны узнать, является ли эта прямая изолированной.
                if (current.Previous.Previous != null)
                {
                    //Перед выделенной прямой есть еще одна.
                    if (current.Previous.Previous.X != -1)
                    {
                        st = 1;
                        Px1 = current.Previous.Previous.X;
                        Py1 = current.Previous.Previous.Y;
                        Px2 = current.Previous.X;
                        Py2 = current.Previous.Y;
                    }
                    else
                        st = 0;
                }
                if(current.Next!=null)
                {
                    //После выделенной прямой есть еще одна.
                    if (current.Next.X != -1)
                    {
                        Nx1 = current.X;
                        Ny1 = current.Y;
                        Nx2 = current.Next.X;
                        Ny2 = current.Next.Y;
                        //Нужно знать наверняка, с одного ли конца выделенная прямая связана с другими или с обоих.
                        if (st == 1)
                            st = 3;
                        else
                            st = 2;
                    }
                }
            }
            //Теперь, когда мы нашли нужную нам прямую, действуем по следующей программе:
            //1. Выделяем линию вместе с ее точками зеленым.
            //2. Заправшиваем возможность удаления выделенной прямой.
            Bitmap A = (Bitmap)Map;
            Bitmap B = new Bitmap(Map.Width, Map.Height);
            Brush b = Brushes.LightGreen;
            Graphics graph;                                   
            Color a = Color.Green;
            Pen p = new Pen(a, 5);
            graph = Graphics.FromImage(B);
            graph.FillRectangle(b, current.Previous.X - 5, current.Previous.Y - 5, 10, 10);
            graph.FillRectangle(b, current.X - 5, current.Y - 5, 10, 10);
            graph.DrawLine(p, current.Previous.X, current.Previous.Y, current.X, current.Y);
            graph.Dispose();
            Graphics g = Graphics.FromImage(A);
            g.DrawImage(B, 0, 0, Map.Width, Map.Height);
            g.Dispose();
            Map = A;
            return Map;
        }

        //Поиск прямой для удаления внутри данной диаграммы
        //x,y - координаты клика мышки, по которым надо найти прямую
        //x1_s, y1_s, x2_s, y2_s, координаты точек прямой, которая ближе всех проверенных подходит на роль искомой.
        //Name - имя диаграммы, в которой хранятся координаты, о которых говорилось выше
        public void SearchOfTheLine(int x, int y, ref int x1_s, ref int y1_s, ref int x2_s, ref int y2_s, ref string Name1, ref double min)
        {
            //Переменная, с помощью которой мы проверяем, входит ли точка в прямоугольник, диагональю которого 
            //является изучаемая в данный момент прямая
            int op = 0;

            double cat1_1 = 0;
            double cat2_1 = 0;
            double cat1_2 = 0;
            double cat2_2 = 0;
            double gip1;
            double gip2;
            Point1<T> current = head;
            while (current != null)
            {
                op = 0;
                if (current.Previous != null)
                {
                    if (current.Previous.X != -1 && current.X != -1)
                    {
                        //Прямая идет вправо вверх
                        if (current.Previous.X < current.X && current.Previous.X < x && x < current.X && current.Previous.Y > current.Y && y > current.Y && current.Previous.Y > y)
                        {
                            op++;
                            cat1_1 = current.X - current.Previous.X;
                            cat2_1 = current.Previous.Y - current.Y;
                            cat1_2 = x - current.Previous.X;
                            cat2_2 = current.Previous.Y - y;
                        }

                        //Прямая идет вправо вниз
                        if (current.Previous.X < current.X && x < current.X && current.Previous.X < x && current.Previous.Y < current.Y && y < current.Y && current.Previous.Y < y)
                        {
                            op++;
                            cat1_1 = current.Y - current.Previous.Y;
                            cat2_1 = current.X - current.Previous.X;
                            cat1_2 = y - current.Previous.Y;
                            cat2_2 = x-current.Previous.X;
                        }

                        //Прямая идет влево вниз
                        if (current.Previous.X > current.X && x > current.X && current.Previous.X > x && current.Previous.Y < current.Y)
                        {
                            op++;
                            cat1_1 = current.Y - current.Previous.Y;
                            cat2_1 = current.Previous.X - current.X;
                            cat1_2 = current.Y - y;
                            cat2_2 = x - current.X;
                        }

                        //Прямая идет влево вверх
                        if (current.Previous.X > current.X && x > current.X && current.Previous.X > x && current.Previous.Y > current.Y && y > current.Y && current.Previous.Y > y)
                        {
                            op++;
                            cat1_1 = current.Previous.Y - current.Y;
                            cat2_1 = current.Previous.X - current.X;
                            cat1_2 = y - current.Previous.Y;
                            cat2_2 = x - current.X;
                        }
                        if (op > 0)
                        {
                            gip1 = Math.Sqrt((cat1_1 * cat1_1) + (cat2_1 * cat2_1));
                            gip2 = Math.Sqrt((cat1_2 * cat1_2) + (cat2_2 * cat2_2));
                            gip1 = Math.Round(Math.Acos(cat1_1 / gip1) * 180 / 3.14, 2);
                            gip2 = Math.Round(Math.Acos(cat1_2 / gip2) * 180 / 3.14, 2);

                            /*
                             * Маловероятно, что точка на прямой, куда мы кликнем, будет лежать на кратчайшем расстоянии между крайними точками оной 
                             * (а gip1 будет представлять из себя именно кратчайшую прямую). А потому косинус по gip1 (прямой, которую мы хотим удалить) 
                             * и косинус по gip2 (новосозданной прямой, лежащей между точкой клика мышки и одной из точек исходной прямой) скорее всего не будут
                             * друг другу равны, поэтому мы:
                             * 1. Ищем только те прямые, чьи диагонали создают прямоугольник, в котором лежит точка клика мышки.
                             * 2. Выбираем из числа прямых, подходящих по критерию 2 (если их будет несколько) ту, чей косинус по gip1 меньше всего отличается от
                             * косинуса по gip2
                             */

                            if (gip1 < gip2)
                                gip2 -= gip1;
                            else
                                gip2 = gip1 - gip2;

                            //Если min = -1, значит мы подобрали только первого кандидата на прямую для удаления и сравнивать нам не с чем
                            if (min != -1 && gip2 < min || min == -1)
                            {
                                min = gip2;
                                Name1 = Name;
                                x1_s = current.Previous.X;
                                y1_s = current.Previous.Y;
                                x2_s = current.X;
                                y2_s = current.Y;
                            }
                        }
                    }
                }
                current = current.Next;
            }
        }

        //Зная координаты центральной прямой, её угол, а также угол сектора узнаем, по каким секторам диаграммы должна будет проходить центральная трещина.
        void Get_Sector(double q)
        {
            double bt = q;
            for (int j = 0; j < (360 / q) && C_Sector == 0; j++)//Считаем число точек, лежащих в том или ином секторе, чтобы собрать данные для рисования диаграммы
            {
                double a = j * bt - (bt / 2);
                double b = j * bt + (bt / 2);
                int k = 0;//Переменная, сигнализирующая о нестандартных ситуациях
                if (a < 0)
                {
                    k = 1;
                    a *= (-1);
                    a = 360 - a;
                }
                if (b > 360)
                {
                    b -= 360;
                    k = 2;
                }
                if (k == 0)
                {
                    if (C_Corner > a && C_Corner < b || C_Corner == a && C_Corner < b || C_Corner > a && C_Corner == b)
                        C_Sector = j;
                }
                else
                {
                    if (C_Corner > a && C_Corner > b || C_Corner < a && C_Corner < b || C_Corner == a && C_Corner < b || C_Corner < a && C_Corner == b || C_Corner > a && C_Corner == b || C_Corner == a && C_Corner > b)
                        C_Sector = j;
                }
            }

        }
    }
}