using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Еще_одна_попытка_в_геологию
{
    //Здесь мы будем хранить список диаграмм, чтоб потом их по очереди показать.
    class ListOfDiagrams<T>
    {
        //Первый элемент.
        Diagram<T> head;

        //Последний элемент.
        Diagram<T> tail;

        //Указатель на диаграмму, в которую добавляются новые трещины
        Diagram<T> current3;

        //Меняем текущую диаграмму
        public void Changing(string Name)
        {
            Diagram<T> current = head;
            while (current.Name != Name)
                current = current.Next;
            current3 = current;
        }

        //Откатываем смену диаграммы и возвращаемся к последней из созданных
        public void CanselChanging()
        {
            current3 = tail;
        }

        //Перемещаем прямую с указанными координатами из диаграммы Name1 в диаграмму Name2
        public void MovingLines(int x1, int y1, int x2, int y2, string Name1, string Name2)
        {
            //Работа по перемещению прямой должна быть проведена в 2 этапа:
            //1. Удалить прямую из диаграммы Name1.
            //2. Добавить прямую в диаграмму Name2.
            
            //Этап 1.
            Diagram<T> current = head;
            while (current.Name != Name1)
                current = current.Next;
            current.RemoveMidle(x1, y1, x2, y2);

            //Этап 2.
            current = head;
            while (current.Name != Name2)
                current = current.Next;
            current.Add(x1, y1);
            current.Add(x2, y2);
            current.Add(-1, -1);
        }

        //Мы воссоединяем прямую и (если требуется) меняем координаты ее крайних точек.
        //Координаты меняются только при st = 1.
        //При st = 2 мы лишь воссоединяем прямую.
        public void ReunionAndChange(int x1, int y1, int x2, int y2, int x1_2, int y1_2, int x2_2, int y2_2, string Name, int st)
        {
            Diagram<T> current = head;
            while (current.Name != Name)
                current = current.Next;
            current.ReunionAndChange(x1, y1, x2, y2, x1_2, y1_2, x2_2, y2_2, st);
        }

        //Осуществляем смещение прямой
        public Image MovingPoints(Image Map, string Name, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4,int st)
        {
            int i = 0;
            Diagram<T> current = head;
            while(i==0)
            {
                if (current.Name != Name)
                    current = current.Next;
                else
                {
                    i++;
                   Map = current.DrawC(Map, ref i, x1, y1, x2, y2);
                    if (st > 0)
                        Map = current.DrawC(Map, ref i, x3, y3, x4, y4);
                }
            }
            return Map;
        }

        //Добавляем новую диаграмму.
        public void Add(string NewName)
        {
            //Прежде чем мы создадим новую диаграмму, надо замкнуть старую
            Diagram<T> node = new Diagram<T>(NewName);

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
            current3 = tail;
        }

        //Ищем диаграмму для удаления точки, не являющейся последней из введеных нами
        public void RemoveMidlePoint(int x1,int y1,int x2,int y2,string Name)
        {
            int i = 0;
            Diagram<T> current = head;
            while(i==0)
            {
                if (current.Name == Name)
                {
                    i++;
                    current.RemoveMidle(x1, y1, x2, y2);
                }
                else
                    current = current.Next;
            }
        }

        //Работа с данными, сохраненными в файле
        public void AddSaved(string path,string Name)
        {
            string textFromFile;
            using (FileStream fstream = File.OpenRead($"{path}\\" + Name + ".txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                textFromFile = System.Text.Encoding.Default.GetString(array);
            }

            //Сначала мы разбиваем содержимое файла на отдельные строки
            string[] subs = textFromFile.Split('\n');

            //Затем каждую строку разбиваем уже на отдельные символы
            for (int i=0;i<subs.Length;i++)
            {
                if (i == 0)
                    Add(Name);
                if (i != 0)
                    tail.AddSaved(subs[i]);
                else
                    tail.AddSavedMain(subs[i]);              
            }
        }

        //Добавить новую точку в последнюю на данный момент диаграмму.
        public void AddPoint(int x, int y)
        {
            current3.Add(x, y);
        }

        //Удалить последнюю точку из последней на данный момент диаграммы.
        public void RemovePoint(ref int a)
        {
            if (current3.IsEmpty())
                current3.Remove(ref a);
            else
            {
                if (current3.Previous != null)
                {
                    a = 0;
                    Diagram<T> current = current3;
                    current3 = current3.Previous;
                    current3.Next = current = null;
                    current3.Remove(ref a);
                }
                else
                    a = -1;
            }
        }

        //По координатам клика мышки ищем прямую для удаления
        public Image SearchOfTheLine(int x, int y, Image Map,ref int x1,ref int y1,ref int x2,ref int y2, ref int Px1, ref int Py1, ref int Px2, ref int Py2, ref int Nx1, ref int Ny1, ref int Nx2, ref int Ny2, ref string Name,ref int st)
        {
            int i = 0;
            double min = -1;
            Diagram<T> current = head;
            //Поиск по диаграммам
            while (current != null)
            {
                current.SearchOfTheLine(x, y, ref x1, ref y1, ref x2, ref y2, ref Name, ref min);
                current = current.Next;
            }

            /*
             * Теперь когда мы нашли нужную линию, надо выделить ее на карте
             */
            current = head;
            while (i == 0)
            {
                if (current.Name != Name)
                    current = current.Next;
                else
                {
                    i++;
                    Map = current.WorkOnTheLine(x1, y1, x2, y2, ref Px1, ref Py1, ref Px2, ref Py2, ref Nx1, ref Ny1, ref Nx2, ref Ny2, Map,ref st);
                }
            }
            return Map;
        }
        //Здесь мы создаем текстовые файлы, в каждом из которых будут храниться координаты определенной диаграммы
        public void SaveData(string path)
        {
            string s;
            Diagram<T> current = head;

            using (FileStream fstream = new FileStream($"{path}\\List of names.txt", FileMode.OpenOrCreate))
            {
                while (current != null)
                {
                    s = current.Name;
                    if (current.Next != null)
                        s += " ";
                    byte[] array = Encoding.Default.GetBytes(s);
                    fstream.Write(array, 0, array.Length);
                    current.SaveData(path);
                    current = current.Next;
                }
            }
        }

        //Узнаем число версий карты, что были созданы после появления диаграммы с данным именем.
        public int GetCount(string Name)
        {
            Diagram<T> current = tail;
            int a = 0;
            int b = 0;
            while (current != null && b == 0)
            {
                if (current.Name == Name)
                    b++;
                a = a + current.Count;
                if (b == 0)
                    current = current.Previous;
            }
            return a;
        }

        //Наносим на карту точки и прямые, полученные из сохраненных данных
        public Image PaintSaved(Image SavedMap, ref int i)
        {
            Diagram<T> current = head;
            while (current != null)
            {
                SavedMap = current.Draw(SavedMap, ref i, 0, 1,current3.Name);
                if (current.GetC_Length() != 0)
                    SavedMap = current.DrawC(SavedMap, ref i, -1, -1, -1, -1);
                current = current.Next;
            }
            return SavedMap;
        }

        //Если мы пришли сюда, значит пришлось значительно откатиться назад, дабы удалить не самую последнюю из созданных диаграмм.
        //В этом случае нам необходимо поочередно перерисовать все точки и линии оставшихся диаграмм и, что ещё важнее, сохранить эту поочередность.
        //Здесь мы имеем карту, которую надо заполнить всеми диаграммами что были после удаленной.
        public Image RedrawingTheMap(Image OldMap, string Name, ref int num, int koef, int st)
        {
            Diagram<T> current;
            if (st == 1 || st == 3 || st == 4)
            {
                current = head;
                while (current.Name != Name)
                    current = current.Next;
                //Теперь же мы должны по очереди заполнить карту
                if (st == 1)
                {
                    while (current != null)
                    {
                        OldMap = current.Draw(OldMap, ref num, koef, st, current3.Name);
                        current = current.Next;
                    }
                }
                else
                    OldMap = current.Draw(OldMap, ref num, koef, st, current3.Name);
            }
            else
            {
                current = current3;
                OldMap = current.Draw(OldMap, ref num, koef, 2, current3.Name);
            }
            return OldMap;
        }

        //Узнаем имя диаграммы, на которой мы сейчас находимся
        public string GetName() { return current3.Name; }

        //Удаление ненужной диаграммы по её имени.
        public void Remove(ref string Name, ref int a,ref int b)
        {
            Diagram<T> current = tail;//Поиск не нужной начинаем с конца
            if (head != null)
            {
                while (current != null && a == 0)//Идем по списку, пока либо не удалим диаграмму, либо убедимся что диаграммы с данным именем нет
                {
                    if (tail.Previous != null)
                    {
                        if (current.Name == Name)
                        {
                            //Мы все же удалили текущую диаграмму
                            if (Name == current3.Name)
                                b++;
                            if (current.Previous != null) { current.Previous.Next = current.Next; }
                            if (current.Next != null) { current.Next.Previous = current.Previous; }
                            if (Name == tail.Name) { tail = tail.Previous; }
                            Name = current.Next.Name;
                            current = null;
                            a++;
                        }
                    }
                    else
                    {
                        head = tail = null;
                    }
                    if (current != null)
                        current = current.Previous;
                }
                if (a == 0)
                    a--;

            }            //Если мы удаляем диаграмму, надо чтобы можно было сразу добавлять линии в предыдущую, для чего необходимо удалить все замыкания,
            //которые были поставлены перед переходом на новую диаграмму.
            current = tail;
            int f = -1;
            while (f == -1)
            {
                if (current != null)
                {
                    if (current.GetLastX() == -1)
                    {
                        current.Remove(ref f);
                        //В этот момент нужно отработать ещё один откат карты
                        f = current.GetLastX();
                    }
                    else
                        f++;
                }
                else
                    f++;
            }
        }

        //Проверяем, вводилось ли данное имя раньше, дабы избежать повторов.
        public int CheckNames(string Name)
        {
            int a = 0;
            Diagram<T> current = tail;
            if (head != null)
            {
                while (current != null && a == 0)
                {
                    if (current.Name == Name)
                        a++;
                    else
                        current = current.Next;
                }
            }
            return a;
        }

        //Создание карты, которая будет использована при работе со смещением прямой.
        //Переменная ch - наша страховка от попыток вбить клин между одними и теми же прямыми
        public Image SaveMapForWork(Image Map, string Name, int x1, int y1, int x2, int y2, int Px1, int Py1, int Px2, int Py2, int Nx1, int Ny1, int Nx2, int Ny2, int st,int ch)
        {
            Diagram<T> current = head;
            int i = -5;
            while (current != null)
            {
                i = -5;
                if (current.Name == Name)
                {
                    if (ch == 0)
                        current.RemoveMidle(x1, y1, x2, y2);
                    if (st == 1)
                        current.RemoveMidle(Px1, Py1, Px2, Py2);
                    if (st == 2)
                        current.RemoveMidle(Nx1, Ny1, Nx2, Ny2);
                }
                Map = current.Draw(Map, ref i, 0, 1, current3.Name);
                current = current.Next;
            }
            return Map;
        }

        //Проверяем, к какой из точек прямой место нажатия кнопкой мыши (x3,y3)
        public int CheckPoints(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            int a = 0;
            double cat1_1 = 0;
            double cat2_1 = 0;
            double cat1_2 = 0;
            double cat2_2 = 0;
            double gip1 = 0;
            double gip2 = 0;

            if (x1 > x3)
                cat1_1 = x1 - x3;
            else
                cat1_1 = x3 - x1;

            if (y1 > y3)
                cat2_1 = y1 - y3;
            else
                cat2_1 = y3 - y1;

            if (x2 > x3)
                cat1_2 = x2 - x3;
            else
                cat1_2 = x3 - x2;

            if (y2 > y3)
                cat2_2 = y2 - y3;
            else
                cat2_2 = y3 - y2;

            //Расположенные ниже прямые обозначают расстояние от точки нажатия кнопки мыши до каждой из крайних точек прямой.
            //Кратчайшее из них будет принадлежать точке, за которую будет тянуть курсор
            gip1 = Math.Sqrt((cat1_1 * cat1_1) + (cat2_1 * cat2_1));
            gip2 = Math.Sqrt((cat1_2 * cat1_2) + (cat2_2 * cat2_2));
            if (gip1 < gip2)
                a = 1;
            else
                a = 2;
            return a;
        }

        //Здесь мы проводим расчеты над каждой диаграммой в отдельности и выводим их все.
        public void FinalCalculates(double grad, int zal, int par, double S, string s, int ImageHeight, int ImageWidth, int on)
        {
            //Идем по диаграммам с самого начала
            Diagram<T> current = head;
            while (current != null)
            {
                current.Calculate(grad, zal, par, S, s, ImageHeight, ImageWidth, on);
                current = current.Next;
            }
        }

        //Очищаем программу от следов работы над старой картой, чтоб можно было бы сразу начать работу над новой.
        public void Clear()
        {
            Diagram<T> current = head;
            while (current != null)
            {
                while (current != head)
                {
                    current.Clear();
                    current = tail.Previous;
                    if (current != null)
                        current.Next = null;
                    tail = null;
                    tail = current;
                }
                head = null;
            }
        }

        //Добавляем новую линию к карте (для задания масштаба).
        public void Draw1(Bitmap B, ref int i, int x1, int y1, int x2, int y2)
        {                                   //Добавить красную точку к началу прямой
            Graphics graph = Graphics.FromImage(B);
            if (x2 != 0 && y2 != 0)//Если вводим уже не первую красную точку, Закрашиваем предыдущую и ставим новую
            {
                Pen p = new Pen(Color.Green, 5);
                graph.FillRectangle(Brushes.Green, x1 - 5, y1 - 5, 10, 10);
                graph.FillRectangle(Brushes.Green, x2 - 5, y2 - 5, 10, 10);
                graph.DrawLine(p, x1, y1, x2, y2);
                graph.Dispose();
                i++;
            }
            else
            {
                graph.FillRectangle(Brushes.Red, x1 - 5, y1 - 5, 10, 10);
                graph.Dispose();
                i++;
            }
        }

        //Узнаем координаты последней введеной нами точки в текущей диаграмме. 
        public int GetLast()
        {
            return current3.GetLastX();
        }

        //Возвращает имя последней из созданных диаграмм
        public string GetNameOfLast()
        {
            return tail.Name;
        }

        //Вычисляем угол и длинну центральной трещины
       public void GetC(int x1, int x2, int y1, int y2)
        {
            current3.GetC(x1, x2, y1, y2);
        }
    }
}