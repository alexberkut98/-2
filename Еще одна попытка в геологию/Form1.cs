using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Еще_одна_попытка_в_геологию
{
    public partial class Form1 : Form 
    {
        ListOfDiagrams <Diagram<int>> List1 = new ListOfDiagrams<Diagram<int>>();
        string s;
        List<string> Names = new List<string>();
        int x, y;

        //Через эту переменную мы отслеживаем состояние папки save, это нужно для двух вещей
        //1. Создать эту папку, если она еще не существует.
        //2. Не создавать эту папку, если она уже существует.
        int Made = 0;

        //Эта переменная нужна для фиксирования нажатия клавиши BACKSPACE
        //Дабы программа точно знала, что удалять надо именно последнюю из введеных точек
        int Delete = 0;

        //Через эту переменную я слежу за выделением диаграмм, т.к. считаю, что в случае выделения той или иной диаграммы все процессы по
        //добавлению/удалению точек/диаграмм должны быть запрещены.
        int stop = 0;

        //Через эту переменную отслеживаем процесс поиска прямой для удаления
        int search = 0;

        //Переменная используется в тех случаях, когда прямая наносится на увеличенную карту, для корректности вычислений.
        int koef = 1;

        //Координаты левого верхнего угла карты, будут использоваться в случае, если мы случайно потеряем карту.
        int B_X;
        int B_Y;

        //Через эту прямую мы будем выяснять, является ли прямая, выбранная для смещения, частью боьшей цепи
        int CH_ST = 0;

        //Координаты прямой, которую мы будем смещать, переменная, которая будет отслеживать существование отдельной версии карты для смещений,
        //имя диаграммы, где эта прямая находится, файл, в котором будет храниться версия карты для работы и копия данного файла,
        //с помощью которого мы и будем работать.
        int CH_X1 = 0;
        int CH_Y1 = 0;
        int CH_X2 = 0;
        int CH_Y2 = 0;
        int CH_PR = 0;
        string CH_Name = "";
        Image CH_Map = null;
        Image CH_Map2 = null;

        //Переменные, распаложенные ниже будут использованы лишь в том случае, 
        //если выбранная для смещения прямая окажется частью большей цепи.

        int CH_C = 3;

        //Координаты прямой, распаложенной перед той, что мы собрались сместить и копии этих координат.
        int CH_PX1 = 0;
        int CH_PY1 = 0;
        int CH_PX2 = 0;
        int CH_PY2 = 0;

        int CH_PX1_COPY = 0;
        int CH_PY1_COPY = 0;
        int CH_PX2_COPY = 0;
        int CH_PY2_COPY = 0;

        //Координаты прямой, распаложенной после той, что мы собрались сместить и их копии.
        int CH_NX1 = 0;
        int CH_NY1 = 0;
        int CH_NX2 = 0;
        int CH_NY2 = 0;

        int CH_NX1_COPY = 0;
        int CH_NY1_COPY = 0;
        int CH_NX2_COPY = 0;
        int CH_NY2_COPY = 0;

        //Переменные для сохранения исходных координат точек прямой, предназначенной для смещения.
        //Они нужны как для того, чтобы при фиксации изменений знать какие координаты мы хотим изменить,
        //так и для того, чтобы откатить смещение к исходной точке, если мы сделали что то не так.
        int CH_X1_COPY = 0;
        int CH_Y1_COPY = 0;
        int CH_X2_COPY = 0;
        int CH_Y2_COPY = 0;

        //Костыли, нужные для обхода бага, природа которого остается для меня загадкой.
        int kost = 0;
        int kosti = 0;

        //Длинна и ширина неувеличенной карты.
        int ImageHeight;
        int ImageWidth;

        //Переменная, по которой мы будем получать масштаб карты.
        double S = 0;
        double S1 = 0;

        //Переменная, по которой мы будем судить о количестве сохраненных фото.
        int num1 = 0;

        //Х-е координаты прямой, с помощью которой мы будем определять масштаб карты.
        int X1 = 0;
        int X2 = 0;

        //У-е координаты прямой, с помощью которой мы будем определять масштаб карты.
        int Y1 = 0;
        int Y2 = 0;

        //Если эта переменная не равна 0, значит мы работаем над еденичным отрезком для вычисления масштаба карты.
        int Q = 0;

        //Эти переменные используются для, собственно, поиска этого самого еденичного отрезка.
        int Q1 = 0;
        int Q2 = 0;

        //Если эта переменная не равна 0, значит мы работаем над отрезком для выделения центральной трещины.
        int Z = 0;
        int Z2 = 0;
        //Через эту переменную мы определяем, нужно ли осуществить заливку.
        int zal = 0;

        //С помощью этой переменной мы проверяем, только ли что мы начали.
        int on = 0;

        //Здесь мы определяем ключевой параметр работаем ли мы количеством трещин или с сумами их длинн.
        int par = 0;
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            button1.BackColor = Color.LightGray;
            button2.BackColor = Color.LightGray;
            button3.BackColor = Color.LightGray;
            button4.BackColor = Color.LightGray;
            button5.BackColor = Color.LightGray;
            button6.BackColor = Color.LightGray;
            button7.BackColor = Color.LightGray;
            button8.BackColor = Color.LightGray;
            button9.BackColor = Color.LightGray;
            button10.BackColor = Color.LightGray;
            button13.BackColor = Color.LightGray;
            button14.BackColor = Color.LightGray;
            button15.BackColor = Color.LightGray;
            button16.BackColor = Color.LightGray;
            button17.BackColor = Color.LightGray;
            button18.BackColor = Color.LightGray;
        }

        //По этой кнопке будут загружаться изображения карт для работы.
        private void button1_Click(object sender, EventArgs e)
        {
            if (Q1 == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();//Открытие диалогового окна для выбора файла
                ofd.Filter = "Image Files (*.JPG;*.PNG;*TIFF)|*.JPG;*.PNG;*TIFF|All files (*.*)|*.*";//Заявлено что мы должны уметь работать с файлами форматов jpg, png, tiff

                if (ofd.ShowDialog() == DialogResult.OK)//Если нажато, ОК, значит мы выбрали изображение и его нужно отработать
                {
                    try
                    {
                        int w;
                        int h;
                        int g = 1;
                        Bitmap X = new Bitmap(ofd.FileName);
                        //открываем картинку
                        if ((X.Height > 900) || (X.Width > 1800))//если картинка действительно вылазит по одной из координат
                        {
                            if ((X.Width - 1800) > (X.Height - 900)) //если картинка вылазит за экран по ширине больше чем по высоте
                            {
                                w = SystemInformation.PrimaryMonitorSize.Width - 150;
                                h = X.Height * w / X.Width;//тогда новая высота при фиксированой ширине равной ширине экрана равна (здесь используется для рассчета коэфициент соотношения высоты и ширины)
                            }
                            else//если по высоте вылазит больше
                            {
                                h = SystemInformation.PrimaryMonitorSize.Height - 150;
                                w = X.Width * h / X.Height;//новая ширина при большой высоте
                            }
                            X = new Bitmap(X, new Size(w, h));
                        }
                        pictureBox1.Image = X;
                        pictureBox1.Height = pictureBox1.Image.Height;
                        pictureBox1.Width = pictureBox1.Image.Width;
                        ImageHeight = pictureBox1.Height;
                        ImageWidth = pictureBox1.Width;
                        B_X = pictureBox1.Location.X;
                        B_Y = pictureBox1.Location.Y;
                        koef = 1;
                        if (Made == 0)
                        {
                            Made++;
                            DirectoryInfo dirInfo = new DirectoryInfo("save");
                            dirInfo.Create();
                        }
                        pictureBox1.Image.Save("save/" + "Map" + Convert.ToString(num1));
                        num1++;
                        Q1++;
                        Form6 t = new Form6();
                        while (g != 0)
                        {
                            t.ShowDialog();
                            g = List1.CheckNames(t.Name);
                            if (g == 1)
                                MessageBox.Show("Диаграмма с данным именем уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (t.Name == "")
                            {
                                MessageBox.Show("Не введено имя для диаграммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                g = 0;
                            }
                        }
                        List1.Add(t.Name);//Задаем первую диаграмму
                        Names.Add(t.Name);
                        t.Dispose();
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        MessageBox.Show("You F****d 3 up", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //С помощью этой программы мы осуществляем перемещение карты мышкой.
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
        }

        //Для перемещения изображения.
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //Перемещаем прямую, зажимая один из ее концов правой кнопкой мыши
            if (e.Button == MouseButtons.Right && search == 3 && CH_PR == 2)
            {
                int x3 = 0;
                int y3 = 0;
                int x4 = 0;
                int y4 = 0;
                /*
                 * Поскольку мы не можем гарантированно попасть прямо в точку, 
                 * необходимо регулярно вычислять, к какой из точек прямой находится
                 * место нажатия кнопки мышки.
                 */
                int i = List1.CheckPoints(CH_X1, CH_Y1, CH_X2, CH_Y2, e.X / koef, e.Y / koef);
                if (CH_C == i || CH_C == 3)
                {
                    CH_C = i;
                    if (i == 1)
                    {
                        x4 = CH_X1 = e.X / koef;
                        y4 = CH_Y1 = e.Y / koef;
                        x3 = CH_PX1;
                        y3 = CH_PY1;
                    }
                    else
                    {
                        x3 = CH_X2 = e.X / koef;
                        y3 = CH_Y2 = e.Y / koef;
                        x4 = CH_NX2;
                        y4 = CH_NY2;
                    }
                    byte[] bytes = null;
                    //Теперь, когда у нас есть координаты крайних точек выделенной прямой, осуществив тем самым смящение прямой.
                    pictureBox1.Image = List1.MovingPoints(CH_Map, CH_Name, CH_X1, CH_Y1, CH_X2, CH_Y2, x3, y3, x4, y4, CH_ST);
                    Image A1;
                    if (CH_ST == 0)
                        bytes = File.ReadAllBytes("save/" + "MapX");
                    else
                    {
                        if (i == 1)
                            bytes = File.ReadAllBytes("save/" + "MapX1");
                        if (i == 2)
                            bytes = File.ReadAllBytes("save/" + "MapX2");
                    }
                    var ms = new MemoryStream(bytes);
                    var img = Image.FromStream(ms);
                    A1 = new Bitmap(img);
                    CH_Map = A1;
                }
                else
                {
                    if (CH_C != 3)
                        MessageBox.Show("Пожалуйста зафиксируйте или откатите изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (e.Button != MouseButtons.Left) return;
                pictureBox1.Left += (e.X - x);
                pictureBox1.Top += (e.Y - y);
            }
        }

        //Стоп - команда, дающая понять, что перемещение карты в данный момент не осуществляется.
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            x = y = 0;
        }

        //Мы закончили работу над новой трещиной.
        private void button2_Click(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                if (CH_PR != 2)
                {
                    Block();
                    if (kosti > 0)
                        kost = 1;
                }
                else
                {
                    //Кроме вышенаписаного, мы можем использовать нажатия пробела еще и для фиксации изменений, произошедших со смещенной прямой. 
                    //Прежде всего надо уточнить, довольны ли мы результатами смещения или надо откатиться к исходной точке. 
                    Form16 f = new Form16();
                    f.ShowDialog();
                    if (f.i == 1)
                    {
                        //Было решено зафиксировать изменения, но для этого потребуется сделать 2 вещи:
                        //1. Воссоеденить крайние точки выделенной прямой (которую мы стерли с экрана именно через разьединение этих самых точек)
                        //2. Переписать координаты этих точек в соответствии с произошедшим смещением
                        List1.ReunionAndChange(CH_X1_COPY, CH_Y1_COPY, CH_X2_COPY, CH_Y2_COPY, CH_X1, CH_Y1, CH_X2, CH_Y2, CH_Name, 1);
                        CH_C = 3;
                        Form12 f1 = new Form12();
                        f1.ShowDialog();
                        if (f1.i == 1)
                        {
                            search = 0;
                            button5.BackColor = Color.LightGray;
                        }
                    }
                    if (f.i == 2)
                    {
                        byte[] bytes = null;
                        int x3 = 0;
                        int y3 = 0;
                        int x4 = 0;
                        int y4 = 0;
                        //Было решено откатить смещение назад
                        CH_X1 = CH_X1_COPY;
                        CH_Y1 = CH_Y1_COPY;
                        CH_X2 = CH_X2_COPY;
                        CH_Y2 = CH_Y2_COPY;
                        if (CH_C == 1)
                        {
                            x4 = CH_X1;
                            y4 = CH_Y1;
                            x3 = CH_PX1;
                            y3 = CH_PY1;
                        }
                        else
                        {
                            x3 = CH_X2;
                            y3 = CH_Y2;
                            x4 = CH_NX2;
                            y4 = CH_NY2;
                        }
                        Image A1;
                        if (CH_ST == 0)
                            bytes = File.ReadAllBytes("save/" + "MapX");
                        else
                            bytes = File.ReadAllBytes("save/" + "MapX1");
                        var ms = new MemoryStream(bytes);
                        var img = Image.FromStream(ms);
                        A1 = new Bitmap(img);
                        CH_Map = A1;
                        pictureBox1.Image = List1.MovingPoints(CH_Map, CH_Name, CH_X1, CH_Y1, CH_X2, CH_Y2, x3, y3, x4, y4, CH_ST);
                        CH_C = 3;
                    }
                    if (f.i == 1 || f.i == 5)
                    {
                        //Было решено отказаться смещать прямые и стоавить все как есть
                        CH_X1 = CH_X1_COPY = 0;
                        CH_Y1 = CH_Y1_COPY = 0;
                        CH_X2 = CH_X2_COPY = 0;
                        CH_Y2 = CH_Y2_COPY = 0;
                        CH_NX1 = CH_NX1_COPY = 0;
                        CH_NY1 = CH_NY1_COPY = 0;
                        CH_NX2 = CH_NX2_COPY = 0;
                        CH_NY2 = CH_NY2_COPY = 0;
                        CH_PR = 0;
                        CH_Name = "";
                        CH_Map = null;
                        CH_C = 3;
                    }
                    if (f.i == 1 || f.i == 4 || f.i == 5)
                    {
                        if (f.i == 5)
                        {
                            search = 0;
                            button5.BackColor = Color.LightGray;
                        }
                        num1 = 0;
                        Image A1;
                        var bytes = File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1));
                        var ms = new MemoryStream(bytes);
                        var img = Image.FromStream(ms);
                        A1 = new Bitmap(img);
                        pictureBox1.Image = A1;
                        pictureBox1.Image = List1.PaintSaved(pictureBox1.Image, ref num1);
                    }
                }
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Работа завершена. Время для расчетов.
        private void button4_Click(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                if (kost == 0)
                {
                    kosti++;
                    if (Q1 != 0)
                    {
                        S = S1;
                        int t1 = 0;
                        Form4 f = new Form4();
                        while (t1 == 0)
                        {
                            f.ShowDialog();
                            double grad = f.Grad;
                            zal = f.Form;
                            par = f.Key;
                            if (par != -1)
                            {
                                if (grad != 0 && zal != 0 && par != 0)
                                {
                                    if (num1 > 0)
                                    {
                                        num1--;
                                        while (num1 != -1)
                                        {
                                            File.Delete("save/" + "Map" + Convert.ToString(num1));//Удаляем текущую версию карты
                                            num1--;
                                        }
                                    }
                                    t1++;
                                    if (par == 1 || par == 2 && S != 0 || par == 3 && S != 0)
                                    {
                                        List1.FinalCalculates(grad, zal, par, S, s, ImageHeight, ImageWidth, on);
                                    }
                                    else
                                        MessageBox.Show("Пожалуйста, введите единичный отрезок", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                    MessageBox.Show("Не введены все необходимые параметры", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                                t1++;
                        }
                    }
                }
                else
                    kost = 0;
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Отменяем ошибочный ввод последней точки и откатываемся на предыдущую версию карты.
        private void button5_Click(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                int t = 0;//С помощью этой переменной программа будет уточнять, Что именно мы хотим сделать с трещиной
                Form9 f = new Form9();
                if (Delete == 1)
                {
                    Delete = 0;
                    t++;
                }
                while (t == 0)
                {
                    f.ShowDialog();
                    t = f.i;
                }
                if (t == 1)
                {
                    //Удаляем последнюю трещину
                    if (Q1 != 0)
                    {
                        int s = 0;
                        List1.RemovePoint(ref s);
                        /*
                         * Если s  выдаст -1, значит диаграмма, которая на данный момент является хвостовой, пуста.
                         * Нужно написать механизм, который позволит перевести рельсы на предыдущую диаграмму, если оная будет в наличии.
                         * Лишь если предыдущей диаграммы не окажется, мы должны выдать ошибку.
                         */
                        if (Q2 != 0 && Z == 3 || Q2 != 0 && Z == 0 || Z != 0 && Q2 == 3 || Z != 0 && Q2 == 0 || Z == 0 && Q2 == 0 || Z == 2 && Q2 == 1)
                            s = 1;
                        if (s != (-1))
                        {
                            num1--;
                            File.Delete("save/" + "Map" + Convert.ToString(num1));
                            num1--;
                            Image A1;
                            var bytes = File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1));
                            var ms = new MemoryStream(bytes);
                            var img = Image.FromStream(ms);
                            A1 = new Bitmap(img);
                            num1++;
                            pictureBox1.Image = A1;

                            if (Q2 == 1 && Z == 3 || Q2 == 1 && Z == 0 || Z == 1 && Q2 == 3 || Z == 1 && Q2 == 0 || Z == 2 && Q2 == 1)
                            {
                                if (!(Z == 2 && Q2 == 1))
                                {
                                    X1 = 0;
                                    Y1 = 0;
                                }
                                if (Z == 0 || Z == 3)
                                    Q2 = 0;
                                else
                                {
                                    if (!(Z == 2 && Q2 == 1))
                                        Z = 0;
                                }
                                if (!(Z == 2 && Q2 == 1))
                                    Q = 1;
                            }
                            if (Q2 == 2 || Z == 2 && Q2 == 1)
                            {
                                X2 = 0;
                                Y2 = 0;
                                if (Z == 0 || Z == 3)
                                    Q2 = 1;
                                else
                                {
                                    Z = 2;
                                    Z2++;
                                    if (Z2 == 2)
                                    {
                                        Z2 = 0;
                                        X1 = 0;
                                        Y1 = 0;
                                        Z = -1;
                                    }
                                }
                            }
                        }
                        if (s == -1)
                            MessageBox.Show("Список координат пуст, новые удаления невозможны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (t == 2 || t == 4 || t == 5)
                {
                    //Если мы здесь, значит мы хотим или удалить некую трещину, или передать ее другой диаграмме или же сместить трещину
                    button5.BackColor = Color.Red;
                    search++;
                    if (t == 4)
                        search++;
                    if (t == 5)
                        search += 2;
                }
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        //Отработка нажатий соответствующих клавиш.
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Back)
            {
                Delete++;
                button5_Click(button5, null);
            }
            if (e.KeyCode == Keys.E)
                button4_Click(button4, null);
            if (e.KeyValue == (char)Keys.Space)
                button2_Click(button2, null);
            if (e.KeyValue == (char)Keys.C)
                button3_Click(button3, null);
            if (e.KeyValue == (char)Keys.S)
                button6_Click(button6, null);
            if (e.KeyValue == (char)Keys.R)
                button8_Click(button8, null);
            if (e.KeyValue == (char)Keys.A)
            {
                if (trackBar1.Value > 1)
                    trackBar1.Value--;
            }
            if (e.KeyValue == (char)Keys.D)
            {
                if (trackBar1.Value < 10)
                    trackBar1.Value++;
            }
            if (e.KeyValue == (char)Keys.X)
                button9_Click(button9, null);
            if (e.KeyValue == (char)Keys.Z)
                button10_Click(button10, null);
            if (e.KeyValue == (char)Keys.B)
                button13_Click(button13, null);
        }

        //Удалить нынешнюю карту, чтоб можно было начать работу с другой.
        private void button3_Click(object sender, EventArgs e)
        {
            List1.Clear();
            Names.Clear();
            pictureBox1.Image = null;
            if (num1 > 0)
            {
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo("save");
                dirInfo.Delete(true);
                num1 = 0;
            }
            Q1 = 0;
            on = 0;
            S = S1 = 0;
            Z = 0;
            Q = 0;
            CH_PR = 0;
            CH_ST = 0;
            Made = 0;
            search = 0;
            button1.BackColor = Color.LightGray;
            button2.BackColor = Color.LightGray;
            button3.BackColor = Color.LightGray;
            button4.BackColor = Color.LightGray;
            button5.BackColor = Color.LightGray;
            button6.BackColor = Color.LightGray;
            button7.BackColor = Color.LightGray;
            button8.BackColor = Color.LightGray;
            button16.BackColor = Color.LightGray;
            button15.BackColor = Color.LightGray;
            button14.BackColor = Color.LightGray;
            button13.BackColor = Color.LightGray;
            button10.BackColor = Color.LightGray;
            button9.BackColor = Color.LightGray;
        }

        //Соответствующая кнопка меняет свой цвет на красный - сигнал о том, что мы готовимся задать масштаб карты.
        private void button6_Click(object sender, EventArgs e)
         {
            if (Q == 0)
            {
                Q = 1;
                button6.BackColor = Color.Red;
            }            
        }

        //Соответствующая кнопка меняет свой цвет на зеленый - сигнал о том, что масштаб карты задан.
        private void button7_Click(object sender, EventArgs e)
        {
            if (button6.BackColor == Color.Red)
            {
                Q2 = X1 = X2 = Y1 = Y2 = Q = 0;
                button6.BackColor = Color.LightGray;
            }
        }

        //Увеличение\уменьшение карты.
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                pictureBox1.Size = new Size(ImageWidth * trackBar1.Value, ImageHeight * trackBar1.Value);
                koef = trackBar1.Value;
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Удалить нынешнюю карту, чтоб можно было начать работу с другой.
        private void Form1_Load(object sender, EventArgs e)
        {
            button3_Click(button3, null);
        }

        //Возвращаем карту на место.
        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(B_X, B_Y); 
        }

        //Увеличение\уменьшение карты.
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                pictureBox1.Size = new Size(ImageWidth * trackBar1.Value, ImageHeight * trackBar1.Value);
                koef = trackBar1.Value;
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Добавляем новую диаграмму, для которой будем заполнять точки.
        private void button9_Click(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                if (kost == 0)
                {
                    kosti++;
                    int g = 1;
                    if (List1.GetLast() != -1)
                        Block();
                    Form6 t = new Form6();
                    while (g != 0)
                    {
                        t.ShowDialog();
                        if (t.st != -1)
                        {
                            g = List1.CheckNames(t.Name);
                            if (g == 1)
                                MessageBox.Show("Диаграмма с данным именем уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (t.Name == "")
                            {
                                MessageBox.Show("Не введено имя для диаграммы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                g = 0;
                            }
                        }
                        else
                            g = 0;
                    }
                    if (t.st != -1)
                    {
                        List1.Add(t.Name);//Задаем новую диаграмму
                        Names.Add(t.Name);

                        //Если мы создаем новую диаграмму, надо зафиксировать работу над предыдущей 
                        //то.е. закрасить ее прямые синим
                        num1 = 1;
                        var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                        var ms = new System.IO.MemoryStream(bytes);
                        var img = Image.FromStream(ms);
                        Image A1 = new Bitmap(img);
                        A1.Save("save/" + "Map" + Convert.ToString(0));
                        pictureBox1.Image = List1.PaintSaved(A1, ref num1);
                        Z = 0;
                        button13.BackColor = Color.LightGray;
                    }
                    t.Dispose();
                }
                else
                    kost = 0;
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Удаляем диаграмму. В процессе необходимо предусмотреть выбор между удалением последней диаграммы и удалением по конкретному имени.
        private void button10_Click(object sender, EventArgs e)
        {
            if (stop == 0)
            {
                int a = 0;
                /*
                 * При удалении по имени, надо будет откатиться к самой первой карте, а потом заного отрисовать все диаграммы кроме удаленной.
                 * При удалении последней диаграммы хватит и множественных откатов.
                 */
                Form7 f = new Form7(Names);
                f.ShowDialog();
                if (f.f != -1)
                {
                    a = f.f;
                    num1--;
                    string A;
                    if (a == 1)
                        A = List1.GetName();
                    else
                        A = f.Name;
                    Image A1;
                    Names.Remove(A);
                    int p = num1 - List1.GetCount(A) - 1;
                    if (a == 2) p++;
                    while (num1 != p)
                    {
                        System.IO.File.Delete("save/" + "Map" + Convert.ToString(num1));//Удаляем текущую версию карты
                        num1--;
                    }
                    var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1));
                    var ms = new System.IO.MemoryStream(bytes);
                    var img = Image.FromStream(ms);
                    A1 = new Bitmap(img);
                    /*
                     * Удалив текущую версию карты, мы должны, в зависимости от значения переменной а, решить что делать дальше.
                     * Если а = 1, надо отследить где заканчивается нынешняя диаграмма и начинается предыдущая
                     * Для этого нужно получить значение счетчика точек, принадлежащих данной диаграмме, поскольку оно будет соответствовать количеству версий карты, что нам придется удалить.
                     */

                    //Через эту переменную мы отслеживаем статус текущей диаграммы, дабы, в случае удаления оной, принять решение о том, какая диаграмма станет текущей
                    int pr = 0;
                    if (a == 1)
                    {
                        a--;
                        pictureBox1.Image = A1;
                        num1++;
                        List1.Remove(ref A, ref a,ref pr);
                    }
                    /*
                     * Если же а = 2, ситуация усложняется, т.к. теперь перед нами встают уже 3 задачи.
                     * 1. Найти и удалить ненужную диаграмму. (DONE)
                     * 2. Откатить карту до последней из тех её версий, где ненужная нам диограмма отсутствовала. (DONE)
                     * 3. Вернуть на карту не подвергшиеся удалению диаграммы.
                     */
                    if (a == 2)
                    {
                        num1++;
                        a = 0;
                        List1.Remove(ref A, ref a,ref pr);
                        pictureBox1.Image = List1.RedrawingTheMap(A1, A, ref num1, koef, 1);
                    }

                    //Итак, мы удалили текущую диаграмму, а значит у нас два варианта действий:
                    //1. Создать новую диаграмму и именно ее сделать текущей.
                    //2. Выбрать одну из оставшихся диаграмм.
                    if (pr > 0)
                    {
                        Form17 f1 = new Form17();
                        f1.ShowDialog();
                        while (f1.i != 0)
                        {
                            if (f1.i == 1)
                                button9_Click(button9, null);
                            if (f1.i == 2)
                                button17_Click(button17, null);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Помимо запрета на любые преобразования, который должен действовать в момент выделения одной из диаграмм, нельзя забывать о ситуации когда мы хотим выделить точки и прямые 
        //принадлежащие одной диаграмме, вместо точек и прямых, принадлежащих другой, не заканчивая однако сам процесс выделений диаграмм.
        private void button11_Click(object sender, EventArgs e)
        {
            if (stop == 1)
            {
                var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1 - 1));
                var ms = new System.IO.MemoryStream(bytes);
                var img = Image.FromStream(ms);
                pictureBox1.Image = new Bitmap(img);
            }
            Form8 t = new Form8(Names);
            t.ShowDialog();
            if (t.Name != "Form8")
            {
                pictureBox1.Image = List1.RedrawingTheMap(pictureBox1.Image, t.Name, ref num1, koef, 3);
                stop = 1;
            }
        }

        //Отмена выделения диаграммы.
        private void button12_Click(object sender, EventArgs e)
        {
            if (stop == 1)
            {
                var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1 - 1));
                var ms = new System.IO.MemoryStream(bytes);
                var img = Image.FromStream(ms);
                pictureBox1.Image = new Bitmap(img);
                stop = 0;
            }
        }

        //Добавление координат новой точки
        private void pictureBox1_Click1(object sender, MouseEventArgs e)
        {
            if (stop == 0)
            {
                if (e.Button != MouseButtons.Right) return;
                else
                {
                    if (search == 0)
                    {
                        if (Q == 0 && Z == 3 || Q == 3 && Z == 3 || Q == 3 && Z == 0 || Q == 0 && Z == 0)
                        {
                            List1.AddPoint(e.X / koef, e.Y / koef);
                            pictureBox1.Image = (Bitmap)List1.RedrawingTheMap(pictureBox1.Image, List1.GetName(), ref num1, koef, 2);
                        }
                        else
                        {
                            int prov = 0;//Проверка на правильность введенного отрезка
                            if (Q == 1 && Z == 3 || Q == 1 && Z == 0 || Z == 1 && Q == 3 || Z == 1 && Q == 0 || Z == -1)
                            {
                                Q2 = 1;
                                X1 = e.X / koef;
                                Y1 = e.Y / koef;
                                int l = 0;
                                Bitmap A = (Bitmap)pictureBox1.Image;                                     //A - ныняшняя версия карты
                                Bitmap B = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);//B - Холст, на который мы нанесеем последнюю прямую и который потом обьеденится с А
                                List1.Draw1(B, ref l, X1, Y1, 0, 0);
                                Graphics g = Graphics.FromImage(A);
                                g.DrawImage(B, 0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
                                g.Dispose();
                                pictureBox1.Image = A;
                                if (l == 1)
                                {
                                    pictureBox1.Image.Save("save/" + "Map" + Convert.ToString(num1));
                                    num1++;
                                }
                            }
                            if (Q == 2 && Z == 3 || Q == 2 && Z == 0 || Z == 2 && Q == 3 || Z == 2 && Q == 0)
                            {
                                int z = 0;
                                int W = 0;
                                int l = 0;
                                X2 = e.X / koef;
                                W = X1;
                                Y2 = e.Y / koef;
                                Bitmap A = (Bitmap)pictureBox1.Image;                                     //A - ныняшняя версия карты
                                Bitmap B = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);//B - Холст, на который мы нанесеем последнюю прямую и который потом обьеденится с А
                                List1.Draw1(B, ref l, W, Y1, X2, Y2);
                                if (Z > 0)
                                    List1.GetC(W, X2, Y1, Y2);
                                Graphics g = Graphics.FromImage(A);
                                g.DrawImage(B, 0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
                                g.Dispose();
                                pictureBox1.Image = A;
                                if (l == 1)
                                {
                                    pictureBox1.Image.Save("save/" + "Map" + Convert.ToString(num1));
                                    num1++;
                                }
                                int z1 = 0;
                                Form5 f5 = new Form5();
                                while (z1 == 0)
                                {
                                    f5.ShowDialog();
                                    if (f5.st != 0)
                                        z1++;
                                }
                                if (f5.st == 1)
                                    prov++;
                                else
                                {
                                    if (Q == 2)
                                    {
                                        Q2 = 2;
                                        Q--;
                                    }
                                    else
                                        Z--;
                                }
                                if (prov == 1)
                                {
                                    if (X1 > X2)
                                        X1 = X1 - X2;//Как только мы получим длинну еденичного отрезка в пикселях, потребуется выяснить сколько метров или километров вложено в эти пиксели
                                    else            //с тем, что бы разделив второе на первое мы оценили масштаб нашей карты
                                        X1 = X2 - X1;
                                    Q2 = 0;
                                    if (Q == 2 && Z == 3 || Q == 2 && Z == 0)
                                        button6.BackColor = Color.Green;
                                    else
                                        button13.BackColor = Color.Green;
                                    if (Z == 0 || Z == 3)
                                    {
                                        Form2 f = new Form2();
                                        while (z == 0)
                                        {
                                            f.ShowDialog();
                                            if (f.S != 0)
                                                z++;
                                            else
                                                MessageBox.Show("Не введена длинна еденичного отрезка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        S1 = Math.Round(f.T / (double)X1, 2);//Теперь мы знаем какое расстояние заложено меежду двумя соседними пикселями
                                        if (f.S == 1)
                                            s = " km";
                                        if (f.S == 2)
                                            s = " m";
                                        if (f.S == 3)
                                            s = " sm";
                                    }
                                }

                            }
                            if (Z > 0 && Z < 3 && Z != -1)
                                Z++;
                            else
                            {
                                if (Z == -1)
                                    Z = 2;
                                else
                                    Q++;
                            }

                        }
                    }
                    else
                    {
                        int x1 = 0;
                        int y1 = 0;
                        int x2 = 0;
                        int y2 = 0;

                        //Переменная с помощью которой мы определяем надо ли перерисовывать карту
                        int ch = 0;
                        string Name2 = "";

                        //Если мы не собираемся смещать выделенную прямую, то нам и незачем знать о ее потенциальных соседях.
                        //В противном же случае нужно подать четкий сигнал о небходимости изучения ситуации вокруг выделенной прямой.
                        if (search != 3)
                            CH_ST = -1;
                        else
                            CH_ST = 5;
                        if (CH_PR != 2)
                            pictureBox1.Image = List1.SearchOfTheLine(e.X / koef, e.Y / koef, pictureBox1.Image, ref x1, ref y1, ref x2, ref y2,ref CH_PX1,ref CH_PY1,ref CH_PX2,ref CH_PY2,ref CH_NX1,ref CH_NY1,ref CH_NX2,ref CH_NY2, ref Name2, ref CH_ST);
                        if (search == 1)
                        {
                            Form11 f = new Form11();
                            f.ShowDialog();
                            if (f.i == 1)
                            {
                                ch++;
                                //Решено, что выбранную нами прямую действительно нужно удалить.
                                //Однако, после удаления прямой перед нами должен предстать выбор:
                                //1. Остаться в режиме поиска прямых и найти нового кандидата на удаление.
                                //2. Отказаться от новых удалений и выйти из соответствующего режима.
                                List1.RemoveMidlePoint(x1, y1, x2, y2, Name2);                               
                            }
                            if (f.i == 0)
                            {
                                //Решено что мы не хотим удалять данную точку, в связи с чем перед нами предстает выбор из 2-х вариантов.
                                //1. Оставаясь в режиме поиска прямых выбрать какую нибудь другую.
                                //2. Отказаться от поиска прямых и выйти из соответствующего режима.
                                Image A1;
                                var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1 - 1));
                                var ms = new System.IO.MemoryStream(bytes);
                                var img = Image.FromStream(ms);
                                A1 = new Bitmap(img);
                                pictureBox1.Image = A1;
                            }
                        }
                        if (search == 2)
                        {
                            //Здесь мы выбираем для выделенной прямой другую диаграмму и передаем ее туда
                            Form13 f2 = new Form13(Names, Name2);
                            f2.ShowDialog();
                            if (f2.f != -1)
                            {
                                //Мы решили сменить диаграмму для прямой
                                if (f2.f == 2)
                                {
                                    ch++;
                                    List1.MovingLines(x1, y1, x2, y2, Name2, f2.Name);
                                }
                            }
                        }
                        if (search == 3)
                        {
                            //Нужно убедиться, что мы действительно хотим сместить именно выделенную прямую.
                            if (CH_PR == 0)
                            {
                                Form15 f3 = new Form15();
                                f3.ShowDialog();
                                if (f3.i == 1)
                                {
                                    CH_PR++;
                                }
                            }

                            /*
                             * Решено сместить прямую, а точнее "потянуть" один её за один из концов.
                             * Чтобы это осуществить нужно создать версию карты, где будут нарисованы все прямые кроме смещаемой,
                             */
                            if (CH_PR == 1)
                            {
                                CH_PR++;
                                CH_Name = Name2;
                                Image A1;
                                var bytes = File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                                var ms = new MemoryStream(bytes);
                                var img = Image.FromStream(ms);
                                A1 = new Bitmap(img);
                                CH_Map = A1;

                                //Если прямая изолирована, либо имеет только одного соседа, то для работы нам потребуется лишь 2 карты.
                                //В ином же случае нам потребуются 2 карты.
                                //Одна - на случай если мы потянем за точку (x1,y1).
                                //тогда потребуется удалить из карты прямую расположенную перед той, что мы собираемся сместить.
                                //Другая - на случай, если мы потянем за точку (x2,y2),
                                //в этом случае потребуется удалить из карты прямую, расположенную после той, что мы собираемся сместить.
                                if (CH_ST == 0)
                                {
                                    CH_Map = List1.SaveMapForWork(CH_Map, Name2, x1, y1, x2, y2, CH_PX1, CH_PY1, CH_PX2, CH_PY2, CH_NX1, CH_NY1, CH_NX2, CH_NY2, CH_ST, 0);
                                    CH_Map.Save("save/" + "MapX");
                                }
                                else
                                {
                                    int a = 1;
                                    int b = 2;
                                    //ST = 1. Обнаружена одна прямая перед выделенной.
                                    //ST = 2. Обнаружена одна прямая после выделенной.
                                    //ST = 3. Обнаружены прямые по обе стороны от выделенной.
                                    if (CH_ST == 1)
                                        b = -1;
                                    if (CH_ST == 2)
                                        a = -1;
                                    CH_Map = List1.SaveMapForWork(CH_Map, Name2, x1, y1, x2, y2, CH_PX1, CH_PY1, CH_PX2, CH_PY2, CH_NX1, CH_NY1, CH_NX2, CH_NY2, a, 0);
                                    CH_Map.Save("save/" + "MapX1");
                                    if (a != -1)
                                        List1.ReunionAndChange(CH_PX1, CH_PY1, CH_PX2, CH_PY2, 0, 0, 0, 0, Name2, 0);
                                    bytes = File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                                    ms = new MemoryStream(bytes);
                                    img = Image.FromStream(ms);
                                    A1 = new Bitmap(img);
                                    CH_Map2 = List1.SaveMapForWork(A1, Name2, x1, y1, x2, y2, CH_PX1, CH_PY1, CH_PX2, CH_PY2, CH_NX1, CH_NY1, CH_NX2, CH_NY2, b, 1);
                                    CH_Map2.Save("save/" + "MapX2");
                                    if (b != -1)
                                        List1.ReunionAndChange(CH_NX1, CH_NY1, CH_NX2, CH_NY2, 0, 0, 0, 0, Name2, 0);
                                }
                                CH_X1 = x1;
                                CH_Y1 = y1;
                                CH_X2 = x2;
                                CH_Y2 = y2;
                                CH_X1_COPY = x1;
                                CH_Y1_COPY = y1;
                                CH_X2_COPY = x2;
                                CH_Y2_COPY = y2;
                                CH_NX1_COPY = CH_NX1;
                                CH_NX2_COPY = CH_NX2;
                                CH_NY1_COPY = CH_NY1;
                                CH_NY2_COPY = CH_NY2;
                                CH_PX1_COPY = CH_PX1;
                                CH_PX2_COPY = CH_PX2;
                                CH_PY1_COPY = CH_PY1;
                                CH_PY2_COPY = CH_PY2;
                            }
                        }
                        if (ch > 0)
                        {
                            num1 = 0;
                            Image A1;
                            var bytes = File.ReadAllBytes("save/" + "Map" + Convert.ToString(num1));
                            var ms = new MemoryStream(bytes);
                            var img = Image.FromStream(ms);
                            A1 = new Bitmap(img);
                            pictureBox1.Image = A1;
                            pictureBox1.Image = List1.PaintSaved(pictureBox1.Image, ref num1);
                        }
                        //Здесь мы задаем себе вопрос, хотим ли мы продолжить процесс работы над прямыми
                        //или же мы пока повременим с этим
                        if (search != 3)
                        {
                            Form12 f1 = new Form12();
                            f1.ShowDialog();
                            if (f1.i == 1)
                            {
                                search = 0;
                                button5.BackColor = Color.LightGray;
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Выделяем центральную трещину (по двум точкам) с тем, чтобы потом она была видна на диаграмме.
        private void button13_Click(object sender, EventArgs e)
        {
            if (Z == 0)
            {
                Z++;
                button13.BackColor = Color.Red;
            }
        }

        //Отменяем работу над центральной трещиной.
        private void button14_Click(object sender, EventArgs e)
        {
            if (button13.BackColor == Color.Red)
            {
                Z = X1 = X2 = Y1 = Y2 = Q = Q2 = 0;
                button13.BackColor = Color.LightGray;
            }
        }

        //Здесь будут сохраняться незавершенные работы по диаграммам.
        private void button15_Click(object sender, EventArgs e)
        {
            int i = 0;
            Form10 f = new Form10();
            string parth = "";
            //Создаем файл, куда сохраним текущую работу
            DirectoryInfo dirInfo;
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.SelectedPath))//Если нажато, ОК, значит мы выбрали изображение и его нужно отработать
            {
                dirInfo = new DirectoryInfo(ofd.SelectedPath);
                i++;
            }
            while (i == 1)
            {
                f.ShowDialog();
                f.Name = ofd.SelectedPath + "/" + f.Name;
                dirInfo = new DirectoryInfo(f.Name);                
                if (f.Name != "Form10")
                {
                    if (dirInfo.Exists)
                        f.Name = "";
                    else
                    {
                        dirInfo.Create();
                        i++;
                        parth = dirInfo.FullName;
                    }
                }
                else
                    i = 10;
            }            //Начинаем сохранять данные на новосозданный файл.
            //В данном файле будет одна картинка, в которой будет храниться исходная карта, 
            //и несколько текстовых файлов,
            //в каждом из которых координаты одной из диаграмм           
            //Сохраняем исходную карту
            if (i != 10 && i != 0)
            {
                Bitmap b;
                var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                var ms = new System.IO.MemoryStream(bytes);
                var img = Image.FromStream(ms);
                b = new Bitmap(img);
                b.Save(f.Name + "/" + "Map");
                //Теперь сохраняем координаты точек для диаграмм, в текстовых файлах.
                List1.SaveData(parth);
            }
        }

        //Здесь мы возвращаемся к работе, некогда сохраненной.
        private void button16_Click(object sender, EventArgs e)
        {
            //Страховка от случайных попыток загрузить карту на еще не очищенное полею.
            if (Q1 == 0)
            {
                Q1++;
                int i = 0;
                string path="";
                string Names2;
                Image A1 = null;
                //Создаем файл, куда сохраним текущую работу
                DirectoryInfo dirInfo;
                FolderBrowserDialog ofd = new FolderBrowserDialog();//Открытие диалогового окна для выбора файла
                while (i == 0)
                {
                    if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.SelectedPath))//Если нажато, ОК, значит мы выбрали изображение и его нужно отработать
                    {
                        dirInfo = new DirectoryInfo(ofd.SelectedPath);
                        if (dirInfo.Exists)
                        {
                            i++;
                            path = dirInfo.FullName;
                            var bytes = File.ReadAllBytes(path + "/" + "Map");
                            var ms = new MemoryStream(bytes);
                            var img = Image.FromStream(ms);
                            A1 = new Bitmap(img);
                            num1++;
                            pictureBox1.Height = A1.Height;
                            pictureBox1.Width = A1.Width;
                            ImageHeight= A1.Height;
                            ImageWidth = A1.Width;
                            B_X = pictureBox1.Location.X;
                            B_Y = pictureBox1.Location.Y;
                            if (Made == 0)
                            {
                                Made++;
                                DirectoryInfo dirInfo1 = new DirectoryInfo("save");
                                dirInfo1.Create();
                            }
                            A1.Save("save/" + "Map" + Convert.ToString(num1 - 1));
                        }
                    }
                    else
                        i = -1;
                }                   
                if (i != -1)
                {
                    //Далее мы должны вытащить из текстовых файлов данные по всем диаграммам и проявить их на карте
                    //Начать следует с изучения списка диаграмм
                    using (FileStream fstream = File.OpenRead($"{path}\\List of names.txt"))
                    {
                        // преобразуем строку в байты
                        byte[] array = new byte[fstream.Length];
                        // считываем данные
                        fstream.Read(array, 0, array.Length);
                        // декодируем байты в строку
                        Names2 = System.Text.Encoding.Default.GetString(array);
                    }

                    //И вот перед нами список имен диаграмм
                    string[] subs = Names2.Split(' ');

                    //Дальнейшая работа над сохраненными данными будет представлять из себя следующий цикл:
                    //1. Создать диаграмму
                    //2. Заполнить ее данными из соответствующего списка
                    for (int j = 0; j < subs.Length; j++)
                    {
                        List1.AddSaved(path, subs[j]);
                        Names.Add(subs[j]);
                    }

                    //Теперь надо нанести на карту точки и прямые, что мы получили из сохраненных данных
                    pictureBox1.Image = List1.PaintSaved(A1, ref num1);
                }
            }
        }

        //Мы хотим сменить диаграмму и добавлять новые трещины не в последнюю из созданных
        private void button17_Click(object sender, EventArgs e)
        {
            if (kost == 0)
            {
                kosti++;
                Form13 f = new Form13(Names, List1.GetName());
                f.ShowDialog();
                if (f.f != -1)
                {
                    //Мы решили сменить диаграмму
                    if (f.f == 2)
                    {
                        List1.Changing(f.Name);

                        //Если мы меняем диаграмму, надо зафиксировать работу над текущей 
                        //то.е. закрасить ее прямые синим
                        num1 = 1;
                        var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                        var ms = new System.IO.MemoryStream(bytes);
                        var img = Image.FromStream(ms);
                        Image A1 = new Bitmap(img);
                        A1.Save("save/" + "Map" + Convert.ToString(0));
                        pictureBox1.Image = List1.PaintSaved(A1, ref num1);
                    }
                }
            }
            else
                kost = 0;
        }

        //Мы хотим откатить все назад и продолжить добавлять трещины в последнюю из созданных диаграмм
        private void button18_Click(object sender, EventArgs e)
        {
            if (List1.GetNameOfLast() != List1.GetName())
            {
                Form14 f = new Form14();
                f.ShowDialog();
                if (f.i == 1)
                {
                    List1.CanselChanging();

                    //Если мы создаем новую диаграмму, надо зафиксировать работу над предыдущей 
                    //то.е. закрасить ее прямые синим
                    num1 = 1;
                    var bytes = System.IO.File.ReadAllBytes("save/" + "Map" + Convert.ToString(0));
                    var ms = new System.IO.MemoryStream(bytes);
                    var img = Image.FromStream(ms);
                    Image A1 = new Bitmap(img);
                    A1.Save("save/" + "Map" + Convert.ToString(0));
                    pictureBox1.Image = List1.PaintSaved(A1, ref num1);
                }
            }
        }

        //Замыкаем текущую прямую.
        private void Block()
        {
            if (stop == 0)
            {
                if (List1.GetLast() != -1)
                {
                    if (Q1 != 0)
                        List1.AddPoint(-1, -1);
                }
                pictureBox1.Image = List1.RedrawingTheMap(pictureBox1.Image, " ", ref num1, koef, 2);
            }
            else
                MessageBox.Show("Выделена одна из диаграмм, обратите процесс выделения, для продолжения работы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}