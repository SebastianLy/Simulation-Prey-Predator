using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Symulacja
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        Bitmap Function;
        List<Prey> PreyList = new List<Prey>();
        List<Predator> PredatorList = new List<Predator>();
        const float radius = 8;
        float width = 2 * radius;
        float height = 2 * radius;
        int pregnanttime = 50;
        const int food = 300;
        int ticks;
        Point preyT0 = new Point(0,122);
        Point predatorT0 = new Point(0, 122);

        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            Function = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            pictureBox2.Image = Function;
        }

        public class Prey
        {
            public Point position;
            public char gender;
            public int pregnant = -1;
            public Prey(Point p, char g)
            {
                position = p;
                gender = g;
            }
        }

        public class Predator
        {
            public Point position;
            public char gender;
            public int food = Form1.food;
            public int pregnant = -1;
            public Predator(Point p, char g)
            {
                position = p;
                gender = g;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(DrawArea);
            Point center = PointToClient(Cursor.Position);
            float x = center.X - radius;
            float y = center.Y - radius;
            if (radioButton1.Checked)
            {
                Prey prey;
                if (radioButton3.Checked)
                {
                    prey = new Prey(center, 'M');
                    g.DrawEllipse(new Pen(Brushes.Blue), x, y, width, height);
                }
                else
                {
                    prey = new Prey(center, 'F');
                    g.DrawEllipse(new Pen(Brushes.Yellow), x, y, width, height);
                }
                g.FillEllipse(Brushes.Green, x, y, width, height);
                PreyList.Add(prey);
            }
            else
            {
                Predator predator;
                if (radioButton3.Checked)
                {
                    predator = new Predator(center, 'M');
                    g.DrawEllipse(new Pen(Brushes.Blue), x, y, width, height);
                }
                else
                {
                    predator = new Predator(center, 'F');
                    g.DrawEllipse(new Pen(Brushes.Yellow), x, y, width, height);
                }
                g.FillEllipse(Brushes.Red, x, y, width, height);
                PredatorList.Add(predator);
            }
            pictureBox1.Image = DrawArea;
        }

        private void tryEat()
        {
            for (int i = 0; i < PredatorList.Count; i++)
            {
                Predator predator = PredatorList[i];
                for (int j = 0; j < PreyList.Count; j++)
                {
                    Prey prey = PreyList[j];
                    double distance = Math.Sqrt(Math.Pow(prey.position.X - predator.position.X, 2) + Math.Pow(prey.position.Y - predator.position.Y, 2));
                    if (distance <= 2 * radius)
                    {
                        PreyList.RemoveAt(j);
                        predator.food = food;
                    }
                }
            }
        }

        private bool isInsideArea(int x, int y)
        {
            if (x <= 0 || x >= pictureBox1.Size.Width || y <= 0 || y >= pictureBox1.Size.Height)
                return false;
            return true;
        }

        private void moveAnimals(Bitmap bitmap)
        {
            Random rnd = new Random();
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < PredatorList.Count; i++)
            {
                Predator predator = PredatorList[i];
                int rand = rnd.Next(1, 10);
                switch (rand)
                {
                    case 1:
                        if (isInsideArea(predator.position.X - 1, predator.position.Y - 1))
                        {
                            --predator.position.X;
                            --predator.position.Y;
                        }
                        break;
                    case 2:
                        if (predator.position.Y - 1 > 0)
                            --predator.position.Y;
                        break;
                    case 3:
                        if (isInsideArea(predator.position.X + 1, predator.position.Y - 1))
                        {
                            ++predator.position.X;
                            --predator.position.Y;
                        }
                        break;
                    case 4:
                        if (predator.position.X - 1 > 0)
                            --predator.position.X;
                        break;
                    case 5:
                        break;
                    case 6:
                        if (predator.position.X + 1 < pictureBox1.Width)
                            ++predator.position.X;
                        break;
                    case 7:
                        if (isInsideArea(predator.position.X - 1, predator.position.Y + 1))
                        {
                            --predator.position.X;
                            ++predator.position.Y;
                        }
                        break;
                    case 8:
                        if (predator.position.Y + 1 < pictureBox1.Height)
                            ++predator.position.Y;
                        break;
                    case 9:
                        if (isInsideArea(predator.position.X + 1, predator.position.Y + 1))
                        {
                            ++predator.position.X;
                            ++predator.position.Y;
                        }
                        break;
                    default:
                        break;
                }
                g.FillEllipse(Brushes.Red, predator.position.X, predator.position.Y, width, height);
                if (predator.gender == 'M')
                    g.DrawEllipse(new Pen(Brushes.Blue), predator.position.X, predator.position.Y, width, height);
                else
                    g.DrawEllipse(new Pen(Brushes.Yellow), predator.position.X, predator.position.Y, width, height);
            }
            for (int i = 0; i < PreyList.Count; i++)
            {
                Prey prey = PreyList[i];
                int rand = rnd.Next(1, 10);
                switch (rand)
                {
                    case 1:
                        if (isInsideArea(prey.position.X - 1, prey.position.Y - 1))
                        {
                            --prey.position.X;
                            --prey.position.Y;
                        }
                        break;
                    case 2:
                        if (prey.position.Y - 1 > 0)
                            --prey.position.Y;
                        break;
                    case 3:
                        if (isInsideArea(prey.position.X + 1, prey.position.Y - 1))
                        {
                            ++prey.position.X;
                            --prey.position.Y;
                        }
                        break;
                    case 4:
                        if (prey.position.X - 1 > 0)
                            --prey.position.X;
                        break;
                    case 5:
                        break;
                    case 6:
                        if (prey.position.X + 1 < pictureBox1.Width)
                            ++prey.position.X;
                        break;
                    case 7:
                        if (isInsideArea(prey.position.X - 1, prey.position.Y + 1))
                        {
                            --prey.position.X;
                            ++prey.position.Y;
                        }
                        break;
                    case 8:
                        if (prey.position.Y + 1 < pictureBox1.Height)
                            ++prey.position.Y;
                        break;
                    case 9:
                        if (isInsideArea(prey.position.X + 1, prey.position.Y + 1))
                        {
                            ++prey.position.X;
                            ++prey.position.Y;
                        }
                        break;
                    default:
                        break;
                }
                g.FillEllipse(Brushes.Green, prey.position.X, prey.position.Y, width, height);
                if (prey.gender == 'M')
                    g.DrawEllipse(new Pen(Brushes.Blue), prey.position.X, prey.position.Y, width, height);
                else
                    g.DrawEllipse(new Pen(Brushes.Yellow), prey.position.X, prey.position.Y, width, height);
            }
            pictureBox1.Image = bitmap;
        }

        private void Starving()
        {
            for (int i = 0; i < PredatorList.Count; i++)
            {
                Predator predator = PredatorList[i];
                if (predator.food == 0)
                    PredatorList.RemoveAt(i);
                predator.food--;
            }
        }

        private void makeChildren()
        {
            for (int i = 0; i < PredatorList.Count - 1; i++)
            {
                Predator predator1 = PredatorList[i];
                for (int j = i + 1; j < PredatorList.Count; j++)
                {
                    Predator predator2 = PredatorList[j];
                    double distance = Math.Sqrt(Math.Pow(predator1.position.X - predator2.position.X, 2) + Math.Pow(predator1.position.Y - predator2.position.Y, 2));
                    if (predator1.gender == 'M' && predator2.gender == 'F' && distance <= 2 * radius && predator2.pregnant == -1)
                    {
                        predator2.pregnant = pregnanttime;
                    }
                    if (predator1.gender == 'F' && predator2.gender == 'M' && distance <= 2 * radius && predator1.pregnant == -1)
                    {
                        predator1.pregnant = pregnanttime;
                    }
                }
            }
            for (int i = 0; i < PreyList.Count - 1; i++)
            {
                Prey prey1 = PreyList[i];
                for (int j = i + 1; j < PreyList.Count; j++)
                {
                    Prey prey2 = PreyList[j];
                    double distance = Math.Sqrt(Math.Pow(prey1.position.X - prey2.position.X, 2) + Math.Pow(prey1.position.Y - prey2.position.Y, 2));
                    if (prey1.gender == 'M' && prey2.gender == 'F' && distance <= 2 * radius && prey2.pregnant == -1)
                    {
                        prey2.pregnant = pregnanttime;
                    }
                    if (prey1.gender == 'F' && prey2.gender == 'M' && distance <= 2 * radius && prey1.pregnant == -1)
                    {
                        prey1.pregnant = pregnanttime;
                    }
                }
            }
        }

        private void Born(Bitmap bitmap)
        {
            Random rnd = new Random();
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < PredatorList.Count; i++)
            {
                Predator predator = PredatorList[i];
                if (predator.gender == 'F' && predator.pregnant >= 0)
                {
                    if (predator.pregnant == 0)
                    {
                        int a = rnd.Next(1, 3);
                        Predator newPredator;
                        switch (a)
                        {
                            case 1:
                                newPredator = new Predator(new Point(predator.position.X + 25, predator.position.Y + 25), 'M');
                                PredatorList.Add(newPredator);
                                g.FillEllipse(Brushes.Red, newPredator.position.X, newPredator.position.Y, width, height);
                                g.DrawEllipse(new Pen(Brushes.Blue), newPredator.position.X, newPredator.position.Y, width, height);

                                break;
                            case 2:
                                newPredator = new Predator(new Point(predator.position.X + 25, predator.position.Y + 25), 'F');
                                PredatorList.Add(newPredator);
                                g.FillEllipse(Brushes.Red, newPredator.position.X, newPredator.position.Y, width, height);
                                g.DrawEllipse(new Pen(Brushes.Yellow), newPredator.position.X, newPredator.position.Y, width, height);
                                break;
                            default:
                                break;
                        }       
                    }
                    predator.pregnant--;
                }
            }
            for (int i = 0; i < PreyList.Count; i++)
            {
                Prey prey = PreyList[i];
                if (prey.gender == 'F' && prey.pregnant >= 0)
                {
                    if (prey.pregnant == 0)
                    {
                        int a = rnd.Next(1, 3);
                        Prey newPrey;
                        switch (a)
                        {
                            case 1:
                                newPrey = new Prey(new Point(prey.position.X + 3, prey.position.Y + 3), 'M');
                                PreyList.Add(newPrey);
                                g.FillEllipse(Brushes.Green, newPrey.position.X, newPrey.position.Y, width, height);
                                g.DrawEllipse(new Pen(Brushes.Blue), newPrey.position.X, newPrey.position.Y, width, height);

                                break;
                            case 2:
                                newPrey = new Prey(new Point(prey.position.X + 3, prey.position.Y + 3), 'F');
                                PreyList.Add(newPrey);
                                g.FillEllipse(Brushes.Green, newPrey.position.X, newPrey.position.Y, width, height);
                                g.DrawEllipse(new Pen(Brushes.Yellow), newPrey.position.X, newPrey.position.Y, width, height);
                                break;
                            default:
                                break;
                        }
                    }
                    prey.pregnant--;
                }
            }
        }

        private void functionDisplay(Bitmap bitmap)
        {
            try
            {
                Graphics g = Graphics.FromImage(bitmap);
                Point predatorT1 = new Point(ticks, 122 - PredatorList.Count);
                Point preyT1 = new Point(ticks, 122 - PreyList.Count);
                g.DrawLine(new Pen(Brushes.Red), predatorT0, predatorT1);
                g.DrawLine(new Pen(Brushes.Green), preyT0, preyT1);
                predatorT0 = predatorT1;
                preyT0 = preyT1;
                pictureBox2.Image = bitmap;
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(PreyList.Count);
            textBox2.Text = Convert.ToString(PredatorList.Count);
            Bitmap bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            tryEat();
            Starving();
            makeChildren();
            Born(bitmap);
            moveAnimals(bitmap);
            DrawArea = bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PredatorList.Clear();
            PreyList.Clear();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            timer1.Stop();
            timer2.Stop();
            Function = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            pictureBox2.Image = Function;
            ticks = 0;
            textBox1.Text = Convert.ToString(PreyList.Count);
            textBox2.Text = Convert.ToString(PredatorList.Count);
            preyT0 = new Point(0, 122);
            predatorT0 = new Point(0, 122);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ++ticks;
            functionDisplay(Function);
        }
    }
}
