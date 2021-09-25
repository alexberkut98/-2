namespace Еще_одна_попытка_в_геологию
{
    class Point1<T>
    {
        public Point1(int x, int y)
        {
            X = x;
            Y = y;
            Corner = 0;
            Length = 0;
            Koef = 1;
        }
        public int X { get; set; }//Координаты точки по оси X
        public int Y { get; set; }//Координаты точки по оси Y
        public double Corner { get; set; }//Угол прямой, идущей из точки, по отношению к северу
        public double Length { get; set; }//Длинна этой самой прямой
        public int Koef { get; set; }//Коэффициент, на который нам в случае масштабирования придется умножать длинну отрезка
        public Point1<T> Previous { get; set; }
        public Point1<T> Next { get; set; }
    }
}